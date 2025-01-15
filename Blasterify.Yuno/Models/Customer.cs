namespace Blasterify.Yuno.Models
{
    public class Customer
    {
        private string? _id;
        private string? _merchantCustomerId;
        private string? _merchantCustomerCreatedAt;
        private string? _firstName;
        private string? _lastName;
        private string? _dateOfBirth;
        private string? _email;

        public string? Id
        {
            get => _id;
            set
            {
                if (value != null && (value.Length < 36 || value.Length > 64))
                {
                    throw new ArgumentException($"{nameof(Id)}must be between 36 and 64 characters.");
                }
                _id = value;
            }
        }

        public string? Merchant_Customer_Id
        {
            get => _merchantCustomerId;
            set
            {
                if (value != null && (value.Length < 1 || value.Length > 255))
                {
                    throw new ArgumentException($"{nameof(Merchant_Customer_Id)} must be between 1 and 255 characters.");
                }
                _merchantCustomerId = value;
            }
        }

        /// <summary>
        /// Customer´s registration date on the merchants platform (ISO 8601 MAX 27; MIN 27).
        /// </summary>
        /// <value>
        /// Example: 2022-05-09T20:46:54.786342Z
        /// </value>
        public string? Merchant_Customer_Created_At
        {
            get => _merchantCustomerCreatedAt;
            set
            {
                if (value != null && value.Length != 27)
                {
                    throw new ArgumentException($"{nameof(Merchant_Customer_Created_At)} must be exactly 27 characters.");
                }
                _merchantCustomerCreatedAt = value;
            }
        }

        public string? First_Name
        {
            get => _firstName;
            set
            {
                if (value != null && (value.Length < 1 || value.Length > 255))
                {
                    throw new ArgumentException($"{nameof(First_Name)} must be between 1 and 255 characters.");
                }
                _firstName = value;
            }
        }

        public string? Last_Name
        {
            get => _lastName;
            set
            {
                if (value != null && (value.Length < 1 || value.Length > 255))
                {
                    throw new ArgumentException($"{nameof(Last_Name)} must be between 1 and 255 characters.");
                }
                _lastName = value;
            }
        }

        /// <value>
        /// Possible enum values: M, F or NB
        /// </value>
        public string? Gender { get; set; }

        /// <summary>
        /// The customer's date of birth in the YYYY-MM-DD format (MAX 10; MIN 10).
        /// </summary>
        /// <value>
        /// Example: 1990-02-28
        /// </value>
        public string? Date_Of_Birth
        {
            get => _dateOfBirth;
            set
            {
                if (value != null && value.Length != 10)
                {
                    throw new ArgumentException($"{nameof(Date_Of_Birth)} must be exactly 10 characters.");
                }
                _dateOfBirth = value;
            }
        }

        public string? Email
        {
            get => _email;
            set
            {
                if (value != null && (value.Length < 3 || value.Length > 255))
                {
                    throw new ArgumentException($"{nameof(Email)} must be between 3 and 255 characters.");
                }
                _email = value;
            }
        }

        public string? Nationality { get; set; }

        public string? Country { get; set; }

        /// <summary>
        /// Customer creation date and time (MAX 27; MIN 27; ISO 8601).
        /// </summary>
        /// <value>
        /// Example: 2022-05-09T20:46:54.786342Z
        /// </value>
        public string? Created_At { get; set; }

        /// <summary>
        /// Last customer update date and time (MAX 27; MIN 27; ISO 8601).
        /// </summary>
        /// <value>
        /// Example: 2022-05-09T20:46:54.786342Z
        /// </value>
        public string? Updated_At { get; set; }
    }
}
