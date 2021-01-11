using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NomadAPI.Entities
{
    public class AppUser : IdentityUser<int>
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActive { get; set; } = DateTime.Now;
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public bool IsVerified { get; set; } = false;
        public string Phone { get; set; }
        public string Ocupation { get; set; }
        public string Education { get; set; }
        public string Passionate { get; set; }
        public int NumberOfAds { get; set; } = 0;
        public int NumberOfApplications { get; set; } = 0;
        public string AboutMe { get; set; }
        public City City { get; set; }
        public int? CityId { get; set; }
        public Gender Gender { get; set; }
        public int? GenderId { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<UserReaction> ReactedUsers { get; set; }
        public ICollection<UserReaction> ReactedByUsers { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<Travel> Travels { get; set; }
        public ICollection<Application> Applications { get; set; }
        public ICollection<Report> UsersReports { get; set; }
        public ICollection<Report> UsersReported { get; set; }
        public ICollection<CountryUser> Countries { get; set; }
        public ICollection<LanguageUser> Languages { get; set; }
        public ICollection<Friendship> UsersSentRequest { get; set; }
        public ICollection<Friendship> UsersReceivedRequest { get; set; }


        [NotMapped]
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = $"{FirstName} {LastName}"; }
        }
    }
}
