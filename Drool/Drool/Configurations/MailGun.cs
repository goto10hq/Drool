using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Drool.Configurations
{
    /// <summary>
    /// MailGun configuration.
    /// </summary>
    /// <remarks>https://mailgun.com</remarks>

    public class MailGun : IConfiguration
    {
        private const string Header = "X-Mailgun-Variables";

        private class Token
        {
            [JsonProperty("category")]
            public string Category { get; set; }
            
            public Token(string category)
            {
                Category = category;                
            }
        }

        /// /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="category">E-mail category.</param>

        public MailGun(string category)
        {                        
            var token = new Token(category);
            var output = JsonConvert.SerializeObject(token);
            HeaderValues.Add(Header, output);
        }

        /// /// <summary>
        /// Ctor.
        /// </summary>        
        /// <param name="obj">Object to be serialized as a collection of variables.</param>
        public MailGun(object obj)
        {
            var output = JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            HeaderValues.Add(Header, output);
        }

        /// <summary>
        /// Get header values.
        /// </summary>
        public NameValueCollection HeaderValues { get; } = new NameValueCollection();
    }
}
