namespace Drool.Tokens
{
    public class EmailImage
    {
        public string OriginalNameWithFullPath { get; }
        public string OriginalNameWithPath { get; }
        public string OriginalName { get; }
        public string ImageNameInHtml { get; }

        public EmailImage(string originalNameWithFullPath, string originalNameWithPath, string originalName, string imageNameInHtml)
        {
            OriginalNameWithFullPath = originalNameWithFullPath;
            OriginalNameWithPath = originalNameWithPath;
            OriginalName = originalName;
            ImageNameInHtml = imageNameInHtml;
        }
    }
}
