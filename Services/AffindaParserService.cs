namespace RecruitmentManagementSystem.Services
{
    public interface IThirdPartyParser
    {
        Task<ParsedResume?> ParseResumeAsync(string filePath);
    }

    public class ParsedResume
    {
        public string? Skills { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    public class AffindaParserService : IThirdPartyParser
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public AffindaParserService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<ParsedResume?> ParseResumeAsync(string filePath)
        {
            // Example: POST file to parser API and parse response
            var apiKey = _config["Affinda:ApiKey"];
            var request = new HttpRequestMessage(HttpMethod.Post, _config["Affinda:Endpoint"]);
            var mp = new MultipartFormDataContent();
            mp.Add(new StringContent(apiKey), "api_key");
            mp.Add(new ByteArrayContent(await File.ReadAllBytesAsync(filePath)), "file", Path.GetFileName(filePath));
            request.Content = mp;

            var res = await _http.SendAsync(request);
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            // parse json into ParsedResume (map fields)
            return new ParsedResume { /* map fields */ };
        }
    }

}
