using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FCWriteCDSendEmail
{
    class DirectoryManagement
    {

        public DirectoryManagement()
        {

        }

        public void CreateTempDirectory()
        {
            if (Directory.Exists(@"C:\fc_write_cd_temp"))
            {
                return;
            }

            Directory.CreateDirectory(@"C:\fc_write_cd_temp");
        }

        public void DeleteTempDirectory()
        {
            Directory.Delete(@"C:\fc_write_cd_temp", true);
        }

    }
}
