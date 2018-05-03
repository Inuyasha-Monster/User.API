using System.Collections.Generic;

namespace Contact.API.Data
{
    /// <summary>
    /// 通讯录
    /// </summary>
    public class ContactBook
    {
        public int UserId { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}