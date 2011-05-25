using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Common
{
    public sealed class MaskSingleton
    {
        static MaskSingleton _instance = null;
        static string RtmLevel = "" ;
        static readonly object Padlock = new object();
        public bool ShouldThisObserverTakeAction(LoggerObserver l)
        {
            switch (RtmLevel)
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
            switch (RtmLevel)
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
            switch (RtmLevel)
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
                        RtmLevel = Convert.ToString(ConfigurationManager.AppSettings["RTMLevel"]);
                        _instance = new MaskSingleton();
                    }
                    return _instance;
                }
            }
        }
    }
}
