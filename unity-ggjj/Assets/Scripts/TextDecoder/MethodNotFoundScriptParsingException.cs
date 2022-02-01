namespace TextDecoder.Parser
{
    public class MethodNotFoundScriptParsingException : ScriptParsingException
    {
        public MethodNotFoundScriptParsingException(string className, string methodName) : base($"Class '{className}' contains no method '{methodName}()'")
        {
        }
    }
}