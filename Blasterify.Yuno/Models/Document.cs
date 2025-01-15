namespace Blasterify.Yuno.Models
{
    public class Document
    {
        private string? _document_number;
        private string? _document_type;

        public string? Document_Number
        {
            get => _document_number;
            set
            {
                if (value != null && (value.Length < 3 || value.Length > 40))
                {
                    throw new ArgumentException($"{nameof(Document_Number)} must be between 3 and 40 characters.");
                }
                _document_number = value;
            }
        }

        public string? Document_Type
        {
            get => _document_type;
            set
            {
                if (value != null && (value.Length < 2 || value.Length > 6))
                {
                    throw new ArgumentException($"{nameof(Document_Type)} must be between 2 and 6 characters.");
                }
                _document_type = value;
            }
        }
    }
}