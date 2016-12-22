using System.Collections.Generic;
using Drool;
using Drool.Configurations;
using Microsoft.Azure;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var from = CloudConfigurationManager.GetSetting("EmailFrom");
            var to = CloudConfigurationManager.GetSetting("EmailFrom");

            var mailer = new Mailer("EmailTemplate/index.html", new SendGridConfiguration("Drool", false));

            mailer.Send(from, to, "Test", new Dictionary<string, object>
                                                                        {
                                                                            { "Salutation", "Hello my lovely robot," },
                                                                            { "Yes", "http://www.goto10.cz" },
                                                                            { "No", "http://www.github.com" }
                                                                        });
        }
    }
}
