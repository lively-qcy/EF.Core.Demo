using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF.Core.Demo.Data.Base
{
    public class EntityBase: IDelete
    {
        public EntityBase()
        {
            Id = Guid.NewGuid().ToString("N");
            CreationTime = DateTime.Now;
            IsDelete = false;
        }

        /// <summary>
        /// 主键，GUID,去掉- 32字符
        /// </summary>
        [Key]                               //默认ID,Id,id为主键 key可以标识指定
        [MaxLength(32)]
        [Column(TypeName = "char")]
        public string Id { get; set; }

        [MaxLength(50)]
        public string CreatorId { get; set; }
        public DateTime CreationTime { get; set; }

        [Column(TypeName = "bit")]
        public bool IsDelete { get; set; }
    }
}
