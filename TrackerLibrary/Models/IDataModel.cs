using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Model required to have Id
    /// </summary>
    public interface IDataModel
    {
        int Id { get; set; }
    }
}
