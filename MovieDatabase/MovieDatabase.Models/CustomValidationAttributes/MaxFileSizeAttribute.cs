using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.CustomValidationAttributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true; //service doesn't update avatar if value is null
            }

            var file = value as IFormFile;

            return file.Length <= _maxFileSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_maxFileSize.ToString());
        }
    }
}
