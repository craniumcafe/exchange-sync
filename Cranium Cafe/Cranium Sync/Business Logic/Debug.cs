using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraniumCafeSync.Business_Logic
{
    class Debug
    {
        static System.IO.FileStream myTraceLog;
        static TextWriterTraceListener myListener;
        static BooleanSwitch myBoolSwitch;
        static System.Diagnostics.TraceSwitch myTraceSwitch;
        private static string TracePath = "";
        private static int _DebugLevel = 4;
        private static bool _showMessages = false;
        static bool _debugOn = false;
        public static bool ErrorLogInitialized = true;
        static string _clientGUID;
        static string _controllerType;
        public static long _connectionID;

        public static void InitialiseTrace()
        {
            if (_debugOn) return;

            try
            {
                _DebugLevel = 3;
                string appDatapath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);               
                var myname = Assembly.GetEntryAssembly().GetName().CodeBase;
                var myDirectory = System.IO.Path.GetDirectoryName(myname);
                Uri uri = new Uri(myDirectory);
                TracePath = uri.LocalPath + "\\Logs";
                myBoolSwitch = new BooleanSwitch("myBoolSwitch", "CraniumSync" + " General");
                myTraceSwitch = new System.Diagnostics.TraceSwitch("myTraceSwitch", "CraniumSync" + "2 General");
                if (!Directory.Exists(TracePath)) Directory.CreateDirectory(TracePath);

                myTraceLog = new FileStream(TracePath + "\\" + "CraniumSync" + "-Log (" + DateTime.Now.ToString("yy-MM-dd") + ").txt", FileMode.Append);
                myListener = new TextWriterTraceListener(myTraceLog);
                Trace.AutoFlush = true;
                Trace.Listeners.Add(myListener);
                Trace.WriteLineIf(myTraceSwitch.TraceVerbose, "Initialise Trace");
                System.Diagnostics.Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
                System.Diagnostics.Debug.AutoFlush = true;
                System.Diagnostics.Debug.Indent();
                //DebugMessage(1,  appName + " Debug Initialised");
                ClearOldDebugFiles();
            }
            catch (Exception ex)
            {
                //Can't initialise if trace is already running
                //Debug.DebugMessage(3, "Utils.Debug.InitialiseTrace", "Data", ex.Message, ex.StackTrace);
            }
        }

        public static void DebugMessage(Int16 Level, string action, string errorType, string errorMsg, string stackTrace)
        {
            if (_debugOn) return;

            if (TracePath == "") return;


            if (ErrorLogInitialized == true)
            {
                //ErrorLog Data Object Initialization END

                try
                {
                    switch (Level)
                    {
                        case 1:
                            myListener.WriteLine(DateTime.Now + " -1-sync- " + action + " :  " + errorMsg + " -NewMethod-");
                            if (_showMessages) MessageBox.Show(" -1-sync- " + errorMsg + " -NewMethod-");
                            break;

                        case 2:
                            if (_DebugLevel >= 2)
                            {
                                myListener.WriteLine(DateTime.Now + " -2-sync- " + action + " :  " + errorMsg + " -NewMethod-");
                                if (_showMessages) MessageBox.Show(" -2-sync- " + errorMsg + " -NewMethod-");
                            }
                            break;
                        case 3:
                            if (_DebugLevel >= 3)
                            {
                                myListener.WriteLine(DateTime.Now + " -3-sync- " + action + " :  " + errorMsg + " -NewMethod-");
                                //if (_showMessages) MessageBox.Show(" -3-sync- " + errorMsg + " -NewMethod-");
                            }
                            break;

                        case 4:
                            if (_DebugLevel >= 4)
                            {
                                myListener.WriteLine(DateTime.Now + " -4-sync- " + errorMsg + " -NewMethod-");
                                //if (_showMessages) MessageBox.Show(" -4-sync- " + errorMsg + " -NewMethod-");
                            }
                            break;
                    }
                    myListener.Flush();
                }
                catch (Exception ex)
                {
                    //if Debug.DebugMessage errors we can not write a Debug.DebugMessage or the main program will crash

                    //Debug.DebugMessage(2, "Utils.Debug.DebugMessage.ErrorSave", "Data", ex.Message, ex.StackTrace);
                    //  MessageBox.Show(@"Error writing Trace : " + ex.Message);
                }
            }
            else
            {
                DebugMessage(Level, errorMsg); //Backup Debug Method
            }
        }

        public static void DebugMessage(Int16 Level, string dbMessage)
        {
            if (_debugOn) return;

            if (TracePath == "") return;

            if ((ErrorLogInitialized == false) || (myListener == null)) return;

            try
            {
                switch (Level)
                {
                    case 1:
                        myListener.WriteLine(DateTime.Now + " -1- " + dbMessage);
                        if (_showMessages) MessageBox.Show(" -1- " + dbMessage);
                        break;

                    case 2:
                        if (_DebugLevel >= 2)
                        {
                            myListener.WriteLine(DateTime.Now + " -2- " + dbMessage);
                            if (_showMessages) MessageBox.Show(" -2- " + dbMessage);
                        }
                        break;
                    case 3:
                        if (_DebugLevel >= 3)
                        {
                            myListener.WriteLine(DateTime.Now + " -3- " + dbMessage);
                            //if (_showMessages) MessageBox.Show(" -3- " + dbMessage);
                        }
                        break;

                    case 4:
                        if (_DebugLevel >= 4)
                        {
                            myListener.WriteLine(DateTime.Now + " -4- " + dbMessage);
                            //if (_showMessages) MessageBox.Show(" -4- " + dbMessage);
                        }
                        break;
                }
                myListener.Flush();
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Utils.Debug.DebugMessage.FileSave", "Data", ex.Message, ex.StackTrace);
                //  MessageBox.Show(@"Error writing Trace : " + ex.Message);
            }
        }

        private static bool ClearOldDebugFiles()
        {
            try
            {
                var filesArray = Directory.GetFiles(TracePath);
                Array.Sort(filesArray);
                Debug.DebugMessage(3, filesArray.Length.ToString() + " Log files found");
                foreach (var item in filesArray)
                {
                    string shortFilePath = item.Replace(TracePath, "");
                    int indexOfOpenBrace = shortFilePath.IndexOf("(") + 1;
                    int indexOfCloseBrace = shortFilePath.IndexOf(")");
                    string dateStr = shortFilePath.Substring(indexOfOpenBrace, indexOfCloseBrace - indexOfOpenBrace);
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime datetimeVal = DateTime.ParseExact(dateStr, "yy-MM-dd", provider);
                    if ((DateTime.Now - datetimeVal).Days > 5)
                    {
                        File.Delete(item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Utils.Debug.ClearOldDebugFiles", "Data", ex.Message, ex.StackTrace);
                //DebugMessage(2, "Error in ClearOldDebugFiles : " + ex.Message);
                return false;
            }
        }

        public static void SetDebugLevel(int debuglevel, bool showMessages)
        {
            try
            {
                _DebugLevel = debuglevel;
                _showMessages = showMessages;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Utils.Debug.SetDebugLevel", "Data", ex.Message, ex.StackTrace);
                //DebugMessage(2, "Error in SetDebugLevel : " + ex.Message);
            }
        }

        public static void DebugOn(bool dubugOn)
        {
            _debugOn = dubugOn;
        }

        public static bool DebugOn()
        {
            return _debugOn;
        }

        public static void CloseTrace()
        {
            try
            {
                myTraceLog.Flush();
                myTraceLog.Close();

            }
            catch (Exception)
            {

            }

        }
    }
}
