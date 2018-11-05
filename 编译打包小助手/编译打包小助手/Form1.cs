using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 编译打包小助手
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process proc = null;
            try
            {
                proc = new Process();
                proc.StartInfo.WorkingDirectory = m_strDebugPath;
                proc.StartInfo.FileName = "AutoDeleteUselessFiles.bat";
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = m_strDebugPath + "DotNetReactor.nrproj";
            if(File.Exists(str) == false)
            {
                richTextBox1.AppendText("\n 找不到文件：" + str);
                return;
            }
            System.Diagnostics.Process.Start(str);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_strDebugPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            m_strDebugPath = Path.GetDirectoryName(m_strDebugPath);
            if (m_strDebugPath.EndsWith("\\") == false)
                m_strDebugPath += "\\";
        }

        string m_strDebugPath;

        private void button3_Click(object sender, EventArgs e)
        {
            Process proc = null;
            try
            {
                proc = new Process();
                proc.StartInfo.WorkingDirectory = m_strDebugPath;
                proc.StartInfo.FileName = "AutoCopyDll.bat";
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process proc = null;
            try
            {
                proc = new Process();
                proc.StartInfo.WorkingDirectory = m_strDebugPath;
                proc.StartInfo.FileName = "AutoCompilePackage.bat";
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process proc = null;
            try
            {
                proc = new Process();
                proc.StartInfo.WorkingDirectory = m_strDebugPath;
                proc.StartInfo.FileName = "AutoRenamePackage.bat";
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
            }
        }
    }
}
