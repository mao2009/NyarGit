using System.Collections.Generic;
using LibGit2Sharp;

namespace NyarGit.Models.Git
{
    /// <summary>
    /// Represents the status of a Git operation.
    /// </summary>
    public record GitStatus
    {
        /// <summary>
        /// Gets the status of the repository.
        /// </summary>
        public RepositoryStatus? Status { get; init; }

        /// <summary>
        /// Gets the message associated with the Git operation.
        /// </summary>
        public string Message { get; init; } = string.Empty;

        /// <summary>
        /// Indicates whether the Git operation was successful.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitStatus"/> record.
        /// </summary>
        /// <param name="success">Indicates if the operation was successful.</param>
        /// <param name="status">The status of the repository.</param>
        /// <param name="message">A message related to the operation.</param>
        public GitStatus(bool success, RepositoryStatus? status, string message)
        {
            Success = success;
            Status = status;
            Message = message;
        }
        
     }
}