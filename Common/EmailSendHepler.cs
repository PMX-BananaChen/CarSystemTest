using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace CarSystemTest.Common
{
    public  class EmailSendHepler
    {
        #region Transmission attribute

        private MailAddress mailfrom;
        /// <summary>
        /// Transmits the person address 
        /// </summary>
        public string MailFrom
        {
            set
            {
                this.mailfrom = new MailAddress(value);
            }
        }

        private string[] mailto;
        /// <summary>
        /// Addressee address 
        /// </summary>
        public string MailTo
        {
            set
            {
                //remove empty items.--modified on 2010-02-06,by mingliang.qu
                this.mailto = value.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
            get
            {
                string result = string.Empty;
                for (int i = 0; i < mailto.Length; i++)
                {
                    result += mailto[i];
                    if (i != mailto.Length - 1)
                    {
                        result += ",";
                    }
                }
                return result;
            }
        }


        private string subject;
        /// <summary>
        /// Mail subject 
        /// </summary>
        public string Subject
        {
            set
            {
                this.subject = value;
            }
        }

        private string boby;
        /// <summary>
        /// Mail main text 
        /// </summary>
        public string Body
        {
            set
            {
                this.boby = value;
            }
        }

        private string[] mailcc;
        /// <summary>
        /// Sends duplicate the address 
        /// </summary>
        public string MailCc
        {
            set
            {
                //remove empty items.--modified on 2010-02-06,by mingliang.qu
                this.mailcc = value.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries); // modified by WangJunwei on 2009-05-11 to support comma & semi-colon delimiter.
            }
        }

        private Attachment annex;
        /// <summary>
        /// Appendix 
        /// </summary>
        public string Annex
        {
            set
            {
                this.annex = new Attachment(value);
            }
        }

        private string smtpclient;
        /// <summary>
        /// Mailbox server name 
        /// </summary>
        public string SmtpClient
        {
            set
            {
                this.smtpclient = value;
            }
        }

        private bool validate;
        /// <summary>
        /// Whether the server does need to confirm 
        /// </summary>
        public bool Validate
        {
            set
            {
                this.validate = value;
            }
        }

        private string username;
        /// <summary>
        /// Confirmation user
        /// </summary>
        public string UserName
        {
            set
            {
                this.username = value;
            }
        }

        private string password;
        /// <summary>
        /// Confirmation password 
        /// </summary>
        public string Password
        {
            set
            {
                this.password = value;
            }
        }

        private MailPriority priority = MailPriority.High;
        /// <summary>
        /// Priority 
        /// </summary>
        public MailPriority Priority
        {
            set
            {
                this.priority = value;
            }
        }

        #endregion

        #region  Transmission mail
        /// <summary>
        /// Transmission mail 
        /// </summary>
        /// <returns></returns>
        public bool SendMail()
        {
            SmtpClient client = new SmtpClient(smtpclient);
            client.UseDefaultCredentials = false;
            if (password == null)
            {
                password = "";
            }
            client.Credentials = new System.Net.NetworkCredential(mailfrom.ToString().Split('@')[0].ToString(), password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //if (validate)
            //{
            //    client.UseDefaultCredentials = true;//Establishes for the transmission authentication news  
            //    client.Credentials = new System.Net.NetworkCredential(username, password);//Authentication news  
            //}

            MailMessage message = new MailMessage();
            message.From = mailfrom;  //Mail addresser 
            //message.To.Add(mailto); //Mail addressee
            if (mailto != null)
            {
                foreach (string item in mailto)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        message.To.Add(item.ToString());//Mail addressee
                    }
                }
            }

            if (mailcc != null)
            {
                foreach (string item in mailcc)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        message.CC.Add(item.ToString());//The mail sends duplicate the address 
                    }
                }
            }

            message.Subject = subject.Replace("\r", "").Replace("\n", ""); //Mail title 
            message.Body = boby;  //Mail content 

            if (annex != null)
            {
                message.Attachments.Add(annex);  //Increase appendix 
            }
            message.SubjectEncoding = Encoding.GetEncoding("UTF-8");
            message.BodyEncoding = Encoding.GetEncoding("UTF-8");
            message.IsBodyHtml = true;     //The establishment is the HTML form 
            message.Priority = priority;    //Priority 

            try
            {
                client.Send(message);
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
