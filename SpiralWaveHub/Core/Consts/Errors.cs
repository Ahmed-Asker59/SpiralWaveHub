namespace SpiralWaveHub.Core.Consts
{
    public static class Errors
    {
        public const string NotAllowedDate = "Date cannot be in the future";
        public const string NotAllowedExtension = "The extenstions allowed are jpg, jpeg and png";
        public const string MaxSize = "File cannot be more than 2 MB!";
        public const string MaxLength = "Cannot be more than {0}";
        public const string Duplicated = "Such value already exist!";
        public const string OnlyEnglishLetters = "Only English letters are allowed.";
        public const string InvalidMobileNumber = "Invalid mobile number.";
        public const string WeakPassword = "Passwords contain an uppercase character, lowercase character, a digit, and a non-alphanumeric character. Passwords must be at least 8 characters long";
    }
}
