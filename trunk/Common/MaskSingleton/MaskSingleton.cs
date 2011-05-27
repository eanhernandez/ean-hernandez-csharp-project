using System;
using System.Configuration;
using Common.RTMObserver;

// this class helps the real time monitoring system determine whether or not
// it should log a given message, based on the application's log level.  It's 
// a singleton because the data is static and all apps can just read from it.
namespace Common.MaskSingleton
{
    public sealed class MaskSingleton
    {
        static MaskSingleton _instance = null;
        static string _rtmLevel = "" ;
        static readonly object Padlock = new object();
        public bool ShouldThisObserverTakeAction(LoggerObserver l)
        {
            switch (_rtmLevel)
            {
                case "DEBUG":
                    return true;
                case "VERBOSE":
                    return true;
                case "NORMAL":
                    return true;
                default:
                    return false;
            }
        }
        public bool ShouldThisObserverTakeAction(EmailerObserver e)
        {
            switch (_rtmLevel)
            {
                case "DEBUG":
                    return false;
                case "VERBOSE":
                    return true;
                case "NORMAL":
                    return false;
                default:
                    return false;
            }
 
        }
        public bool ShouldThisObserverTakeAction(ScreenPrinterObserver s)
        {
            switch (_rtmLevel)
            {
                case "DEBUG":
                    return true;
                case "VERBOSE":
                    return true;
                case "NORMAL":
                    return false;
                default:
                    return false;
            }
        }
        MaskSingleton(){}
        public static MaskSingleton Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _rtmLevel = Convert.ToString(ConfigurationManager.AppSettings["RTMLevel"]);
                        _instance = new MaskSingleton();
                    }
                    return _instance;
                }
            }
        }
    }
}
