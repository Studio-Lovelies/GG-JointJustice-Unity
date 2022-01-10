using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Scanner;

public class MethodInfo
{
    private const string VALID_FILES_ATTRIBUTE_NAME = "validFiles";
    public string Name;
    public bool IsInstant;
    public string Summary;
    public Dictionary<string, (string parameterType, string parameterComment)> ParameterInfoByName = new();
    public List<string> Examples;

    public MethodInfo(XmlNode doc, string methodName, List<ParameterSyntax> parameterList, bool isInstant, ref Dictionary<string, string> parameterTypesToPaths)
    {
        Name = methodName;
        Summary = doc.SelectSingleNode("summary").FirstChild.Value;
        IsInstant = isInstant;
        Examples = doc.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "example").Select(node => node.FirstChild.Value).ToList();
        // sanity check
        foreach (var example in Examples.Where(example => !example.Contains(methodName)))
        {
            throw new NotSupportedException($"The following example for method '{methodName}'() doesn't contain the method name: '{example}'");
        }

        var parameters = doc.ChildNodes.Cast<XmlNode>().Where(node => node.Name.ToLower() == "param").ToList();
        var parametersWithFiles = parameters.Where(node => node.Attributes.Cast<XmlAttribute>().Any(attribute => attribute.Name == VALID_FILES_ATTRIBUTE_NAME));
        var parametersWithoutFiles = parameters.Where(node => node.Attributes.Cast<XmlAttribute>().All(attribute => attribute.Name != VALID_FILES_ATTRIBUTE_NAME));
        foreach (var parameter in parametersWithFiles)
        {
            var parameterName = parameter.Attributes.Cast<XmlAttribute>().First(attribute => attribute.Name == "name").FirstChild.Value;
            var pathQualifier = parameter.Attributes.Cast<XmlAttribute>().First(attribute => attribute.Name == VALID_FILES_ATTRIBUTE_NAME).FirstChild.Value.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

            var parameterType = parameterList.FirstOrDefault(parameter => parameter.Identifier.Text == parameterName)?.Type?.ToFullString().Trim();
            if (string.IsNullOrEmpty(parameterType))
            {
                throw new NotSupportedException($"Found a comment for parameter '{parameterName}' on method '{methodName}' but this method has no parameter matching that name");
            }

            if (parameterTypesToPaths.ContainsKey(parameterType))
            {
                if (parameterTypesToPaths[parameterType] != pathQualifier)
                {
                    throw new NotSupportedException($"A comment for the parameter '{parameterName}' of '{methodName}()' is attempting to map '{pathQualifier}' as '{VALID_FILES_ATTRIBUTE_NAME}' for '{parameterType}' parameters, but '{parameterTypesToPaths[parameterType]}' is already defined as a conflicting path in a previous method (Check all methods with '{parameterType}' parameters and make sure they all use the same value for '{VALID_FILES_ATTRIBUTE_NAME}'.)");
                };
                return;
            }

            parameterTypesToPaths.Add(
                parameterType,
                pathQualifier
            );
            ParameterInfoByName.Add(parameterName, (parameterType, parameter.FirstChild.Value));
        }

        foreach (var parametersWithoutFile in parametersWithoutFiles)
        {
            var parameterName = parametersWithoutFile.Attributes.Cast<XmlAttribute>().First(attribute => attribute.Name == "name").FirstChild.Value;
            var parameterType = parameterList.FirstOrDefault(parameter => parameter.Identifier.Text == parameterName)?.Type?.ToFullString();
            var comment = parametersWithoutFile.FirstChild.Value;
            ParameterInfoByName.Add(parameterName, (parameterType, comment));
        }
    }
}