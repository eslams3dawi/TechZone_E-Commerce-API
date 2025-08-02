namespace TechZone.BLL.Wrappers
{
    public class Result<T>
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public ActionCode ActionCode { get; set; }

        public string? Token { get; set; }
        public IList<string?> Roles { get; set; }
        public Result()
        {
            Id = Guid.NewGuid().ToString();
            DateTime = DateTime.UtcNow;
        }
        public static Result<T> SuccessAndSendToken(string token, IList<string> roles, ActionCode code = ActionCode.Success)
            => new Result<T> { Token = token, Roles = roles, Succeeded = true, ActionCode = code };
        public static Result<T> Success(T data, string message = "", ActionCode code = ActionCode.Success)
            => new Result<T> { Succeeded = true, Message = message, Data = data, ActionCode = code};
        public List<string> Errors { get; set; }
        public static Result<T> Failure(string message,List<string>? errors, ActionCode code = ActionCode.InternalServerError)
            => new Result<T> { Succeeded = false, Message = message, Errors = errors, ActionCode = code};
    }
    public enum ActionCode
    {   
        Success = 0,
        InternalServerError = 1,
        NotFound = 2,
        Unauthorized = 3,
        ValidationError = 4,
        BadRequest = 5,
        NullReference = 6
    }
}
