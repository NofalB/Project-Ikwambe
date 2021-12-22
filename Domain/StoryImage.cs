namespace Domain
{
    public class StoryImage
    {
        public string Title { get; set; }

        public string ImageLink { get; set; }

        public StoryImage(string title, string imageLink)
        {
            Title = title;
            ImageLink = imageLink;
        }
    }
}