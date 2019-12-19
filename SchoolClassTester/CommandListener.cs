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
                .As<IFileService>();

            var commands = typeof(ICommand).Assembly
                .GetTypes()
                .Where(c => !c.IsAbstract && c.GetInterfaces().Contains(typeof(ICommand)));
            foreach(var command in commands)
            {
                builder.RegisterType(command).As<ICommand>();
                builder.RegisterDecorator(command, typeof(ICommand));
            }

            Container = builder.Build();
        }


        public void Start()
        {
            while (true)
            {
                string rawInput = Console.ReadLine();
                var splitted = rawInput.Split(' ');
                var command = splitted.First();
                string args = "";
                if(rawInput.Length > command.Length + 1)
                    args = rawInput.Remove(0, splitted.First().Length + 1); //Command length + the space

                var foundCommands = Container.Resolve<IEnumerable<ICommand>>();
                var foundCommand = foundCommands.SingleOrDefault(c => c.Name.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if(foundCommand == null)
                {
                    Console.WriteLine($"Command not found: {command}!");
                    continue;
                }
                var result = foundCommand.Execute(args);
                Console.WriteLine(result);
            }
        }
    }
}
