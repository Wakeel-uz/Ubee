namespace Ubee.Service.Exceptions;

public class UbeeException : Exception
{
    public int Code { get; set; }
    public UbeeException(int code, string message) : base(message)
    {
        this.Code = code;
    } 
}
