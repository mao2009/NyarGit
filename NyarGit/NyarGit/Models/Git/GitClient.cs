using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace NyarGit.Models.Git
{
    public class GitClient : IGitClient
    {
        private readonly string _repoPath;


        public GitClient(string repoPath)
        {
            _repoPath = repoPath;
        }
        
        public GitStatus  GetStatus()
        {
            try
            {
                using var repo = new Repository(_repoPath);
                return new GitStatus(true, repo.RetrieveStatus(), "");
            }
            catch (LibGit2SharpException ex)
            {
                Debug.WriteLine($"Error retrieving status: {ex.Message}");
                return new GitStatus(false, null , ex.Message);
            }
        }

        public GitResult Add(string filePath)
        {
            try
            {
                using var repo = new Repository(_repoPath);
                Commands.Stage(repo, filePath);
                return new GitResult(true, "File added to stage.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding file to stage: {ex.Message}");
                return new GitResult(false, "Failed to add file.");
            }
        }

        public GitResult Commit(string message, string authorName, string authorEmail)
        {
            try
            {
                using var repo = new Repository(_repoPath);
                var author = new Signature(authorName, authorEmail, DateTimeOffset.Now);
                var committer = author;

                repo.Commit(message, author, committer);

                return new GitResult(true, "Commit successful.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error committing changes: {ex.Message}");
                return new GitResult(false, "Failed to commit changes.");
            }
        }

        
        public GitResult Push()
        {
            try
            {
                using var repo = new Repository(_repoPath);
                var currentBranch = repo.Branches.FirstOrDefault(x => x.IsCurrentRepositoryHead);

                if (currentBranch == null)
                {
                    return new GitResult(false, "No branch is currently checked out.");
                }

                repo.Network.Push(currentBranch);
                return new GitResult(true, "Push successful.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error pushing changes: {ex.Message}");
                return new GitResult(false, "Failed to push changes.");
            }
        }
    
        public GitResult CloneRepository(string repoUrl)
        {
            try
            {
                if (Repository.IsValid(_repoPath))
                {
                    Debug.WriteLine("Repository already exists at the specified path.");
                    return new GitResult(false, "Repository already exists.");
                }

                Repository.Clone(repoUrl, _repoPath);
                return new GitResult(true, "Repository cloned successfully.");
            }
            catch (LibGit2SharpException ex)
            {
                Debug.WriteLine($"Error cloning repository: {ex.Message}");
                return new GitResult(false, ex.Message);
            }
        }
    }
}
