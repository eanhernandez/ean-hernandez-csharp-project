using System;
using System.Collections.Generic;

namespace Common.RTMObserver
{
    // this concrete observer provides the RTM system with a means to log data to an
    // email and send it out.  Data is aggregated and then send once a threshold of new
    // data is accumulated.  The actually emailing is disabled due to issues connecting
    // to the university's smtp server.  
    public class EmailerObserver : IObserver 
    {
        public EmailerObserver()
        {
            _name = "Emailer";
            _message = new List<string>();
        }
        public void Update(DataGatherer t)
        {
            if (MaskSingleton.MaskSingleton.Instance.ShouldThisObserverTakeAction(this))
            {
                _message.Add(t.GetMessage() + " from " + t.Name);
                if (_message.Count > 20)
                {
                    try
                    {

                        // this is commented out because I don't want to actually send
                        // emails when running this, just emulate.  Also, there seems to
                        // be some problem connecting to the school's smtp server.

                        //System.Web.Mail.MailMessage message = new System.Web.Mail.MailMessage();

                        //message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", 1);
                        //message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "eanh");
                        //message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "xxxxxx");

                        //message.From = "eanh@uchicago.edu";
                        //message.To = "e@eanh.net";
                        //message.Subject = "UC Trading";
                        //message.Body = "Message Body";

                        //System.Web.Mail.SmtpMail.SmtpServer = "smtpauth.uchicago.edu";
                        Console.WriteLine("would send email notification as follows:");    
                        foreach (string s in _message)
                        {
                            Console.WriteLine(s);
                        }
                        
                        _message.Clear();
                        //System.Web.Mail.SmtpMail.Send(message);
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
        public string GetName()
        {
            return _name;
        }
        private readonly string _name;
        private List<string> _message;    // saving up messages to send one big one
    }
}
