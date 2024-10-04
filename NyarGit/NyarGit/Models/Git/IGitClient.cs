using System.Collections.Generic;
using System.Threading.Tasks;

namespace NyarGit.Models.Git
{
    public interface IGitClient
    {
        string Path { get; init; } 
        GitResult Clone(string repoUrl);

        GitStatus GetStatus();

        GitResult StageAllChanges();
        GitResult Stage(string filePath);
        GitResult Stage(IEnumerable<string> filePaths);

        GitResult Commit(string message, string authorName, string authorEmail);

        GitResult Push();
    }
}
