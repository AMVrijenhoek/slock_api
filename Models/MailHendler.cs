using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Models
{
    public class MailHendler
    {
        public async Task Execute(string email, string frist_name, string verifyToken)
        {
            Environment.SetEnvironmentVariable("apiKeySendGrid",
                "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");
            
            var apiKey = Environment.GetEnvironmentVariable("apiKeySendGrid");
            var client = new SendGridClient(apiKey);
            var emailMessage = new SendGridMessage();
            emailMessage.SetFrom("no_reply@slock.nl", "SLOCK");
            emailMessage.AddTo(email, frist_name);
            emailMessage.SetTemplateId("d-fb3aaa7fd40247219195393ce7459ae1");
            emailMessage.SetTemplateData(new dynamicTemplateData
            {
                email = email,
                first_name = frist_name,
                verify_token = verifyToken
            });
            
            var response = client.SendEmailAsync(emailMessage).Result;
        }
        public class dynamicTemplateData
        {
            [JsonProperty("email")]
            public string email { get; set; }
            
            [JsonProperty("first_name")]
            public string first_name { get; set; }
            
            [JsonProperty("verify_token")]
            public string verify_token { get; set; }
        }
    }
}