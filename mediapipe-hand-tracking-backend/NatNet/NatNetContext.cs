using MediaPipeHandTrackingBackend.Models;
using NatNetML;

namespace MediaPipeHandTrackingBackend.NatNet;

public class NatNetContext
{
    private NatNetClientML natNet = new NatNetClientML();
    private ConnectionSettings connectionSettings = new ConnectionSettings();
    private bool isRecording = false;
}