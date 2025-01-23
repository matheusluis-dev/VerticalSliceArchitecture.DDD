namespace Application.Domain.Todo.ValueObjects;

using System.Collections.Generic;
using System.Linq;
using Vogen;

[ValueObject<string>]
public readonly partial struct Color
{
    public static readonly Color White = From("#FFFFFF");
    public static readonly Color Red = From("#FF5733");
    public static readonly Color Orange = From("#FFC300");
    public static readonly Color Yellow = From("#FFFF66");
    public static readonly Color Green = From("#CCFF99");
    public static readonly Color Blue = From("#6666FF");
    public static readonly Color Purple = From("#9966CC");
    public static readonly Color Grey = From("#999999");

    private static readonly IEnumerable<Color> _supportedColors =
    [
        White,
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple,
        Grey,
    ];

    private static Validation Validate(string input)
    {
        return
            !string.IsNullOrWhiteSpace(input)
            && _supportedColors.Any(color =>
                color.Value.Equals(input, StringComparison.OrdinalIgnoreCase)
            )
            ? Validation.Ok
            : Validation.Invalid("Invalid color.");
    }

    private static string NormalizeInput(string input)
    {
        return input.ToUpperInvariant();
    }
}
