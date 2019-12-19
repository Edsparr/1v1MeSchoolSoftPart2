using Autofac;
using SchoolClassTester.Commands;
using SchoolClassTester.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolClassTester
{
    public class CommandListener
    {
        public CommandListener()
        {
        }

        public IContainer Container { get; private set; } = null;

        public void Initiate()
        {
            if (Container != null)
                throw new Exception(nameof(Container));

            var builder = new ContainerBuilder();
            builder.RegisterType<FileService>().InstancePerLifetimeScope()
                .As<IFileService>(); //Registers file service as IFileService

            var commands = typeof(ICommand).Assembly
                .GetTypes()
                .Where(c => !c.IsAbstract && c.GetInterfaces().Contains(typeof(ICommand))); //Finds all classes in this assembly (project) that inherits from ICommand and isn't abstract
            foreach(var command in commands)
            {
                builder.RegisterType(command).As<ICommand>(); //Register the command type as ICommand
                builder.RegisterDecorator(command, typeof(ICommand)); //Autofac thingy, makes it so we can resolve ICommand as a collection, aka more than 1 command ata time
            }

            Container = builder.Build(); //I build the container to show it's finished being configured
        }


        public void Start()
        {
            while (true)
            {
                string rawInput = Console.ReadLine(); 
                var splitted = rawInput.Split(' '); //Splitting for parsing
                var command = splitted.First(); //Command name
                string args = "";
                if(rawInput.Length > command.Length + 1)
                    args = rawInput.Remove(0, splitted.First().Length + 1); //Command length + the space means we got args after

                var foundCommands = Container.Resolve<IEnumerable<ICommand>>(); //I get all classes that are registred as ICommand which is all Commands
                var foundCommand = foundCommands.SingleOrDefault(c => c.Name.Equals(command, StringComparison.InvariantCultureIgnoreCase)); 
                //I get one command that has the same command name as the input command name
                if(foundCommand == null) //If I can't find one I tell the user and try again
                {
                    Console.WriteLine($"Command not found: {command}!");
                    continue;
                }
                var result = foundCommand.Execute(args); //I execute the command with args
                Console.WriteLine(result); //I print the result it gave me via return value.
            }
        }
    }
}
