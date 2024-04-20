using MediaPipeHandTrackingBackend.Models;
using MediaPipeHandTrackingBackend.NatNet;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MediapipeHandTrackingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionSettingsController : ControllerBase
    {
        private readonly NatNetService natNetService;

        public ConnectionSettingsController(NatNetService service)
        {
            natNetService = service;
        }

        // GET: api/ConnectionSettings
        [HttpGet]
        public ConnectionSettings GetConnectionSettings()
        {
            return natNetService.ConnectionSettings;
        }

        // POST: api/ConnectionSettings
        [HttpPost]
        public ActionResult Post(ConnectionSettings connectionSettings)
        {
            natNetService.ConnectionSettings = connectionSettings;
            return Ok();
        }
    }
}
