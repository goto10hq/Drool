using System.Collections.Specialized;

namespace Drool.Configurations
{
    /// <summary>
    /// Plain SMTP configuration.
    /// </summary>    
    public class PlainSmtp : IConfiguration
    {        
        /// <summary>
        /// Ctor.
        /// </summary>                
        public PlainSmtp()
        {                        
        }

        /// <summary>
        /// Get header values.
        /// </summary>
        public NameValueCollection HeaderValues { get; } = new NameValueCollection();
    }
}
