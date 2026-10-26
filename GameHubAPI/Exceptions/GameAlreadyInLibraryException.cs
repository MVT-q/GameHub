namespace GameHubAPI.Exceptions
{
    public class GameAlreadyInLibraryException : Exception
    {
        public GameAlreadyInLibraryException(string message) : base(message) { }
    }
}
