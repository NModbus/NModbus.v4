using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
using NModbus.Transport.IP;
using NModbus.Transport.IP.ConnectionStrategies;
using System.Net;
using System.Net.Security;

namespace NModbus.Examples.ModbusClient;

internal class ModbusIpClientSampleTransportFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public ModbusIpClientSampleTransportFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public IModbusClientTransport CreateTcpInsecureClient(IPAddress serverAddress)
    {
        IStreamFactory streamFactory = new TcpStreamFactory(new(serverAddress, ModbusIPPorts.Insecure));

        return BuildClientTransport(streamFactory);
    }

    public IModbusClientTransport CreateUpdClient(IPAddress serverAddress)
    {
        IStreamFactory streamFactory = new UdpStreamFactory(new(serverAddress, ModbusIPPorts.Insecure), s =>
        {
            s.Client.ReceiveTimeout = 5000;
            s.Client.SendTimeout = 5000;
        });

        return BuildClientTransport(streamFactory);
    }

    public async Task<IModbusClientTransport> CreateTcpSecureClient(string serverDnsName, RemoteCertificateValidationCallback certificateValidationCallback)
    {
        var options = new SslClientAuthenticationOptions
        {
            TargetHost = serverDnsName,
            EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
            RemoteCertificateValidationCallback = certificateValidationCallback
        };

        IPHostEntry serverAddresses = await Dns.GetHostEntryAsync(serverDnsName);

        IStreamFactory streamFactory = new TcpStreamFactory(new IPEndPoint(serverAddresses.AddressList[0], ModbusIPPorts.Secure), null, options);

        return BuildClientTransport(streamFactory);
    }

    private IModbusClientTransport BuildClientTransport(IStreamFactory streamFactory)
    {
        IConnectionStrategy strategy = new SingletonStreamConnectionStrategy(streamFactory, _loggerFactory);

        return new ModbusIPClientTransport(strategy, _loggerFactory);
    }
}
