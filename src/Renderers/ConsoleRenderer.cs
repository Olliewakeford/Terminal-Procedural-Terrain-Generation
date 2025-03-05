using TerminalProceduralTerrainGeneration.Models;
using TerminalProceduralTerrainGeneration.Exceptions;

namespace TerminalProceduralTerrainGeneration.Renderers;

/// <summary>
/// Base abstract class for console-based terrain renderers.
/// Provides common functionality for rendering terrain maps in the console.
/// </summary>
public abstract class ConsoleRenderer
{
    /// <summary>
    /// Gets the display name of the renderer.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Gets a description of the renderer and its visualization style.
    /// </summary>
    public string Description { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleRenderer"/> class.
    /// </summary>
    /// <param name="name">The display name of the renderer.</param>
    /// <param name="description">A brief description of the renderer.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when name or description is null or empty.
    /// </exception>
    protected ConsoleRenderer(string name, string description)
    {
        // Validate that renderers have a name and description
        Name = !string.IsNullOrWhiteSpace(name) ? name : 
            throw new ArgumentException("Renderer name cannot be null or empty", nameof(name));
            
        Description = !string.IsNullOrWhiteSpace(description) ? description : 
            throw new ArgumentException("Renderer description cannot be null or empty", nameof(description));
    }
    
    /// <summary>
    /// Renders the terrain map to the console using the renderer's visualization style.
    /// </summary>
    /// <param name="map">The terrain map to render.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the map parameter is null.
    /// </exception>
    /// <exception cref="TerrainGenerationException">
    /// Thrown when an error occurs during the rendering process.
    /// </exception>
    public virtual void Render(TerrainMap map)
    {
        if (map == null)
            throw new ArgumentNullException(nameof(map), "TerrainMap cannot be null");
            
        try
        {
            Console.Clear(); // Clear the console before rendering
            RenderHeader(map);
            RenderTerrain(map);
            RenderLegend(map);
            RenderStatistics(map);
        }
        catch (Exception ex)
        {
            // Reset colors in case of an error to avoid leaving the console in an unexpected state
            Console.ResetColor();
            throw new TerrainGenerationException($"Error rendering terrain using {Name}: {ex.Message}", ex);
        }
    }
    
    /// <summary>
    /// Renders the header information for the terrain visualization.
    /// </summary>
    /// <param name="map">The terrain map to render header information for.</param>
    protected virtual void RenderHeader(TerrainMap map)
    {
        Console.WriteLine($"Terrain Visualization - {Name}");
        Console.WriteLine($"Map Size: {map.Width}x{map.Height}");
    }
    
    /// <summary>
    /// Renders the terrain using the specific visualization approach of the renderer.
    /// </summary>
    /// <param name="map">The terrain map to render.</param>
    protected abstract void RenderTerrain(TerrainMap map);
    
    /// <summary>
    /// Renders a legend explaining the visualization symbols used in the terrain rendering.
    /// </summary>
    /// <param name="map">The terrain map associated with the legend.</param>
    protected virtual void RenderLegend(TerrainMap map)
    {
        Console.ResetColor();
        Console.WriteLine("\nHeight Legend:");
    }
    
    /// <summary>
    /// Renders statistical information about the terrain map.
    /// </summary>
    /// <param name="map">The terrain map to analyze and render statistics for.</param>
    protected virtual void RenderStatistics(TerrainMap map)
    {
        var stats = map.GetStatistics();
        Console.WriteLine("\nTerrain Statistics:");
        Console.WriteLine($"Minimum Height: {stats.min:F3}");
        Console.WriteLine($"Maximum Height: {stats.max:F3}");
        Console.WriteLine($"Average Height: {stats.average:F3}");
        
        // Calculate additional statistics
        float range = stats.max - stats.min;
        float midpoint = (stats.max + stats.min) / 2;
        
        Console.WriteLine($"Height Range: {range:F3}");
        Console.WriteLine($"Midpoint: {midpoint:F3}");
    }
}