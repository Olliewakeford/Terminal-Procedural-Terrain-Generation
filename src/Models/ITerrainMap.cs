namespace TerminalProceduralTerrainGeneration.Models;

/// <summary>
/// Defines the interface for a two-dimensional terrain height map.
/// Provides methods and properties for accessing and manipulating terrain height data.
/// </summary>
public interface ITerrainMap
{
    /// <summary>
    /// Gets the width of the terrain map in grid cells.
    /// </summary>
    int Width { get; }
    
    /// <summary>
    /// Gets the height of the terrain map in grid cells.
    /// </summary>
    int Height { get; }
    
    /// <summary>
    /// Gets or sets the height value at the specified coordinates.
    /// Height values are normalized between 0.0 (minimum height) and 1.0 (maximum height).
    /// </summary>
    /// <param name="x">The x-coordinate (column) of the cell.</param>
    /// <param name="y">The y-coordinate (row) of the cell.</param>
    /// <returns>The normalized height value at the specified position.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the provided coordinates are outside the map boundaries.
    /// </exception>
    float this[int x, int y] { get; set; }
    
    /// <summary>
    /// Resets all height values in the terrain map to zero.
    /// </summary>
    void Clear();
    
    /// <summary>
    /// Calculates and returns basic statistics about the terrain heights.
    /// </summary>
    /// <returns>
    /// A tuple containing:
    /// - min: The minimum height value in the terrain.
    /// - max: The maximum height value in the terrain.
    /// - average: The average height value across all cells.
    /// </returns>
    (float min, float max, float average) GetStatistics();
}