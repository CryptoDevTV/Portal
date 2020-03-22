using System.ComponentModel;

namespace Portal.Shared.Model.Domain
{
    public enum ContactType
    {
        [Description("Pytanie z serwisu CryptoDev.TV")]
        Question = 0,

        [Description("Rezerwacja pakietu Standard")]
        Standard = 1,

        [Description("Rezerwacja pakietu Premium")]
        Premium = 2,

        [Description("Powiadomienie o dostępności - GitHub dla inwestorów")]
        Notification = 10,

        [Description("Reklama - Pre Roll")]
        AdvPreRoll = 20,

        [Description("Reklama - Post Roll")]
        AdvPostRoll = 21,

        [Description("Reklama - Pre Image")]
        AdvPreImage = 22,

        [Description("Reklama - Post Image")]
        AdvPostImage = 23,

        [Description("Reklama - Text")]
        AdvText = 24,

        [Description("Reklama - Image")]
        AdvImage = 25,

        [Description("Reklama - Other")]
        AdvOther = 26,

        [Description("Inna metoda płatności")]
        OtherPaymentMethod = 90,

        [Description("Pytanie z serwisu Academia CryptoDev.TV")]
        AcademiaMain = 100,
    }
}