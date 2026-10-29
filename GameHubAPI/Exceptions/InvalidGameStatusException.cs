namespace GameHubAPI.Exceptions
{
    public class InvalidGameStatusException : Exception
    {
        public InvalidGameStatusException(string message) : base(message) { }
    }
}
