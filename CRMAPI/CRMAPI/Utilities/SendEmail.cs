using System.Net;
using System.Net.Mail;

namespace CRMAPI.Utilities
{
    public class SendEmail
    {
        public static async void SendEmailToNewOperador(string email, string token)
        {
            string smtpServer = "smtp.live.com";
            int smtpPort = 465; //or 25 for live.com

            bool enableSsl = true; 
            string fromEmail = "";
            string fromPass = "";

            string toEmail = email;

            string subject = "Bem vindo ao CRM";
            string body = "Olá, seja bem vindo ao CRM. Para finalizar seu cadastro, clique no link abaixo: https://localhost:5001/Operador/ConfirmarCadastro?token=" + token;

            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.EnableSsl = enableSsl;
                smtpClient.Credentials = new NetworkCredential(fromEmail, fromPass);

                MailMessage mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
                //System.Net.Mail.SmtpException: 'Syntax error, command unrecognized. The server response was: '
                //await smtpClient.SendMailAsync(mailMessage);
                /*
                    System.Net.Mail.SmtpException: 'Failure sending mail.'

                    ExtendedSocketException: Uma tentativa de conexão falhou porque o componente conectado não respondeu
                    corretamente após um período de tempo ou a conexão estabelecida falhou
                    porque o host conectado não respondeu. [::ffff:204.79.197.212]:587
                 */
            }
        }
    }
}
