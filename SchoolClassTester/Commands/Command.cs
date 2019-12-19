using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolClassTester.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Command name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Execute method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        string Execute(string args);
    }

    public abstract class CommandBase : ICommand
    {
        public abstract string Name { get; }

        public abstract string Execute(string args);
    }
}
