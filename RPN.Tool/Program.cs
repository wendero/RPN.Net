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
                List<object> parsedParameters = ParseParameters(parameters, files);
                try
                {
                    var val = (RPN.Evaluate(new RPNExpression(expression, parsedParameters.ToArray())).ToString());
                    Console.WriteLine(val);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(1);
                }
            });

            rootCommand.InvokeAsync(args);
        }

        private static List<object> ParseParameters(List<string> parameters, List<FileInfo> files)
        {
            try
            {
                var parsedParameters = new List<object>();
                parsedParameters.AddRange(parameters);

                files.ForEach(file =>
                {
                    parsedParameters.Add(file.OpenText().ReadToEnd());
                });
                return parsedParameters;
            }
            catch
            {
                Console.WriteLine("ERROR: Error parsing parameters");
                Environment.Exit(2);
                return null;
            }
        }
    }
}
