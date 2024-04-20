using MediaPipeHandTrackingBackend.Models;
using NatNetML;

namespace MediaPipeHandTrackingBackend.NatNet;

public class NatNetService
{
    private readonly NatNetClientML client = new NatNetClientML();
    public ConnectionSettings ConnectionSettings { get; set; } = new ConnectionSettings();
    public bool IsRecording { get; set; } = false;
}