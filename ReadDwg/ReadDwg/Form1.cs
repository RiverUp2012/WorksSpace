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

        public Form1()
        {
            InitializeComponent();

            dd = new DWGdirect.Runtime.Services();
            m_strCADPath = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Multiselect = false;
            openDlg.Filter = "dwg文件(*.dwg)|*.dwg";
            if (DialogResult.OK != openDlg.ShowDialog()) return;

            listViewFile.Items.Clear();

            try
            {
                Database db = null;
                db = new Database(false, false);
                db.ReadDwgFile(openDlg.FileName, FileOpenMode.OpenForReadAndAllShare, false, "");

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
                                if(string.IsNullOrEmpty(str) == false)
                                {
                                    if (str.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) == false)
                                    {
                                        if (str.EndsWith(".shx", StringComparison.OrdinalIgnoreCase) == false)
                                            str += ".shx";
                                        if(listFile.Contains(str) == false)
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
                if(strCadVer == "CAD 2005")
                    m_strCADPath = regCad.GetAutoCadPath(CadVersion.Cad2005);
                else if(strCadVer == "CAD 2008")
                    m_strCADPath = regCad.GetAutoCadPath(CadVersion.Cad2008);
                else if (strCadVer == "CAD 2009")
                    m_strCADPath = regCad.GetAutoCadPath(CadVersion.Cad2009);
                else if (strCadVer == "CAD 2010")
                    m_strCADPath = regCad.GetAutoCadPath(CadVersion.Cad2010_64);

                m_strCADPath += "\\fonts";
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

                    if(listCadFile.Contains(str))
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
           
    }
}
