using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myConsoleApp
{
    class ArgsOptions
    {
        [Option('f', "file", Required = true, HelpText = "需要处理的文件")]
        public IEnumerable<string> Files { get; set; }
        [Option('o', "override", Required = false, HelpText = "是否覆盖原有文件")]
        public bool Override { get; set; }
    }

    [Verb("check", HelpText = "检查")]
    class CheckOptions
    {
        [Value(0, HelpText = "一个 .sln 文件，一个或者多个 .csproj 文件。")]
        public IEnumerable<string> InputFiles { get; set; }
    }
    [Verb("fix", HelpText = "修复")]
    class FixOptions
    {
        [Value(0, HelpText = "一个 .sln 文件，一个或者多个 .csproj 文件。")]
        public IEnumerable<string> InputFiles { get; set; }
        [Option('o', "outputFiles", Required = true, HelpText = "修复之后的文件集合。")]
        public IEnumerable<string> OutputFiles { get; set; }
        [Option('r', "override", Required = false, HelpText = "是否覆盖文件")]
        public bool Override { get; set; }
    }

    class CmdLineParserExample
    {
        public void DoArgs(string []args)
        {
            Parser.Default.ParseArguments<ArgsOptions>(args).WithParsed(Run);

            
        }

        public int DoArgs_MoreEnvironment(string[] args)
        {
            var exitCode = Parser.Default.ParseArguments<CheckOptions, FixOptions>(args)
                         .MapResult(
                                  (CheckOptions o) => CheckSolutionOrProjectFiles(o),
                                  (FixOptions o) => FixSolutionOrProjectFiles(o), error => 1);
            return exitCode;
        }
        private void Run(ArgsOptions obj)
        {
            foreach (var file in obj.Files)
            {
                var verb = obj.Override;
                Console.WriteLine($" {file} {verb}");
            }
        }
        private  int CheckSolutionOrProjectFiles(CheckOptions options) {

            Console.WriteLine($"{options.InputFiles.Count()} ");
            return 0; 
        }
        private  int FixSolutionOrProjectFiles(FixOptions options) {
            Console.WriteLine($"{options.InputFiles.Count()} {options.OutputFiles.Count()} {options.Override}");
            return 0; 
        }


    }
}
