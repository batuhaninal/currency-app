using Application.Models.Enums;

namespace Application.Models.Constants.Messages
{
    public static class ErrorMessage
    {
        public static Language CurrentLanguage { get; set; } = Language.TR;

        public static string PASSIVEorDELETED => CurrentLanguage switch
        {
            Language.TR => "Silinmiş veya aktif değil!",
            Language.EN => "Deleted or inactive!",
            _ => "Deleted or inactive!"
        };
        #region Common
        public static string INTERNAL => CurrentLanguage switch
        {
            Language.TR => "Beklenmeyen Hata!",
            Language.EN => "Internal server error!",
            _ => "Internal server error!"
        };
        public static string UNAUTHORIZED => CurrentLanguage switch
        {
            Language.TR => "Lütfen giriş yapınız!",
            Language.EN => "Please log in!",
            _ => "Please log in!"
        };
        #endregion

        #region User
        public static string USEREXIST => CurrentLanguage switch
        {
            Language.TR => "Kullanici bulunamadi!",
            Language.EN => "User is not found!",
            _ => "User is not found!"
        };
        public static string USEREMAILDUPLICATE => CurrentLanguage switch
        {
            Language.TR => "Kullanici maili zaten kullanimda!",
            Language.EN => "User email already taken!",
            _ => "User email already taken!"
        };
        #endregion

        #region Role
        public static string ROLEEXIST => CurrentLanguage switch
        {
            Language.TR => "Rol bulunamadi!",
            Language.EN => "Role is not found!",
            _ => "Role is not found!"
        };
        public static string ROLETITLEDUPLICATE => CurrentLanguage switch
        {
            Language.TR => "Rol ismi zaten kullanimda!",
            Language.EN => "Role title already taken!",
            _ => "User email already taken!"
        };
        public static string USERROLEEXIST => CurrentLanguage switch
        {
            Language.TR => "Kullanici Rolu bulunamadi!",
            Language.EN => "User Role is not found!",
            _ => "User Role is not found!"
        };
        public static string ROLEDUPLICATEFORUSER => CurrentLanguage switch
        {
            Language.TR => "Kullanici Rolu zaten bulunmakta",
            Language.EN => "User Role already exist!",
            _ => "User Role already exist!"
        };
        #endregion

        #region Category
        public static string CATEGORYEXIST => CurrentLanguage switch
        {
            Language.TR => "Kategori bulunamadi!",
            Language.EN => "Category is not found!",
            _ => "Category is not found!"
        };
        public static string CATEGORYTITLEDUPLICATE => CurrentLanguage switch
        {
            Language.TR => "Kategori ismi zaten kullanimda!",
            Language.EN => "Category title already exist!",
            _ => "Category title already exist!"
        };
        #endregion

        #region Currency
        public static string CURRENCYEXIST => CurrentLanguage switch
        {
            Language.TR => "Birim bulunamadi!",
            Language.EN => "Currency is not found!",
            _ => "Currency is not found!"
        };

        public static string CURRENCYTITLEDUPLICATE => CurrentLanguage switch
        {
            Language.TR => "Birim ismi zaten kullanimda!",
            Language.EN => "Currency title already exist!",
            _ => "Currency is not exist!"
        };

        public static string CURRENCYDAYLIMIT => CurrentLanguage switch
        {
            Language.TR => "Daha fazla birim gecmisi eklenemez!",
            Language.EN => "Much more currency history cannot be add!",
            _ => "Much more currency history cannot be add!"
        };

        public static string CURRENCYHOURLIMIT => CurrentLanguage switch
        {
            Language.TR => "Daha fazla birim gecmisi bu saate eklenemez!",
            Language.EN => "Much more currency history cannot be add for hour!",
            _ => "Much more currency history cannot be add!"
        };
        #endregion

        #region Asset
        public static string ASSETEXIST => CurrentLanguage switch
        {
            Language.TR => "Varlik bulunamadi!",
            Language.EN => "Asset is not found!",
            _ => "Asset is not found!"
        };
        #endregion

        #region Validation
        public static class Validation
        {
            public static string NotNull() => CurrentLanguage switch
            {
                Language.TR => "{PropertyName} alanı boş olamaz!",
                Language.EN => "{PropertyName} field cannot be empty!",
                _ => "{PropertyName} field cannot be empty!"
            };

            public static string Length() => CurrentLanguage switch
            {
                Language.TR => "{PropertyName} alanı {TotalLength} karakter olmalı!",
                Language.EN => "{PropertyName} field must be {TotalLength} characters long!",
                _ => "{PropertyName} field must be {TotalLength} characters long!"
            };

            public static string MaxLength() => CurrentLanguage switch
            {
                Language.TR => "{PropertyName} alanı en fazla {MaxLength} karakter olabilir!",
                Language.EN => "{PropertyName} field can be maximum {MaxLength} characters long!",
                _ => "{PropertyName} field can be maximum {MaxLength} characters long!"
            };

            public static string MinLength() => CurrentLanguage switch
            {
                Language.TR => "{PropertyName} alanı en az {MinLength} karakter olmalı!",
                Language.EN => "{PropertyName} field must be at least {MinLength} characters long!",
                _ => "{PropertyName} field must be at least {MinLength} characters long!"
            };

            public static string BetweenLength() => CurrentLanguage switch
            {
                Language.TR => "{PropertyName} alanı {MinLength} ile {MaxLength} karakter arasında olmalı!",
                Language.EN => "{PropertyName} field must be between {MinLength} and {MaxLength} characters long!",
                _ => "{PropertyName} field must be between {MinLength} and {MaxLength} characters long!"
            };

            public static string GreaterThan() => CurrentLanguage switch
            {
                Language.TR => "{PropertyName} alanı {ComparisonValue}'den büyük olmalı!",
                Language.EN => "{PropertyName} field must be greater than {ComparisonValue}!",
                _ => "{PropertyName} field must be greater than {ComparisonValue}!"
            };

            public static string LessThan() => CurrentLanguage switch
            {
                Language.TR => "{PropertyName} alanı {ComparisonValue}'den küçük olmalı!",
                Language.EN => "{PropertyName} field must be less than {ComparisonValue}!",
                _ => "{PropertyName} field must be less than {ComparisonValue}!"
            };

            public static string GreaterThanOrEqual() => CurrentLanguage switch
            {
                Language.TR => "{PropertyName} alanı {ComparisonValue} veya daha büyük olmalı!",
                Language.EN => "{PropertyName} field must be greater than or equal to {ComparisonValue}!",
                _ => "{PropertyName} field must be greater than or equal to {ComparisonValue}!"
            };

            public static string LessThanOrEqual() => CurrentLanguage switch
            {
                Language.TR => "{PropertyName} alanı {ComparisonValue} veya daha küçük olmalı!",
                Language.EN => "{PropertyName} field must be less than or equal to {ComparisonValue}!",
                _ => "{PropertyName} field must be less than or equal to {ComparisonValue}!"
            };

            public static string Email => CurrentLanguage switch
            {
                Language.TR => "Geçerli bir e-posta giriniz!",
                Language.EN => "Please enter a valid email!",
                _ => "Please enter a valid email!"
            };

            public static string PasswordsNotMatches => CurrentLanguage switch
            {
                Language.TR => "Şifreler uyuşmuyor!",
                Language.EN => "Passwords do not match!",
                _ => "Passwords do not match!"
            };
        }
        #endregion
    }
}