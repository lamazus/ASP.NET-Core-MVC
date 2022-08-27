using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace WebUI.Services
{
    public class EmailSender
    {
        public void SendReceipt(string name, string email, int order)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Beauty Shop", "beautyshop@myrambler.ru"));
            msg.To.Add(new MailboxAddress(name, email));
            msg.Subject = "Подтверждение об оплате";

            msg.Body = new TextPart("plain")
            {
                Text = $"Здравствуйте, {name}!\n" +
                $"Ваш заказ с номером {order} успешно оплачен. В день ожидаемой достваки с вами свяжутся за час до приезда.\n" +
                $"\n" +
                $"С уважением,\n" +
                $"Beauty Shop. "
            };

            using(var client = new SmtpClient())
            {
                client.Connect("smtp.rambler.ru", 465, true);
                client.Authenticate("beautyshop@myrambler.ru", "TZyB6SMgkEpFW3V3MR");
                client.Send(msg);
                client.Disconnect(true);
            }
        }
    }
}
