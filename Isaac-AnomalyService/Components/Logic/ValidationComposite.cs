using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Models;

namespace Isaac_AnomalyService.Components.Logic
{
    public class ValidationComposite
    {
        
        //Calculate the relative difference 2 sensors may differ 
        public bool IsValueValid(double first, double second, double minuteDif, double maxDifference)
        {
            if (minuteDif.Equals(0))
            {
                minuteDif = 0.50;
            }
            var relativeDifference = maxDifference * minuteDif;

            return !(Math.Abs(first - second) >= relativeDifference);

        }
    }
}
