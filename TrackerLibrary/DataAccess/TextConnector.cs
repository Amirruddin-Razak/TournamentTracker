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
        private const string PrizesFileName = "PrizeModels.csv";

        /// <summary>
        /// Save new prize to text
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizesFileName.FullFilePath().LoadFile().ConvertTextToPrizeModel();

            int currentId = 1;
            if (prizes.Count != 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            prizes.Add(model);

            prizes.SaveToPrizeFile(PrizesFileName);

            return model;
        }
    }
}
