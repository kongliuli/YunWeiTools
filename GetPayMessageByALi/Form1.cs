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
            //path+="//统计数据//";
            var db = dataGridView1.DataSource as DataTable;

            if(dataGridView1.DataSource!=null)
            {
                ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
                // 创建一个新的Excel包
                using ExcelPackage excelPackage = new();
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
                FileInfo excelFile = new FileInfo($"{path}\\第{comboBox1.SelectedItem}周原始数据.xlsx");
                excelPackage.SaveAs(excelFile);
            }
        }

        private void button4_Click(object sender,EventArgs e)
        {
            ExcelPackage.LicenseContext=LicenseContext.NonCommercial;

            #region 原始数据清洗
            var 采购 = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("消费时间"))
                     where CheckPrice(row.Field<string>("现金支付"))
                     select row;

            var 买量 = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("账单结束时间"))
                     where CheckPrice(row.Field<string>("现金支付"))
                     select row;
            #endregion

            #region 分类数据拆分
            List<aLiCloudData> groupdata = new();
            foreach(var data in 买量)
            {
                string username = data["账号"].ToString();
                string itemname = data["产品"].ToString();
                string type = "买量";

                if(groupdata.Any(x => x.Username==username&&x.Itemname==itemname&&x.PriceType==type))
                {
                    var g = groupdata.FindAll(x => x.Username==username&&x.Itemname==itemname&&x.PriceType==type).First();
                    g?.dataRows.Add(data);
                }
                else
                {
                    aLiCloudData g = new()
                    {
                        Username=username,
                        Itemname=itemname,
                        PriceType=type,
                        price=decimal.Parse(data["现金支付"].ToString()),
                        dataRows=new()
                    };
                    g.dataRows.Add(data);
                    groupdata.Add(g);
                }
            }
            foreach(var data in 采购)
            {
                string username = data["账号"].ToString();
                string itemname = data["产品"].ToString();
                string type = "采购";

                if(groupdata.Any(x => x.Username==username&&x.Itemname==itemname&&x.PriceType==type))
                {
                    var g = groupdata.FindAll(x => x.Username==username&&x.Itemname==itemname&&x.PriceType==type).First();
                    g?.dataRows.Add(data);
                }
                else
                {
                    aLiCloudData g = new()
                    {
                        Username=username,
                        Itemname=itemname,
                        PriceType=type,
                        price=decimal.Parse(data["现金支付"].ToString()),
                        dataRows=new()
                    };
                    g.dataRows.Add(data);
                    groupdata.Add(g);
                }
            }
            #endregion

            #region 数据汇总
            //间断测试
            MessageBox.Show($"data集总共有{groupdata.Count}种");

            foreach(var data in groupdata)
            {
                data.Match();
            }

            DataTable dt1 = new();
            dt1.Columns.Add("序号");
            dt1.Columns.Add("账号");
            dt1.Columns.Add("产品名称");
            dt1.Columns.Add("消费类型");
            dt1.Columns.Add("消费金额");
            dt1.Columns.Add("账单总数");

            for(int i = 0;i<groupdata.Count;i++)
            {
                var row = new object[6];
                row[0]=i;
                row[1]=groupdata[i].Username;
                row[2]=groupdata[i].Itemname;
                row[3]=groupdata[i].PriceType;
                row[4]=groupdata[i].price;
                row[5]=groupdata[i].dataRows.Count;

                dt1.Rows.Add(row);
            }
            #endregion

            #region 成表导出
            //将数据源编排成各种格式的excel文件
            using ExcelPackage excelPackage = new();
            //定义数据表
            ExcelWorksheet database = excelPackage.Workbook.Worksheets.Add("database");

            #region 初始化  字号设置以及居中
            database.Cells.AutoFitColumns();
            database.Cells.Style.HorizontalAlignment=OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            database.Cells.Style.VerticalAlignment=OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            database.Cells.Style.Font.Size=12;
            int merge1 = groupdata.FindAll(x => x.PriceType=="买量").Count+2;
            int merge2 = groupdata.FindAll(x => x.PriceType=="采购").Count+1;
            #endregion

            #region 标题行的初始化设置
            //固定前3行7列为标题行,合并单元格,字号20,加粗并且居中
            database.Cells[1,1,3,7].Merge=true;
            database.Cells[1,1].Value=$"第{comboBox1.SelectedItem}周阿里云资费统计({firstDayOfWeek:yyyy-MM-dd}至{lastDayOfWeek:yyyy-MM-dd})";
            database.Cells[1,1].Style.Font.Size=20;
            database.Cells[1,1].Style.Font.Bold=true;

            //首列合并标题,字号20加粗居中
            database.Cells[4,1,merge1+4,1].Merge=true;
            database.Cells[4,1].Value="买量费用";
            database.Cells[4,1].Style.Font.Bold=true;

            database.Cells[merge1+4,2].Value="总计(元)";//买量列总计
            database.Cells[merge1+4,2].Style.Font.Bold=true;

            database.Cells[merge1+5,1,merge1+merge2+5,1].Merge=true;
            database.Cells[merge1+5,1].Value="采购费用";
            database.Cells[merge1+5,1].Style.Font.Bold=true;

            database.Cells[merge1+merge2+5,2].Value="总计(元)";//采购列总计
            database.Cells[merge1+merge2+5,2].Style.Font.Bold=true;

            database.Cells[merge1+merge2+6,1,merge1+merge2+7,1].Merge=true;
            database.Cells[merge1+merge2+6,1].Value="总计(元)";//全体总计
            database.Cells[merge1+merge2+6,1].Style.Font.Bold=true;

            //次标题加粗
            database.Cells[4,2].Value="云服务产品";
            database.Cells[4,2].Style.Font.Bold=true;
            database.Cells[4,3].Value="地平线";
            database.Cells[4,3].Style.Font.Bold=true;
            database.Cells[4,4].Value="花苼书城1";
            database.Cells[4,4].Style.Font.Bold=true;
            database.Cells[4,5].Value="花苼书城2";
            database.Cells[4,5].Style.Font.Bold=true;
            database.Cells[4,6].Value="总计(元)";//行总计
            database.Cells[4,6].Style.Font.Bold=true;
            database.Cells[4,7].Value="备注";
            database.Cells[4,7].Style.Font.Bold=true;
            #endregion

            #region 数值插入
            #region 子标题插入
            var data01 = (from dataRow in groupdata.FindAll(x => x.PriceType=="买量")
                          select dataRow).ToList();
            List<string> name = new List<string>();


            if(data01.Count==merge1-2)
            {
                for(int i = 5;i<data01.Count+5;i++)
                {
                    database.Cells[i,2].Value=data01[i-5];
                }
            }
            var data02 = (from dataRow in groupdata.FindAll(x => x.PriceType=="采购")
                          select dataRow.Itemname).ToList();
            if(data01.Count==merge2-1)
            {
                for(int i = 4+merge1;i<data02.Count+4+merge1;i++)
                {
                    database.Cells[i,2].Value=data02[i-4-merge1];
                }
            }
            #endregion


            //插值计算

            /*
             int j = 0;
                    var data = groupdata.Find(x => x.Itemname==b[i-5]);
                    if(data!=null)
                    {
                        switch(data.Username)
                        {
                            case "zh@kujiangcom\r\n":
                            j=3;
                            break;
                            case "南京花笙书城1\r\n":
                            j=4;
                            break;
                            case "南京花笙书城2\r\n":
                            j=5;
                            break;
                        }
                    }
                    if(j==0)
                    {
                        //errorreport
                        break;
                    }
             */

            #endregion

            //待做,从配置文件读取待做图表格式
            if(false)
            {
                ExcelWorksheet image = excelPackage.Workbook.Worksheets.Add("展示图表");
            }

            FileInfo excelFile = new FileInfo($"第{comboBox1.SelectedItem}周计算数据.xlsx");
            excelPackage.SaveAs(excelFile);
            #endregion

            comboBox1.Enabled=true;
        }
    }

    public class aLiCloudData
    {
        public string Username
        {
            get; set;
        }
        public string Itemname
        {
            get; set;
        }
        public string PriceType
        {
            get; set;
        }

        public List<DataRow> dataRows
        {
            get; set;
        }
        public decimal price
        {
            get; set;
        } = 0m;

        public void Match()
        {
            price=0;
            foreach(var row in dataRows)
            {
                var p = decimal.Parse(row[20].ToString());
                price+=p;

            }
        }

    }
}