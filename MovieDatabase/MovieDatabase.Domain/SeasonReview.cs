﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class SeasonReview
    {
        [Required]
        public string SeasonId { get; set; }
        public virtual Season Season { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual MovieDatabaseUser User { get; set; }        

        [Required]
        public string Content { get; set; }

        public int Rating { get; set; }

        public DateTime Date { get; set; }
    }
}
