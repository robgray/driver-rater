namespace DriverRater.UI.Api;

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using DriverRater.Shared.Drivers.v1.Models;
using DriverRater.Shared.RacingService.v1.Models;

public class DriverRaterApi : IDriverRaterApi
{
    private readonly HttpClient client;
    private readonly User user;
    private readonly ILocalStorageService localStorage;

    public DriverRaterApi(
        HttpClient client,
        User user,
        ILocalStorageService localStorage)
    {
        this.client = client;
        this.user = user;
        this.localStorage = localStorage;
    }

    private async Task<List<T>> GetListItemFromLocalStorage<T>(string itemKey, string expiryKey, Func<Task<IEnumerable<T>>> fallbackToApi, int expirationMinutes = 30)
    {
        bool expirationExists = await localStorage.ContainKeyAsync(expiryKey);
        if (expirationExists)
        {
            DateTimeOffset expiration = await localStorage.GetItemAsync<DateTimeOffset>(expiryKey);
            if (expiration > DateTimeOffset.Now)
            {
                if (await localStorage.ContainKeyAsync(itemKey))
                {
                    return await localStorage.GetItemAsync<List<T>>(itemKey);
                }
            }
        }

        var recentRaces = (await fallbackToApi()).ToList();

        await localStorage.SetItemAsync(itemKey, recentRaces);
        await localStorage.SetItemAsync(expiryKey, DateTimeOffset.Now.AddMinutes(expirationMinutes));
        
        return recentRaces;
    }
    
    public async Task<IEnumerable<RecentMemberRaceSummary>> GetRecentRaces(int customerId)
    {
        return await GetListItemFromLocalStorage(
            LocalStorageKeys.RecentRaces,
            LocalStorageKeys.RecentRacesExpiration,
            async () => (await client.GetFromJsonAsync<IEnumerable<RecentMemberRaceSummary>>($"/api/v1/racingservice/{customerId}/recent"))
                        ?? throw new Exception("Null response returned"));
    }

    public async Task<IEnumerable<DriversRankModel>> GetDriversInRace(int subsessionId)
    {
        return await JsonSerializer.DeserializeAsync<IEnumerable<DriversRankModel>>
        (await client.GetStreamAsync($"/api/v1/racingservice/{subsessionId}/drivers/{user.Id}"),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() },
            });
    }
        
    public string DownloadHelmetPackForUserUrl() => $"/api/v1/helmetpack/{user.Id}";
    

    public string DownloadHelmetPackForAllUrl() => "/api/v1/helmetpack/all";

    public async Task<IEnumerable<DriversRankModel>> GetDriversForUser() =>
        await JsonSerializer.DeserializeAsync<IEnumerable<DriversRankModel>>
        (await client.GetStreamAsync($"/api/v1/driver/{user.Id}"),
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        });
     
    public async Task<UpdateDriverRankResponse> UpdateDriverRankResponse(UpdateDriverRankRequest request)
    {
        var response = await client.PostAsJsonAsync("/api/v1/driver/rank", request);
        return await response.Content.ReadFromJsonAsync<UpdateDriverRankResponse>()
               ?? throw new Exception("null response returned");

    }
}