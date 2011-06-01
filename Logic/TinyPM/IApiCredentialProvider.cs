namespace Librarian.Logic.TinyPM
{
    public interface IApiCredentialProvider
    {
        ApiCredential Credentials { get; }
    }
}