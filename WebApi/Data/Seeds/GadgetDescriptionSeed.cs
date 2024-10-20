using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class GadgetDescriptionSeed
{
    public readonly static List<GadgetDescription> Default =
    [

        ];

    private static GadgetDescriptionType ParseGadgetDescriptionType(string typeString)
    {
        if (Enum.TryParse<GadgetDescriptionType>(typeString, out var result))
        {
            return result;
        }
        throw new ArgumentException($"Invalid GadgetDescriptionType: {typeString}");
    }
}
