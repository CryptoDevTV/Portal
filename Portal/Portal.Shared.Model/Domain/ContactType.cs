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
    }
}