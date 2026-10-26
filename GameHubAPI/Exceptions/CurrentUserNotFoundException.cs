namespace GameHubAPI.Exceptions
{
    public class CurrentUserNotFoundException : Exception
    {
        public CurrentUserNotFoundException(string message) : base(message) { }
    }
}
