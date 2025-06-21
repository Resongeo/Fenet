namespace Fenet.Net;

public class ResourceLoader
{
    private readonly HttpClient _httpClient = new();

    public async Task<string> LoadStringAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new Exception("URL cannot be null or empty");
        }

        Uri uri;

        try
        {
            uri = new Uri(url);
        }
        catch (UriFormatException e)
        {
            throw new Exception($"Invalid URL format: {url}", e);
        }

        if (uri.IsFile)
        {
            throw new NotImplementedException("File loading is not yet supported");
        }

        var response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}