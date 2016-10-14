using System.Collections.Generic;
using Drool;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var mailer = new Mailer("EmailTemplate/index.html");

            mailer.Send("info@goto10.cz", "frohikey@gmail.com", "Test", new Dictionary<string, object>
                                                                        {
                                                                            { "Salutation", "Hello my lovely robot," },
                                                                            { "Yes", "http://www.goto10.cz" },
                                                                            { "No", "http://www.github.com" }
                                                                        });
        }
    }
}
