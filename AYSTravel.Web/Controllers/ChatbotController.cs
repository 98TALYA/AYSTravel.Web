using AYSTravel.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly ApiService _apiService;

        public ChatbotController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message))
                return BadRequest(new { error = "Message vide" });

            var response = await _apiService.GetChatbotResponse(
                request.Message,
                request.Context ?? ""
            );

            return Ok(new { response });
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
        public string Context { get; set; }
    }
}