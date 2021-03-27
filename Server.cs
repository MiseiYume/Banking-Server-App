using System;
using Operations;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            Operation Operate = new Operation();
            Operate.Listen();
        }
    }
}
