using Microsoft.AspNetCore.Mvc;

namespace Nure.Commerce.Forecasting.Controllers
{
    [Route("documents")]
    public class DocumentsController
    {
        [HttpPost("report")]
        public async Task<IActionResult> GenerateReport([FromBody] string reportName)
        {
            return new OkObjectResult("ok");
        }
    }
}
