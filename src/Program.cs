using TerminalProceduralTerrainGeneration.Models;
using TerminalProceduralTerrainGeneration.Services;
using TerminalProceduralTerrainGeneration.Exceptions;
using TerminalProceduralTerrainGeneration.Renderers;
using TerminalProceduralTerrainGeneration.TerrainGenerators;

namespace TerminalProceduralTerrainGeneration;

class Program
{
    // Default terrain size 
    private const int MapWidth = 70;
    private const int MapHeight = 35;

    static void Main(string[] args)
    {
        try
        {
            var map = new TerrainMap(MapWidth, MapHeight); // Create a new map with default dimensions
            
            List<ConsoleRenderer> renderers = InitializeRenderers(); // List of available renderers
            List<ITerrainGenerator> generators = InitializeGenerators(); // List of available generators

            // Create application with renderers and generators then run it
            var app = new TerrainGeneratorApp(map, renderers, generators);
            app.Run();
        }
        catch (TerrainGenerationException ex)
        {
            // Handle terrain application related exceptions
            DisplayError($"Terrain Generation Error: {ex.Message}");
            
        }
        catch (Exception ex)
        {
            // For unexpected exceptions
            DisplayError($"An unexpected error occurred: {ex.Message}");
        }
    }
    
    // Initialize all available renderers by creating instances of them and checking for errors
    private static List<ConsoleRenderer> InitializeRenderers()
    {
        try
        {
            var renderers = TerrainFactory.CreateAllRenderers();
            if (renderers.Count == 0)
            {
                throw new ConfigurationException("No renderers exist. Ensure there is at least one Renderer");
            }
            return renderers;
        }
        catch (Exception ex)
        {
            throw new ConfigurationException("Failed to initialize renderers", ex);
        }
    }
    
    // Initialize all available procedural terrain generators by creating instances of them and checking for errors
    private static List<ITerrainGenerator> InitializeGenerators()
    {
        try
        {
            var generators = TerrainFactory.CreateAllGenerators();
            if (generators.Count == 0)
            {
                throw new ConfigurationException("No generators exist. Ensure there is at least one terrain generator");
            }
            return generators;
        }
        catch (Exception ex)
        {
            throw new ConfigurationException("Failed to initialize generators", ex);
        }
    }
    
    // Displays an error message to the user in red text
    private static void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n" + message);
        Console.ResetColor();
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey(true);
    }
}