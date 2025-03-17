# Terminal Procedural Terrain Generator - Developer Documentation

This document provides technical information for developers who want to understand or extend the Terminal Procedural Terrain Generator application.

## Project Structure

```
TerminalProceduralTerrainGeneration/
├── Models/                  # Data structures and interfaces
│   ├── ITerrainMap.cs       # Interface for terrain maps
│   └── TerrainMap.cs        # Implementation of terrain map
├── TerrainGenerators/       # Terrain generation algorithms
│   ├── ITerrainGenerator.cs # Interface for terrain generators
│   ├── RandomTerrainGenerator.cs
│   ├── PerlinNoiseTerrainGenerator.cs
│   └── MidpointDisplacementGenerator.cs
├── Renderers/               # Visualization components
│   ├── ConsoleRenderer.cs   # Base renderer class
│   ├── BlackAndWhiteRenderer.cs
│   └── ColourHeightRenderer.cs
├── Exceptions/              # Custom exceptions
│   └── TerrainGenerationExceptions.cs
├── Services/                # Utilities
│   └── TerrainFactory.cs    # Factory for creating components
├── Program.cs               # Application entry point
└── TerrainGeneratorApp.cs   # Main application logic
```

## Core Interfaces and Classes

### Models

#### `ITerrainMap` (Interface)
Defines the contract for a two-dimensional terrain height map. Key features:
- Provides properties for width and height
- Indexer for accessing and modifying height values
- Methods for clearing the map and retrieving statistics

#### `TerrainMap` (Class)
Implementation of the `ITerrainMap` interface that:
- Stores terrain height data in a 2D array
- Normalizes height values between 0.0 and 1.0
- Provides validation for coordinates
- Calculates statistics about the terrain (min/max/average heights)

### TerrainGenerators

#### `ITerrainGenerator` (Interface)
Contract for all terrain generation algorithms with:
- `Generate` method for populating a terrain map
- Properties for name and description

#### Concrete Generator Implementations
- `RandomTerrainGenerator`: Simple random noise generator
- `PerlinNoiseTerrainGenerator`: Realistic terrain using Perlin noise
- `MidpointDisplacementGenerator`: Fractal terrain using diamond-square algorithm

### Renderers

#### `ConsoleRenderer` (Abstract Class)
Base class for all console-based renderers that:
- Defines common rendering operations
- Provides methods for rendering header, terrain, legend, and statistics
- Encapsulates error handling during rendering

#### Concrete Renderer Implementations
- `BlackAndWhiteRenderer`: ASCII-based visualization
- `ColourHeightRenderer`: Colored block characters for visualization

### Application Components

#### `TerrainGeneratorApp` (Class)
Main application class that:
- Controls the application flow
- Manages user interaction through console menus
- Coordinates terrain generation and rendering

#### `TerrainFactory` (Static Class)
Factory class that:
- Creates all available terrain generators
- Creates all available renderers
- Serves as the registration point for new components

## Exception Handling

The application uses a custom exception hierarchy:

- `TerrainGenerationException`: Base exception for all terrain-related errors
- `ConfigurationException`: For configuration issues with renderers or generators

Exceptions are handled at the appropriate level, with user-friendly error messages displayed to the user.

## Extending the Application

### Adding a New Terrain Generator

1. Create a new class in the `TerrainGenerators` directory
2. Implement the `ITerrainGenerator` interface
3. Register your generator in `TerrainFactory.CreateAllGenerators()`

Example:

```csharp
using TerminalProceduralTerrainGeneration.Models;

namespace TerminalProceduralTerrainGeneration.TerrainGenerators;

public class MyNewGenerator : ITerrainGenerator
{
    // Implement required properties
    public string Name => "My New Generator";
    public string Description => "Description of how this generator works";

    // Implement terrain generation logic
    public void Generate(TerrainMap map)
    {
        // Your terrain generation algorithm here
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                // Calculate height values and set them
                map[x, y] = /* your height calculation */;
            }
        }
    }
}
```

Then register it in `TerrainFactory.cs`:

```csharp
public static List<ITerrainGenerator> CreateAllGenerators()
{
    return
    [
        new RandomTerrainGenerator(),
        new PerlinNoiseTerrainGenerator(),
        new MidpointDisplacementGenerator(),
        new MyNewGenerator() // Add your new generator here
    ];
}
```

### Adding a New Renderer

1. Create a new class in the `Renderers` directory
2. Extend the `ConsoleRenderer` abstract class
3. Implement the required abstract methods
4. Register your renderer in `TerrainFactory.CreateAllRenderers()`

Example:

```csharp
using TerminalProceduralTerrainGeneration.Models;

namespace TerminalProceduralTerrainGeneration.Renderers;

public class MyNewRenderer : ConsoleRenderer
{
    public MyNewRenderer() 
        : base("My New Renderer", "Description of this rendering style")
    {
    }
    
    protected override void RenderTerrain(TerrainMap map)
    {
        // Your terrain rendering logic here
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                float height = map[x, y];
                // Render each cell based on height
                // ...
            }
        }
    }
    
    // Optionally override other methods
    protected override void RenderLegend(TerrainMap map)
    {
        base.RenderLegend(map);
        // Add custom legend rendering
    }
}
```

Then register it in `TerrainFactory.cs`:

```csharp
public static List<ConsoleRenderer> CreateAllRenderers()
{
    return
    [
        new BlackAndWhiteRenderer(),
        new ColourHeightRenderer(),
        new MyNewRenderer() // Add your new renderer here
    ];
}
```

## Application Flow

1. `Program.cs` initializes the application, creates the terrain map, and initializes renderers and generators
2. `TerrainGeneratorApp` handles the main loop, displaying menus and processing user input
3. When a generator is selected, it populates the terrain map with height data
4. The current renderer visualizes the terrain by rendering each cell based on its height value
5. Statistics are calculated and displayed about the terrain

## Future Development Ideas

- Add way to save/load terrain maps
- Implement more complex terrain generation algorithms
- Add rendering to view the terrain from side angle
- Include smoothing and erosion methods
- Give the user more control on terrain size
- Allow the user to set parameters of generating algorithms
- Use reflection library so new renderers and generators are included automatically in the program
