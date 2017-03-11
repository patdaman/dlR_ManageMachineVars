using CommonUtils.Html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CommonUtils.Email
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Email object </summary>
    ///
    /// <remarks>   Pdelosreyes, 20160825. </remarks>
    ///-------------------------------------------------------------------------------------------------
    /// System.Net.Mail MailMessage() 
    ///     - MailMessage properties are read only  
    ///     -  so if the Signal Email is instantiated with   
    ///     -  all NULLs, the you must specify the rest  
    ///     -  of the required fields 
    /// -------------------------------------------------------------------------------------------------
    /// string <param name="emailTo" />               // Required
    /// string <param name="fromAddress" />           // Required
    /// string <param name="emailSubject" />          // Can be null on send (Really??)
    /// List<string> <param name="bccAddresses" />    // Can be null on send
    /// List<string> <param name="ccAddresses" />     // Can be null on send
    /// string <param name="emailUsername" />         // Required
    /// string <param name="emailPassword" />         // Required
    /// string <param name="emailBody" />             // Can be null on send (Really??)
    /// List<string> <param name="replyToList" />     // Can be null on send
    /// string <param name="hostAddress" />           // Required
    /// int <param name="hostPort" />                 // Defaults to 587
    /// List<string> <param name="imagePaths" />      // Can be null on send
    /// 
    ///-------------------------------------------------------------------------------------------------
    public class EmailMessage : MailMessage
    {
        public EmailObject emailValues { get; set; }

        public string FromAddress { get; set; }
        public string EmailSubject { get; set; }
        public string EmailTo { get; set; }
        public List<string> BccAddresses { get; set; }
        public List<string> CcAddresses { get; set; }
        public string EmailBody { get; set; }
        public List<string> ReplyToList { get; set; }
        public List<string> ImagePaths { get; set; }

        public int HostPort { get; set; }
        public string HostAddress { get; set; }
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
        public SmtpClient client { get; set; }
        public MailMessage message { get; set; }


        public EmailMessage()
        {
            if (emailValues != null)
            {
                FromAddress = emailValues.FromAddress;
                EmailTo = emailValues.EmailTo;
                BccAddresses = emailValues.BccAddresses;
                CcAddresses = emailValues.CcAddresses;
                EmailSubject = emailValues.EmailSubject;
                ReplyToList = emailValues.ReplyToList;
                EmailUsername = emailValues.EmailUsername;
                EmailPassword = emailValues.EmailPassword;
                EmailBody = emailValues.EmailBody;
                ImagePaths = emailValues.ImagePaths;
                HostAddress = emailValues.HostAddress;
                HostPort = emailValues.HostPort;
                HostAddress = emailValues.HostAddress;
            }
            if (BccAddresses == null)
                BccAddresses = new List<string>();
            if (CcAddresses == null)
                CcAddresses = new List<string>();
            if (ReplyToList == null)
                ReplyToList = new List<string>();
            if (ImagePaths == null)
                ImagePaths = new List<string>();
            if (HostPort == 0)
                HostPort = 587;
        }

        public EmailMessage(EmailObject emailValues)
        {
            this.emailValues = emailValues;
        }

        public EmailMessage CreateEmailObject(EmailObject emailValues)
        {
            this.emailValues = emailValues;
            if (emailValues != null)
            {
                FromAddress = emailValues.FromAddress;
                EmailTo = emailValues.EmailTo;
                BccAddresses = emailValues.BccAddresses;
                CcAddresses = emailValues.CcAddresses;
                EmailSubject = emailValues.EmailSubject;
                ReplyToList = emailValues.ReplyToList;
                EmailUsername = emailValues.EmailUsername;
                EmailPassword = emailValues.EmailPassword;
                EmailBody = emailValues.EmailBody;
                ImagePaths = emailValues.ImagePaths;
                HostAddress = emailValues.HostAddress;
                HostPort = emailValues.HostPort;
                HostAddress = emailValues.HostAddress;
            }
            if (BccAddresses == null)
                BccAddresses = new List<string>();
            if (CcAddresses == null)
                CcAddresses = new List<string>();
            if (ReplyToList == null)
                ReplyToList = new List<string>();
            if (ImagePaths == null)
                ImagePaths = new List<string>();
            if (HostPort == 0)
                HostPort = 587;
            return CreateEmailObject();
        }

        public EmailMessage CreateEmailObject()
        {
            message = new MailMessage();
            if (ValidateEmailAddress(EmailTo) && !String.IsNullOrWhiteSpace(EmailTo))
            {
                message.To.Add(new MailAddress(EmailTo));
            }
            if (ValidateEmailAddress(FromAddress) && !String.IsNullOrWhiteSpace(FromAddress))
            {
                message.From = new MailAddress(FromAddress);
            }
            if (BccAddresses != null && BccAddresses.Count > 0)
            {
                foreach (var address in BccAddresses)
                {
                    message.Bcc.Add(new MailAddress(address));
                }
            }
            if ((CcAddresses != null) && (CcAddresses.Count > 0))
            {
                foreach (var address in CcAddresses)
                {
                    message.CC.Add(new MailAddress(address));
                }
            }
            if ((ReplyToList != null) && (ReplyToList.Count > 0))
            {
                foreach (var address in CcAddresses)
                {
                    message.ReplyToList.Add(new MailAddress(address));
                }
            }
            if (!String.IsNullOrWhiteSpace(EmailBody))
            {
                if (ValidateHtml.IsValidHtml(EmailBody))
                {
                    // Create two views: HTML and Plain Text
                    var converter = new HtmlToText();
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(EmailBody, null, "text/html");
                    AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(converter.ConvertHtml(EmailBody), null, "text/plain");
                    List<LinkedResource> resourceList = new List<LinkedResource>();
                    if (ImagePaths != null && ImagePaths.Count != 0)
                    {
                        foreach (var image in ImagePaths)
                        {
                            var imgResource = GetLinkedImageResource(AddImageObject(image));
                            resourceList.Add(imgResource);
                            htmlView.LinkedResources.Add(imgResource);
                        }
                    }
                    message.AlternateViews.Add(plainTextView);
                    message.AlternateViews.Add(htmlView);
                }
                else
                {
                    message.Body = EmailBody;
                }
            }
            if (!String.IsNullOrWhiteSpace(EmailSubject))
            {
                message.Subject = EmailSubject;
            }
            message.IsBodyHtml = true;
            this.client = CreateSmtpClient();
            return this;
        }

        private LinkedResource GetLinkedImageResource(EmbeddedImage imgModel)
        {
            // Make sure the imgModel filename exists in your HTML template:
            //  ie: <img src="cid:SignalLogo" />
            LinkedResource embeddedImage = null;
            MemoryStream ms = new MemoryStream(imgModel.Image);
            {
                embeddedImage = new LinkedResource(ms, "image/" + imgModel.ImageType);
                embeddedImage.ContentType.Name = imgModel.ImageDisplayName + "." + imgModel.ImageType;
                embeddedImage.ContentId = imgModel.ImageDisplayName;
                return embeddedImage;
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            try
            {
                // SMTP connection variables
                SmtpClient client = new SmtpClient();
                client.Port = HostPort;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential(EmailUsername, EmailPassword);
                client.Host = HostAddress;
                client.EnableSsl = true;
                return client;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool ValidateEmailAddress(string email)
        {
            if (!String.IsNullOrWhiteSpace(email))
            {
                try
                {
                    Regex rx = new Regex(
                @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                    return rx.IsMatch(email);
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public EmbeddedImage AddImageObject(string imagePath)
        {
            string displayName = Path.GetFileNameWithoutExtension(imagePath);
            string extention = Path.GetExtension(imagePath).Substring(1);
            Bitmap bitmapImage = new Bitmap(imagePath);
            System.Drawing.Image img = bitmapImage;
            byte[] byteImage = null;
            MemoryStream ms = new MemoryStream();
            {
                img.Save(ms, img.RawFormat);
                byteImage = ms.ToArray();

                return new EmbeddedImage()
                {
                    ImageDisplayName = displayName,
                    ContentId = displayName,
                    ImageUrl = "data:image/" + extention + ";base64," + Convert.ToBase64String(byteImage),
                    ImageType = extention,
                    Image = byteImage
                };
            }
        }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Sends a mail. </summary>
    ///
    /// <remarks>   Pdelosreyes, 20160823. </remarks>
    ///
    /// <param name="message">  The message. </param>
    ///-------------------------------------------------------------------------------------------------

    public void Send()
        {
            try
            {
                this.client.Send(this.message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
