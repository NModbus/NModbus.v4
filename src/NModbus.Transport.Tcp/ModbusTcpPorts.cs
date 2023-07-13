namespace NModbus.Transport.Tcp
{
    /// <summary>
    /// Default ports for Modbus over TCP.
    /// </summary>
    public static class ModbusTcpPorts
    {
        /// <summary>
        /// 502: mbap/TCP
        /// </summary>
        public const int Insecure = 502;

        /// <summary>
        /// 802: mbap/TLS/TCP
        /// </summary>
        public const int Secure = 802;
    }
}
