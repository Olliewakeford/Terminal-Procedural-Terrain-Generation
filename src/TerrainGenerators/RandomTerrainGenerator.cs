using TerminalProceduralTerrainGeneration.Models;

namespace TerminalProceduralTerrainGeneration.TerrainGenerators;

/// <summary>
/// Generates terrain using simple random height values.
/// Produces a noise-like terrain with no coherent features.
/// </summary>
public class RandomTerrainGenerator : ITerrainGenerator
{
    private float _minHeight = 0;    // Default minimum height
    private float _maxHeight = 1.0f;    // Default maximum height
    private int _seed = 42;              // Current seed
    private Random _random;             // Random number generator
    
    /// <summary>
    /// Gets the display name of the terrain generator.
    /// </summary>
    public string Name => "Random Terrain";
    
    /// <summary>
    /// Gets a description of the random terrain generator.
    /// </summary>
    public string Description => "Generates terrain using random noise with configurable height range";

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomTerrainGenerator"/> class
    /// with default settings.
    /// </summary>
    public RandomTerrainGenerator()
    {
        _random = new Random(_seed);
    }

    /// <summary>
    /// Generates random terrain data by assigning random height values to each position.
    /// </summary>
    /// <param name="map">The terrain map to populate with height data.</param>
    public void Generate(TerrainMap map)
    {
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                map[x, y] = _minHeight + (float)_random.NextDouble() * (_maxHeight - _minHeight);
            }
        }
    }
}