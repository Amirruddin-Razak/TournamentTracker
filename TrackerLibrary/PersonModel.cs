using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    /// <summary>
    /// Track data of each team member
    /// </summary>
    public class PersonModel
    {
        /// <summary>
        /// Store team member first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Store team member last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Track team member full name
        /// </summary>
        public string FullName => $"{LastName} {FirstName}";

        /// <summary>
        /// Store team member email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Store team member phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Store team member id
        /// </summary>
        public int PersonId { get; set; }

    }
}
