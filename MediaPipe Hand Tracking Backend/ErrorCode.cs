using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPipe_Hand_Tracking_Backend
{
    public enum ErrorCode
    {
        OK,
        Internal,
        External,
        Network,
        Other,
        InvalidArgumnet,
        InvalidOperation
    }

    public static class ErrorCodeExtensions
    {
        public static string Description(this ErrorCode error) => error switch
        {
            ErrorCode.OK => "Operation successful",
            ErrorCode.Internal => "Suspect internal errors. Contact support.",
            ErrorCode.External => "External errors. Make sure correct parameters are used for input arguments when calling the methods.",
            ErrorCode.Network => "The error occurred on the network side.",
            ErrorCode.Other => "Unlisted error is conflicting the method call.",
            ErrorCode.InvalidArgumnet => "Invalid input arguments have been inputted.",
            ErrorCode.InvalidOperation => "Invalid operation.",
            _ => $"Unknown error code {error}."
        };
    }
}
