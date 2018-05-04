using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Dtos
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name} {Company} {Title} {Phone} {Avatar}";
        }
    }
}
