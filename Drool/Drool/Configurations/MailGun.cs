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
        private const string _headerVariables = "X-Mailgun-Variables";
        private const string _headerTag = "X-Mailgun-Tag";
        private const string _headerTrack = "X-Mailgun-Track";

        /// /// <summary>
        /// Ctor.
        /// </summary>        
        /// <param name="tag">Tag name.</param>
        /// <param name="enableClickTracking">Enable click tracking.</param>
        /// <param name="variables">Object to be serialized as a collection of variables.</param>
        public MailGun(string tag, bool enableClickTracking, object variables = null)
        {            
            HeaderValues.Add(_headerTag, tag);
            HeaderValues.Add(_headerTrack, enableClickTracking ? "yes" : "no");

            if (variables != null)
            {
                var output = JsonConvert.SerializeObject(variables, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                HeaderValues.Add(_headerVariables, output);
            }
        }

        /// <summary>
        /// Get header values.
        /// </summary>
        public NameValueCollection HeaderValues { get; } = new NameValueCollection();
    }
}
