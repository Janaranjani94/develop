using Microsoft.AspNetCore.Mvc;
using webapisample.Data;

namespace webapisample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeytestController : ControllerBase
    {
        private KeytestRepository _keytestRepository;
        private readonly ILogger<KeytestController> _logger;
        public KeytestController(KeytestRepository keytestRepository, ILogger<KeytestController> logger)
        {
            _keytestRepository = keytestRepository;
            _logger = logger;
        }

        [HttpGet("getcollegeresponse")]
        public async Task<IActionResult> GetCollegeResponse()
        {
            try
            {
                string secretvalue = await _keytestRepository.GetDBConnectionfromAzure(); 
                _logger.LogInformation("Secret value : {0}", secretvalue);
                string jsonData = _keytestRepository.FetchDBValues(secretvalue);
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in Get method.");
                return BadRequest(ex.ToString());
            }
         

        }
    }
}
