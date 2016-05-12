using CraniumCafeSync.Business_Logic;
using CraniumCafeSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraniumCafeSync
{
    class AppContext : ApplicationContext
    {
        public AppContext()
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("CraniumSync");
            if (processes.Length <= 1)
            {
                Debug.InitialiseTrace();
                Globals.SetFilePaths();
                ProcessUsers();
            }
            else
            {
                //close the executable
                Environment.Exit(0);
            }
        }

        private void ProcessUsers()
        {
            try
            {
                Debug.DebugMessage(3, "START TO PROCESS USERS");

                CraniumConfig craniumConfig = new CraniumConfigAccess().GetCraniumConfig();

                if (craniumConfig != null)
                {
                    Debug.DebugMessage(3, "Found " + craniumConfig.UserDetailsList.Count + " users from config file.");

                    if (RedemptionCode.InitialiseRedemption(craniumConfig.AgentUsername, craniumConfig.ExcServerName))
                    {
                        foreach (UserDetails ud in craniumConfig.UserDetailsList)
                        {
                            try
                            {
                                Debug.DebugMessage(3, "START---------  " + ud.SMTPAddress + "'s sync process");

                                SyncStatus lastSyncStatus = new CraniumConfigAccess().GetSyncStatus(ud.SMTPAddress);
                                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                                

                                var syncData = RedemptionCode.StartExchangeSynchonizer(lastSyncStatus.LstSyncStatus, craniumConfig, ud);
                                SaveLastSyncData(syncData, ud);

                                Debug.DebugMessage(3, "END--------- " + ud.SMTPAddress + "'s sync process");                                
                            }
                            catch (Exception ex1)
                            {
                                Debug.DebugMessage(2, "Error in ProcessUsers:- " + ex1.Message);
                            }
                        }
                    }
                }
                else
                {
                    Debug.DebugMessage(3, "Can not find the craniumcafe config file!");
                }

                Debug.DebugMessage(3, "END TO PROCESS USERS \r\n");
                //close the executable
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in ProcessUsers:- " + ex.Message);
                //close the executable
                Environment.Exit(0);
            }

            Environment.Exit(0);
        }

        private void SaveLastSyncData(string lastSyncData, UserDetails ud)
        {
            try
            {
                SyncStatus status = new SyncStatus();
                status.LstSyncStatus = lastSyncData;
                status.LstSyncTime = DateTime.Now.ToString(); ;
                status.UserID = ud.UserID;

                new CraniumConfigAccess().SaveLastSyncStatus(status, ud.SMTPAddress);

                Debug.DebugMessage(3, "Last sync time saved sucessfully.");
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SaveLastSyncData:- " + ex.Message);
            }
        }


    }
}
