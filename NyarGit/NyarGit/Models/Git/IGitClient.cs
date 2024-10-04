using System.Threading.Tasks;

namespace NyarGit.Models.Git
{
    public interface IGitClient
    {
        GitResult CloneRepository(string repoUrl);

        GitStatus GetStatus();

        GitResult Add(string filePath);

        GitResult Commit(string message, string authorName, string authorEmail);

        GitResult Push();
    }
}
