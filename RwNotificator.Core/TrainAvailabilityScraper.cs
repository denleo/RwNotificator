using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;

namespace RwNotificator;

public partial class TrainAvailabilityScraper
{
    private readonly string _scheduleUrl;
    private readonly HttpClient _httpClient = new(
        new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromHours(2),
        }
    );
    
    [GeneratedRegex("https://pass.rw.by/ru/route/.*")]
    private static partial Regex RwRouteRegex();
    
    public TrainAvailabilityScraper(string scheduleUrl)
    {
        ArgumentNullException.ThrowIfNull(scheduleUrl);
        
        if (!RwRouteRegex().IsMatch(scheduleUrl))
            throw new ArgumentException("Invalid url parameter", nameof(scheduleUrl));
        
        _scheduleUrl = scheduleUrl;
    }

    public async Task<bool> CheckTrainAvailabilityAsync(int trainNumber)
    {
        var document = await FetchDocument();
        
        var trainsTable = document.QuerySelector(".sch-table__body-wrap>.sch-table__body");
        var trainsRecords = trainsTable!.QuerySelectorAll(".sch-table__row");
        
        return !HasNoSeats(trainsRecords[trainNumber - 1]);
    }
    
    public async Task<string[]> GetTrainsList()
    {
        var document = await FetchDocument();
        
        var trainsTable = document.QuerySelector(".sch-table__body-wrap>.sch-table__body");
        var trainsRecords = trainsTable!.QuerySelectorAll(".sch-table__row");

        return trainsRecords
            .Select(tr => tr.QuerySelector(".train-number")!.TextContent)
            .ToArray();
    }

    private async Task<IDocument> FetchDocument()
    {
        var response = await _httpClient.GetAsync(_scheduleUrl);
        var htmlContent = await response.Content.ReadAsStringAsync();
        
        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        return await context.OpenAsync(req => req.Content(htmlContent));
    }
    
    private static bool HasNoSeats(IParentNode element)
    {
        return element.QuerySelector(".sch-table__cell.cell-4")
            !.ClassList.Contains("empty");
    }
}