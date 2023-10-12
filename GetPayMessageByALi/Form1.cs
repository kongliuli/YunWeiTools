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

            // ��ȡExcel�ļ�������ת��ΪDataTable
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
            this.Text=$"��ǰ��ȡ���������ܹ�{dt.Rows.Count}��,����Ľ���ʱ��Ϊ{0},����Ľ���ʱ��Ϊ{0}";
        }

        /// <summary>
        /// ��ȡCSV�ļ�
        /// </summary>
        /// <param name="filePath">�ļ�·�� eg��D:\A.csv</param>
        /// <param name="dt">���ݣ��ޱ��⣩</param>
        /// <param name="csvTitles">����</param>
        public static bool ReadCSV(string filePath,out System.Data.DataTable dt,out ArrayList csvTitles)
        {
            dt=new System.Data.DataTable();
            csvTitles=new ArrayList();
            try
            {
                FileStream fs = new FileStream(filePath,FileMode.Open,FileAccess.Read);
                StreamReader sr = new StreamReader(fs,Encoding.GetEncoding("utf-8"));
                //��¼ÿ�ζ�ȡ��һ�м�¼
                string strLine = null;
                //��¼ÿ�м�¼�еĸ��ֶ�����
                string[] arrayLine = null;
                //�ָ���
                string[] separators = { "," };
                //��ͷ��־λ�����ǵ�һ�Σ�������ͷ��
                bool isFirst = true;
                //���ж�ȡCSV�ļ�
                while((strLine=sr.ReadLine())!=null)
                {
                    //ȥ��ͷβ�ո�
                    strLine=strLine.Trim();
                    //�ָ��ַ�������������
                    arrayLine=strLine.Split(separators,StringSplitOptions.TrimEntries);
                    //������ͷ
                    if(isFirst)
                    {
                        for(int i = 0;i<arrayLine.Length;i++)
                        {
                            dt.Columns.Add(arrayLine[i]);//ÿһ������
                            csvTitles.Add(arrayLine[i]);
                        }
                        isFirst=false;
                    }
                    else   //������
                    {
                        DataRow dataRow = dt.NewRow();//�½�һ��
                        for(int j = 0;j<arrayLine.Length;j++)
                        {
                            dataRow[j]=arrayLine[j];
                        }
                        dt.Rows.Add(dataRow);//���һ��
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

            weeks.Text=$"��ǰѡ�е��ǵ� {num} ��,���������Ǵ� {firstDayOfWeek.ToShortDateString()} �� {lastDayOfWeek.ToShortDateString()}";
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
            var �ɹ� = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("����ʱ��"))
                     where CheckPrice(row.Field<string>("�ֽ�֧��"))
                     select row;

            var ���� = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("�˵�����ʱ��"))
                     where CheckPrice(row.Field<string>("�ֽ�֧��"))
                     select row;

            var finaldt = new System.Data.DataTable();
            finaldt=dt.Copy();
            finaldt.Rows.Clear();
            foreach(var row in �ɹ�)
            {
                finaldt.Rows.Add(row.ItemArray);
                var a = row.ItemArray;
                a[2]="��ע";
                finaldt.Rows.Add(a);
            }
            foreach(var row in ����)
            {
                finaldt.Rows.Add(row.ItemArray);
            }
            dataGridView1.DataSource=finaldt;

            this.Text=$"��ǰ��ȡ�������ڵ������ܹ�{����.Count()+�ɹ�.Count()}��";
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
                   int.Parse(times[0].Split("/")[0]),
                   int.Parse(times[0].Split("/")[1]),
                   int.Parse(times[0].Split("/")[2]),
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
            //�������
            var �ɹ� = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("����ʱ��"))
                     where CheckPrice(row.Field<string>("�ֽ�֧��"))
                     select row;

            var ���� = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("�˵�����ʱ��"))
                     where CheckPrice(row.Field<string>("�ֽ�֧��"))
                     select row;

            var a = �ɹ�.Sum(x => decimal.Parse(x.Field<string>("�ֽ�֧��")));
            var b = ����.Sum(x => decimal.Parse(x.Field<string>("�ֽ�֧��")));

            MessageBox.Show($"��ǰ����,��{comboBox1.SelectedItem}��({firstDayOfWeek:yyyy-MM-dd}-{lastDayOfWeek:yyyy-MM-dd})�ɹ�����Ϊ{a}Ԫ,��������Ϊ{b}Ԫ");
        }

        private void button3_Click(object sender,EventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            path+="//ͳ������//";
            var db = dataGridView1.DataSource as DataTable;

            if(dataGridView1.DataSource!=null)
            {
                ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
                // ����һ���µ�Excel��
                using ExcelPackage excelPackage = new();
                // ���һ���µĹ�����
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                for(int i = 0;i<db.Columns.Count;i++)
                {
                    worksheet.Cells[1,i+1].Value=db.Columns[i].ColumnName;
                }

                // ��DataTable������д��Excel������
                for(int i = 1;i<db.Rows.Count;i++)
                {
                    for(int j = 0;j<db.Columns.Count;j++)
                    {
                        worksheet.Cells[i+1,j+1].Value=db.Rows[i][j];
                    }
                }

                // ����Excel�ļ�
                FileInfo excelFile = new FileInfo($"{path}��{comboBox1.SelectedItem}��ԭʼ����.xlsx");
                excelPackage.SaveAs(excelFile);
            }
        }

        private void button4_Click(object sender,EventArgs e)
        {
            ExcelPackage.LicenseContext=LicenseContext.NonCommercial;

            #region ԭʼ������ϴ
            var �ɹ� = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("����ʱ��"))
                     where CheckPrice(row.Field<string>("�ֽ�֧��"))
                     select row;

            var ���� = from row in dt.AsEnumerable()
                     where Checktime(row.Field<string>("�˵�����ʱ��"))
                     where CheckPrice(row.Field<string>("�ֽ�֧��"))
                     select row;
            #endregion

            #region �������ݲ��
            List<aLiCloudData> groupdata = new();
            foreach(var data in ����)
            {
                string username = data["�˺�"].ToString();
                string itemname = data["��Ʒ"].ToString();
                string type = "����";

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
                        price=decimal.Parse(data["�ֽ�֧��"].ToString()),
                        dataRows=new()
                    };
                    g.dataRows.Add(data);
                    groupdata.Add(g);
                }
            }
            foreach(var data in �ɹ�)
            {
                string username = data["�˺�"].ToString();
                string itemname = data["��Ʒ"].ToString();
                string type = "�ɹ�";

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
                        price=decimal.Parse(data["�ֽ�֧��"].ToString())
                    };
                    g.dataRows.Add(data);
                    groupdata.Add(g);
                }
            }
            #endregion

            #region ���ݻ���
            //��ϲ���
            MessageBox.Show($"data���ܹ���{groupdata.Count}��");

            foreach(var data in groupdata)
            {
                data.Match();
            }

            DataTable dt1 = new();
            dt1.Columns.Add("���");
            dt1.Columns.Add("�˺�");
            dt1.Columns.Add("��Ʒ����");
            dt1.Columns.Add("��������");
            dt1.Columns.Add("���ѽ��");
            dt1.Columns.Add("�˵�����");

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

            #region �ɱ���
            //������Դ���ųɸ��ָ�ʽ��excel�ļ�
            using ExcelPackage excelPackage = new();
            //�������ݱ�
            ExcelWorksheet database = excelPackage.Workbook.Worksheets.Add("database");
            //database.cells��Ҫ��ʼ��
            database.InsertRow(0,55);


            //�ϲ���Ԫ��
            database.Cells[0,0,2,6].Merge=true;

            int merge1 = groupdata.FindAll(x => x.PriceType=="����").Count;
            int merge2 = groupdata.FindAll(x => x.PriceType=="�ɹ�").Count;
            database.Cells[3,0,merge1+2+2,0].Merge=true;
            database.Cells[7+merge1,0,7+merge1+merge2+1,0].Merge=true;
            database.Cells[merge2+merge1+2+1+3,0,merge1+merge2+6+2,0].Merge=true;

            //�̶�����ע��
            database.Cells[0,0].Value=$"��num�ܰ������ʷ�ͳ��(time1��time2)";
            //database.Cells[""].Value="��������";
            //database.Cells[""].Value="�ɹ�����";
            //database.Cells[""].Value="�ܼ�(Ԫ)";
            //database.Cells[""].Value="�Ʒ����Ʒ";
            //database.Cells[""].Value="kujiang";
            //database.Cells[""].Value="��Ɓ���1";
            //database.Cells[""].Value="��Ɓ���2";
            //database.Cells[""].Value="�ܼ�(Ԫ)";
            //database.Cells[""].Value="��ע";

            //��ֵ����

            //����

            //����,�������ļ���ȡ����ͼ���ʽ
            if(false)
            {
                ExcelWorksheet image = excelPackage.Workbook.Worksheets.Add("չʾͼ��");
            }

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
            foreach(var row in dataRows)
            {

            }
        }

    }
}