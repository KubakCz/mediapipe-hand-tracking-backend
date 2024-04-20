using System.ComponentModel.DataAnnotations;
using System.Net;
using NatNetML;

namespace MediaPipeHandTrackingBackend.Models;

public class NatNetConnectionSettings
{
    [IPAddress]
    public string LocalIP { get; set; } = "127.0.0.1";
    [IPAddress]
    public string ServerIP { get; set; } = "127.0.0.1";
    public ushort CommandPort { get; set; } = 1510;
    public ushort DataPort { get; set; } = 1511;
    public ConnectionType ConnectionType { get; set; } = ConnectionType.Multicast;

    public override string ToString()
    {
        return $"Local IP: {LocalIP}\nServer IP: {ServerIP}\nCommand Port: {CommandPort}\nData Port: {DataPort}\nConnection Type: {ConnectionType}";
    }
}

public class IPAddressAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string stringValue && IPAddress.TryParse(stringValue, out _))
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult("Invalid IP address");
    }
}