using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LibGit2Sharp;
using NLog;
using NyarGit.Properties;

namespace NyarGit.Models.Git
{
    /// <summary>
    /// A class to manage a Git repository.
    /// </summary>
    public class GitRepository
    {
        private static readonly object _lock = new();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
        /// Retrieves the current status of the Git repository.
        /// </summary>
        /// <returns>
        /// A <see cref="GitStatus"/> object containing the status of the repository.
        /// The <see cref="GitStatus.Success"/> property will be true if the status was retrieved successfully.
        /// In case of an error, the <see cref="GitStatus.Success"/> property will be false, 
        /// and the <see cref="GitStatus.ErrorMessage"/> will contain the error message.
        /// </returns>
        /// <exception cref="LibGit2SharpException">
        /// Thrown when there is an error retrieving the status from the Git repository.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any other unforeseen errors during the execution of this method.
        /// </exception>
        public GitStatus GetStatus()
        {
            lock (_lock)
            {
                try
                {
                    return new GitStatus(true, Repository.RetrieveStatus(), "");
                }
                catch (LibGit2SharpException ex)
                {
                    Logger.Error($"{ErrorResource.FailedToRetrievingStatus}: {ex.Message}");
                    return new GitStatus(false, null, ErrorResource.FailedToRetrievingStatus);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ErrorResource.FailedToRetrievingStatus}: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Stages all changes in the Git repository.
        /// </summary>
        /// <returns>
        /// A <see cref="GitResult"/> object indicating the result of the staging operation.
        /// The <see cref="GitResult.Success"/> property will be true if the changes were staged successfully.
        /// If an error occurs, <see cref="GitResult.Success"/> will be false, 
        /// and <see cref="GitResult.ErrorMessage"/> will contain the error message.
        /// </returns>
        /// <exception cref="LibGit2SharpException">
        /// Thrown when there is an error staging the changes in the Git repository.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any other unforeseen errors during the execution of this method.
        /// </exception>

        public GitResult StageAllChanges()
        {
            lock (_lock)
            {
                try
                {
                    Commands.Stage(Repository, "*");
                    return new GitResult(true, "");
                }
                catch (LibGit2SharpException ex)
                {
                    Logger.Error($"{ErrorResource.FailedToStageChanges}: {ex.Message}");
                    return new GitResult(false, ErrorResource.FailedToCommitChanges);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ErrorResource.FailedToStageChanges}: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Stages the specified changes in the Git repository.
        /// </summary>
        /// <param name="path">The path of the file or directory to stage.</param>
        /// <returns>
        /// A <see cref="GitResult"/> object indicating the result of the staging operation.
        /// The <see cref="GitResult.Success"/> property will be true if the changes were staged successfully.
        /// If an error occurs, <see cref="GitResult.Success"/> will be false, 
        /// and <see cref="GitResult.ErrorMessage"/> will contain the error message.
        /// </returns>
        /// <exception cref="LibGit2SharpException">
        /// Thrown when there is an error staging the specified changes in the Git repository.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any other unforeseen errors during the execution of this method.
        /// </exception>

        public GitResult Stage(string path)
        {
            lock (_lock) { 
                try
                {
                    Commands.Stage(Repository, path);
                    return new GitResult(true, "");
                }
                catch (LibGit2SharpException ex)
                {
                    Logger.Error($"{ErrorResource.FailedToStageChanges}: {ex.Message}");
                    return new GitResult(false, ErrorResource.FailedToCommitChanges);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ErrorResource.FailedToStageChanges}: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Stages the specified changes in the Git repository.
        /// </summary>
        /// <param name="paths">An array of paths of the files or directories to stage.</param>
        /// <returns>
        /// A <see cref="GitResult"/> object indicating the result of the staging operation.
        /// The <see cref="GitResult.Success"/> property will be true if the changes were staged successfully.
        /// If an error occurs, <see cref="GitResult.Success"/> will be false, 
        /// and <see cref="GitResult.ErrorMessage"/> will contain the error message.
        /// </returns>
        /// <exception cref="LibGit2SharpException">
        /// Thrown when there is an error staging the specified changes in the Git repository.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any other unforeseen errors during the execution of this method.
        /// </exception>

        public GitResult Stage(string[] paths)
        {
            lock (_lock)
            {
                try
                {
                    Commands.Stage(Repository, paths);
                    return new GitResult(true, "");
                }
                catch (LibGit2SharpException ex)
                {
                    Logger.Error($"{ErrorResource.FailedToStageChanges}: {ex.Message}");
                    return new GitResult(false, ErrorResource.FailedToCommitChanges);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ErrorResource.FailedToStageChanges}: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Commits the staged changes to the Git repository.
        /// </summary>
        /// <param name="message">The commit message.</param>
        /// <param name="authorName">The name of the author of the commit.</param>
        /// <param name="authorEmail">The email address of the author of the commit.</param>
        /// <returns>
        /// Returns a <see cref="GitResult"/> object indicating the result of the commit operation.
        /// The <see cref="GitResult.Success"/> property will be true if the changes were successfully committed.
        /// If an error occurs, the <see cref="GitResult.Success"/> property will be false, and
        /// the <see cref="GitResult.ErrorMessage"/> will contain the error message.
        /// </returns>
        /// <exception cref="LibGit2SharpException">
        /// Thrown when an error occurs while committing changes to the Git repository.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any unexpected errors that occur during the execution of this method.
        /// </exception>

        public GitResult Commit(string message, string authorName, string authorEmail)
        {
            lock (_lock)
            {

                try
                {
                    var author = new Signature(authorName, authorEmail, DateTimeOffset.Now);
                    var committer = author;

                    Repository.Commit(message, author, committer);

                    return new GitResult(true, "");
                }
                catch (LibGit2SharpException ex)
                {
                    Logger.Error($"{ErrorResource.FailedToCommitChanges}: {ex.Message}");
                    return new GitResult(false, ErrorResource.FailedToCommitChanges);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ErrorResource.FailedToCommitChanges}: {ex.Message}");
                    throw;
                }
            }
        }


        /// <summary>
        /// Pushes the committed changes to the remote Git repository.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="GitResult"/> object indicating the result of the push operation.
        /// The <see cref="GitResult.Success"/> property will be true if the changes were successfully pushed.
        /// If the current branch does not exist or an error occurs, the 
        /// <see cref="GitResult.Success"/> property will be false, and
        /// the <see cref="GitResult.ErrorMessage"/> will contain the related error message.
        /// </returns>
        /// <exception cref="LibGit2SharpException">
        /// Thrown when an error occurs while pushing changes to the remote Git repository.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any unexpected errors that occur during the execution of this method.
        /// </exception>

        public GitResult Push()
        {
            lock (_lock)
            {
                try
                {
                    var currentBranch = Repository.Branches.FirstOrDefault(x => x.IsCurrentRepositoryHead);

                    if (currentBranch == null)
                    {
                        return new GitResult(false, ErrorResource.BranchDoesNotExist);
                    }

                    Repository.Network.Push(currentBranch);
                    return new GitResult(true, "");
                }
                catch (LibGit2SharpException ex)
                {
                    Logger.Error($"{ErrorResource.FailedToPushChanges}: {ex.Message}");
                    return new GitResult(false, ErrorResource.FailedToPushChanges);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ErrorResource.FailedToPushChanges}: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Pulls changes from the specified remote Git repository into the local repository.
        /// </summary>
        /// <param name="remoteName">The name of the remote repository from which to pull changes.</param>
        /// <param name="refSpec">The reference specification for the pull operation.</param>
        /// <param name="user">The name of the user performing the pull operation.</param>
        /// <param name="email">The email address of the user performing the pull operation.</param>
        /// <param name="options">Optional pull options to customize the pull behavior.</param>
        /// <returns>
        /// A <see cref="GitResult"/> object indicating the result of the pull operation.
        /// The <see cref="GitResult.Success"/> property will be true if the changes were pulled successfully.
        /// If an error occurs, <see cref="GitResult.Success"/> will be false, 
        /// and <see cref="GitResult.ErrorMessage"/> will contain the relevant error message.
        /// </returns>
        /// <exception cref="LibGit2SharpException">
        /// Thrown when there is an error pulling changes from the remote Git repository.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any other unforeseen errors during the execution of this method.
        /// </exception>

        public GitResult Pull(string remoteName, string refSpec, string user, string email, PullOptions? options)
        {
            try
            {
                Commands.Pull(Repository, new Signature(user, email, DateTime.Now), options);
                return new GitResult(true, string.Empty);
            }
            catch (LibGit2SharpException ex)
            {
                Logger.Error($"{ErrorResource.FailedToPullChanges}: {ex.Message}");
                return new GitResult(false, ErrorResource.FailedToPullChanges);
            }
            catch (Exception ex)
            {
                Logger.Error($"{ErrorResource.FailedToPullChanges}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a list of paths for all staged files in the Git repository.
        /// </summary>
        /// <returns>
        /// A list of strings representing the paths of all files that are currently staged in the repository.
        /// </returns>

        public List<string> GetStagedFiles()
        {
            return new List<string>(Repository.Index.Select(entry => entry.Path));
        }

        /// <summary>
        /// Checks out the specified branch in the Git repository.
        /// </summary>
        /// <param name="branchName">The name of the branch to check out.</param>
        /// <returns>
        /// A <see cref="GitResult"/> object indicating the result of the checkout operation.
        /// The <see cref="GitResult.Success"/> property will be true if the branch was checked out successfully.
        /// If the branch does not exist or an error occurs, 
        /// <see cref="GitResult.Success"/> will be false, 
        /// and <see cref="GitResult.ErrorMessage"/> will contain the relevant error message.
        /// </returns>
        /// <exception cref="LibGit2SharpException">
        /// Thrown when there is an error checking out the specified branch in the Git repository.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any other unforeseen errors during the execution of this method.
        /// </exception>

        public GitResult CheckoutBranch(string branchName)
        {
            lock (_lock)
            {
                try
                {
                    var branch = Repository.Branches[branchName];
                    if (branch != null)
                    {
                        Commands.Checkout(Repository, branch);
                        return new GitResult(true, string.Empty);
                    }
                    else
                    {
                        var errorMessage = string.Format(ErrorResource.BranchDoesNotExist, branchName);
                        Logger.Error(errorMessage);
                        return new GitResult(false, errorMessage);
                    }
                }
                catch (LibGit2SharpException ex)
                {
                    var errorMessage = string.Format(ErrorResource.BranchDoesNotExist, branchName);
                    Logger.Error($"{errorMessage}: {ex.Message}");
                    return new GitResult(false, ErrorResource.FailedToPullChanges);
                }
                catch (Exception ex)
                {
                    var errorMessage = string.Format(ErrorResource.BranchDoesNotExist, branchName);
                    Logger.Error($"{errorMessage}: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Clones a Git repository from the specified URL to the local path.
        /// </summary>
        /// <param name="repoUrl">The URL of the Git repository to clone.</param>
        /// <returns>
        /// A <see cref="GitResult"/> object indicating the result of the clone operation.
        /// The <see cref="GitResult.Success"/> property will be true if the repository was cloned successfully.
        /// If the repository already exists or an error occurs, 
        /// <see cref="GitResult.Success"/> will be false, 
        /// and <see cref="GitResult.ErrorMessage"/> will contain the relevant error message.
        /// </returns>
        /// <exception cref="LibGit2SharpException">
        /// Thrown when there is an error cloning the repository from the specified URL.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any other unforeseen errors during the execution of this method.
        /// </exception>

        public GitResult Clone(string repoUrl)
        {
            lock (_lock)
            {
                try
                {
                    if (Repository.IsValid(Path))
                    {
                        Logger.Error($"{ErrorResource.RepositoryAlreadyExists}");
                        return new GitResult(false, ErrorResource.RepositoryAlreadyExists);
                    }

                    Repository.Clone(repoUrl, Path);
                    Logger.Info(InfoResource.SuccessClone);
                    return new GitResult(true, InfoResource.SuccessClone);
                }
                catch (LibGit2SharpException ex)
                {
                    Logger.Error($"{ErrorResource.FailedToCloneRepository}: {ex.Message}");
                    return new GitResult(false, ErrorResource.FailedToCloneRepository);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ErrorResource.FailedToCloneRepository}: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
