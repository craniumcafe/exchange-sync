using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using CraniumCafeConfig.Business_Logic;

namespace CraniumCafeConfig.Models
{
    class CraniumJsonConfig
    {
        string AppDataFilePath = "";

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
                Debug.DebugMessage(2, "Error in GetCraniumConfig " + ex.Message);
                return null;
            }
        }
    }
}
