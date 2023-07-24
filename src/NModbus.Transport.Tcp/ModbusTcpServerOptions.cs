using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace NModbus.Transport.Tcp
{
    /// <summary>
    /// Options for a modbus Tcp Server.
    /// </summary>
    public class ModbusTcpServerOptions
    {
        /// <summary>
        /// To enable TLS, specify a certificate.
        /// </summary>
        public X509Certificate Certificate { get; set; }

        /// <summary>
        /// Flag that determines whether a client certificate is required.
        /// </summary>
        public bool ClientCertificateRequired { get; set; }

        /// <summary>
        /// Flag that controls whether CertficateRevocation is checked.
        /// </summary>
        public bool CheckCertificateRevocation { get; set; } = true;

        /// <summary>
        /// Optional action to configure the <see cref="SslStream"/>.
        /// </summary>
        public Action<SslStream> ConfigureSslStream { get; set; }

        /// <summary>
        /// Sets the allowed SSL protocols.
        /// </summary>
        public SslProtocols SslProtocols { get; set; } = SslProtocols.Tls12;

        /// <summary>
        /// Delegate to validate the client certificate.
        /// </summary>
        public RemoteCertificateValidationCallback ClientCertificateValidation { get; set; }
    }
}
