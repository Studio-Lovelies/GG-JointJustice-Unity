using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Scanner;

[DebuggerDisplay("{Item}: {Children}")]
public class PathItem
{
    public string Item { get; init; }
    public string Description { get; init; }
    public string RelativeIconPath { get; init; }
    public bool IsComplex => !string.IsNullOrEmpty(Description) || !string.IsNullOrEmpty(RelativeIconPath);
    public IEnumerable<PathItem> Children { get; init; }

    public PathItem()
    {
        
    }

    public PathItem(string absolutePathToAssetsDirectory, Dictionary<string, string> pathsByGUID, string relativeFileName)
    {
        Item = Path.GetFileNameWithoutExtension(relativeFileName);
        if (!relativeFileName.EndsWith(".asset"))
        {
            return;
        }
        
        var contents = File.ReadAllText(Path.Join(absolutePathToAssetsDirectory, relativeFileName));
        if (contents[0] == '%')
        {
            contents = string.Join("\n", contents.Split('\n').Skip(3));
        }

        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .Build();
        var asset = deserializer.Deserialize<dynamic>(contents)["MonoBehaviour"];
        string type = Path.GetFileNameWithoutExtension(pathsByGUID[asset["m_Script"]["guid"]]);
        switch (type)
        {
            case "Evidence":
                Description = asset["<Description>k__BackingField"];
                var iconField = asset["<Icon>k__BackingField"];
                if (iconField["fileID"] == "0")
                {
                    throw new NotSupportedException($"Evidence '{relativeFileName}' has no icon sprite");
                }

                string relativeIconPath = Path.Join("..", Path.GetFileName(absolutePathToAssetsDirectory), "Assets", pathsByGUID[iconField["guid"]]);
                RelativeIconPath = relativeIconPath.Replace(Path.DirectorySeparatorChar, '/');
                break;
            case "ActorData":
                Description = asset["<Bio>k__BackingField"];
                var profileField = asset["<Profile>k__BackingField"];
                if (profileField["fileID"] == "0")
                {
                    throw new NotSupportedException($"Actor '{relativeFileName}' has no profile sprite");
                }
                string relativeProfilePath = Path.Join("..", Path.GetFileName(absolutePathToAssetsDirectory), "Assets", pathsByGUID[profileField["guid"]]);
                RelativeIconPath = relativeProfilePath.Replace(Path.DirectorySeparatorChar, '/');
                break;
        }
    }
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

    public Dictionary<string, IEnumerable<PathItem>> ResolveAssets(string absolutePathToAssetDirectory, Dictionary<string, string> relativePathsByGUID)
    {
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
                .ToList()
                .Select(path=>new PathItem(absolutePathToAssetDirectory, relativePathsByGUID, path){});
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
                foreach (var absoluteFilePath in Directory.EnumerateFiles(folderPath, filePath))
                {
                    abc.Add(new PathItem(absolutePathToAssetDirectory, relativePathsByGUID, absoluteFilePath));
                }

                filesByType[dependentType] = filesByType[dependentType].Concat(new List<PathItem>(){new() { Children = abc, Item = folder } });
            }

            filesByType[dependentType] = filesByType[dependentType].ToList();
        }

        return filesByType;
    }
}