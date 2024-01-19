using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.Privileges
{
    [Flags]
    public enum UserPrivilege
    {
        [Description("Гость")]
        Guest = 1,
        [Description("Admin")]
        Admin = 16,
    }
}
