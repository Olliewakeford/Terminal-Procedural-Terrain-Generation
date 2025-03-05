using TerminalProceduralTerrainGeneration.Models;

namespace TerminalProceduralTerrainGeneration.Renderers;

/// <summary>
/// Renders terrain using simple ASCII characters without background colors.
/// Provides a clean, high-contrast visualization that works in any console environment.
/// </summary>
public class BlackAndWhiteRenderer : ConsoleRenderer
{
    // Use ASCII characters that work in any console environment
    private static readonly char[] AsciiChars = { ' ', '.', ':', '-', '=', '+', '*', '#', '@' };
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BlackAndWhiteRenderer"/> class.
    /// </summary>
    public BlackAndWhiteRenderer() 
        : base("Black & White", "Simple ASCII visualization that works in any console")
    {
    }
    
    /// <summary>
    /// Renders the terrain using ASCII characters that represent different elevation levels.
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
                int charIndex = (int)(height * (AsciiChars.Length - 1));
                
                // Use only white foreground color for better visibility
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(AsciiChars[charIndex]);
            }
            Console.ResetColor();
            Console.WriteLine("║");
        }
        Console.WriteLine(new string('═', map.Width + 2));
    }
    
    /// <summary>
    /// Renders a legend showing the ASCII characters and their corresponding elevation values.
    /// </summary>
    /// <param name="map">The terrain map associated with the legend.</param>
    protected override void RenderLegend(TerrainMap map)
    {
        base.RenderLegend(map);
        
        Console.ForegroundColor = ConsoleColor.White;
        for (int i = AsciiChars.Length - 1; i >= 0; i--)
        {
            float heightValue = i / (float)(AsciiChars.Length - 1);
            Console.WriteLine($"{AsciiChars[i]} - Height: {heightValue:F2}");
        }
        Console.ResetColor();
    }
}