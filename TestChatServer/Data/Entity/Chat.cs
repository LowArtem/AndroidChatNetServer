using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TestChatServer.Data.Entity
{
    public class Chat : Entity
    {
        [Required]
        public string Name { get; set; }
        public string About { get; set; }
        public int Icon { get; set; }

        public List<User> Members { get; set; }
        public List<Message> Messages { get; set; }

        [Required]
        public long CreatorId { get; set; }

        // если чат - диалог, то собеседник тоже обладает функционалом создателя
        // иначе - id = -1
        public long SecondDialogMemberId { get; set; } = -1;

        // The format of string: "{id1} {id2} {id3}". Example: "1 2 13 423"
        public string AdministratorIds { get; set; }

        public Chat()
        {

        }

        public Chat(string name, int icon, List<User> members, List<Message> messages, long creatorId, string administratorIds, string about)
        {
            Name = name;
            Icon = icon;
            Members = members;
            Messages = messages;
            CreatorId = creatorId;
            AdministratorIds = administratorIds;
            About = about;

            SecondDialogMemberId = -1;
        }

        public List<long> GetListOfAdminIds()
        {
            if (this.AdministratorIds == null || this.AdministratorIds.Replace(" ", "") == "") return new List<long>();

            return AdministratorIds.Split(" ").ToList().ConvertAll<long>(delegate(string i) { return long.Parse(i); }).ToList();
        }

        public static string GetStringOfAdminIds(List<long> admins)
        {
            return String.Join(" ", admins.ConvertAll<string>(delegate(long i) { return i.ToString(); })  );
        }
    }
}
