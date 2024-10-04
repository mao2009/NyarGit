using System.Collections.Generic;
using LibGit2Sharp;

namespace NyarGit.Models.Git
{
    /// <summary>
    /// Represents the status of a Git operation.
    /// </summary>
    public record GitStatus
    {
        public RepositoryStatus?  Status { get; init; }

        public string Message { get; init; }

        public bool Success { get; init; }

        public GitStatus(bool success, RepositoryStatus? status, string message)
        {
            Success = success;
            Status = status;
            Message = message;
        }
        
     }
}