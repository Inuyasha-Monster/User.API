namespace Contact.API.Dtos
{
    public class UserIdentity
    {
        public int CurrentUserId { get; set; }

        // 以下属性包含在token-claim中
        public string Name { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
    }
}
