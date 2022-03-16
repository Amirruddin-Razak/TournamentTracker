using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.Helpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PersonFileName = "PersonModels.csv";
        private const string PrizeFileName = "PrizeModels.csv";


        public PersonModel CreatePerson(PersonModel person)
        {
            List<PersonModel> people = PersonFileName.FullFilePath().LoadFile().ConvertTextToPersonModel();

            int currentId = 1;
            if (people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            person.Id = currentId;
            people.Add(person);

            people.SaveToPersonFile(PersonFileName);

            return person;
        }

        /// <summary>
        /// Save new prize to text
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizeFileName.FullFilePath().LoadFile().ConvertTextToPrizeModel();

            int currentId = 1;
            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            prizes.Add(model);

            prizes.SaveToPrizeFile(PrizeFileName);

            return model;
        }

        public List<PersonModel> GetPerson_All()
        {
            return PersonFileName.FullFilePath().LoadFile().ConvertTextToPersonModel();
        }
    }
}
