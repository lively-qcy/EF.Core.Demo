using System;
using System.Collections.Generic;
using System.Text;

namespace EF.Core.Demo.Service
{
    public class UserAppService : IUserAppService
    {
        public string GetName()
        {
            return "xiaoqiu";
        }
    }
}
