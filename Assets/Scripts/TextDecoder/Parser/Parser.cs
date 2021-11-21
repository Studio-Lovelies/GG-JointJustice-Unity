namespace TextDecoder.Parser
{
    /// <summary>
    ///     Base class responsible for parsing parameters from .ink files
    /// </summary>
    /// <see cref="ActionDecoder.OnNewActionLine" />
    /// <typeparam name="T">The type handled in a parser derivative</typeparam>
    public abstract class Parser<T>
    {
        /// <summary>
        ///     Converts .ink file representation of a parameter to a usable object
        /// </summary>
        public abstract string Parse(string input, out T output);
    }
}