using EF.Core.Demo.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Demo.Data.Domain
{
    [Table("Sys_Role")]
    public class Role : EntityBase
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        [Column(TypeName = "bit")]
        public bool IsAdmin { get; set; }
    }
}
