using RwNotificator;
using RwNotificator.Console;


Console.WriteLine("Enter train table schedule URL: ");
var scheduleUrl = Console.ReadLine()!;

var scraper = new TrainAvailabilityScraper(scheduleUrl);

var trainsList = await scraper.GetTrainsList();
var trainNumber = ConsoleTools.Select("Select the train you are interested in: ", trainsList);

Console.Write("Enter train check timeout(sec): ");
var timeoutSec = int.Parse(Console.ReadLine() ?? "20");

Console.Clear();

while (true)
{
    var isAvailable = await scraper.CheckTrainAvailabilityAsync(trainNumber);
    
    var msg = isAvailable ? $"Train {trainsList[trainNumber - 1]} is available now!" : $"Train {trainsList[trainNumber - 1]} is not available...";
    Console.ForegroundColor =  isAvailable ? ConsoleColor.DarkGreen: ConsoleColor.DarkRed;
    Console.WriteLine($"[{DateTime.Now}]: {msg}");

    if (isAvailable)
    {
        BeepPlayer.PlayAudio();
        
        Console.WriteLine(scheduleUrl);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        
        BeepPlayer.StopAudio();
        
        return;
    }
    
    Thread.Sleep(timeoutSec * 1000);
}