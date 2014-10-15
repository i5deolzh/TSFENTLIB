// ===============================================================================
// 版权    ：Log4Net
// 创建时间：2013-8
// 作者    ：
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

namespace TSF.ENTLIB.Common.Net
{
    using System;
    using System.IO;

    using System.Net.Mail;
    using System.Collections.Generic;

    /// <summary>
    /// 邮件
    /// </summary>
    public class Mail
    {
        private string _smtpHost;
        private int _port = 25;
        private string _username;
        private string _password;

        private string _to;
        private string _from;

        private string _subject;
        private IList<Attachment> _attachments;

        private int _timeout = 1000 * 100;
        private MailPriority _mailPriority = MailPriority.Normal;
        private SmtpAuthentication _authentication = SmtpAuthentication.None;

        /// <summary>
        /// 主机名称或IP地址
        /// </summary>
        public string SmtpHost
        {
            get { return _smtpHost; }
            set { _smtpHost = value; }
        }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// 邮件接受人
        /// </summary>
        public string To
        {
            get { return _to; }
            set { _to = value; }
        }
        /// <summary>
        /// 邮件发送人
        /// </summary>
        public string From
        {
            get { return _from; }
            set { _from = value; }
        }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        /// <summary>
        /// 附件
        /// </summary>
        public IList<Attachment> Attachments
        {
            get { return _attachments; }
            set { _attachments = value; }
        }
        /// <summary>
        /// 超时时间（以秒为单位，默认为100秒）
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = 1000 * value; }
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public MailPriority Priority
        {
            get { return _mailPriority; }
            set { _mailPriority = value; }
        }
        public SmtpAuthentication Authentication
        {
            get { return _authentication; }
            set { _authentication = value; }
        }

        public Mail() { }

        public virtual void SendEmail(string messageBody)
        {
            SmtpClient smtpClient = new SmtpClient();
            if (_smtpHost != null && _smtpHost.Length > 0)
            {
                smtpClient.Host = _smtpHost;
            }
            smtpClient.Port = _port;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            if (_authentication == SmtpAuthentication.Basic)
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(_username, _password);
            }
            else if (_authentication == SmtpAuthentication.Ntlm)
            {
                smtpClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            }

            smtpClient.Timeout = _timeout;

            MailMessage mailMessage = new MailMessage();
            mailMessage.Body = messageBody;
            mailMessage.From = new MailAddress(_from);
            mailMessage.To.Add(_to);
            mailMessage.Subject = _subject;

            mailMessage.Attachments.Clear();
            if (_attachments != null)
            {
                // 附件
                foreach (Attachment attach in _attachments)
                {
                    mailMessage.Attachments.Add(attach);
                }
            }

            mailMessage.Priority = _mailPriority;

            smtpClient.Send(mailMessage);
        }

        public enum SmtpAuthentication
        {
            /// <summary>
            /// No authentication
            /// </summary>
            None,

            /// <summary>
            /// Basic authentication.
            /// </summary>
            /// <remarks>
            /// Requires a username and password to be supplied
            /// </remarks>
            Basic,

            /// <summary>
            /// Integrated authentication
            /// </summary>
            /// <remarks>
            /// Uses the Windows credentials from the current thread or process to authenticate.
            /// </remarks>
            Ntlm
        }
    }
}
