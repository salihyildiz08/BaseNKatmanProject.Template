namespace BaseNKatmanProject.Core.Commons;
public class ResponseMessage<T>
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = "İşlem başarılı.";
    public T Data { get; set; }

    public static ResponseMessage<T> SuccessResult(T data, string message = "İşlem başarılı.")
        => new() { Success = true, Message = message, Data = data };

    public static ResponseMessage<T> Failure(string message)
        => new() { Success = false, Message = message };
}