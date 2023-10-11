using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;

using OfficeOpenXml;

namespace GetPayMessageByALi
{
    public partial class Form1:Form
    {
        DateTime startDate = new DateTime(2023,1,1);
        DateTime firstDayOfWeek;
        DateTime lastDayOfWeek;
        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Clear();
            for(int i = 1;i<54;i++)
            {
                comboBox1.Items.Add(i);
            }
        }

        System.Data.DataTable dt = new();

        private void Form1_DragEnter(object sender,DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if(firstDayOfWeek==DateTime.MinValue)
                {
                    return;
                }
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string extension = Path.GetExtension(files[0]);
                if(extension==".csv")
                {
                    e.Effect=DragDropEffects.Copy;
                }
                comboBox1.Enabled=false;
            }
        }

        private void Form1_DragDrop(object sender,DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string filePath = files[0];

            // 读取Excel文件并将其转换为DataTable
            System.Data.DataTable dataTable = new();
            ArrayList al = new();
            ReadCSV(filePath,out dataTable,out al);
            if(dt.Columns.Count<dataTable.Columns.Count)
            {
                dt.Columns.Clear();
                foreach(DataColumn dr in dataTable.Columns)
                {
                    dt.Columns.Add(dr.ColumnName);
                }
            }
            foreach(DataRow dr in dataTable.Rows)
            {
                dt.Rows.Add(dr.ItemArray);
            }

            TotalInformation();
        }

        private void TotalInformation()
        {
            this.Text=$"当前读取到的数据总共{dt.Rows.Count}条,最早的结束时间为{0},最晚的结束时间为{0}";
        }

        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="filePath">文件路径 eg：D:\A.csv</param>
        /// <param name="dt">数据（无标题）</param>
        /// <param name="csvTitles">标题</param>
        public static bool ReadCSV(string filePath,out System.Data.DataTable dt,out ArrayList csvTitles)
        {
            dt=new System.Data.DataTable();
            csvTitles=new ArrayList();
            try
            {
                FileStream fs = new FileStream(filePath,FileMode.Open,FileAccess.Read);
                StreamReader sr = new StreamReader(fs,Encoding.GetEncoding("utf-8"));
                //记录每次读取的一行记录
                string strLine = null;
                //记录每行记录中的各字段内容
                string[] arrayLine = null;
                //分隔符
                string[] separators = { "," };
                //表头标志位（若是第一次，建立表头）
                bool isFirst = true;
                //逐行读取CSV文件
                while((strLine=sr.ReadLine())!=null)
                {
                    //去除头尾空格
                    strLine=strLine.Trim();
                    //分隔字符串，返回数组
                    arrayLine=strLine.Split(separators,StringSplitOptions.TrimEntries);
                    //建立表头
                    if(isFirst)
                    {
                        for(int i = 0;i<arrayLine.Length;i++)
                        {
                            dt.Columns.Add(arrayLine[i]);//每一列名称
                            csvTitles.Add(arrayLine[i]);
                        }
                        isFirst=false;
                    }
                    else   //表内容
                    {
                        DataRow dataRow = dt.NewRow();//新建一行
                        for(int j = 0;j<arrayLine.Length;j++)
                        {
                            dataRow[j]=arrayLine[j];
                        }
                        dt.Rows.Add(dataRow);//添加一行
                    }
                }
                sr.Close();
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender,EventArgs e)
        {
            int num = int.Parse(((ComboBox)sender).SelectedItem.ToString());

            Calendar calendar = CultureInfo.InvariantCulture.Calendar;

            firstDayOfWeek=GetFirstDayOfWeek(startDate,num,calendar);
            lastDayOfWeek=GetLastDayOfWeek(firstDayOfWeek,calendar);

            weeks.Text=$"当前选中的是第 {num} 周,日期区间是从 {firstDayOfWeek.ToShortDateString()} 到 {lastDayOfWeek.ToShortDateString()}";
        }

        static DateTime GetFirstDayOfWeek(DateTime date,int weekNumber,Calendar calendar)
        {
            DateTime firstDayOfYear = new DateTime(date.Year,1,1);
            int firstWeekNumber = calendar.GetWeekOfYear(firstDayOfYear,CalendarWeekRule.FirstDay,DayOfWeek.Sunday);

            DateTime firstDayOfWeek = firstDayOfYear.AddDays((weekNumber-firstWeekNumber)*7).AddDays(1);
            while(calendar.GetWeekOfYear(firstDayOfWeek,CalendarWeekRule.FirstDay,DayOfWeek.Sunday)<weekNumber)
            {
                firstDayOfWeek=firstDayOfWeek.AddDays(1);
            }

            return firstDayOfWeek;
        }

        static DateTime GetLastDayOfWeek(DateTime firstDayOfWeek,Calendar calendar)
        {
            DateTime lastDayOfWeek = firstDayOfWeek.AddDays(6);
            return lastDayOfWeek;
        }

        private void button1_Click(object sender,EventArgs e)
        {
            var 采购 = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("消费时间"))
                     where CheckPrice(row.Field<string>("现金支付"))
                     select row;

            var 买量 = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("账单结束时间"))
                     where CheckPrice(row.Field<string>("现金支付"))
                     select row;

            var finaldt = new System.Data.DataTable();
            finaldt=dt.Copy();
            finaldt.Rows.Clear();
            foreach(var row in 采购)
            {
                finaldt.Rows.Add(row.ItemArray);
                var a = row.ItemArray;
                a[2]="备注";
                finaldt.Rows.Add(a);
            }
            foreach(var row in 买量)
            {
                finaldt.Rows.Add(row.ItemArray);
            }
            dataGridView1.DataSource=finaldt;

            this.Text=$"当前读取到周期内的数据总共{买量.Count()+采购.Count()}条";
        }

        private bool Checktime(string time)
        {
            if(string.IsNullOrEmpty(time))
            {
                return false;
            }
            if(time.Equals("-"))
            {
                return false;
            }
            //2023/9/25 7:00 ?
            //2023-10-1 10:30:00
            string[] times = time.Split(" ");
            try
            {
                DateTime ti = new DateTime(
                   int.Parse(times[0].Split("-")[0]),
                   int.Parse(times[0].Split("-")[1]),
                   int.Parse(times[0].Split("-")[2]),
                   int.Parse(times[1].Split(":")[0]),
                   int.Parse(times[1].Split(":")[1]),
                    0);
                if(ti>=firstDayOfWeek&&ti<=lastDayOfWeek)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        private bool CheckPrice(string num)
        {
            if(string.IsNullOrEmpty(num))
            {
                return false;
            }
            try
            {
                Double i = Double.Parse(num);
                if(i>0)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;

        }

        private void button2_Click(object sender,EventArgs e)
        {
            //结果汇总
            var 采购 = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("消费时间"))
                     where CheckPrice(row.Field<string>("现金支付"))
                     select row;

            var 买量 = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("账单结束时间"))
                     where CheckPrice(row.Field<string>("现金支付"))
                     select row;

            var a = 采购.Sum(x => decimal.Parse(x.Field<string>("现金支付")));
            var b = 买量.Sum(x => decimal.Parse(x.Field<string>("现金支付")));

            MessageBox.Show($"当前计算,第{comboBox1.SelectedItem}周({firstDayOfWeek:yyyy-MM-dd}-{lastDayOfWeek:yyyy-MM-dd})采购费用为{a}元,买量费用为{b}元");
        }

        private void button3_Click(object sender,EventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            path+="//统计数据//";
            var db = dataGridView1.DataSource as DataTable;

            if(dataGridView1.DataSource!=null)
            {
                ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
                // 创建一个新的Excel包
                using(ExcelPackage excelPackage = new())
                {
                    // 添加一个新的工作表
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    for(int i = 0;i<db.Columns.Count;i++)
                    {
                        worksheet.Cells[1,i+1].Value=db.Columns[i].ColumnName;
                    }

                    // 将DataTable的数据写入Excel工作表
                    for(int i = 1;i<db.Rows.Count;i++)
                    {
                        for(int j = 0;j<db.Columns.Count;j++)
                        {
                            worksheet.Cells[i+1,j+1].Value=db.Rows[i][j];
                        }
                    }

                    // 保存Excel文件
                    FileInfo excelFile = new FileInfo($"{path}第{comboBox1.SelectedItem}周原始数据.xlsx");
                    excelPackage.SaveAs(excelFile);
                }
            }
        }

        private void button4_Click(object sender,EventArgs e)
        {
            #region 原始数据清洗
            var 采购 = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("消费时间"))
                     where CheckPrice(row.Field<string>("现金支付"))
                     select row;

            var 买量 = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("账单结束时间"))
                     where CheckPrice(row.Field<string>("现金支付"))
                     select row;
            List<EnumerableRowCollection<DataRow>?> alldata = new()
            {
                买量,
                采购
            };
            #endregion

            #region 分类数据拆分

            foreach(var data in alldata)
            {

            }



            #endregion

            #region 数据汇总
            #endregion

            #region 成表导出
            #endregion

            comboBox1.Enabled=true;
        }
    }
}