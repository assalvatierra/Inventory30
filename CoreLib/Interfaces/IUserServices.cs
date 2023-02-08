using System.Collections.Generic;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Users;
using CoreLib.Models.API;

namespace CoreLib.Inventory.Interfaces
{
    public interface IUserServices
    {
        public List<AppUser> GetUserList();
    }
}
