namespace NuoroLight.Application.Security.PassWordHashing;
public interface IPasswordHelper
{
    string EncodePasswordSha512(string password);
}

