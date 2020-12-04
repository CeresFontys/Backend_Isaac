using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AnomalyService.Models
{

    public class SensorError
    {
        public int X;
        public int Y;
        public string Floor;
        public string Error;
        public DateTime DateTime;
        public ErrorType Type;
        public double ValueFirst;
        public double ValueSecond;

        public enum ErrorType
        {
            ExtremeTop,
            ExtremeBottom,
            NormalTop,
            NormalBottom,
            NextDif,
            PrevDif
        }

        public SensorError(int x, int y, string floor, string error, DateTime dateTime, ErrorType type, double valueFirst, double valueSecond=-1)
        {
            X = x;
            Y = y;
            Floor = floor;
            Error = error;
            DateTime = dateTime;
            Type = type;
            ValueFirst = valueFirst;
            ValueSecond = valueSecond;
        }


    }
}
