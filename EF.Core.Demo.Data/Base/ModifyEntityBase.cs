using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EF.Core.Demo.Data.Base
{
    public class ModifyEntityBase : EntityBase
    {
        public ModifyEntityBase()
        {
            ModifyTime = DateTime.Now;
        }
        [MaxLength(50)]
        public string ModifiedById { get; set; }

        public DateTime? ModifyTime { get; set; }

    }
}
