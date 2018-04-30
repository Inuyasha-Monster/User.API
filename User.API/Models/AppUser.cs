using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class AppUser
    {
        public AppUser()
        {
            Properties = new List<AppUserProperty>();
        }

        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 公司职位
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 性别 1:男 0:女
        /// </summary>
        public byte Gender { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        public int ProvinceId { get; set; }
        public string Province { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string NameCard { get; set; }
        public List<AppUserProperty> Properties { get; set; }
    }
}
