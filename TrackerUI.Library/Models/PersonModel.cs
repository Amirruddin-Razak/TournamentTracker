using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerUI.Library.Models;

public class PersonModel : IDataModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{ FirstName } { LastName }";
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }

    public PersonModel()
    {

    }

    public PersonModel(TrackerLibrary.Models.PersonModel model)
    {
        Id = model.Id;
        FirstName = model.FirstName;
        LastName = model.LastName;
        EmailAddress = model.EmailAddress;
        PhoneNumber = model.PhoneNumber;
    }
}
