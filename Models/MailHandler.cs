using System;
using System.IO;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Models
{
    public class MailHandler
    {
        public void Execute(string email, string frist_name, string verifyToken)
        {
            
            var apiKey = File.ReadAllText("/home/SLOCK_User/slock_api_key");
            if (apiKey == null)
            {
                apiKey = Environment.GetEnvironmentVariable("slock_api_key"); 
            }
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