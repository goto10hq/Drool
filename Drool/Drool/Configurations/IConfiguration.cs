using System.Collections.Specialized;

namespace Drool.Configurations
{
    public interface IConfiguration
    {
        NameValueCollection HeaderValues { get;  }
    }
}
