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
    class AppointmentHelper
    {
        private static CraniumConfig CraniumConfig = new CraniumConfig();
        private static UserDetails UserDetails = new UserDetails();

        public AppointmentHelper(CraniumConfig craniumConfig, UserDetails ud)
        {
            CraniumConfig = craniumConfig;
            UserDetails = ud;
        }

        public Appointment GetAppointmentObject(ref RDOSyncMessageItem appointmentItem)
        {
            RDOAppointmentItem rdoappointment = null;
            try
            {
                Appointment appointment = new Appointment();
                rdoappointment = ((RDOAppointmentItem)appointmentItem.Item);
                appointment.EntryID = rdoappointment.EntryID;
                appointment.StartTime = rdoappointment.StartUTC;
                appointment.EndTime = rdoappointment.EndUTC;
                appointment.AllDayEvent = rdoappointment.AllDayEvent;
                appointment.Subject = rdoappointment.Subject;
                appointment.Categories = rdoappointment.Categories;
                appointment.IsRecurring = rdoappointment.IsRecurring;
                appointment.RecurrencePattern = rdoappointment.GetRecurrencePattern();
                appointment.Body = rdoappointment.Body;
                return appointment;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetAppointmentObject:- " + ex.Message);
                return null;
            }
            finally
            {
                if (rdoappointment != null) Marshal.ReleaseComObject(rdoappointment);
            }
        }

        public void Item_Add(RDOSyncMessageItem rdoItem)
        {
            try
            {
                Appointment appointment = GetAppointmentObject(ref rdoItem);
                if (appointment != null)
                {
                    bool bAddEvent = true;
                    CraniumAPIRequest apiRequest = new CraniumAPIRequest(CraniumConfig, UserDetails);

                    if (appointment.Subject != null)
                    {
                        if (Regex.Replace(appointment.Subject, @"\s+", "").ToLower() == "craniumcafeofficehours")
                        {
                            if (!appointment.IsRecurring)
                            {
                                apiRequest.AddOfficeHour(appointment);
                                Debug.DebugMessage(3, "Added non-recurring office hours");
                            }
                            else
                            {
                                RecurrenceItem_Add(appointment);
                                Debug.DebugMessage(3, "Added recurring office hours");
                            }
                            bAddEvent = false;
                        }
                    }

                    if (bAddEvent)
                    {
                        apiRequest.AddEvent(appointment);
                        Debug.DebugMessage(3, "Added event");
                    }

                    // save appointment into json file 
                    int count = GetRecurrenceCount(GetAppointmentObject(ref rdoItem));
                    SaveAppointment(appointment, count);
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in Item_Add:- " + ex.Message);
            }
            finally
            {
                if (rdoItem != null) Marshal.ReleaseComObject(rdoItem);
            }
        }

        public void Item_Change(RDOSyncMessageItem rdoItem)
        {
            try
            {
                Appointment appointment = GetAppointmentObject(ref rdoItem);
                if (appointment != null)
                {
                    bool bUpdateEvent = true;
                    CraniumAPIRequest APIRequest = new CraniumAPIRequest(CraniumConfig, UserDetails);

                    if (appointment.Subject != null)
                    {
                        // get a list of office hours defined in the outlook calendar
                        if (Regex.Replace(appointment.Subject, @"\s+", "").ToLower() == "craniumcafeofficehours")
                        {
                            // compare new old appointments
                            CheckOldAppointments(GetAppointmentObject(ref rdoItem));

                            if (!appointment.IsRecurring)
                            {
                                APIRequest.AddOfficeHour(appointment);
                                Debug.DebugMessage(3, "Changed non-recurring office hours");
                            }
                            else
                            {
                                RecurrenceItem_Add(appointment);
                                Debug.DebugMessage(3, "Changed recurring office hours");
                            }

                            bUpdateEvent = false;
                        }
                    }

                    if (bUpdateEvent)
                    {
                        APIRequest.UpdateEvent(appointment);
                        Debug.DebugMessage(3, "Changed event");
                    }

                    // update appointment json file 
                    int count = GetRecurrenceCount(GetAppointmentObject(ref rdoItem));
                    new JsonAppointmentAccess().UpdateAppointmentFromJson(new AppointmentDetails { EntryID = appointment.EntryID, Subject = appointment.Subject, UserID = UserDetails.UserID, ItemCount = count });
                }

            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in Item_Change:- " + ex.Message);
            }
            finally
            {
                if (rdoItem != null) Marshal.ReleaseComObject(rdoItem);
            }
        }

        public void DeletedItem_Add(string entryID)
        {
            try
            {
                bool isDeleteAll = true;
                JsonAppointmentAccess jsonAppointmentAccess = new JsonAppointmentAccess();
                Appointment appointment = GetAppointmentForDelete(entryID);

                if (appointment != null)
                {
                    //get appointment from json file
                    AppointmentDetails jsonAppointment = jsonAppointmentAccess.GetJsonAppointmentByID(UserDetails.UserID, entryID);
                    if (jsonAppointment != null)
                        appointment.Subject = jsonAppointment.Subject;

                    bool bDeleteEvent = true;
                    CraniumAPIRequest apiRequest = new CraniumAPIRequest(CraniumConfig, UserDetails);

                    if (appointment.Subject != null)
                    {
                        // get a list of office hours defined in the outlook calendar
                        if (Regex.Replace(appointment.Subject, @"\s+", "").ToLower() == "craniumcafeofficehours")
                        {
                            if (jsonAppointment != null)
                            {
                                if (jsonAppointment.ItemCount > 1)
                                {
                                    for (int i = 1; i <= jsonAppointment.ItemCount; i++)
                                    {
                                        try
                                        {
                                            appointment.EntryID = entryID + i;
                                            if (!apiRequest.DeleteOfficeHour(appointment))
                                                isDeleteAll = false;
                                        }
                                        catch (Exception ex1)
                                        {
                                            Debug.DebugMessage(2, "Error in DeletedItem_Add(ex1):- " + ex1.Message);
                                            isDeleteAll = false;
                                        }
                                    }
                                }
                                else if (jsonAppointment.ItemCount == 1)
                                {
                                    if (!apiRequest.DeleteOfficeHour(appointment))
                                        isDeleteAll = false;
                                }

                                Debug.DebugMessage(3, jsonAppointment.ItemCount + " occurrence office hours has deleted");
                            }
                            bDeleteEvent = false;
                        }
                    }

                    if (bDeleteEvent)
                    {
                        if (!apiRequest.DeleteEvent(appointment))
                            isDeleteAll = false;

                        Debug.DebugMessage(3, "Event has deleted");
                    }

                    // remove appointment from json file
                    if (isDeleteAll)
                        jsonAppointmentAccess.RemoveAppointmentFromJson(UserDetails.UserID, entryID);
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in DeletedItem_Add:- " + ex.Message);
            }
        }

        private string GetMeetingId(string messageBody)
        {
            Match match = Regex.Match(messageBody, "\\?m=(.*)$", RegexOptions.Multiline);
            if (match.Captures.Count == 1)
            {
                return match.Captures[0].Value.Trim();
            }
            else
            {
                return "";
            }
        }

        private void RecurrenceItem_Add(Appointment appointment)
        {
            RDORecurrencePattern rp = null;
            int itemCount = 1;
            int item = 0;
            try
            {
                CraniumAPIRequest apiRequest = new CraniumAPIRequest(CraniumConfig, UserDetails);

                rp = appointment.RecurrencePattern;

                if (rp.RecurrenceType == Redemption.rdoRecurrenceType.olRecursDaily ||
                         rp.RecurrenceType == Redemption.rdoRecurrenceType.olRecursMonthly ||
                         rp.RecurrenceType == Redemption.rdoRecurrenceType.olRecursYearly ||
                         rp.RecurrenceType == Redemption.rdoRecurrenceType.olRecursWeekly)
                {

                    //get Occurrences count
                    try
                    { itemCount = rp.Occurrences; }
                    catch (Exception) { }

                    for (int i = 1; i <= rp.Occurrences; i++)
                    {
                        RDOAppointmentItem rdoAppointment = null;
                        try
                        {
                            Appointment recurringAppointment;
                            rdoAppointment = (RDOAppointmentItem)rp.GetOccurence(i);
                            if (rdoAppointment != null)
                            {
                                item++;
                                recurringAppointment = GetRecurringAppointment(ref rdoAppointment, item);
                                if (recurringAppointment != null)
                                    apiRequest.AddOfficeHour(recurringAppointment);
                            }
                            else
                            {
                                // deleted one appointment from entire series  
                                Appointment deleteAppoint = GetAppointmentForDelete(appointment.EntryID);
                                deleteAppoint.EntryID = deleteAppoint.EntryID + i;
                                apiRequest.DeleteOfficeHour(deleteAppoint);
                            }
                        }
                        catch (Exception ex1)
                        {
                            Debug.DebugMessage(2, "Error in RecurrenceItem_Add(ex1):- " + ex1.Message);
                        }
                        finally
                        {
                            if (rdoAppointment != null) Marshal.ReleaseComObject(rdoAppointment);
                        }
                    }

                    Debug.DebugMessage(3, itemCount + " occurrence office hours added or changed");
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in RecurrenceItem_Add:- " + ex.Message);
            }
            finally
            {
                if (rp != null) Marshal.ReleaseComObject(rp);
            }
        }

        public Appointment GetRecurringAppointment(ref RDOAppointmentItem rdoappointment, int index)
        {
            try
            {
                Appointment appointment = new Appointment();
                appointment.EntryID = rdoappointment.EntryID + index;
                appointment.StartTime = rdoappointment.StartUTC;
                appointment.EndTime = rdoappointment.EndUTC;
                appointment.AllDayEvent = rdoappointment.AllDayEvent;
                appointment.Subject = rdoappointment.Subject;
                appointment.Categories = rdoappointment.Categories;
                appointment.IsRecurring = false;
                appointment.RecurrencePattern = null;
                appointment.Body = rdoappointment.Body;
                return appointment;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetRecurringAppointment:- " + ex.Message);
                return null;
            }
        }

        public Appointment GetAppointmentForDelete(string entryID)
        {
            try
            {
                Appointment appointment = new Appointment();
                appointment.EntryID = entryID;
                appointment.Subject = "";
                appointment.Categories = "";
                appointment.IsRecurring = false;
                appointment.RecurrencePattern = null;
                appointment.Body = "";
                return appointment;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetAppointmentForDelete:- " + ex.Message);
                return null;
            }
        }

        private void SaveAppointment(Appointment appointment, int itemCount)
        {
            try
            {
                AppointmentDetails appDetaisl = new AppointmentDetails();
                appDetaisl.EntryID = appointment.EntryID;
                appDetaisl.Subject = appointment.Subject;
                appDetaisl.UserID = UserDetails.UserID;
                appDetaisl.ItemCount = itemCount;
                new JsonAppointmentAccess().SaveAppointmentDetails(appDetaisl);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SaveAppointment:- " + ex.Message);
            }
        }

        private void CheckOldAppointments(Appointment appointment)
        {
            try
            {
                int newItemCount = 1;
                int oldItemCount = 1;

                //get Occurrences count
                if (appointment.IsRecurring)
                {
                    Appointment appoin = appointment;
                    newItemCount = GetRecurrenceCount(appoin);

                    //RDORecurrencePattern rp = null;
                    //try
                    //{ rp = appointment.RecurrencePattern; newItemCount = rp.Occurrences; }
                    //catch (Exception ex1) { Debug.DebugMessage(2, "Error in CheckOldAppointments(ex1):- " + ex1.Message); }
                    //finally { if (rp != null) Marshal.ReleaseComObject(rp); }
                }



                //get oldappointment count
                AppointmentDetails appdetails = new JsonAppointmentAccess().GetJsonAppointmentByID(UserDetails.UserID, appointment.EntryID);
                if (appdetails != null)
                    oldItemCount = appdetails.ItemCount;

                // compare new and old occurences count
                if (newItemCount != oldItemCount)
                {
                    Debug.DebugMessage(3, "Changed office hours occurrence new count is- " + newItemCount + " old counts is- " + oldItemCount);
                    // delete all old appointments
                    if (appdetails != null)
                    {
                        CraniumAPIRequest apiRequest = new CraniumAPIRequest(CraniumConfig, UserDetails);
                        if (Regex.Replace(appdetails.Subject, @"\s+", "").ToLower() == "craniumcafeofficehours")
                        {
                            if (oldItemCount > 1)
                            {
                                for (int i = 1; i <= oldItemCount; i++)
                                {
                                    try
                                    {
                                        Appointment deleteAppoint = GetAppointmentForDelete(appointment.EntryID);
                                        deleteAppoint.EntryID = deleteAppoint.EntryID + i;
                                        apiRequest.DeleteOfficeHour(deleteAppoint);
                                    }
                                    catch (Exception ex2) { Debug.DebugMessage(2, "Error in CheckOldAppointments(ex2):- " + ex2.Message); }
                                }
                            }
                            else if (oldItemCount == 1)
                            {
                                Appointment deleteAppoint = GetAppointmentForDelete(appointment.EntryID);
                                apiRequest.DeleteOfficeHour(deleteAppoint);
                            }
                        }
                        else
                        {
                            Appointment deleteAppoint = GetAppointmentForDelete(appointment.EntryID);
                            apiRequest.DeleteEvent(deleteAppoint);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in CheckOldAppointments:- " + ex.Message);
            }

        }

        private int GetRecurrenceCount(Appointment appointment)
        {
            int count = 0;
            RDORecurrencePattern rp = null;
            try
            {
                if (appointment.IsRecurring)
                {
                    rp = appointment.RecurrencePattern;

                    for (int i = 1; i <= rp.Occurrences; i++)
                    {
                        RDOAppointmentItem rdoAppointment = null;
                        try
                        {
                            rdoAppointment = (RDOAppointmentItem)rp.GetOccurence(i);
                            if (rdoAppointment != null)
                                count++;
                        }
                        catch (Exception ex1) { Debug.DebugMessage(2, "Error in GetRecurrenceCount(ex1):- " + ex1.Message); }
                        finally { if (rdoAppointment != null) Marshal.ReleaseComObject(rdoAppointment); }
                    }
                }
                else
                {
                    count = 1;
                }
            }
            catch (Exception ex) { Debug.DebugMessage(2, "Error in GetRecurrenceCount:- " + ex.Message); }
            finally { if (rp != null) Marshal.ReleaseComObject(rp); }
            return count;
        }
    }
}
