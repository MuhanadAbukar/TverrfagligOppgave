namespace BusinessClasses
{
    public class WebhookPost
    {
        public Embed[] embeds { get; set; }
        public class Embed
        {
            public string title { get; set; }
            public string description { get; set; }
        }
    }
    public class Text
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public Paragraphs[] Paragraph { get; set; }
    }
    
    public class Paragraphs
    {
        public string Text { get; set; }
    }
    public enum PermissionType
    {
        Create = 1,
        Delete = 2,
        All = 3,
        Edit = 4
    }

}
