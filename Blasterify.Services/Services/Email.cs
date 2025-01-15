using HtmlRendererCore.PdfSharp;
using System.Net;
using System.Net.Mail;

namespace Blasterify.Services.Services
{
    public static class Email
    {
        public static void SentEmail(string toEmail, string displayName)
        {
            var fromAddress = new MailAddress("daoblur.business@gmail.com", "Blasterify.MVC");
            var toAddress = new MailAddress(toEmail, displayName);

            const string fromPassword = "tsckluvcqvnncwnl";
            const string subject = "Rent";
            const string body = "Dear Customer,\r\n\r\nWe hope you are well.\r\n\r\nWe noticed that you recently initiated a purchase process in our system, but were unable to complete it. We wanted to make sure you didn't have any problems and offer our help if you need it.\r\n\r\nWe understand that sometimes things don't go as we expect, but we are here to help you. If you have any questions or need assistance with your purchase, please do not hesitate to contact us. Our customer service team is always ready to help you.\r\n\r\nAdditionally, we would like to offer you a **10% discount** on your next purchase as a token of our appreciation for choosing us. Just use the discount code **\"REBUY10\"** at checkout.\r\n\r\nRemember, your satisfaction is our number one priority. We are committed to providing you with the best products and service possible.\r\n\r\nWe hope to see you soon in our store.\r\n\r\nBest regards from Blasterify.MVC.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);
        }

        public static void FinishRent(string toEmail, string displayName, string rentId, List<Models.OrderItem> rentItems, List<string> titleMovies)
        {
            var fromAddress = new MailAddress("daoblur.business@gmail.com", "Blasterify.MVC");
            var toAddress = new MailAddress(toEmail, displayName);

            const string fromPassword = "paivizrdmhorpiqa";
            const string subject = "Rent";


            double totalCost = 0;
            string htmlContent = string.Empty;
            htmlContent += "<html> <body>";

            htmlContent += "<div style='margin: 20px auto; max-width: 600px; padding: 20px; border: 1px solid #ccc; background-color: #FFFFFF; font-family: Arial, sans-serif;'>";
            htmlContent += "<div style='margin-bottom: 20px; text-align: center;'>";
            htmlContent += "<img src='https://firebasestorage.googleapis.com/v0/b/blasterify.appspot.com/o/multimedia%2Fwelcome%2Flogo.png?alt=media&token=0b9919f3-6e25-4016-882c-55594e8747fd' alt='Blasterify Logo' style='max-width: 300px; margin-bottom: 10px;' >";
            htmlContent += "</div>";

            htmlContent += $"<p style='margin: 0;'>Rent-{rentId}</p>";
            htmlContent += "<div style='text-align: center; margin-bottom: 20px;'><h1>Rent Detail</h1></div>";

            htmlContent += "<h3>Client Details:</h3>";
            htmlContent += $"<p>Name: {displayName}</p>";
            htmlContent += $"<p>Email: {toEmail}</p>";

            htmlContent += "<table style='width: 100%; border-collapse: collapse;'>";
            htmlContent += "<thead><tr>";
            htmlContent += "<th style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>Movies</th>";
            htmlContent += "<th style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>Price</th>";
            htmlContent += "</tr><hr/></thead>";

            htmlContent += "<tbody>";

            for (int i = 0; i < rentItems.Count; i++)
            {
                totalCost += rentItems[i].Price;
                htmlContent += "<tr>";
                htmlContent += $"<td style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>{titleMovies[i]}</td>";
                htmlContent += $"<td style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>${rentItems[i].Price}</td>";
                htmlContent += "</tr>";
            }
            htmlContent += "</tbody>";

            htmlContent += "<tfoot><tr>";
            htmlContent += $"<td style='padding: 8px; text-align: right; font-weight: bold;'>Total Cost:</td>";
            htmlContent += $"<td style='padding: 8px; text-align: left; border-top: 1px solid #ddd;'>${totalCost}</td>";

            htmlContent += $"</tr></tfoot>";
            htmlContent += $"</table></div>";

            htmlContent += "</body> </html>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var pdf = PdfGenerator.GeneratePdf(htmlContent, PdfSharpCore.PageSize.A4);
            var result = string.Empty;

            using var stream = new MemoryStream();
            {
                pdf.Save(stream, false);
                result = Convert.ToBase64String(stream.ToArray());
                stream.Position = 0;
            }

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true,
                Attachments = { new Attachment(stream, $"Rent-{rentId}.pdf", "application/pdf") }
            };

            try
            {
                if (message != null && smtp != null)
                {
                    smtp.Send(message!);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending email: {e.Message}");
            }
        }
    }
}
