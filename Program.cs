using System.Diagnostics;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;

namespace WiFiScan
{
    public class TestScan
    {
        static string GetLanIdentifierData(LanIdentifier lanIdentifier)
        {
            string lanIdentifierData = string.Empty;
            if (lanIdentifier == null)
            {
                return lanIdentifierData;
            }

            if (lanIdentifier.InfrastructureId != null)
            {
                lanIdentifierData += "Infrastructure Type: " + lanIdentifier.InfrastructureId.Type + "\n";
                lanIdentifierData += "Infrastructure Value: ";
                var infrastructureIdValue = lanIdentifier.InfrastructureId.Value;
                foreach (var value in infrastructureIdValue)
                {
                    lanIdentifierData += value + " ";
                }
            }

            if (lanIdentifier.PortId != null)
            {
                lanIdentifierData += "\nPort Type : " + lanIdentifier.PortId.Type + "\n";
                lanIdentifierData += "Port Value: ";
                var portIdValue = lanIdentifier.PortId.Value;
                foreach (var value in portIdValue)
                {
                    lanIdentifierData += value + " ";
                }
            }

            if (lanIdentifier.NetworkAdapterId != null)
            {
                lanIdentifierData += "\nNetwork Adapter Id : " + lanIdentifier.NetworkAdapterId + "\n";
            }
            return lanIdentifierData;
        }

        static async Task Main(String[] args)
        {
            // First way; using Windows WiFiAdapter:
            Console.WriteLine("; begin 1st way ; Windows WiFiAdapter ;");

            WiFiAdapter firstAdapter;
            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                Console.WriteLine("No access allowed to WiFiAdapter");
            }

            var adapters = await WiFiAdapter.FindAllAdaptersAsync();
            firstAdapter = adapters.First();
            Console.WriteLine("***** Before scan *****");
            await firstAdapter.ScanAsync();
            Console.WriteLine("***** After scan  *****");
            Console.WriteLine("Adapter info:  " + firstAdapter.NetworkAdapter.NetworkAdapterId);
            Console.WriteLine("Adapter Network Report (count):  " + firstAdapter.NetworkReport.AvailableNetworks.Count);
            foreach (var network in firstAdapter.NetworkReport.AvailableNetworks)
            {
                Console.WriteLine("Network Ssid:  " + network.Ssid);
                Console.WriteLine(network.ToString());
            }
            Console.WriteLine("***** End network ssid list *****");


            Console.WriteLine("; begin 1st way ; Windows WiFiAdapter ;");
            var lanIdentifiers = NetworkInformation.GetLanIdentifiers();
            Console.WriteLine("Number of lanIdentifiers found:  " + lanIdentifiers.Count);
            foreach (var lanIdentifier in lanIdentifiers) {
                Console.WriteLine("lanIdentifier data:  " + GetLanIdentifierData(lanIdentifier));
            }

            // Second way; using netsh

            Console.WriteLine("; begin 2nd way ; netsh ;");
            Process pProcess = new Process();
            pProcess.StartInfo.FileName = "cmd.exe";
            pProcess.StartInfo.Arguments = "/c netsh wlan show all 2>&1";
            pProcess.Start();
            pProcess.WaitForExit();
        }
    }
}