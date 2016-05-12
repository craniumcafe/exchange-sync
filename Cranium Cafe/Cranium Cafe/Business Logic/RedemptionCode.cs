using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redemption;

using System.Runtime.InteropServices;

namespace CraniumCafeConfig.Business_Logic
{
    public static class RedemptionCode
    {
        internal static RDOSession rSession;
        internal static string myOutlookUserName = "";
        internal static string OutlookVersion = "";
        internal static string RedemptionVersion = "";


        //This Function initialize the Redemption

        internal static bool InitialiseRedemption(string user, string serverName)
        {
            try
            {
                if (File.Exists(Globals.RedemptionLoader_DllLocation32Bit_FilePath) && File.Exists(Globals.RedemptionLoader_DllLocation64Bit_FilePath))
                {
                    RedemptionLoader.DllLocation32Bit = Globals.RedemptionLoader_DllLocation32Bit_FilePath;
                    RedemptionLoader.DllLocation64Bit = Globals.RedemptionLoader_DllLocation64Bit_FilePath;
                }
                else
                {
                    return false;
                }

                rSession = RedemptionLoader.new_RDOSession();
                //rSession.Logon();
                rSession.LogonExchangeMailbox(user, serverName);
                Debug.DebugMessage(3, "Redemption initialized successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "***Error Initialising Redemption. :- " + ex.Message);
            }
            return false;
        }    
    }
}
