using CraniumCafeSync.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraniumCafeSync.Business_Logic
{
    class JsonAppointmentAccess
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

        public List<AppointmentDetails> GetJsonAppointment()
        {
            try
            {
                SetPath();
                string curFile = AppDataFilePath + "AppointmentDetails.json";
                if (File.Exists(curFile))
                {
                    string text = System.IO.File.ReadAllText(curFile);
                    List<AppointmentDetails> deserializedProduct = JsonConvert.DeserializeObject<List<AppointmentDetails>>(text);
                    return deserializedProduct;
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetJasonAppointment:- " + ex.Message);
                return null;
            }
        }

        public void SaveAllAppointment(List<AppointmentDetails> appointmentDetailsList)
        {
            try
            {
                SetPath();
                string json = JsonConvert.SerializeObject(appointmentDetailsList, Formatting.Indented);
                System.IO.File.WriteAllText(AppDataFilePath + "AppointmentDetails.json", json);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SaveAllAppointment " + ex.Message);
            }
        }

        public void SaveAppointmentDetails(AppointmentDetails appointment)
        {
            try
            {
                List<AppointmentDetails> appointmentList = new List<AppointmentDetails>();
                List<AppointmentDetails> existingAppointments = GetJsonAppointment();

                if (existingAppointments != null)
                    appointmentList = existingAppointments;

                appointmentList.Add(appointment);
                SaveAllAppointment(appointmentList);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SaveAppointmentDetails " + ex.Message);
            }
        }

        public string GetAppointmentSubject(string userID, string entryID)
        {
            string subject = "";
            try
            {
                List<AppointmentDetails> existingAppointments = GetJsonAppointment();

                if (existingAppointments != null)
                {
                    var res = existingAppointments.Find(x => x.EntryID == entryID && x.UserID == userID);
                    if (res != null)
                        subject = res.Subject;
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetAppointmentSubject " + ex.Message);
            }

            return subject;
        }

        public void RemoveAppointmentFromJson(string userID, string entryID)
        {
            try
            {
                List<AppointmentDetails> existingAppointments = GetJsonAppointment();

                if (existingAppointments != null)
                {
                    var res = existingAppointments.Find(x => x.EntryID == entryID && x.UserID == userID);
                    if (res != null)
                    {
                        existingAppointments.Remove(res);
                        SaveAllAppointment(existingAppointments);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in RemoveAppointmentFromJason " + ex.Message);
            }
        }

        public void UpdateAppointmentFromJson(AppointmentDetails appointmentDetails)
        {
            try
            {
                List<AppointmentDetails> existingAppointments = GetJsonAppointment();

                if (existingAppointments != null)
                {
                    var res = existingAppointments.Find(x => x.EntryID == appointmentDetails.EntryID && x.UserID == appointmentDetails.UserID);
                    if (res != null)
                    {
                        existingAppointments.Remove(res);
                        existingAppointments.Add(appointmentDetails);
                        SaveAllAppointment(existingAppointments);
                    }
                    else
                    {
                        existingAppointments.Add(appointmentDetails);
                        SaveAllAppointment(existingAppointments);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in UpdateAppointmentFromJson " + ex.Message);
            }
        }

        public AppointmentDetails GetJsonAppointmentByID(string userID, string entryID)
        {
            AppointmentDetails appointment = null;
            try
            {
                List<AppointmentDetails> existingAppointments = GetJsonAppointment();

                if (existingAppointments != null)
                {
                    appointment = existingAppointments.Find(x => x.EntryID == entryID && x.UserID == userID);
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetJsonAppointmentByID " + ex.Message);
            }

            return appointment;
        }
    }
}
