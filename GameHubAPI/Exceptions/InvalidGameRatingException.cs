namespace GameHubAPI.Exceptions
{
    public class InvalidGameRatingException : Exception
    {
        public InvalidGameRatingException(string message) : base(message) { }
    }
}
