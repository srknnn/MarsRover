using System;

namespace Nasa.MarsRover.Command.Interpret
{
    [Serializable]
    public class CommandException : Exception
    {
        public CommandException(string message, Exception innerException) : base(message, innerException) {}
    }
}