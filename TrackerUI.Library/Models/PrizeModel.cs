using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerUI.Library.Models
{
    public class PrizeModel : IDataModel
    {
        public int Id { get; set; }
        public int PlaceNumber { get; set; }
        public string PrizeName { get; set; }
        public double PrizePercentage { get; set; } = 0;
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

        public PrizeModel(TrackerLibrary.Models.PrizeModel model)
        {
            Id = model.Id;
            PlaceNumber = model.PlaceNumber;
            PrizeName = model.PrizeName;
            PrizePercentage = model.PrizePercentage;
            PrizeAmount = model.PrizeAmount;
        }

        public decimal CalculatePrize(decimal totalIncome) => PrizeAmount + (totalIncome * ((decimal)PrizePercentage / 100m));
    }
}
