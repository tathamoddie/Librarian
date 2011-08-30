using System.Collections.Generic;

namespace Librarian.Logic.TinyPM
{
    public interface IApiClient
    {
        bool Ping();
        IEnumerable<Project> GetAllProjects();
        IEnumerable<UserStory> GetBacklog(int projectId);
        void SetUserStoryColor(int storyId, string color);
        void SetUserStoryPosition(int storyId, int position);
    }
}