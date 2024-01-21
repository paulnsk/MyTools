using System.Net;

namespace AspNetHelpers.Utils
{
    public class MyIpAddress(IPAddress? ipAddress)
    {
        public override string ToString()
        {
            return ipAddress?.IsIPv4MappedToIPv6 == true ? ipAddress.MapToIPv4().ToString() : ipAddress?.ToString() ?? "N/A";
        }
    }
}
