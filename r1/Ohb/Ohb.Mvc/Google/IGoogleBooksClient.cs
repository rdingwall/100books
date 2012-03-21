namespace Ohb.Mvc.Google
{
    public interface IGoogleBooksClient
    {
        GoogleVolume GetVolume(string id);
    }
}