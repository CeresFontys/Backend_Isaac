﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AnomalyService.Models
{

    public class SensorError
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Floor { get; set; }
        public string Error { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime DateTimeNext { get; set; }
        public string ValueType { get; set; }
        public double ApiValue { get; set; }
        public ErrorType Type { get; set; }
        public double ValueFirst { get; set; }
        public double ValueSecond { get; set; }

        public enum ErrorType
        {
            ExtremeTop,
            ExtremeBottom,
            NormalTop,
            NormalBottom,
            NextDif,
        }
    }
}
