using NomadAPI.Entities;
using System;
using System.Collections.Generic;

namespace NomadAPI.Dtos
{
    public class NomadUpdateDto
    {
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Ocupation { get; set; }
        public string Education { get; set; }
        public string Passionate { get; set; }
        public string AboutMe { get; set; }
        public ICollection<CountryUser> Countries { get; set; }
        public ICollection<LanguageUser> Languages { get; set; }

    }
}
