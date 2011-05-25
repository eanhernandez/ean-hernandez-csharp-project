using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Common
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
                        Console.WriteLine("making singleton!");
                        _rtmLevel = Convert.ToString(ConfigurationManager.AppSettings["RTMLevel"]);
                        _instance = new MaskSingleton();
                    }
                    return _instance;
                }
            }
        }
    }
}
