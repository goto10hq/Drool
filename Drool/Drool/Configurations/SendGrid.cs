using System.Collections.Specialized;
using Drool.Helpers;
using Newtonsoft.Json;

namespace Drool.Configurations
{
    /// <summary>
    /// SendGrid configuration.
    /// </summary>
    /// <remarks>http://sendgrid.com</remarks>
    public class SendGrid : IConfiguration
    {
        private const string _header = "X-SMTPAPI";

        private class Filters
        {
            [JsonProperty("clicktrack")]
            public ClickTrack ClickTrack { get; set; }

            public Filters(ClickTrack clickTrack)
            {
                ClickTrack = clickTrack;
            }
        }

        private class Settings
        {            
            [JsonProperty("enable")]
            public bool Enable { get; set; }

            public Settings(bool enable)
            {
                Enable = enable;
            }
        }

        private class ClickTrack
        {
            [JsonProperty("settings")]
            public Settings Settings { get; set; }

            public ClickTrack(Settings settings)
            {
                Settings = settings;
            }
        }

        private class Token
        {
            [JsonProperty("category")]
            public string Category { get; set; }

            [JsonProperty("filters")]
            public Filters Filters { get; set; }

            public Token(string category, Filters filters)
            {
                Category = category;
                Filters = filters;
            }
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="category">E-mail category.</param>
        /// <param name="enableClickTracking">Enable click tracking.</param>
        public SendGrid(string category, bool enableClickTracking)
        {            
            var token = new Token(category, new Filters(new ClickTrack(new Settings(enableClickTracking))));
            var output = JsonConvert.SerializeObject(token, new BoolConverter());
            HeaderValues.Add(_header, output);
        }

        /// <summary>
        /// Get header values.
        /// </summary>
        public NameValueCollection HeaderValues { get; } = new NameValueCollection();
    }
}
