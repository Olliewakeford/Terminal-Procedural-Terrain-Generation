using TerminalProceduralTerrainGeneration.Exceptions;
using TerminalProceduralTerrainGeneration.Models;

namespace TerminalProceduralTerrainGeneration.TerrainGenerators;

/// <summary>
/// Generates terrain using Perlin noise algorithms.
/// Creates natural-looking continuous terrain with hills and valleys.
/// </summary>
public class PerlinNoiseTerrainGenerator : ITerrainGenerator
{
    // Configuration properties with default values
    private float _scale = 20.0f;           // Controls how zoomed in the noise is
    private float _amplitude = 1.0f;        // Controls the height multiplier
    private int _octaves = 4;              // Number of layers of noise
    private float _persistence = 0.5f;      // How much each octave contributes
    private float _lacunarity = 2.0f; // How much detail is added in each octave
    private int _seed = 42;                 // Seed for reproducible results
    private Random _random;                // Random number generator instance

    /// <summary>
    /// Initializes a new instance of the <see cref="PerlinNoiseTerrainGenerator"/> class
    /// with default settings.
    /// </summary>
    public PerlinNoiseTerrainGenerator()
    {
        _random = new Random(_seed);
    }

    /// <summary>
    /// Gets the display name of the terrain generator.
    /// </summary>
    public string Name => "Perlin Noise Terrain";
    
    /// <summary>
    /// Gets a description of the Perlin noise terrain generator.
    /// </summary>
    public string Description => "Generates realistic-looking terrain using Perlin noise with multiple octaves";

    /// <summary>
    /// Generates terrain data using Perlin noise algorithms.
    /// </summary>
    /// <param name="map">The terrain map to populate with height data.</param>
    /// <exception cref="ArgumentNullException">Thrown when map is null.</exception>
    /// <exception cref="TerrainGenerationException">Thrown when an error occurs during generation.</exception>
    public void Generate(TerrainMap map)
    {
        if (map == null)
            throw new ArgumentNullException(nameof(map), "Terrain map cannot be null");
            
        try
        {
            // Initialize permutation table for Perlin noise
            int[] permutation = GeneratePermutationTable();

            // Generate noise for each point in the map
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    float amplitude = _amplitude;
                    float frequency = 1f;
                    float noiseHeight = 0f;

                    // Generate multiple octaves of noise
                    for (int i = 0; i < _octaves; i++)
                    {
                        float sampleX = x / _scale * frequency;
                        float sampleY = y / _scale * frequency;

                        // Generate noise value and add it to the total
                        float perlinValue = GeneratePerlinNoise(sampleX, sampleY, permutation);
                        noiseHeight += perlinValue * amplitude;

                        // Adjust values for next octave
                        amplitude *= _persistence;
                        frequency *= _lacunarity;
                    }

                    // Normalize to 0-1 range and set the height
                    map[x, y] = (noiseHeight + 1f) * 0.5f;
                }
            }
        }
        catch (Exception ex)
        {
            throw new TerrainGenerationException($"Error generating Perlin noise terrain: {ex.Message}", ex);
        }
    }

    private float GeneratePerlinNoise(float x, float y, int[] permutation)
    {
        // Get grid cell coordinates
        int x0 = (int)Math.Floor(x);
        int y0 = (int)Math.Floor(y);
        
        // Get local coordinates within the cell (0 to 1)
        float xf = x - x0;
        float yf = y - y0;

        // Get the corners of the cell
        int x1 = x0 + 1;
        int y1 = y0 + 1;

        // Calculate dot products for each corner
        float topLeft = DotGridGradient(x0, y0, x, y, permutation);
        float topRight = DotGridGradient(x1, y0, x, y, permutation);
        float bottomLeft = DotGridGradient(x0, y1, x, y, permutation);
        float bottomRight = DotGridGradient(x1, y1, x, y, permutation);

        // Interpolate between the values
        float tx = Fade(xf);
        float ty = Fade(yf);

        float top = Lerp(topLeft, topRight, tx);
        float bottom = Lerp(bottomLeft, bottomRight, tx);

        return Lerp(top, bottom, ty);
    }

    private float DotGridGradient(int ix, int iy, float x, float y, int[] permutation)
    {
        // Get gradient from permutation table
        float angle = permutation[(ix + permutation[iy & 255]) & 255] * (2f * MathF.PI / 256f);
        
        // Create gradient vector
        float gradientX = MathF.Cos(angle);
        float gradientY = MathF.Sin(angle);

        // Compute the distance vector
        float dx = x - ix;
        float dy = y - iy;

        // Compute dot product
        return dx * gradientX + dy * gradientY;
    }

    private int[] GeneratePermutationTable()
    {
        // Create and fill array with ordered numbers
        int[] permutation = new int[256];
        for (int i = 0; i < 256; i++)
        {
            permutation[i] = i;
        }

        // Shuffle array using Fisher-Yates algorithm
        for (int i = 255; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (permutation[i], permutation[j]) = (permutation[j], permutation[i]);
        }

        return permutation;
    }

    // Fade function for smooth interpolation (6t^5 - 15t^4 + 10t^3)
    private static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    // Linear interpolation
    private static float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }
}