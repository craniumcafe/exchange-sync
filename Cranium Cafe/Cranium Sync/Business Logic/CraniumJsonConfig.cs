using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using CraniumCafeSync.Models;

namespace CraniumCafeSync.Business_Logic
{
    class CraniumJsonConfig
    {
        string AppDataFilePath = "";

        private void SetPath()
        {
            try
            {
                AppDataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CraniumCafe\";
                if (!Directory.Exists(AppDataFilePath)) Directory.CreateDirectory(AppDataFilePath);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SetPath " + ex.Message);
            }
        }

        public CraniumConfig GetCraniumConfig()
        {
            try
            {
                SetPath();
                string curFile = AppDataFilePath + "CraniumConfig.json";
                if (File.Exists(curFile))
                {
                    string text = System.IO.File.ReadAllText(curFile);
                    CraniumConfig deserializedProduct = JsonConvert.DeserializeObject<CraniumConfig>(text);
                    return deserializedProduct;
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetCraniumConfig:- " + ex.Message);
                return null;
            }
        }

        public void SaveCraniumConfig(CraniumConfig craniumConfig)
        {
            try
            {
                SetPath();
                string json = JsonConvert.SerializeObject(craniumConfig, Formatting.Indented);
                System.IO.File.WriteAllText(AppDataFilePath + "CraniumConfig.json", json);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SaveCraniumConfig " + ex.Message);
            }
        }

        private string GetPathOfSynStatus(string userName)
        {
            try
            {
                string AppDataFilePathUserData = "";
                AppDataFilePathUserData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CraniumCafe\";
                if (!Directory.Exists(AppDataFilePathUserData)) Directory.CreateDirectory(AppDataFilePathUserData);

                AppDataFilePathUserData = AppDataFilePathUserData + @"\UserData\";
                if (!Directory.Exists(AppDataFilePathUserData)) Directory.CreateDirectory(AppDataFilePathUserData);

                if (!Directory.Exists(AppDataFilePathUserData + userName)) Directory.CreateDirectory(AppDataFilePathUserData + userName);

                return AppDataFilePathUserData + userName;
            }
            catch (Exception ex)
            {
               Debug.DebugMessage(2, "Error in GetPathOfSynStatus " + ex.Message);
                return null;
            }
        }

        public SyncStatus GetSyncStatus(string userName)
        {
            try
            {
                string curFile = GetPathOfSynStatus(userName) + "\\SyncStatus.json";
                if (File.Exists(curFile))
                {
                    string text = System.IO.File.ReadAllText(curFile);
                    SyncStatus deserializedProduct = JsonConvert.DeserializeObject<SyncStatus>(text);
                    return deserializedProduct;
                }
                else
                {
                    SyncStatus status = new SyncStatus();
                    status.LstSyncStatus = "";
                    return status;
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetSyncStatus " + ex.Message);
                return null;
            }
        }

        public void SaveLastSyncStatus(SyncStatus lastSyncStatus, string userName)
        {
            try
            {
                string path = GetPathOfSynStatus(userName);
                string json = JsonConvert.SerializeObject(lastSyncStatus, Formatting.Indented);
                System.IO.File.WriteAllText(path + "\\SyncStatus.json", json);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SaveLastSyncStatus " + ex.Message);
            }
        }
    }
}
