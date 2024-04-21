using MediaPipeHandTrackingBackend.Models;
using MediaPipeHandTrackingBackend.NatNet;
using Microsoft.AspNetCore.Mvc;


namespace MediapipeHandTrackingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NatNetConnectionSettingsController : ControllerBase
    {
        private readonly NatNetService natNetService;

        public NatNetConnectionSettingsController(NatNetService service)
        {
            natNetService = service;
        }

        // GET: api/NatNetConnectionSettings
        [HttpGet]
        public ActionResult<NatNetConnectionSettings> GetConnectionSettings()
        {
            if (natNetService.ConnectionSettings == null)
                return NotFound("Not connected to a NatNet server.");
            return natNetService.ConnectionSettings;
        }

        // POST: api/NatNetConnectionSettings
        [HttpPost]
        public ActionResult PostConnectionSettings(NatNetConnectionSettings connectionSettings)
        {
            bool result = natNetService.TryConnectToServer(connectionSettings);
            if (!result)
                return StatusCode(503, "Unable to connect to the server. Please ensure that the server is running and the connection settings are correct.");
            return Ok("Successfully connected to the NatNet server.");
        }

        // POST: api/NatNetConnectionSettings/Default
        [HttpGet("Default")]
        public NatNetConnectionSettings GetDefaultConnectionSettings()
        {
            return new NatNetConnectionSettings();
        }

        // POST: api/NatNetConnectionSettings/Default
        [HttpPost("Default")]
        public ActionResult PostDefaultConnectionSettings()
        {
            return PostConnectionSettings(new NatNetConnectionSettings());
        }
    }
}
