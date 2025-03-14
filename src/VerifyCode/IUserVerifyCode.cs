namespace Pursue.Extension.Cryptologys
{
    public interface IUserVerifyCode
    {
        string CreateVerifyCode();
        bool VerifyCode(string code);
    }
}