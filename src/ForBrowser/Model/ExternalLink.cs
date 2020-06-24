namespace ForBrowser.Model
{
    public class ExternalLink
    {
        public ExternalLink(string text, string url)
        {
            Text = text;
            Url = url;
        }

        public string Text { get; set; }

        public string Url { get; set; }
    }
}
