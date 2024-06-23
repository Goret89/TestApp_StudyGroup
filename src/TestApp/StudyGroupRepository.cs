namespace TestApp
{
    public class StudyGroupRepository : IStudyGroupRepository
    {
        public Task CreateStudyGroup(StudyGroup studyGroup)
        {
            throw new NotImplementedException();
        }

        public Task<IList<StudyGroup>> GetStudyGroups()
        {
            throw new NotImplementedException();
        }

        public Task JoinStudyGroup(int studyGroupId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task LeaveStudyGroup(int studyGroupId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<StudyGroup>> SearchStudyGroups(string subject)
        {
            throw new NotImplementedException();
        }
    }
}
