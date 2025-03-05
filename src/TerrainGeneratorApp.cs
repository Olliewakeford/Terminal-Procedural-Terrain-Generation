using TerminalProceduralTerrainGeneration.Models;
using TerminalProceduralTerrainGeneration.Renderers;
using TerminalProceduralTerrainGeneration.TerrainGenerators;

namespace TerminalProceduralTerrainGeneration;

// The main class that runs the terrain generator application
class TerrainGeneratorApp
{
    private readonly TerrainMap _map;  // The terrain map to apply procedural terrain generating algorithms
    private ConsoleRenderer _currentRenderer;  // The current renderer to visualize the terrain
    private readonly List<ConsoleRenderer> _renderers;  // A collection of available renderers for visualizing the terrain
    private readonly List<ITerrainGenerator> _generators;  // A collection of available generating algorithms for modifying the terrain
    
    /// <summary>
    /// Initializes a new instance of the Terrain Generator App class.
    /// </summary>
    /// <param name="map">The terrain map where to apply procedural terrain generating algorithms.</param>
    /// <param name="renderers">A collection of available renderers for visualizing the terrain.</param>
    /// <param name="generators">A collection of available generating algorithms for modifying the terrain.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the map, renderers, or generators are null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the renderers or generators collections are empty or contain null entries.
    /// </exception>
    public TerrainGeneratorApp(TerrainMap map, IEnumerable<ConsoleRenderer> renderers, IEnumerable<ITerrainGenerator> generators)
    {
        _map = map ?? throw new ArgumentNullException(nameof(map), "Terrain map cannot be null");

        // Validate renderers
        _renderers = renderers.ToList() ?? throw new ArgumentNullException(nameof(renderers), "Renderers collection cannot be null");
        if (_renderers.Count == 0)
        {
            throw new ArgumentException("At least one renderer must be provided", nameof(renderers));
        }

        // Validate generators
        _generators = generators.ToList() ?? throw new ArgumentNullException(nameof(generators), "Generators collection cannot be null");
        if (_generators.Count == 0)
        {
            throw new ArgumentException("At least one generator must be provided", nameof(generators));
        }
        
        // Set default renderer
        _currentRenderer = _renderers.First();
    }
    
    /// <summary>
    /// Runs the terrain generator application, displaying the menu and processing user input.
    /// </summary>
    public void Run()
    {
        DisplayHeader();

        while (true) // Run the program until exited
        {
            DisplayMenu();
            
            if (!ProcessUserChoice()) // Exit the program if user chooses to
                break;

            WaitForKeyPress();
            Console.Clear();
        }
    }

    // Neatly display the header menu when the program first runs
    private void DisplayHeader()
    {
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║      Terrain Generator       ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine($"Map Size: {_map.Width}x{_map.Height}");
        Console.WriteLine();
    }

    // Display the main menu to show the user the options of the terrain generator app
    private void DisplayMenu()
    {
        Console.WriteLine($"Current Renderer: {_currentRenderer.Name}");
        Console.WriteLine($"Current Map Size: {_map.Width}x{_map.Height}");
        Console.WriteLine("──────────────────────");
        
        Console.WriteLine("Available Generators:");
        Console.WriteLine("──────────────────────");
        
        // display all available generators
        for (int i = 0; i < _generators.Count; i++)
        {
            var generator = _generators[i];
            Console.WriteLine($"{i + 1}. {generator.Name}");
            Console.WriteLine($"   {generator.Description}");
            Console.WriteLine();
        }

        Console.WriteLine("──────────────────────");
        Console.WriteLine("R. Change Renderer");
        Console.WriteLine("0. Exit");
    }
    
    // Display the menu to show the different rendering options
    private void DisplayRendererMenu()
    {
        Console.WriteLine("\nAvailable Renderers:");
        Console.WriteLine("──────────────────────");
        
        for (int i = 0; i < _renderers.Count; i++)
        {
            var renderer = _renderers[i];
            Console.WriteLine($"{i + 1}. {renderer.Name}");
            Console.WriteLine($"   {renderer.Description}");
            Console.WriteLine();
        }
        
        Console.WriteLine("──────────────────────");
        Console.WriteLine("0. Back to Main Menu");
    }
    
    // Process the users choices on what they want to do with the terrain generator
    private bool ProcessUserChoice()  // returns false when the user wants to exit the application
    {
        while (true) // Loop until valid input is received
        {
            Console.Write("\nSelect option (generator number, R for renderer, 0 to exit): ");
            string input = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (input == "R") // Change renderer
            {
                ProcessRendererChoice();
                return true;
            }

            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out _))
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
                continue;
            }

            if (int.TryParse(input, out int choice))
            {
                if (choice == 0) return false;

                if (choice > 0 && choice <= _generators.Count)
                {
                    var generator = _generators[choice - 1];

                    GenerateTerrain(generator);
                    return true;
                }

                Console.WriteLine("\nInvalid choice! Please select a number between 0 and " + _generators.Count);
                return true;
            }
        }
    }
    
    // Change the renderer which affects how the map is displayed
    private void ProcessRendererChoice()
    {
        while (true)  // Loop until valid input is received
        {
            DisplayRendererMenu();

            Console.Write("\nSelect renderer (0 to cancel): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                continue;
            }

            if (choice == 0) return;

            if (choice > 0 && choice <= _renderers.Count)
            {
                _currentRenderer = _renderers[choice - 1];
                Console.WriteLine($"\nSwitched to {_currentRenderer.Name}");

                // Re-render the current terrain with the new renderer if map is not empty
                var stats = _map.GetStatistics();
                if (stats.max > 0)
                {
                    _currentRenderer.Render(_map);
                }

                return;
            }

            Console.WriteLine("\nInvalid choice! Please select a number between 0 and " + _renderers.Count);
        }
    }

    // Generate the terrain with all the chosen settings
    private void GenerateTerrain(ITerrainGenerator generator)
    {
        Console.WriteLine($"\nGenerating terrain using {generator.Name}...");
        Console.WriteLine("──────────────────────────────────────────");
        
        _map.Clear();
        generator.Generate(_map);
        _currentRenderer.Render(_map);
    }
    
    // A buffer to wait for the user to press any key before continuing 
    private void WaitForKeyPress()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }
}