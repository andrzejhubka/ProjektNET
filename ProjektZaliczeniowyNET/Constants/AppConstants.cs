namespace ProjektZaliczeniowyNET.Constants
{
    public static class ViewNames
    {
        public const string EmailConfirmationSent = "EmailConfirmationSent";
        public const string EmailConfirmed = "EmailConfirmed";
        public const string Error = "Error";
        public const string Login = "Login";
        public const string Register = "Register";
    }

    public static class ErrorMessages
    {
        public const string EmailNotConfirmed = "Musisz potwierdzić swój adres email przed zalogowaniem. Sprawdź swoją skrzynkę pocztową.";
        public const string InvalidLogin = "Nieprawidłowa próba logowania.";
        public const string RequiredField = "To pole jest wymagane.";
        public const string InvalidEmail = "Nieprawidłowy format email.";
    }

    public static class SuccessMessages
    {
        public const string EmailSent = "Email potwierdzający został wysłany.";
        public const string AccountCreated = "Konto zostało utworzone pomyślnie.";
        public const string LoginSuccessful = "Logowanie zakończone sukcesem.";
    }
}