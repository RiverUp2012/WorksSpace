using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace ReadDwg
{
    /// <summary>
    /// cad 配置相关信息
    /// </summary>
    public class RegistryCAD
    {
        private string currentCadLocation = string.Empty;//当前CAD安装路径
        private string currentCadProductName = string.Empty;//当前CAD版本名称
        static public string currentCadRNumber = string.Empty;//当前CAD的R版本数量
        /// <summary>
        /// 当前的CAD版本 默认是2008
        /// </summary>
        private static CadVersion currentCadVersion = CadVersion.Cad2008;
        /// <summary>
        /// 获取当前的CAD版本
        /// </summary>
        public static CadVersion CurrentCadVersion()
        {
#if CAD2016
                    currentCadVersion = CadVersion.Cad2016_64;
#else
#if CAD2010
                    currentCadVersion = CadVersion.Cad2010_64;
#else
                    currentCadVersion = CadVersion.Cad2008;
#endif

#endif

                    return currentCadVersion;
        }
        private List<CadInfoClass> allCadInfo;

        /// <summary>
        /// 当前CAD版本的路径
        /// </summary>
        public string CurrentCadLocation
        {
            get
            {
                GetCurrentCADLocation();
                return currentCadLocation;
            }
        }

        /// <summary>
        ///当前CAD版本名称
        /// </summary>
        public string CurrentCadProcuctName
        {
            get
            {
                GetCurrentCADProductName();
                return currentCadProductName;
            }
        }

        /// <summary>
        /// 系统已安装CAD信息
        /// </summary>
        public List<CadInfoClass> AllCadInfo
        {
            get
            {
                return allCadInfo;
            }
        }

        public RegistryCAD()
        {
            allCadInfo = new List<CadInfoClass>();
            allCadInfo = GetAllCADInfo();
        }

        /// <summary>
        /// 获取系统中已安装CAD的信息
        /// </summary>
        /// <returns></returns>
        private List<CadInfoClass> GetAllCADInfo()
        {
            try
            {
                // 获取HKEY_LOCAL_MACHINE键
                RegistryKey keyLocalMachine = Registry.LocalMachine;
                // 打开AutoCAD所属的注册表键:HKEY_LOCAL_MACHINE\Software\Autodesk\AutoCAD
                RegistryKey keyAutoCAD = keyLocalMachine.OpenSubKey("Software\\Autodesk\\AutoCAD");
                

                if (keyAutoCAD == null) return null;

                //获得表示系统中安装的各版本的AutoCAD注册表键
                string[] cadVersions = keyAutoCAD.GetSubKeyNames();
                foreach (string cadVersion in cadVersions)
                {
                    //打开特定版本的AutoCAD注册表键
                    RegistryKey keyCADVersion = keyAutoCAD.OpenSubKey(cadVersion);
                    //获取表示各语言版本的AutoCAD注册表键值
                    string[] cadNames = keyCADVersion.GetSubKeyNames();
                    foreach (string cadName in cadNames)
                    {
                        //打开AutoCAD所属的注册表键
                        RegistryKey keyCADName = keyCADVersion.OpenSubKey(cadName);

                        CadInfoClass cadInfo = new CadInfoClass();
                        object obj = keyCADName.GetValue("Location");
                        if (obj == null)
                            continue;
                        cadInfo.Location = obj.ToString();
                        obj = keyCADName.GetValue("ProductName");
                        if (obj == null)
                            continue;
                        cadInfo.ProductName = obj.ToString();
                        obj = keyCADName.GetValue("LocaleId");
                        if (obj == null)
                            continue;
                        cadInfo.LocaleId = obj.ToString();
                        obj = keyCADName.GetValue("Release");
                        if (obj == null)
                            continue;
                        cadInfo.Release = obj.ToString();
                        obj = keyCADName.GetValue("Language");
                        if (obj == null)
                            continue;
                        cadInfo.Language = obj.ToString();

                        allCadInfo.Add(cadInfo);
                    }
                }                
            }
            catch (Exception ex)
            {
                string message = "获取系统中已安装CAD的信息失败，" + ex.Message;
                MessageBox.Show(message, "提示");
            }
            return allCadInfo;
        }

        /// <summary>
        /// 获取Cad安装路径
        /// </summary>
        /// <param name="version">cad版本</param>
        /// <returns></returns>
        public string GetAutoCadPath(CadVersion version)
        {
            if (allCadInfo == null) return "";

            string strProductName = string.Empty;
            string cadVersion = "";
            switch (version)
            {
                case CadVersion.Cad2005:
                    strProductName = "AutoCAD 2005";
                    cadVersion = "R16.1";
                    break;
                case CadVersion.Cad2008:
                    strProductName = "AutoCAD 2008";
                    cadVersion = "R17.1";
                    break;
                case CadVersion.Cad2009:
                    strProductName = "AutoCAD 2009";
                    cadVersion = "R17.2";
                    break;
                case CadVersion.Cad2010_32:
                case CadVersion.Cad2010_64:
                    strProductName = "AutoCAD 2010";
                    cadVersion = "R18.0";
                    break;
                case CadVersion.Cad2011_32:
                case CadVersion.Cad2011_64:
                    strProductName = "AutoCAD 2011";
                    cadVersion = "R18.1";
                    break;
                case CadVersion.Cad2012_32:
                case CadVersion.Cad2012_64:
                    strProductName = "AutoCAD 2012";
                    cadVersion = "R18.2";
                    break;
                case CadVersion.Cad2013_32:
                case CadVersion.Cad2013_64:
                    strProductName = "AutoCAD 2013";
                    cadVersion = "R19.0";
                    break;
                case CadVersion.Cad2014_32:
                case CadVersion.Cad2014_64:
                    strProductName = "AutoCAD 2014";
                    cadVersion = "R19.1";
                    break;
                case CadVersion.Cad2015_32:
                case CadVersion.Cad2015_64:
                    strProductName = "AutoCAD 2015";
                    cadVersion = "R20.0";
                    break;
                case CadVersion.Cad2016_32:
                case CadVersion.Cad2016_64:
                    strProductName = "AutoCAD 2016";
                    cadVersion = "R20.1";
                    break;
                default:break;
            }
            if (strProductName == "")
                return "";
            ///设置当前版本
            RegistryCAD.currentCadRNumber = cadVersion;
            string strCadPath = string.Empty;
            foreach (CadInfoClass info in allCadInfo)
            {
                if (info.ProductName.Contains(strProductName))
                {
                    strCadPath = info.Location;
                    break;
                }
            }
            ///设置当前的cad版本
            SetCurrentCADKeyName(strProductName);
            return strCadPath;
        }

        /// <summary>
        /// 获取当前AutoCAD的注册表键名
        /// </summary>
        /// <returns></returns>
        private string GetCurrentCADKeyName()
        {
            // 获取HKEY_CURRENT_USER键
            RegistryKey keyCurrentUser = Registry.CurrentUser;
            // 打开AutoCAD所属的注册表键:HKEY_CURRENT_USER\Software\Autodesk\AutoCAD
            RegistryKey keyAutoCAD = keyCurrentUser.OpenSubKey("Software\\Autodesk\\AutoCAD");
            //获得表示当前的AutoCAD版本的注册表键值:R18.2
            string valueCurAutoCAD = currentCadRNumber;//keyAutoCAD.GetValue("CurVer").ToString();
            if (valueCurAutoCAD == null) return "";//如果未安装AutoCAD，则返回
            //获取当前的AutoCAD版本的注册表键
            RegistryKey keyCurAutoCAD = keyAutoCAD.OpenSubKey(valueCurAutoCAD);
            //获取表示AutoCAD当前语言的注册表键值:ACAD-a001:804
            string language = keyCurAutoCAD.GetValue("CurVer").ToString();
            //获取AutoCAD当前语言的注册表键
            RegistryKey keyLanguage = keyCurAutoCAD.OpenSubKey(language);
            //返回去除HKEY_LOCAL_MACHINE前缀的当前AutoCAD注册表项的键名
            return keyLanguage.Name.Substring(keyCurrentUser.Name.Length + 1);
        }

        /// <summary>
        /// 设置当前CAD版本
        /// </summary>
        /// <param name="cadProductName"></param>
        public static void SetCurrentCADKeyName(string cadProductName)
        {
            string cadVersion = "";
            switch (cadProductName)
            {
                case "AutoCAD2004":
                case "AutoCAD 2004":
                    cadVersion = "R16.0";
                    break;
                case "AutoCAD2005":
                case "AutoCAD 2005":
                    cadVersion = "R16.1";
                    break;
                case "AutoCAD2006":
                case "AutoCAD 2006":
                    cadVersion = "R16.2";
                    break;
                case "AutoCAD2007":
                case "AutoCAD 2007":
                    cadVersion = "R17.0";
                    break;
                case "AutoCAD2008":
                case "AutoCAD 2008":
                    cadVersion = "R17.1";
                    break;
                case "AutoCAD2009":
                case "AutoCAD 2009":
                    cadVersion = "R17.2";
                    break;
                case "AutoCAD2010":
                case "AutoCAD 2010":
                    cadVersion = "R18.0";
                    break;
                case "AutoCAD2011":
                case "AutoCAD 2011":
                    cadVersion = "R18.1";
                    break;
                case "AutoCAD2012":
                case "AutoCAD 2012":
                    cadVersion = "R18.2";
                    break;
                case "AutoCAD2013":
                case "AutoCAD 2013":
                    cadVersion = "R19.0";
                    break;
                case "AutoCAD2014":
                case "AutoCAD 2014":
                    cadVersion = "R19.1";
                    break;
                case "AutoCAD2015":
                case "AutoCAD 2015":
                    cadVersion = "R20.0";
                    break;
                case "AutoCAD2016":
                case "AutoCAD 2016":
                    cadVersion = "R20.1";
                    break;
            }
            if (string.IsNullOrEmpty(cadVersion)) return;

            // 获取HKEY_CURRENT_USER键
            RegistryKey keyCurrentUser = Registry.CurrentUser;
            // 打开AutoCAD所属的注册表键:HKEY_CURRENT_USER\Software\Autodesk\AutoCAD
            RegistryKey keyAutoCAD = keyCurrentUser.OpenSubKey("Software\\Autodesk\\AutoCAD" , true);
            //获得表示当前的AutoCAD版本的注册表键值:R18.2
            keyAutoCAD.SetValue("CurVer", cadVersion);
        }

        /// <summary>
        /// 当前AutoCAD版本名称
        /// </summary>
        /// <returns></returns>
        private void GetCurrentCADProductName()
        {
            //获取当前AutoCAD的注册表键名
            string cadKeyName = GetCurrentCADKeyName();
            //打开HKEY_LOCAL_MACHINE下当前AutoCAD的注册表键以获得版本号
            RegistryKey keyCAD = Registry.LocalMachine.OpenSubKey(cadKeyName);
            currentCadProductName = keyCAD.GetValue("ProductName").ToString();
        }

        /// <summary>
        /// 当前CAD安装路径
        /// </summary>
        private void GetCurrentCADLocation()
        {
            //获取当前AutoCAD的注册表键名
            string cadKeyName = GetCurrentCADKeyName();
            //打开HKEY_LOCAL_MACHINE下当前AutoCAD的注册表键以获得版本号
            RegistryKey keyCAD = Registry.LocalMachine.OpenSubKey(cadKeyName);
            currentCadLocation = keyCAD.GetValue("Location").ToString();
        }

    }
    public class CadInfoClass
    {
        /// <summary>
        /// CAD安装路径
        /// </summary>
        public string Location;
        /// <summary>
        /// CAD版本名称
        /// </summary>
        public string ProductName;
        /// <summary>
        /// 地区标识码
        /// </summary>
        public string LocaleId;
        /// <summary>
        /// CAD版本号
        /// </summary>
        public string Release;
        /// <summary>
        /// 语言
        /// </summary>
        public string Language;
    }
    /// <summary>
    /// AutoCad版本
    /// </summary>
    public enum CadVersion
    {
        Cad2004 = 1,
        Cad2005 = 2,
        Cad2006 = 3,
        Cad2007 = 4,      
        Cad2008 = 5,
        Cad2009 = 6,
        Cad2010_32 = 7,
        Cad2010_64 = 8,
        Cad2011_32 = 9,
        Cad2011_64 = 10,
        Cad2012_32 = 11,
        Cad2012_64 = 12,
        Cad2013_32 = 13,
        Cad2013_64 = 14,
        Cad2014_32 = 15,
        Cad2014_64 = 16,
        Cad2015_32 = 17,
        Cad2015_64 = 18,
        Cad2016_32 = 19,
        Cad2016_64 = 20
    }
}
