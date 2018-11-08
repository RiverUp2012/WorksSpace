using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DWGdirect.DatabaseServices;
using System.IO;

namespace ReadDwg
{
    public partial class Form1 : Form
    {
        DWGdirect.Runtime.Services dd;
        string m_strCADPath;
        string m_strDwgFile;

        public Form1()
        {
            InitializeComponent();
        }

        /*
        void CheckDwgVersion(Database db)
        {
            DwgVersion version = db.OriginalFileVersion;
            switch (version)
            {
                case DwgVersion.vAC21:
                    {
                        comboCADVersion.Text = ("CAD 2008");
                        break;
                    }
                case DwgVersion.vAC18:
                    {
                        comboCADVersion.Text = ("CAD 2005");
                        break;
                    }
                case DwgVersion.vAC15:
                    {
                        comboCADVersion.Text = ("CAD 2000");
                        break;
                    }
                default:
                        {
                            comboCADVersion.Text = ("未知CAD版本");
                            break;
                        }
            }
        }
        */

        bool CheckDwgFile()
        {
            if (string.IsNullOrEmpty(m_strDwgFile)) return false;
            if (File.Exists(m_strDwgFile) == false) return false;

            try
            {
                Database db = new Database(false, false);
                db.ReadDwgFile(m_strDwgFile, FileOpenMode.OpenForReadAndAllShare, false, "");

                List<string> listFile = new List<string>();
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    ObjectId objTextTableId = db.TextStyleTableId;
                    DBObject dbObj = tr.GetObject(objTextTableId, OpenMode.ForRead);
                    TextStyleTable textTable = dbObj as TextStyleTable;
                    if (textTable != null)
                    {
                        foreach (ObjectId obj in textTable)
                        {
                            TextStyleTableRecord textRecord = tr.GetObject(obj, OpenMode.ForRead) as TextStyleTableRecord;
                            if (textRecord != null)
                            {
                                string str = textRecord.FileName.ToLower();
                                if (string.IsNullOrEmpty(str) == false)
                                {
                                    if (str.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) == false)
                                    {
                                        if (str.EndsWith(".shx", StringComparison.OrdinalIgnoreCase) == false)
                                            str += ".shx";
                                        if (listFile.Contains(str) == false)
                                            listFile.Add(str);
                                    }
                                }

                                str = textRecord.BigFontFileName.ToLower();
                                if (string.IsNullOrEmpty(str) == false)
                                {
                                    if (str.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) == false)
                                    {
                                        if (str.EndsWith(".shx", StringComparison.OrdinalIgnoreCase) == false)
                                            str += ".shx";
                                        if (listFile.Contains(str) == false)
                                            listFile.Add(str);
                                    }
                                }
                            }

                        }
                    }
                    tr.Abort();
                }

                RegistryCAD regCad = new RegistryCAD();
                string strCadVer = comboCADVersion.Text;
                if (strCadVer == "CAD 2005")
                    m_strCADPath = regCad.GetAutoCadPath(CadVersion.Cad2005);
                else if (strCadVer == "CAD 2008")
                    m_strCADPath = regCad.GetAutoCadPath(CadVersion.Cad2008);
                else if (strCadVer == "CAD 2009")
                    m_strCADPath = regCad.GetAutoCadPath(CadVersion.Cad2009);
                else if (strCadVer == "CAD 2010")
                    m_strCADPath = regCad.GetAutoCadPath(CadVersion.Cad2010_64);

                if (string.IsNullOrEmpty(m_strCADPath))
                {
                    richTextMsg.AppendText("\n 未知DWG文件版本，请转换成对应的版本再打开！");
                    return false;
                }

                m_strCADPath += "\\fonts";
                richTextMsg.AppendText("\n 字体文件夹路径：" + m_strCADPath);

                DirectoryInfo root = new DirectoryInfo(m_strCADPath);
                List<string> listCadFile = new List<string>(), listCanFind = new List<string>();
                foreach (FileInfo f in root.GetFiles())
                {
                    string name = f.Name;
                    if (name.EndsWith(".shx", StringComparison.OrdinalIgnoreCase))
                    {
                        listCadFile.Add(name.ToLower());
                    }
                }

                foreach (string str in listFile)
                {
                    ListViewItem listView = new ListViewItem();
                    listView.Text = str;

                    if (listCadFile.Contains(str))
                        listView.SubItems.Add("已有");
                    else
                        listView.SubItems.Add("未找到");

                    listViewFile.Items.Add(listView);
                }


            }
            catch (Exception ex)
            {

                throw;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Multiselect = false;
            openDlg.Filter = "dwg文件(*.dwg)|*.dwg";
            if (DialogResult.OK != openDlg.ShowDialog()) return;

            listViewFile.Items.Clear();
            richTextMsg.Text = "";
            m_strDwgFile = openDlg.FileName;

            CheckDwgFile();

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            richTextMsg.Text = "";

            try
            {
                bool bFirst = true;
                foreach (ListViewItem listView in listViewFile.Items)
                {
                    if (listView.SubItems[1].Text == "未找到")
                    {
                        string strFileName = listView.Text;

                        if (bFirst)
                        {
                            richTextMsg.AppendText(string.Format("开始下载 {0} 字体", strFileName));
                            bFirst = false;
                        }
                        else
                        {
                            richTextMsg.AppendText(string.Format("\n开始下载 {0} 字体", strFileName));
                        }

                        HttpDownLoadFile downLoad = new HttpDownLoadFile();
                        string strUrl = string.Format(@"http://www.cadfonts.com/CADFonts/{0}", strFileName);
                        string strPath = string.Format("{0}\\{1}", m_strCADPath, strFileName);
                        if (downLoad.Download(strUrl, strPath))
                        {
                            listView.SubItems[1].Text = "下载完成";
                            richTextMsg.AppendText(string.Format("\n{0} 字体下载完成", strFileName));
                        }
                        else
                        {
                            listView.SubItems[1].Text = "下载失败";
                            richTextMsg.AppendText(string.Format("\n{0} 字体下载失败：", downLoad.ErrorMsg));
                        }
                        

                    }
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dd = new DWGdirect.Runtime.Services();
            m_strCADPath = "";
            m_strDwgFile = "";
            comboCADVersion.SelectedIndex = 1;
        }

        private void comboCADVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewFile.Items.Clear();
            richTextMsg.Text = "";

            CheckDwgFile();
        }
           
    }
}
