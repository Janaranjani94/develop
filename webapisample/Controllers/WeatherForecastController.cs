using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http.Json;

namespace webapisample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("cricketreport2")]
        public IActionResult GetCricketreport2()
        {
            return Ok("success");
        }

        [HttpGet("cricketreport")]       
        public IActionResult GetCricketreport()
        {
            string jsonFilePath = "C:\\Users\\VINOTH PC\\source\\repos\\webapisample\\webapisample\\Json\\cricketreport.json";
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            return Ok(jsonContent);
           
        }
        [HttpGet("cricketreport1")]
        public IActionResult GetCricketreport1()
        {
            Console.WriteLine("Hello, world!");

            string logMessage = "This is a dynamic log message.";

            // Write the log to Azure Blob Storage
            //    await WriteLogToAzureBlobAsync(logMessage);
            //await ReadBlobFromAzureStorageAsync();

            string logFolderPath = @"D:\home\LogFiles";
            string logFilePath = Path.Combine(logFolderPath, "app-log.txt");

            try
            {
                // Ensure the directory exists
                Directory.CreateDirectory(logFolderPath);

                // Append log message to the text file
                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    writer.WriteLine($"{DateTime.UtcNow}: {logMessage}");
                }

                Console.WriteLine("Log written to file.");
                return Ok("success");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }

            Console.WriteLine("Log written to Azure Blob Storage successfully!");

            return Ok("success");
            // Open a stream to write the log

        }
    }
}