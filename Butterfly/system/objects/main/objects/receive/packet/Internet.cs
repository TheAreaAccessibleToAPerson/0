using System;
using System.Collections.Generic;
using System.Text;

namespace Butterfly.system.objects.main.objects.receive.packet
{
    class Internet
    {

        public void Run()
        {
            byte[][][][][] ipAddress = new byte[247][][][][];

            int ip1 = 128;
            int ip2 = 0;
            int ip3 = 0;
            int ip4 = 1;

            //ipAddress[ip1][ip2][ip3][ip4] = "Hello";

            ipAddress[ip1] = new byte[255][][][];
            ipAddress[ip1][ip2] = new byte[255][][];
            ipAddress[ip1][ip2][ip3] = new byte[255][];
            ipAddress[ip1][ip2][ip3][ip4] = new byte[] { 128, 0, 0, 1 };
            ipAddress[1][1][1][1] = new byte[] { 22, 0, 0, 1 };

            System.Console.WriteLine(ipAddress[ip1][ip2][ip3][ip4][0]);
        }
    }
}
