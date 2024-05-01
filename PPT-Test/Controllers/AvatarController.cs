using Microsoft.AspNetCore.Mvc;
using PPT_Test.Model;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

[Route("api/[controller]")]
[ApiController]
public class AvatarController : ControllerBase
{
    private readonly ImagesContext _context;
    private readonly HttpClient _httpClient;

    public AvatarController(ImagesContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserImage(string userId)
    {
        string? imageUrl;


        char lastDigit = userId.Last();

        //If the last character of the user identifier is [6, 7, 8, 9]..... Contains.....
        if ("6789".Contains(lastDigit))
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/{lastDigit}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                // Deserialize JSON response into a dictionary
                var imageInfo = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                // Extract the URL from the dictionary
                imageUrl = imageInfo?["url"]?.ToString();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching image from JSON server: {ex.Message}");
                return StatusCode(500, "Error fetching image from JSON server");
            }
        }
        //If the user last character of the user identifier is [1, 2, 3, 4, 5].... Contains
        else if ("12345".Contains(lastDigit))
        {
            //DB context _context
            var image = _context.Images.FirstOrDefault(i => i.Id == int.Parse(lastDigit.ToString()));
            imageUrl = image?.Url;
        }
        //If the user identifier contains at least one vowel character ( aeiou ), display the image from.... Contains
        else if (userId.Any(c => "aeiouAEIOU".Contains(c)))
        {
            imageUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150";
        }
        //If the user identifier contains a non-alphanumeric character..... Random
        else if (userId.Any(c => !char.IsLetterOrDigit(c)))
        {
            //...pick a random number between 1-5 and display the image with the appropriate seed......
            var randomNumber = new Random().Next(1, 6);
            imageUrl = $"https://api.dicebear.com/8.x/pixel-art/png?seed={randomNumber}&size=150";
        }
        //If the user is Mr-X....
        else if (userId == "MRX")
        {
            imageUrl = "https://localhost:7048/IMG/oip.jpeg";
        }
        //If none of the above conditions are met....
        else
        {
            imageUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
        }

        return Ok(new { imageUrl });
    }
}
