using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redemption;
using CraniumCafeSync.Models;
using System.Runtime.InteropServices;

namespace CraniumCafeSync.Business_Logic
{
    public static class RedemptionCode
    {
        internal static RDOSession rSession;
        internal static string myOutlookUserName = "";
        internal static string OutlookVersion = "";
        internal static string RedemptionVersion = "";
        private static CraniumConfig CraniumConfig = new CraniumConfig();
        private static UserDetails UserDetails = new UserDetails();
        private static int CountAdd = 0;
        private static int CountChangd = 0;
        private static int CountDelete = 0;


        //This Function initialize the Redemption

        internal static bool InitialiseRedemption(string userName, string serverName)
        {
            try
            {
                Debug.DebugMessage(3, "Redemption initialization started.");
                var enc = new EnDecrypt();
                string sName = enc.Decrypt(serverName);
                string uName = enc.Decrypt(userName);
                Debug.DebugMessage(3, "Redemption initialization" + sName+ " "+uName);
                if (File.Exists(Globals.RedemptionLoader_DllLocation32Bit_FilePath) && File.Exists(Globals.RedemptionLoader_DllLocation64Bit_FilePath))
                {
                    RedemptionLoader.DllLocation32Bit = Globals.RedemptionLoader_DllLocation32Bit_FilePath;
                    RedemptionLoader.DllLocation64Bit = Globals.RedemptionLoader_DllLocation64Bit_FilePath;
                }
                else
                {
                    Debug.DebugMessage(3, "Redemption files not found.");
                    return false;
                }

                rSession = RedemptionLoader.new_RDOSession();
                //rSession.Logon();
                //rSession.Logon("Outlook");
                rSession.LogonExchangeMailbox(uName, sName);
                Debug.DebugMessage(3, "Redemption initialized successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "***Error Initialising Redemption. :- " + ex.Message);
            }
            return false;
        }

        internal static string StartExchangeSynchonizer(string lastSyncStstus, CraniumConfig craniumConfig, UserDetails ud)
        {
            RDOFolder2 synchronizFolder = null;
            RDOFolderSynchronizer synchronizer = null;
            RDOSyncMessagesCollection messageCollection = null;

            CountAdd = 0;
            CountChangd = 0;
            CountDelete = 0;

            try
            {
                CraniumConfig = craniumConfig;
                UserDetails = ud;

                //synchronizFolder = (RDOFolder2)rSession.GetDefaultFolder(rdoDefaultFolders.olFolderCalendar);               
                var sessionObj = rSession.AddressBook.ResolveName(ud.SMTPAddress);
                string address = sessionObj.Address;
                synchronizFolder = (RDOFolder2)rSession.GetSharedDefaultFolder(address, rdoDefaultFolders.olFolderCalendar);
                synchronizer = synchronizFolder.ExchangeSynchonizer;
                messageCollection = synchronizer.SyncItems(lastSyncStstus, "");

                for (int i = 1; i < messageCollection.Count + 1; i++)
                {
                    RDOSyncMessageItem message = null;
                    try
                    {
                        message = messageCollection.Item(i);
                        Process_SyncItem(ref message);
                    }
                    catch (Exception ex1)
                    {
                        Debug.DebugMessage(2, "Error in StartExchangeSynchonizer:- " + ex1.Message);
                    }
                    finally
                    {
                        if (message != null) Marshal.ReleaseComObject(message);
                    }
                }

                if (CountAdd > 0)
                    Debug.DebugMessage(3, CountAdd + " items added successfully.");

                if (CountChangd > 0)
                    Debug.DebugMessage(3, CountChangd + " items changed successfully.");

                if (CountDelete > 0)
                    Debug.DebugMessage(3, CountDelete + " items deleted successfully.");

                return messageCollection.SyncData;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in StartExchangeSynchonizer:- " + ex.Message);
                return lastSyncStstus;
            }
            finally
            {
                if (messageCollection != null) Marshal.ReleaseComObject(messageCollection);
                if (synchronizer != null) Marshal.ReleaseComObject(synchronizer);
                if (synchronizFolder != null) Marshal.ReleaseComObject(synchronizFolder);
            }
        }

        private static void Process_SyncItem(ref RDOSyncMessageItem Item)
        {
            try
            {
                AppointmentHelper appointmentHelper = new AppointmentHelper(CraniumConfig, UserDetails);
                if (Item.Kind == rdoSyncItemKind.sikChanged)
                {
                    if (Item.IsNewMessage)
                    {
                        // new item 
                        appointmentHelper.Item_Add(Item);
                        CountAdd++;
                    }
                    else
                    {
                        // update item 
                        appointmentHelper.Item_Change(Item);
                        CountChangd++;
                    }
                }
                else if (Item.Kind == rdoSyncItemKind.sikDeleted)
                {
                    // delete item 
                    appointmentHelper.DeletedItem_Add(Item.EntryID);
                    CountDelete++;
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in Process_SyncItem:- " + ex.Message);
            }

        }
    }
}
