using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SVGtoIMG.Desktop
{
    public static class Comun
    {
        public static string ProcessID { get; set; }

        public static void GenerateProcessID()
        {
            Comun.ProcessID = Guid.NewGuid().ToString();
        }

        public static void CleanProcessID()
        {
            Comun.ProcessID = string.Empty;
        }

        private static void ComparePrecessID()
        {
            if (Comun.ProcessID == null)
            {
                CleanProcessID();
            }
        }

        

        public static string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                /*log.Debug(
                    "Found MAC Address: " + nic.GetPhysicalAddress() +
                    " Type: " + nic.NetworkInterfaceType);*/

                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH &&
                    nic.OperationalStatus == OperationalStatus.Up
                    )
                {
                    //log.Debug("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }
    }
}
