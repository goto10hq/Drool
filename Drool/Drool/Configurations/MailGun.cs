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
        private const string HeaderVariables = "X-Mailgun-Variables";
        private const string HeaderTag = "X-Mailgun-Tag";
        private const string HeaderTrack = "X-Mailgun-Track";

        /// /// <summary>
        /// Ctor.
        /// </summary>        
        /// <param name="tag">Tag name.</param>
        /// <param name="enableClickTracking">Enable click tracking.</param>
        /// <param name="variables">Object to be serialized as a collection of variables.</param>
        public MailGun(string tag, bool enableClickTracking, object variables = null)
        {            
            HeaderValues.Add(HeaderTag, tag);
            HeaderValues.Add(HeaderTrack, enableClickTracking ? "yes" : "no");

            if (variables != null)
            {
                var output = JsonConvert.SerializeObject(variables, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                HeaderValues.Add(HeaderVariables, output);
            }
        }

        /// <summary>
        /// Get header values.
        /// </summary>
        public NameValueCollection HeaderValues { get; } = new NameValueCollection();
    }
}
