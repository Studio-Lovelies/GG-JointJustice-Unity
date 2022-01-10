using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Scanner;

[DebuggerDisplay("{Item}: {Children}")]
public class PathItem
{
    public string Item { get; init; }
    public IEnumerable<PathItem> Children { get; init; }
}

public class XMLDocParser
{
    public Dictionary<string, string> ParameterTypesToPath = new();
    public Dictionary<string, List<MethodInfo>> MethodsByCategory = new();

    public void Parse(string methodName, string commentsForMethod, List<ParameterSyntax> parameterList)
    {
        var doc = new XmlDocument();
        doc.LoadXml(commentsForMethod);

        var root = doc.SelectSingleNode("root");

        var category = root.SelectSingleNode("category");
        var isInstant = bool.Parse(root.SelectSingleNode("isInstant").FirstChild.Value);
        if (category == null)
        {
            throw new FormatException("No <category> supplied");
        }

        if (!MethodsByCategory.ContainsKey(category.FirstChild.Value))
        {
            MethodsByCategory.Add(category.FirstChild.Value, new());
        }

        MethodsByCategory[category.FirstChild.Value].Add(new(root, methodName, parameterList, isInstant, ref ParameterTypesToPath));
    }

    public Dictionary<string, IEnumerable<PathItem>> ResolveAssets(string pathToAssetDirectory)
    {
        var absolutePathToGameDirectory = pathToAssetDirectory.TrimEnd(Path.DirectorySeparatorChar);
        var dependentPaths = new List<(string, string)>();
        var filesByType = new Dictionary<string, IEnumerable<PathItem>>();
        foreach (var (parameterType, parameterValuesPath) in ParameterTypesToPath)
        {
            if (parameterValuesPath.Contains("{"))
            {
                dependentPaths.Add((parameterType, parameterValuesPath));
                continue;
            }

            var folderPath = Path.GetDirectoryName(parameterValuesPath);
            var fileMask = Path.GetFileName(parameterValuesPath);
            filesByType[parameterType] = Directory.EnumerateFiles(folderPath, fileMask)
                .Select(filePath => filePath.Replace(pathToAssetDirectory, ""))
                .Select(Path.GetFileNameWithoutExtension)
                .Select(filePath => filePath.Split(Path.DirectorySeparatorChar).Last())
                .ToList()
                .Select(path=>new PathItem(){Item = path});
        }

        var subPathRegex = new Regex("\\{(.*)\\}");
        foreach (var (dependentType, dependentPath) in dependentPaths)
        {
            if (!filesByType.ContainsKey(dependentType))
            {
                filesByType[dependentType] = new List<PathItem>();
            }
            var key = subPathRegex.Match(dependentPath).Groups[1].Captures[0].Value;
            foreach (var subItem in filesByType[key])
            {
                var subFolder = dependentPath.Replace($"{{{key}}}", Path.GetFileNameWithoutExtension(subItem.Item));
                var folderPath = Path.GetDirectoryName(subFolder)!;
                var filePath = Path.GetFileName(subFolder)!;
                var folder = folderPath.Split(Path.DirectorySeparatorChar).Last();

                List<PathItem> abc = new List<PathItem>();
                foreach (var file in Directory.EnumerateFiles(folderPath, filePath))
                {
                    var files = file.Split(Path.DirectorySeparatorChar);
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    abc.Add(new PathItem(){Item = fileName });
                }

                filesByType[dependentType] = filesByType[dependentType].Concat(new List<PathItem>(){new() { Children = abc, Item = folder } });
            }

            filesByType[dependentType] = filesByType[dependentType].ToList();
        }

        return filesByType;
    }
}