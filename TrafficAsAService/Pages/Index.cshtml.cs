using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrafficAsAService.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public bool IsIPv6 { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        private void Initialize()
        {
            IPAddress ipAddress = HttpContext.Connection.RemoteIpAddress;
            IsIPv6 = ipAddress.AddressFamily == AddressFamily.InterNetworkV6 && !ipAddress.IsIPv4MappedToIPv6 ;
        }

        public void OnGet()
        {
            Initialize();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Initialize();

            if (!IsIPv6)
                return Page();

            IPAddress clientIp = HttpContext.Connection.RemoteIpAddress;

            _logger.LogInformation($"Client {clientIp} war blöd genug den Knopf zu drücken... Sende 100MB...");

            using var udpClient = new UdpClient(AddressFamily.InterNetworkV6);
            await using var fileStream = new FileStream("file.bin", FileMode.Open);

            Memory<byte> buffer = new byte[65000];
            while (true)
            {
                int read = await fileStream.ReadAsync(buffer, HttpContext.RequestAborted);
                if (read <= 0)
                    break;

                await udpClient.SendAsync(buffer.ToArray(), read, new IPEndPoint(clientIp, 4242));
            }

            _logger.LogInformation($"Client {clientIp} wurde erfolgreich beliefert.");

            return Page();
        }
    }
}
