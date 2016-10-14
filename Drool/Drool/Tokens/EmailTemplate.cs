using System.Collections.Generic;

namespace Drool.Tokens
{
    public class EmailTemplate
    {        
        public IEnumerable<EmailImage> Images { get; set; }
        public string Content { get; set; }

        public EmailTemplate(string content, IEnumerable<EmailImage> images)
        {
            Content = content;
            Images = images;            
        }
    }
}
