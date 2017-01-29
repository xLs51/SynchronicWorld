using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SynchronicWorld.Models
{
    public class Friend
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int FriendId { get; set; }

        public int UserId { get; set; }

        public int HisFriendId { get; set; }
    }

    public class FindFriendModel
    {
        public string FriendSearch { get; set; }

        public string TypeSearch { get; set; }

        public List<User> ListUsers { get; set; }
    }

    public class MyFriendModel
    {
        public List<User> ListUsers { get; set; }
    }

    public class InviteFriendModel
    {
        public List<UserInvited> ListUsersInvited { get; set; }
    }
}