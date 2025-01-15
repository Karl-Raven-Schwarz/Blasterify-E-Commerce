namespace Blasterify.Yuno.Models
{
    public class Phone
    {
        private string? _country_code;
        private string? _number;

        /// <value>
        /// Example: 55
        /// </value>
        public string? Country_Code
        {
            get => _country_code;
            set
            {
                if (value != null && (value.Length < 1 || value.Length > 3))
                {
                    throw new ArgumentException($"{nameof(Country_Code)} must be between 1 and 3 characters.");
                }
                _country_code = value;
            }
        }

        public string? Number
        {
            get => _number;
            set
            {
                if (value != null && (value.Length < 1 || value.Length > 32))
                {
                    throw new ArgumentException($"{nameof(Number)} must be between 1 and 32 characters.");
                }
                _number = value;
            }
        }
    }
}