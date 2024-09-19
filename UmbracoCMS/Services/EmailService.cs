using Azure;
using Azure.Communication.Email;

namespace UmbracoCMS.Services;

public class EmailService
{

    public async Task<bool> SendConfirmationEmailAsync(string email)
    {
        var connectionstring = "endpoint=https://onatrix-acs.europe.communication.azure.com/;accesskey=3FTbHT7iugRTZxIu4OScegDQs5UHv9FH8PT5TSP5ooOBVaFrWhGaJQQJ99AIACULyCpq7IbPAAAAAZCSfexA";
        var emailClient = new EmailClient(connectionstring);

        var emailContent = new EmailContent("Thank you for contacting us!")
        {
            PlainText = $"Hello {email}! \nThank you for contacting Onatrix.\nWe will get in contact with you as soon as possible!",
            Html = $"<p>Hello {email}!</p> <p>Thank you for contacting Onatrix.</p> <p>We will get in contact with you as soon as possible!</p>"
        };

        var emailRecipients = new EmailRecipients(new List<EmailAddress>
        { 
            new EmailAddress(email) 
        });

        var emailMessage = new EmailMessage(
            senderAddress: "donotreply@2d687f62-8c4c-4a0f-af43-d98f39a9af27.azurecomm.net",
            content: emailContent,
            recipients: emailRecipients
        );

        try
        {
            EmailSendOperation emailSendOperation = await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
            return emailSendOperation.HasCompleted;
        }
        catch (Exception) 
        {
            return false;
        }
    }
}
