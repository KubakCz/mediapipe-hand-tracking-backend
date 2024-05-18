using MediaPipeHandTrackingBackend.Models;
using MediaPipeHandTrackingBackend.NatNet;
using Microsoft.AspNetCore.Mvc;

namespace MediapipeHandTrackingBackend.Controllers;

/// <summary>
/// Controller for managing the connection settings for the NatNet server.
/// </summary>
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
    /// <summary>
    /// Gets the current connection settings for the NatNet server.
    /// </summary>
    /// <returns>Connection settings of the current NatNet server. 404 Not Found if not connected to a server.</returns>
    [HttpGet]
    public ActionResult<NatNetConnectionSettings> GetConnectionSettings()
    {
        if (natNetService.ConnectionSettings == null)
            return NotFound("Not connected to a NatNet server.");
        return natNetService.ConnectionSettings;
    }

    // POST: api/NatNetConnectionSettings
    /// <summary>
    /// Sets the connection settings for the NatNet server and tries to connect to it.
    /// </summary>
    /// <param name="connectionSettings">Connection settings for the server.</param>
    /// <returns>200 OK if the connection was successful, 503 Service Unavailable otherwise.</returns>
    [HttpPost]
    public ActionResult PostConnectionSettings(NatNetConnectionSettings connectionSettings)
    {
        bool result = natNetService.TryConnectToServer(connectionSettings);
        if (!result)
            return StatusCode(503, "Unable to connect to the NatNet server. Please ensure that the server is running and the connection settings are correct.");
        return Ok("Successfully connected to the NatNet server.");
    }

    // POST: api/NatNetConnectionSettings/Default
    /// <summary>
    /// Gets the default connection settings for the NatNet server.
    /// </summary>
    /// <returns>Default connection settings for the NatNet server.</returns>
    [HttpGet("Default")]
    public NatNetConnectionSettings GetDefaultConnectionSettings()
    {
        return new NatNetConnectionSettings();
    }

    // POST: api/NatNetConnectionSettings/Default
    /// <summary>
    /// Sets the connection settings for the NatNet server to the default values and tries to connect.
    /// </summary>
    /// <returns>200 OK if the connection was successful, 503 Service Unavailable otherwise.</returns>
    [HttpPost("Default")]
    public ActionResult PostDefaultConnectionSettings()
    {
        return PostConnectionSettings(new NatNetConnectionSettings());
    }
}
