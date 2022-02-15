using System.Collections.Generic;
using System.Reflection;

public struct InvocationDetails
{
    public MethodInfo MethodInfo;
    public List<object> ParsedMethodParameters;
}