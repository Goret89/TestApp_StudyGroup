namespace TestApp
{
    public interface IStudyGroupRepository
    {
        public Task CreateStudyGroup(StudyGroup studyGroup);

        public Task<IList<StudyGroup>> GetStudyGroups();

        public Task<IList<StudyGroup>> SearchStudyGroups(string subject);

        public Task JoinStudyGroup(int studyGroupId, int userId);

        public Task LeaveStudyGroup(int studyGroupId, int userId);
    }
}
