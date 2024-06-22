namespace RwNotificator.Console;

public static class ConsoleTools
{
    public static int Select(string message, string[] options)
    {
        var selectedIndex = 0;
    
        System.Console.CursorVisible = false;
        while (true)
        {
            System.Console.ResetColor();
            System.Console.WriteLine(message);
            
            for (var i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                else
                {
                    System.Console.ResetColor();
                }
    
                System.Console.WriteLine(options[i]);
            }
    
            var keyInfo = System.Console.ReadKey();
    
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = options.Length - 1; // wrap around to the bottom
                }
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedIndex++;
                if (selectedIndex >= options.Length)
                {
                    selectedIndex = 0; // wrap around to the top
                }
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }
    
            System.Console.Clear(); // Clear the console for the next drawing of the menu
        }
    
        System.Console.CursorVisible = true;
        System.Console.ResetColor();
        
        return selectedIndex + 1;
    }
}