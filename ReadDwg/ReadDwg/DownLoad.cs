using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace ReadDwg
{
    public class HttpDownLoadFile
    {
        string m_strError = "";
        public string ErrorMsg
        {
            get { return m_strError; }
        }

        /// <summary>
        /// Http方式下载文件
        /// </summary>
        /// <param name="url">http地址</param>
        /// <param name="localfile">本地文件</param>
        /// <returns></returns>
        public bool Download(string url, string localfile)
        {
            bool flag = false;

            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);// 打开网络连接

                Stream readStream = myRequest.GetResponse().GetResponseStream();// 向服务器请求,获得服务器的回应数据流

                FileStream writeStream = new FileStream(localfile, FileMode.Create);// 文件不保存创建一个文件

                byte[] btArray = new byte[512];// 定义一个字节数据,用来向readStream读取内容和向writeStream写入内容
                int contentSize = readStream.Read(btArray, 0, btArray.Length);// 向远程文件读第一次

                while (contentSize > 0)// 如果读取长度大于零则继续读
                {
                    writeStream.Write(btArray, 0, contentSize);// 写入本地文件
                    contentSize = readStream.Read(btArray, 0, btArray.Length);// 继续向远程文件读取
                }

                //关闭流
                writeStream.Close();
                readStream.Close();

                flag = true;        //返回true下载成功
            }
            catch (Exception ex)
            {
                m_strError = ex.Message;
                flag = false;       //返回false下载失败
            }

            return flag;
        }
    }
    
}
