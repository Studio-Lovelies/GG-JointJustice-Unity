using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Scanner;

public class Scanner
{
    private static Dictionary<string, (string, List<ParameterSyntax>, bool)> GenerateMethodInfo(string sourceCode)
    {
        var tree = SyntaxFactory.ParseCompilationUnit(sourceCode);
        var members = tree.Members;
        var classMember = (ClassDeclarationSyntax)members[0];
        var syntaxToken = classMember.Identifier;
        var className = (string)syntaxToken.Value;
        Debug.Assert(className == "ActionDecoder");
        var methodName = new Regex("^[A-Z_]+$");
        var relevantMethods = classMember.Members.Where(m => m is MethodDeclarationSyntax syntax && methodName.IsMatch(syntax.Identifier.Text));
        return relevantMethods.ToDictionary(
            relevantMethod => ((MethodDeclarationSyntax)relevantMethod).Identifier.Text,
            relevantMethod => {
                var comments = relevantMethod.GetLeadingTrivia().FirstOrDefault(trivia => trivia.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia);
                if (comments == default)
                {
                    return ("", null, false);
                }
                var fullComment = comments.ToFullString();
                return (string.Join("\n", fullComment.Split("\n").Select(line => line.Replace("///", "").Trim())), ((MethodDeclarationSyntax)relevantMethod).ParameterList.Parameters.ToList(), ((MethodDeclarationSyntax)relevantMethod).Body!.GetText(Encoding.UTF8).ToString().Contains("OnActionDone?.Invoke()"));
            });
    }

    public static void Main()
    {
        var relativePathToAssetFolder = Path.Join(Environment.CurrentDirectory, "Assets");
        if (!Directory.Exists(relativePathToAssetFolder))
        {
            throw new NotSupportedException($"This app's working directory must be the root of the Unity project (current location: '{Environment.CurrentDirectory}')");
        }
        var parser = new XMLDocParser();

        var methodInfoByName = GenerateMethodInfo(File.ReadAllText(Path.Join(relativePathToAssetFolder, "Scripts", "TextDecoder", "ActionDecoder.cs")));
        foreach (var (methodName, (comments, parameterList, isInstant)) in methodInfoByName)
        {
            if (string.IsNullOrEmpty(comments))
            {
                continue;
            }

            parser.Parse(methodName, $"<root>{comments}<isInstant>{isInstant}</isInstant></root>", parameterList);
        }

        var info = parser.ResolveAssets(Path.Join(Environment.CurrentDirectory));
        File.WriteAllText(Path.Join(Environment.CurrentDirectory, "..", "docs", "constants.md"), GenerateAssetDocument(info));
        foreach (var (category, methods) in parser.MethodsByCategory)
        {
            var methodFolderPath = Path.Join(Environment.CurrentDirectory, "..", "docs", "methods");
            Directory.CreateDirectory(methodFolderPath);
            File.WriteAllText(Path.Join(methodFolderPath, $"{category}.md"), GenerateFileForMethods(methods, parser.ParameterTypesToPath.Keys));
        }
    }

    private static string GenerateFileForMethods(List<MethodInfo> methods, IEnumerable<string> complexTypes)
    {
        return string.Join("\n\n", methods.Select(method => GenerateTextForMethod(method, complexTypes)));
    }

    private static string GenerateTextForMethod(MethodInfo methodInfo, IEnumerable<string> complexTypes)
    {
        var methodName = $"## {methodInfo.Name}";
        var parameterInfo = methodInfo.ParameterInfoByName.Select(pair => {
            if (complexTypes.Contains(pair.Value.parameterType))
            {
                return $"  - [{pair.Value.parameterComment}](../constants.md#{pair.Value.parameterType})";
            }
            return $"  - {pair.Value.parameterComment}";
        }).ToList();
        var values = parameterInfo.Any() ? $"Values: \n{string.Join("\n", parameterInfo)}" + "\n" : "";
        var example = $"Examples: \n{string.Join("\n", methodInfo.Examples.Select(example => $"  - `{example}`"))}";
        var description = $"{(methodInfo.IsInstant ? "Instant" : "Waits for completion")}\n\n{methodInfo.Summary}";
        return string.Join("\n", methodName, values, description, "", example);
    }

    private static string GenerateAssetDocument(Dictionary<string, IEnumerable<PathItem>> info)
    {
        var regularValues = info.Where(entry => entry.Value.First().Children != null).Select(store => {
            var (key, value) = store;
            return new KeyValuePair<string, IEnumerable<PathItem>>(key, value.OrderBy(entry => entry.Item));
        });
        var nestedValues = info.Where(entry => entry.Value.First().Children == null).Select(store => {
            var (key, value) = store;
            return new KeyValuePair<string, IEnumerable<PathItem>>(key, value.OrderBy(entry => entry.Item));
        });

        var constantsOutput = "# Available constants" + "\n";

        foreach (var (constant, value) in nestedValues)
        {
            constantsOutput += $"## {constant}\n";
            constantsOutput += string.Join("\n", value.Select(v => $"  - {v.Item}"));
            constantsOutput += "\n";
            constantsOutput += "\n";
        }

        foreach (var (constant, value) in regularValues)
        {
            constantsOutput += $"## {constant}\n";
            foreach (var character in value)
            {
                constantsOutput += $"### {character.Item}\n";
                constantsOutput = character.Children.Aggregate(constantsOutput, (current, pose) => current + $"  - {pose.Item}\n");
            }

            constantsOutput += "\n";
        }

        return constantsOutput;
    }
}