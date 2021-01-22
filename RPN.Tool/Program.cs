using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace RPN.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Argument<string>("expression", "RPN expression to be evaluated"),
                new Option<List<string>>(new string[] {"-p", "--parameters"}, description: "List of parameters (primitive or json)"),
                new Option<List<FileInfo>>(new string[] {"-f", "--files"}, description: "Files to be appended to parameters list"),
            };

            rootCommand.Description = "RPN Evaluator based on RPN.Net";

            rootCommand.Handler = CommandHandler.Create<string, List<string>, List<FileInfo>>((expression, parameters, files) =>
            {
                var parsedParameters = new List<object>();
                parsedParameters.AddRange(parameters);

                files.ForEach(file =>
                {
                    parsedParameters.Add(file.OpenText().ReadToEnd());
                });

                var val = (RPN.Evaluate(new RPNExpression(expression, parsedParameters.ToArray())).ToString());
                Console.WriteLine(val);
            });

            rootCommand.InvokeAsync(args);
        }
    }
}
