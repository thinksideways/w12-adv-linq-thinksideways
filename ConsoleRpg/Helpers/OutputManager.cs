namespace ConsoleRpg.Helpers;

// This file caused major headaches with handling basic user interactions.

// There are bugs with the WriteLine and Display when followed by Readlines in more complex control flow situations.

// Most of these templates haven't worked well for me out of the box and they always require so much troubleshooting
// before I can even run them and start the assigned tasks.

// I don't know if it's from being a VS Code user on an M4 Mac or if these are all just as untested as they seem.

public class OutputManager
{
    private readonly List<(string message, ConsoleColor color)> _outputBuffer; // A list of messages with associated colors

    public OutputManager()
    {
        _outputBuffer = new List<(string message, ConsoleColor color)>();
    }

    public void Clear()
    {
        Console.Clear();
        _outputBuffer.Clear();
    }

    public void Display()
    {
        foreach (var (message, color) in _outputBuffer)
        {
            WriteColorToConsole(message, color); // Write stored messages with color
        }

        _outputBuffer.Clear(); // Clear the buffer after displaying
    }

    public void Write(string message, ConsoleColor color = ConsoleColor.White)
    {
        _outputBuffer.Add((message, color));
    }

    public void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
    {
        _outputBuffer.Add((message + Environment.NewLine, color));
    }

    private void WriteColorToConsole(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color; // Set the text color
        Console.Write(message); // Write the message to the console
        Console.ResetColor(); // Reset the console color back to default
    }
}
