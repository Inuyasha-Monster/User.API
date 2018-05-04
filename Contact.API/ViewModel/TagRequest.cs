namespace Contact.API.ViewModel
{
    public class TagRequest
    {
        public TagRequest()
        {
            Tags = new string[] { };
        }
        public int FriendUserId { get; set; }
        public string[] Tags { get; set; }
    }
}