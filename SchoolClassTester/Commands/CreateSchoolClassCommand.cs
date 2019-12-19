using SchoolClassTester.SchoolClasses;
using SchoolClassTester.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolClassTester.Commands
{
    public class CreateSchoolClassCommand : CommandBase
    {
        private readonly IFileService fileService;

        public CreateSchoolClassCommand(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public override string Name => "createclass";

        public override string Execute(string args)
        {
            if (args.Length < 2) //I check if args.Length < 2, then the format isn't correct.
                return "You need to assign prefix and suffix e.g 8H!";

            string suffix = args.Last().ToString(); //I don't use char because the prefix could possibly be longer than 1 char, just happens to parsing don't want it that way

            string prefix = args.Remove(args.Length - 1);

            if (fileService.DoesClassExist(prefix, suffix)) //Checks if a file with the name prefix-suffix exists, if so then the class exists
                return $"{prefix}{suffix} already exists!";

            var schoolClass = new SchoolClass() //Initiates a new instance of school class
            {
                ClassPrefix = prefix,
                ClassSuffix = suffix
            };
            fileService.SaveClass(schoolClass); //Saves it so it gets saved.
            return $"Succesfully created class {prefix}{suffix}";
        }
    }
}
