using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    /// <summary>
    /// Track each individual prize for tournament
    /// </summary>
    public class PrizeModel
    {
        /// <summary>
        /// Store the place number to receive the prize
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// Store the name of the prize
        /// </summary>
        public string PrizeName { get; set; }

        /// <summary>
        /// Store the percentage of total fees collected to be given as prize
        /// </summary>
        public double PrizePercentage { get; set; } = 0;

        /// <summary>
        /// Store the amount of the prize to be given if percentage is not set
        /// </summary>
        public decimal PrizeAmount { get; set; } = 0;

        public PrizeModel()
        {

        }
        public PrizeModel(string prizeName, string prizePlaceNumber, bool usePrizeAmount, string prizeQuantity)
        {
            PrizeName = prizeName;

            int.TryParse(prizePlaceNumber, out int prizePlaceNumberValue);
            PlaceNumber = prizePlaceNumberValue;

            if (usePrizeAmount)
            {
                decimal.TryParse(prizeQuantity, out decimal prizeQuantityValue);
                PrizeAmount = prizeQuantityValue;
                PrizePercentage = 0;
            }
            else
            {
                double.TryParse(prizeQuantity, out double prizeQuantityValue);
                PrizePercentage = prizeQuantityValue;
                PrizeAmount = 0;
            }
        }
    }
}
