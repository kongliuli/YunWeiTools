using System.Collections;
using System.Data;
using System.Text;

namespace GetImageGroupByAnyData
{
    public partial class Form1:Form
    {
        List<DataTable> tables = new List<DataTable>();
        List<string> infomations = new();//界面显示的绑定文本项
        List<string> tablenames = new();
        public Form1()
        {
            InitializeComponent();
            tables=new();
            infomations=new();
            tablenames=new();
        }

        private void AddInfo(string info)
        {
            if(infomations.Count>200)
                infomations.RemoveAt(0);
            infomations.Add(info);

            richTextBox1.Lines=infomations.ToArray();
        }

        #region 事件(拖拽接收/打开读取)
        private void Form1_DragEnter(object sender,DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string extension = Path.GetExtension(files[0]);
                if(extension==".csv")
                {
                    e.Effect=DragDropEffects.Copy;
                }
                if(extension==".xlsx"||extension==".xls")
                {
                    e.Effect=DragDropEffects.Copy;
                }
            }
        }

        private void Form1_DragDrop(object sender,DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach(string file in files)
            {
                // 读取Excel文件并将其转换为DataTable
                DataTable dataTable = new();
                dataTable.TableName=file;
                if(file.EndsWith(".csv"))
                {
                    ArrayList array = new ArrayList();
                    ReadCSV(file,out dataTable,out array);
                }
                if(file.EndsWith(".xls")||file.EndsWith(".xlsx"))
                {

                }
                tables.Add(dataTable);
                AddInfo($"{file}已经成功读取并解析");
            }

            UpdateListByTables();
        }
        private void UpdateListByTables()
        {
            tablenames.Clear();
            foreach(DataTable dt in tables)
            {
                string dtname = dt.TableName;
                if(!tablenames.Contains(dtname))
                {
                    tablenames.Add(dtname);
                }
            }
            listBox1.Items.AddRange(tablenames.ToArray());
        }


        #endregion




        #region 文件操作
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


        #endregion




        #region 
        #endregion


    }
}