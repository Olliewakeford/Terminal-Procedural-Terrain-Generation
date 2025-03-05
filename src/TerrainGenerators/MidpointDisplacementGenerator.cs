using TerminalProceduralTerrainGeneration.Models;

namespace TerminalProceduralTerrainGeneration.TerrainGenerators;

/// <summary>
/// Generates terrain using the midpoint displacement (diamond-square) algorithm.
/// Creates fractal-like landscapes with adjustable roughness.
/// </summary>
public class MidpointDisplacementGenerator : ITerrainGenerator
{
    private float _roughness = 0.5f;     // Controls terrain roughness (0-1)
    private float _minHeight = 0;            // Minimum initial corner height
    private float _maxHeight = 1.0f;     // Maximum initial corner height
    private int _seed = 42;                   // Seed for reproducible results
    private Random _random;              // Random number generator instance

    /// <summary>
    /// Initializes a new instance of the <see cref="MidpointDisplacementGenerator"/> class
    /// with default settings.
    /// </summary>
    public MidpointDisplacementGenerator()
    {
        _random = new Random(_seed);
    }

    /// <summary>
    /// Gets the display name of the terrain generator.
    /// </summary>
    public string Name => "Midpoint Displacement Terrain";
    
    /// <summary>
    /// Gets a description of the midpoint displacement terrain generator.
    /// </summary>
    public string Description => "Generates terrain using the midpoint displacement (diamond-square) algorithm";

    /// <summary>
    /// Generates terrain data using the midpoint displacement algorithm.
    /// </summary>
    /// <param name="map">The terrain map to populate with height data.</param>
    public void Generate(TerrainMap map)
    {
        // First, we need to determine the size of our working grid
        // It needs to be a power of 2 plus 1 to work with the algorithm
        int size = GetNextPowerOfTwo(Math.Max(map.Width, map.Height)) + 1;
        
        // Create a temporary grid for the algorithm
        float[,] tempGrid = new float[size, size];

        // Initialize the corners
        tempGrid[0, 0] = GetRandomHeight();
        tempGrid[0, size - 1] = GetRandomHeight();
        tempGrid[size - 1, 0] = GetRandomHeight();
        tempGrid[size - 1, size - 1] = GetRandomHeight();

        // Perform the midpoint displacement
        int step = size - 1;
        float displacement = (_maxHeight - _minHeight) * _roughness;

        while (step > 1)
        {
            int halfStep = step / 2;

            // Square steps
            for (int y = 0; y < size - 1; y += step)
            {
                for (int x = 0; x < size - 1; x += step)
                {
                    float average = (
                        tempGrid[y, x] +                    // top left
                        tempGrid[y, x + step] +            // top right
                        tempGrid[y + step, x] +            // bottom left
                        tempGrid[y + step, x + step]       // bottom right
                    ) * 0.25f;

                    tempGrid[y + halfStep, x + halfStep] = average + RandomDisplacement(displacement);
                }
            }

            // Diamond steps
            for (int y = 0; y < size; y += halfStep)
            {
                for (int x = (y + halfStep) % step; x < size; x += step)
                {
                    float sum = 0;
                    int count = 0;

                    // Check each diamond point
                    if (y >= halfStep) { sum += tempGrid[y - halfStep, x]; count++; }
                    if (y + halfStep < size) { sum += tempGrid[y + halfStep, x]; count++; }
                    if (x >= halfStep) { sum += tempGrid[y, x - halfStep]; count++; }
                    if (x + halfStep < size) { sum += tempGrid[y, x + halfStep]; count++; }

                    tempGrid[y, x] = (sum / count) + RandomDisplacement(displacement);
                }
            }

            // Reduce the random displacement for the next iteration
            displacement *= _roughness;
            step = halfStep;
        }

        // Copy the generated terrain to the actual map, scaling as needed
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                // Scale coordinates to match our temporary grid
                float scaledX = x * (float)(size - 1) / (map.Width - 1);
                float scaledY = y * (float)(size - 1) / (map.Height - 1);

                // Interpolate between the nearest points
                int x1 = (int)scaledX;
                int y1 = (int)scaledY;
                int x2 = Math.Min(x1 + 1, size - 1);
                int y2 = Math.Min(y1 + 1, size - 1);

                float fx = scaledX - x1;
                float fy = scaledY - y1;

                // Bilinear interpolation
                float value = 
                    tempGrid[y1, x1] * (1 - fx) * (1 - fy) +
                    tempGrid[y1, x2] * fx * (1 - fy) +
                    tempGrid[y2, x1] * (1 - fx) * fy +
                    tempGrid[y2, x2] * fx * fy;

                map[x, y] = Math.Clamp(value, 0f, 1f);
            }
        }
    }

    private float GetRandomHeight()
    {
        return _minHeight + (float)_random.NextDouble() * (_maxHeight - _minHeight);
    }

    private float RandomDisplacement(float displacement)
    {
        return ((float)_random.NextDouble() * 2 - 1) * displacement;
    }

    private static int GetNextPowerOfTwo(int n)
    {
        n--;
        n |= n >> 1;
        n |= n >> 2;
        n |= n >> 4;
        n |= n >> 8;
        n |= n >> 16;
        return n + 1;
    }
}