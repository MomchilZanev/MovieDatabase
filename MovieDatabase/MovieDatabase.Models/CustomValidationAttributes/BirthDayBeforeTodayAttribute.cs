using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.CustomValidationAttributes
{
    public class BirthDayBeforeTodayAttribute : ValidationAttribute
    {
        public BirthDayBeforeTodayAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var parseSucceeded = DateTime.TryParse(value.ToString(), out DateTime birthDay);

            if (parseSucceeded)
            {
                return birthDay <= DateTime.UtcNow;
            }

            return false;
        }
    }
}
