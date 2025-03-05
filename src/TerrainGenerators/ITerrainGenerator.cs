using TerminalProceduralTerrainGeneration.Models;

namespace TerminalProceduralTerrainGeneration.TerrainGenerators;

/// <summary>
/// Defines the interface for terrain generation algorithms.
/// Classes implementing this interface provide different methods
/// for generating height maps for terrain visualization.
/// </summary>
public interface ITerrainGenerator
{
    /// <summary>
    /// Generates terrain data in the provided map.
    /// </summary>
    /// <param name="map">The terrain map to modify the height data on.</param>
    void Generate(TerrainMap map);
    
    /// <summary>
    /// Gets the display name of the terrain generator.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Gets a description of the terrain generator and its algorithm.
    /// </summary>
    string Description { get; }
}