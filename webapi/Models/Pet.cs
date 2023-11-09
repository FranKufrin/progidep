﻿using System.ComponentModel.DataAnnotations;

namespace MP7_progi.Models
{
    public class Pet:Table
    {
        public int petID { get; set; }
        public string? namePet { get; set; }

        [DataType(DataType.Date)]
        public DateTime dateHourMis { get; set; }
        public string location { get; set; }
        public string? species { get; set; }

        public string? age { get; set; }
        public string? description { get; set; }

        public int adID { get; set; }
    }
}