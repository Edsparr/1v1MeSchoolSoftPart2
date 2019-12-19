using System;

namespace SchoolClassTester
{
    public static class Program
    {
        public static CommandListener CommandListener { get; } = new CommandListener();

        static void Main(string[] args)
        {
            CommandListener.Initiate();
            CommandListener.Start();

            
        }
    }
}
