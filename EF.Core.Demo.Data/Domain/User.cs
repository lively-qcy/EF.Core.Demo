using EF.Core.Demo.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Demo.Data.Domain
{
    [Table("Users")]
    public class User : ModifyEntityBase
    {
        [Required]
        [MaxLength(4)]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        public string Account { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        public int Age { get; set; }

        /// <summary>
        /// 性别 1：男  0：女
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(11)]
        [Column(TypeName = "char")]
        public string MobileNo { get; set; }

        [MaxLength(30)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        /// <summary>
        /// 启用状态  
        /// A：Active     启用
        /// D：Deactive   停用
        /// </summary>
        [Required]
        [MaxLength(1)]
        [Column(TypeName = "char")]
        public string Status { get; set; }

        /// <summary>
        /// 是否是管理员(系统级别)
        /// </summary>
        [Column(TypeName = "bit")]
        public bool IsAdmin { get; set; }

        [MaxLength(100)]
        public string HeadImgUrl { get; set; }
    }
}
