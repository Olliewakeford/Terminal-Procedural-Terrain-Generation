using TerminalProceduralTerrainGeneration.Models;

namespace TerminalProceduralTerrainGeneration.Renderers;

/// <summary>
/// Renders terrain using colored height characters that visually represent elevation.
/// Uses a gradient of different block characters and colors to show terrain height variations.
/// </summary>
public class ColourHeightRenderer : ConsoleRenderer
{
    private static readonly char[] HeightChars = ['▁', '▂', '▃', '▄', '▅', '▆', '▇', '█'];
    
    // Color for different height levels
    private static readonly ConsoleColor[] HeightColors =
    [
        ConsoleColor.DarkBlue,    // Lowest
        ConsoleColor.Blue,
        ConsoleColor.DarkGreen,
        ConsoleColor.Green,
        ConsoleColor.DarkYellow,
        ConsoleColor.Yellow,
        ConsoleColor.DarkRed,
        ConsoleColor.Red          // Highest
    ];
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ColourHeightRenderer"/> class.
    /// </summary>
    public ColourHeightRenderer() 
        : base("Color Height View", "Colored block characters showing terrain elevation")
    {
    }
    
    /// <summary>
    /// Renders the terrain using block characters of varying heights and colors.
    /// </summary>
    /// <param name="map">The terrain map to render.</param>
    protected override void RenderTerrain(TerrainMap map)
    {
        Console.WriteLine(new string('═', map.Width + 2));
        for (int y = 0; y < map.Height; y++)
        {
            Console.Write("║");
            for (int x = 0; x < map.Width; x++)
            {
                float height = map[x, y];
                int colorIndex = (int)(height * (HeightColors.Length - 1));
                int charIndex = (int)(height * (HeightChars.Length - 1));

                Console.ForegroundColor = HeightColors[colorIndex];
                Console.Write(HeightChars[charIndex]);
            }
            Console.ResetColor();
            Console.WriteLine("║");
        }
        Console.WriteLine(new string('═', map.Width + 2));
    }
    
    /// <summary>
    /// Renders a legend showing the height characters and their corresponding elevation values.
    /// </summary>
    /// <param name="map">The terrain map associated with the legend.</param>
    protected override void RenderLegend(TerrainMap map)
    {
        base.RenderLegend(map);
        
        for (int i = HeightColors.Length - 1; i >= 0; i--)
        {
            float heightValue = i / (float)(HeightColors.Length - 1);
            Console.ForegroundColor = HeightColors[i];
            
            char symbol = HeightChars[Math.Min(i, HeightChars.Length - 1)];
            Console.WriteLine($"{symbol} - Height: {heightValue:F2}");
        }
        Console.ResetColor();
    }
}