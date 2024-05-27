using OperationManagmentProject.Attributes;
using System.Text.Json.Serialization;

namespace OperationManagmentProject.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<WeaknessType>))]
    public enum WeaknessType
    {
        [Translation("مال")]
        Money = 1,

        [Translation("نساء")]
        Women = 2,

        [Translation("سفر")]
        Travel = 3
    }
}
