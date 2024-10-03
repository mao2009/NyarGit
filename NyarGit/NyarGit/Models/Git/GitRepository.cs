using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using NyarGit.Properties;

namespace NyarGit.Models.Git
{
    /// <summary>
    /// A class to manage a Git repository.
    /// </summary>
    public class GitRepository
    {
        /// <summary>
        /// Gets the path of the repository.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the LibGit2Sharp repository object.
        /// </summary>
        public Repository Repository { get; private set; }

        /// <summary>
        /// Initializes a new Git repository at the specified path.
        /// </summary>
        /// <param name="path">The path of the repository.</param>
        public GitRepository(string path)
        {
            Path = path;
            Repository = new Repository(path);
        }

        /// <summary>
        /// Stages all changes.
        /// </summary>
        public void StageAllChanges()
        {
            try
            {
                Commands.Stage(Repository, "*"); // Stage all changes
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorResource.FailedToStageChanges}: {ex.Message}");
            }
        }

        /// <summary>
        /// Commits changes with the specified message.
        /// </summary>
        /// <param name="message">The commit message.</param>
        /// <param name="authorName">The author's name.</param>
        /// <param name="authorEmail">The author's email address.</param>
        public void CommitChanges(string message, string authorName, string authorEmail)
        {
            try
            {
                var author = new Signature(authorName, authorEmail, DateTime.Now);
                var committer = author;
                Repository.Commit(message, author, committer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorResource.FailedToCommitChanges}: {ex.Message}");
            }
        }

        /// <summary>
        /// Pushes changes to the specified remote.
        /// </summary>
        /// <param name="remoteName">The name of the remote.</param>
        /// <param name="refSpec">The reference to push.</param>
        /// <param name="options">Push options.</param>
        public void PushChanges(string remoteName, string refSpec, PushOptions? options)
        {
            try
            {
                var remote = Repository.Network.Remotes[remoteName];
                Repository.Network.Push(remote, refSpec, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorResource.FailedToPushChanges} {ex.Message}");
            }
        }

        /// <summary>
        /// Pulls changes from the specified remote.
        /// </summary>
        /// <param name="remoteName">The name of the remote.</param>
        /// <param name="refSpec">The reference to pull.</param>
        /// <param name="user">The user's name.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="options">Pull options.</param>
        public void PullChanges(string remoteName, string refSpec, string user, string email, PullOptions? options)
        {
            try
            {
                var remote = Repository.Network.Remotes[remoteName];
                Commands.Pull(Repository, new Signature(user, email, DateTime.Now), options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorResource.FailedToPullChanges}: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the list of staged files.
        /// </summary>
        /// <returns>A list of paths of staged files.</returns>
        public List<string> GetStagedFiles()
        {
            return new List<string>(Repository.Index.Select(entry => entry.Path));
        }

        /// <summary>
        /// Checks out the specified branch.
        /// </summary>
        /// <param name="branchName">The name of the branch to check out.</param>
        public void CheckoutBranch(string branchName)
        {
            try
            {
                var branch = Repository.Branches[branchName];
                if (branch != null)
                {
                    Commands.Checkout(Repository, branch);
                }
                else
                {
                    Console.WriteLine(string.Format(ErrorResource.BranchDoesNotExist, branchName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorResource.FailedToStageChanges} {ex.Message}");
            }
        }
    }
}
