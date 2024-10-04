namespace NyarGit.Models.Git
{
    public record GitResult
    {
        public bool Success { get; }
        public string Message { get; }

        public GitResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

}
