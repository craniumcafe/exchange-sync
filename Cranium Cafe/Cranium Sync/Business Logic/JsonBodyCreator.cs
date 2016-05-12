using CraniumCafeSync.Models;
using Redemption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CraniumCafeSync.Business_Logic
{
    class JsonBodyCreator
    {
        public string GetEventJson(Appointment appointment)
        {
            try
            {
                string strEventsJson = "{\"event_guid\":\"" + appointment.EntryID + "\"," +
                    "\"start_time\": \"" + appointment.StartTime.ToString("o") + "\", " +
                    "\"end_time\": \"" + appointment.EndTime.ToString("o") + "\", " +
                    "\"all_day_event\": " + FormatBool(appointment.AllDayEvent);
                if (appointment.Subject != null)
                {
                    // sanitize appointment subject string ...
                    string appointmentSubject = Regex.Replace(appointment.Subject, "[^0-9a-zA-Z ]+", "");

                    strEventsJson += ", \"subject\": \"" + appointmentSubject + "\"}";
                }
                else
                {
                    strEventsJson += "}";
                }


                return strEventsJson;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetEventJson:- " + ex.Message);
                return "";
            }
        }

        private string FormatBool(Boolean myBool)
        {
            try
            {
                if (myBool)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in FormatBool:- " + ex.Message);
                return "0";
            }
        }

        public string GetOfficeHourJson(Appointment appointment)
        {
            RDORecurrencePattern rp = null;
            try
            {
                string strOfficeHourJson = "{\"event_guid\": \"" + appointment.EntryID + "\",";

                if (appointment.IsRecurring)
                {
                    rp = appointment.RecurrencePattern;
                    if (rp.RecurrenceType == Redemption.rdoRecurrenceType.olRecursWeekly)
                    {
                        strOfficeHourJson += "\"is_recurring\": true," +
                            "\"week_day_mask\": " + ((int)rp.DayOfWeekMask).ToString() + "," +
                            "\"start_time\": \"" + appointment.StartTime.ToString("HH:mm:00") + "\"," +
                            "\"end_time\": \"" + appointment.EndTime.ToString("HH:mm:00") + "\"," +
                            "\"recurring_start_date\": \"" + rp.PatternStartDate.ToUniversalTime().ToString("yyyy-MM-dd") + "\",";

                        if (rp.NoEndDate)
                        {
                            strOfficeHourJson += "\"recurring_end_date\": \"\"";
                        }
                        else
                        {
                            strOfficeHourJson += "\"recurring_end_date\": \"" + rp.PatternEndDate.ToUniversalTime().ToString("yyyy-MM-dd") + "\"";
                        }
                    }
                    else
                    {
                        strOfficeHourJson += "\"is_recurring\": false," +
                            "\"start_time\": \"" + appointment.StartTime.ToString("HH:mm:00") + "\"," +
                            "\"end_time\": \"" + appointment.EndTime.ToString("HH:mm:00") + "\"," +
                            "\"nonrecurring_date\": \"" + appointment.StartTime.ToString("yyyy-MM-dd") + "\"";
                    }
                }
                else
                {
                    strOfficeHourJson += "\"is_recurring\": false," +
                        "\"start_time\": \"" + appointment.StartTime.ToString("HH:mm:00") + "\"," +
                        "\"end_time\": \"" + appointment.EndTime.ToString("HH:mm:00") + "\"," +
                        "\"nonrecurring_date\": \"" + appointment.StartTime.ToString("yyyy-MM-dd") + "\"";
                }
                strOfficeHourJson += "}";

                return strOfficeHourJson;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetOfficeHourJson:- " + ex.Message);
                return "";
            }
            finally
            {
                if (rp != null) Marshal.ReleaseComObject(rp);
            }
        }

        public string GetMeetingJson(string strMeetingId, Appointment appointment)
        {
            try
            {
                string strMeetingJson = "{\"meeting_id\":\"" + strMeetingId + "\"," +
                               "\"start_time\": \"" + appointment.StartTime.ToString("o") + "\", " +
                               "\"end_time\": \"" + appointment.EndTime.ToString("o") + "}";

                return strMeetingJson;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetMeetingJson:- " + ex.Message);
                return "";
            }

        }

    }
}
