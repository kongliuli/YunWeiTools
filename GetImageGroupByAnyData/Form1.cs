using System.Collections;
using System.Data;
using System.Text;

namespace GetImageGroupByAnyData
{
    public partial class Form1:Form
    {
        List<DataTable> tables = new List<DataTable>();
        List<string> infomations = new();//������ʾ�İ��ı���
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

        #region �¼�(��ק����/�򿪶�ȡ)
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
                // ��ȡExcel�ļ�������ת��ΪDataTable
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
                AddInfo($"{file}�Ѿ��ɹ���ȡ������");
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




        #region �ļ�����
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


        #endregion




        #region 
        #endregion


    }
}