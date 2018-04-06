using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace ReviewBot
{
    class PullRequestInfo
    {
        static Octokit.GitHubClient Client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("DarkstarProject-ReviewBot"));
        Octokit.PullRequestEventPayload Payload;
        public string User, Diff, DiffUrl, CommitHash;

        public PullRequestInfo(Octokit.PullRequestEventPayload payload)
        {
            Payload = payload;
            User = payload.PullRequest.User.Name;
            DiffUrl = payload.PullRequest.DiffUrl;
            CommitHash = payload.PullRequest.Head.Sha;

            byte[] buf;
            using (var client = new WebClient())
            {
                buf = client.DownloadData(DiffUrl);
            }
            Diff = UTF8Encoding.UTF8.GetString(buf);
        }
    };

    public class Bot
    {
        Queue<PullRequestInfo> Payloads;
        GitSharp.Repository Repo;
        string Name = "DankStar";        

        public Bot()
        {
            Payloads = new Queue<PullRequestInfo>();       
        }

        public void Pull(Octokit.PullRequestEventPayload payload)
        {
            if (Repo == null)
            {
                var dir = Path.Combine(Environment.CurrentDirectory, payload.Repository.Owner.Name, payload.Repository.Name);
                if (!Directory.Exists(dir))
                    Repo = GitSharp.Git.Clone($"https://github.com/{payload.Repository.Owner.Name}/{payload.Repository.Name}.git", dir);
                else
                    Repo = Repo ?? new GitSharp.Repository(dir);
            }
        }

        public void Queue(Octokit.PullRequestEventPayload payload)
        {
            lock (Payloads)
                Payloads.Enqueue(new PullRequestInfo(payload));

            Pull(payload);
        }

        private void Run()
        {
            // todo: 
        }
    }
}
