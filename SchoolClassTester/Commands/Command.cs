using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolClassTester.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Execute(string args);
    }
    public abstract class CommandBase : ICommand
    {
        public abstract string Name { get; }

        public abstract string Execute(string args);
    }
}
