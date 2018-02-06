using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FCWriteCD
{
    class IniModule
    {
        public string path;

        //[DllImport("kernel32")]
        //private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        //[DllImport("kernel32")]
        //private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);


        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniModule(string INIPath)
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();

        }

        public string IniReadValueSpecial(string section, string Key)
        {
            bool isFoundSection = false;
            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\setting.ini", Encoding.UTF8))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    else if (line == string.Empty)
                    {
                        continue;
                    }
                    else if (line.Substring(0, 1).Equals("[") && line.EndsWith("]"))
                    {
                        if (isFoundSection)
                        {
                            break;
                        }
                        if (line.Substring(1, line.Length - 2).Equals(section))
                        {
                            isFoundSection = true;
                        }
                        continue;
                    }
                    else
                    {
                        string[] splitWord = line.Split('=');
                        if (splitWord[0].Equals(Key))
                        {
                            return splitWord[1];
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }
            return null;
        }
    }

    public class SettingBean
    {
        public String dataSource { get; set; }
        public String databaseName { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public int databaseTimeout { get; set; }
        public string CDName { get; set; }

        //public int testFlag { get; set; }
    }
}
