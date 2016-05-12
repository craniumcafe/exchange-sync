using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using System.Reflection;
using System.IO;

namespace CraniumCafeSync.Business_Logic
{
    class Globals
    {
        internal static string RedemptionLoader_DllLocation64Bit_FilePath;
        internal static string RedemptionLoader_DllLocation32Bit_FilePath;
        internal static NameSpace objNS;
        internal static string AppDataFolderPath;
        internal static string AssemblyLocationFolderPath;
        internal static _Application objOutlook;
        public static int c = 1849;

        public static void SetFilePaths()
        {
            try
            {
                AppDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\CraniumCafe";
                Assembly assinf = Assembly.GetExecutingAssembly();
                Uri uriCodeBase = new Uri(assinf.CodeBase);
                AssemblyLocationFolderPath = Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());

                RedemptionLoader_DllLocation64Bit_FilePath = AssemblyLocationFolderPath + "\\Redemption64.dll";
                RedemptionLoader_DllLocation32Bit_FilePath = AssemblyLocationFolderPath + "\\Redemption.dll";

                if (!(Directory.Exists(AppDataFolderPath)))
                {
                    Directory.CreateDirectory(AppDataFolderPath);
                }

            }
            catch (System.Exception ex)
            {
                Debug.DebugMessage(2, "Error in SetFilePaths:- " + ex.Message);
            }
        }
    }
}
