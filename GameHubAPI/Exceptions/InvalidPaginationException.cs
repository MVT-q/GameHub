namespace GameHubAPI.Exceptions
{
    public class InvalidPaginationException : Exception
    {
        public InvalidPaginationException(string message) : base(message) { }
    }
}
