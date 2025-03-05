namespace TerminalProceduralTerrainGeneration.Exceptions;

/// <summary>
/// Base exception class for all terrain generation related exceptions
/// </summary>
public class TerrainGenerationException : Exception
{
    protected TerrainGenerationException(string message) : base(message) { }
    
    public TerrainGenerationException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when there's an issue with terrain configuration (renderers or generators)
/// </summary>
public class ConfigurationException : TerrainGenerationException
{
    public ConfigurationException(string message) : base(message) { }
    
    public ConfigurationException(string message, Exception innerException) 
        : base(message, innerException) { }
}