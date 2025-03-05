using System.Collections.Generic;
using TerminalProceduralTerrainGeneration.Renderers;
using TerminalProceduralTerrainGeneration.TerrainGenerators;

namespace TerminalProceduralTerrainGeneration.Services;

/// <summary>
/// Factory class for creating and managing terrain generators and renderers.
/// This class provides a central place to register and retrieve all available generators and renderers.
/// </summary>
public static class TerrainFactory
{
    /// <summary>
    /// Creates all available terrain generators.
    /// Add new generators here when extending the application.
    /// </summary>
    public static List<ITerrainGenerator> CreateAllGenerators()
    {
        return
        [
            new RandomTerrainGenerator(),
            new PerlinNoiseTerrainGenerator(),
            new MidpointDisplacementGenerator()
            // Add new generators here
        ];
    }

    /// <summary>
    /// Creates all available terrain renderers.
    /// Add new renderers here when extending the application.
    /// </summary>
    public static List<ConsoleRenderer> CreateAllRenderers()
    {
        return
        [
            new BlackAndWhiteRenderer(), // Default renderer
            new ColourHeightRenderer()
            // Add new renderers here
        ];
    }
}