using MediaPipeHandTrackingBackend.NatNet;
using Microsoft.AspNetCore.Mvc;

namespace MediapipeHandTrackingBackend.Controllers;

/// <summary>
/// Controller for managing the recording state of the NatNet server.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RecordingController : ControllerBase
{
    private readonly NatNetService natNetService;

    public RecordingController(NatNetService service)
    {
        natNetService = service;
    }

    // GET: api/Recording
    /// <summary>
    /// Gets the current recording state of the NatNet server.
    /// </summary>
    [HttpGet]
    public ActionResult<bool> GetRecording()
    {
        return natNetService.IsRecording;
    }

    // POST: api/Recording
    /// <summary>
    /// Starts or stops recording on the NatNet server.
    /// </summary>
    /// <param name="recording">True to start recording, false to stop recording.</param>
    /// <returns>200 OK if the recording was started or stopped successfully, 
    ///          409 Conflict if the server is not connected or the recording is already in progress/stopped,
    ///          503 Service Unavailable if connection to the server failed,
    ///          500 Internal Server Error if starting/stopping recording failed for another reason.</returns>
    [HttpPost]
    public ActionResult PostRecording(bool recording)
    {
        Console.WriteLine($"Trying to {(recording ? "start" : "stop")} recording...");

        if (!natNetService.IsConnected)
            return Conflict("Not connected to a NatNet server.");
        if (recording == natNetService.IsRecording)
            return Conflict(recording ? "Recording already in progress." : "Recording already stopped.");

        NatNetErrorCode result;
        if (recording)
            result = natNetService.StartRecording();
        else
            result = natNetService.StopRecording();

        if (result == NatNetErrorCode.Network)
            return StatusCode(503, $"Unable to connect to the server. Please ensure that the server is running and the connection settings are correct.");
        if (result != NatNetErrorCode.OK)
            return StatusCode(500, $"Failed to {(recording ? "start" : "stop")} recording. Error code: {result}");

        Console.WriteLine($"Recording {(recording ? "started" : "stopped")}.");
        return Ok(recording ? "Recording started." : "Recording stopped.");
    }
}
