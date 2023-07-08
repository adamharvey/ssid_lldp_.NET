//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using Windows.Devices.WiFi;
using System.Collections.ObjectModel;
using System.Collections;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Microsoft.VisualBasic;
using Windows.Security.Credentials;

namespace WiFiScan
{
    public class TestScan
    {
        private WiFiAdapter firstAdapter;
        private static ArrayList wlanList;
//        private static ArrayList wifiList;

        static async Task Main(String[] args)
        {
            TestScan ts = new TestScan();
            Console.WriteLine("Listing SSID's ::");
            await ts.initializeAdapter();
            Console.WriteLine("Finished");
            //            foreach (var item in wifiList)
            //            {
            //                WiFiAdapter wiFiAdapter = (WiFiAdapter)item;
            //                Task task = await DisplayNetworkReport(wiFiAdapter.NetworkReport);
            //            }

            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                // No access
            }

            var adapters = await WiFiAdapter.FindAllAdaptersAsync();
            var firstAdapter = adapters.First();
            Console.WriteLine("***** Before scan *****");
            await firstAdapter.ScanAsync();
            Console.WriteLine("***** After scan  *****");
            Console.WriteLine("Adapter info:  " + firstAdapter.NetworkAdapter.NetworkAdapterId);
            Console.WriteLine("Adapter Network Report (count):  " + firstAdapter.NetworkReport.AvailableNetworks.Count);
//            var wifiNetwork = firstAdapter.NetworkReport.AvailableNetworks.First(x => x.Ssid == "Wifi_Network_Name");

//            Console.WriteLine("wifiNetwork:  " + wifiNetwork.Ssid);

        }

        protected async Task initializeAdapter()
        {
            // RequestAccessAsync must have been called at least once by the app before using the API
            // Calling it multiple times is fine but not necessary
            // RequestAccessAsync must be called from the UI thread
            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                Console.WriteLine("Access denied");
            }
            else
            {
                var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                Console.WriteLine("Device Result count:  " + result.Count);
                if (result.Count >= 1)
                {
                    firstAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                }
                else
                {
                    Console.WriteLine("No WiFi Adapters detected on this machine");
                }
//                for(int i  = 0; i < result.Count; i++)
//                {
//                    wifiList.Add(await WiFiAdapter.FromIdAsync(result[i].Id));
//                }
            }
        }

        private async Task scan()
        {
            await firstAdapter.ScanAsync();
            await DisplayNetworkReport(firstAdapter.NetworkReport);
        }

        private async Task DisplayNetworkReport(WiFiNetworkReport report)
        {
            Console.WriteLine(string.Format("Network Report String:  {0}", report.AvailableNetworks.Count));
            Console.WriteLine(string.Format("Network Report Timestamp: {0}", report.Timestamp));

            await Task.Run(() => Parallel.ForEach(report.AvailableNetworks, network =>
            {
                Console.WriteLine("Adding network to list:  " + network.Ssid.ToString());
                wlanList.Add(network);
            }));

//            foreach (var network in report.AvailableNetworks)
 //           {
 //               Console.WriteLine("Adding network to list:  " + network.Ssid);
 //               wlanList.Add(network);
 //           }
        }
    }
}