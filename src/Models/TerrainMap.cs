namespace TerminalProceduralTerrainGeneration.Models;

/// <summary>
/// Represents a two-dimensional height map for terrain generation.
/// Stores elevation data as normalized float values between 0.0 and 1.0.
/// </summary>
public class TerrainMap : ITerrainMap
{
    private readonly float[,] _heights;

    /// <summary>
    /// Gets the width of the terrain map in grid cells.
    /// </summary>
    public int Width { get; } 

    /// <summary>
    /// Gets the height of the terrain map in grid cells.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TerrainMap"/> class with the specified dimensions.
    /// All height values are initialized to zero.
    /// </summary>
    /// <param name="width">The width of the terrain map.</param>
    /// <param name="height">The height of the terrain map.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when either width or height is less than or equal to zero.
    /// </exception>
    public TerrainMap(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero");
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than zero");
            
        Width = width;
        Height = height;
        _heights = new float[width, height];
    }

    /// <summary>
    /// Gets or sets the height value at the specified coordinates.
    /// Height values are automatically clamped between 0.0 and 1.0.
    /// </summary>
    /// <param name="x">The x-coordinate (column) of the cell.</param>
    /// <param name="y">The y-coordinate (row) of the cell.</param>
    /// <returns>The normalized height value at the specified position.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the provided coordinates are outside the map boundaries.
    /// </exception>
    public float this[int x, int y]
    {
        get
        {
            ValidateCoordinates(x, y);
            return _heights[x, y];
        }
        set
        {
            ValidateCoordinates(x, y);
            _heights[x, y] = Math.Clamp(value, 0f, 1f); // Heights between 0 and 1
        }
    }

    // Helper method to validate coordinates are within the map bounds
    private void ValidateCoordinates(int x, int y)
    {
        if (x < 0 || x >= Width)
            throw new ArgumentOutOfRangeException(nameof(x), $"X coordinate {x} is out of range [0, {Width - 1}]");
        if (y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y), $"Y coordinate {y} is out of range [0, {Height - 1}]");
    }

    /// <summary>
    /// Resets all height values in the terrain map to zero.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_heights, 0, Width * Height);
    }
    
    /// <summary>
    /// Calculates and returns basic statistical information about the terrain heights.
    /// </summary>
    /// <returns>
    /// A tuple containing:
    /// - min: The minimum height value in the terrain.
    /// - max: The maximum height value in the terrain.
    /// - average: The average height value across all cells.
    /// Returns (0, 0, 0) if the terrain has no cells or contains no data.
    /// </returns>
    public (float min, float max, float average) GetStatistics()
    {
        // Early return for empty terrains
        if (Width == 0 || Height == 0)
            return (0f, 0f, 0f);
            
        float min = float.MaxValue;
        float max = float.MinValue;
        float sum = 0;
        bool hasData = false;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                hasData = true;
                float height = _heights[x, y];
                min = Math.Min(min, height);
                max = Math.Max(max, height);
                sum += height;
            }
        }

        // Handle the case where there might be no valid data
        if (!hasData)
            return (0f, 0f, 0f);
            
        return (min, max, sum / (Width * Height));
    }
}   