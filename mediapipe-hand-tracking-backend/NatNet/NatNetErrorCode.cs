﻿namespace MediaPipeHandTrackingBackend.NatNet;

/// <summary>
/// Error codes returned by the NatNet SDK.
/// </summary>
public enum NatNetErrorCode
{
    OK,
    Internal,
    External,
    Network,
    Other,
    InvalidArgumnet,
    InvalidOperation
}

public static class NatNetErrorCodeExtensions
{
    /// <summary>
    /// Returns a human-readable description of the error code.
    /// </summary>
    public static string Description(this NatNetErrorCode error) => error switch
    {
        NatNetErrorCode.OK => "Operation successful",
        NatNetErrorCode.Internal => "Suspect internal errors. Contact support.",
        NatNetErrorCode.External => "External errors. Make sure correct parameters are used for input arguments when calling the methods.",
        NatNetErrorCode.Network => "The error occurred on the network side.",
        NatNetErrorCode.Other => "Unlisted error is conflicting the method call.",
        NatNetErrorCode.InvalidArgumnet => "Invalid input arguments have been inputted.",
        NatNetErrorCode.InvalidOperation => "Invalid operation.",
        _ => $"Unknown error code {error}."
    };
}
