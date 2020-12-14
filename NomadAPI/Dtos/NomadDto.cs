using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NomadAPI.Dtos
{
    public class NomadDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public bool IsVerified { get; set; }
        public string Phone { get; set; }
        public string Ocupation { get; set; }
        public string Education { get; set; }
        public string Passionate { get; set; }
        public int NumberOfAds { get; set; }
        public int NumberOfApplications { get; set; }
        public string AboutMe { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }

    }
}
