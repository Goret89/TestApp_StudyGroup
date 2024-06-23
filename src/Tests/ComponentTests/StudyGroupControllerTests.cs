using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp;
using TestAppAPI.Context;
using TestAppAPI.Controllers;

namespace ComponentTests
{
    public class StudyGroupControllerTests
    {
        private StudyGroupController _studyGroupController = null!;
        private Mock<IStudyGroupRepository> _studyGroupRepositoryMock = null!;
        private DbContextOptions<AppDbContext> _dbContextOptions = null!;
        private AppDbContext _fakeDbContext = null!;

        [SetUp]
        public void Setup()
        {
            _studyGroupRepositoryMock = new Mock<IStudyGroupRepository>();
            _studyGroupController = new StudyGroupController(_studyGroupRepositoryMock.Object);

            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            _fakeDbContext = new AppDbContext(_dbContextOptions);
        }


        #region CreateStudyGroup

        [Test]
        public async Task CreateStudyGroup_ShouldReturnOkObjectResult_MathSubject()
        {
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, DateTime.UtcNow, new List<User>());
            var result = await _studyGroupController.CreateStudyGroup(studyGroup);

            result.Should().NotBeNull().And.BeOfType<OkResult>();

            var savedStudyGroup = _fakeDbContext.StudyGroups.FirstAsync(x => x.Name == "Test Study Group 1");
            savedStudyGroup.Should().NotBeNull().And.BeEquivalentTo(studyGroup);
        }

        [Test]
        public async Task CreateStudyGroup_ShouldReturnOkObjectResult_ChemistrySubject()
        {
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Chemistry, DateTime.UtcNow, new List<User>());
            var result = await _studyGroupController.CreateStudyGroup(studyGroup);

            result.Should().NotBeNull().And.BeOfType<OkResult>();
        }

        [Test]
        public async Task CreateStudyGroup_ShouldReturnOkObjectResult_PhysicsSubject()
        {
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Physics, DateTime.UtcNow, new List<User>());
            var result = await _studyGroupController.CreateStudyGroup(studyGroup);

            result.Should().NotBeNull().And.BeOfType<OkResult>();
        }

        [Test]
        public async Task CreateStudyGroup_ShouldReturnUnprocessableEntityObjectResult_5Charactars()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Group", Subject.Math, DateTime.UtcNow, new List<User>());
            var expectedErrorMessage = "The study group name must be between 5 and 30 characters. You entered 5 characters.";
            // Act
            var result = await _studyGroupController.CreateStudyGroup(studyGroup);

            // Assert
            result.Should().NotBeNull().And.BeOfType<UnprocessableEntityObjectResult>();
            var resultStudyModel = (ErrorMessage)((UnprocessableEntityObjectResult)result).Value;
            resultStudyModel.Should().NotBeNull().And.BeOfType<ErrorMessage>();
            resultStudyModel.Value.Should().NotBeNull().And.Be(expectedErrorMessage);
        }

        [Test]
        public async Task CreateStudyGroup_ShouldReturnUnprocessableEntityObjectResult_31Charactars()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Test Test Test Test Group1", Subject.Math, DateTime.UtcNow, new List<User>());
            var expectedErrorMessage = "The study group name must be between 5 and 30 characters. You entered 5 characters.";
            // Act
            var result = await _studyGroupController.CreateStudyGroup(studyGroup);

            // Assert
            result.Should().NotBeNull().And.BeOfType<UnprocessableEntityObjectResult>();
            var resultStudyModel = (ErrorMessage)((UnprocessableEntityObjectResult)result).Value;
            resultStudyModel.Should().NotBeNull().And.BeOfType<ErrorMessage>();
            resultStudyModel.Value.Should().NotBeNull().And.Be(expectedErrorMessage);
        }

        #endregion

        #region GetStudyGroup

        [Test]
        public async Task Get_StudyGroups_ShouldReturn200_2AmountOfStudyGroup()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, DateTime.UtcNow, new List<User>());
            var studyGroup2 = new StudyGroup(112, "Test Study Group 2", Subject.Math, DateTime.UtcNow, new List<User>());

            await _studyGroupController.CreateStudyGroup(studyGroup);
            await _studyGroupController.CreateStudyGroup(studyGroup2);

            // Act
            var result = await _studyGroupController.GetStudyGroups();

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var resultStudyModel = (List<StudyGroup>)((OkObjectResult)result).Value;
            resultStudyModel.Should().NotBeNull().And.BeOfType<List<StudyGroup>>();
            resultStudyModel.Count.Should().Be(2);
        }

        #endregion

        #region SeachStudyGroup

        [Test]
        public async Task FilterStudyGroups_ShouldReturn200_ReturnBody()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, DateTime.UtcNow, new List<User>());
            var studyGroup2 = new StudyGroup(112, "Test Study Group 2", Subject.Chemistry, DateTime.UtcNow, new List<User>());
            var studyGroup3 = new StudyGroup(113, "Test Study Group 3", Subject.Physics, DateTime.UtcNow, new List<User>());
            var studyGroup4 = new StudyGroup(114, "Test Study Group 4", Subject.Math, DateTime.UtcNow, new List<User>());

            await _studyGroupController.CreateStudyGroup(studyGroup);
            await _studyGroupController.CreateStudyGroup(studyGroup2);
            await _studyGroupController.CreateStudyGroup(studyGroup3);
            await _studyGroupController.CreateStudyGroup(studyGroup4);

            // Act
            var result = await _studyGroupController.SearchStudyGroups("filter=Subject eq 'Math'");

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var resultStudyModel = (List<StudyGroup>)((OkObjectResult)result).Value;
            resultStudyModel.Should().NotBeNull().And.BeOfType<List<StudyGroup>>();
            resultStudyModel.Count.Should().Be(2);
        }

        [Test]
        public async Task SortStudyGroupsAscending_ShouldReturn200_ReturnBody()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());
            var studyGroup2 = new StudyGroup(112, "Test Study Group 2", Subject.Chemistry, new DateTime(2024, 6, 21, 9, 47, 00, 123), new List<User>());
            var studyGroup3 = new StudyGroup(113, "Test Study Group 3", Subject.Physics, new DateTime(2024, 6, 21, 9, 48, 00, 123), new List<User>());

            await _studyGroupController.CreateStudyGroup(studyGroup);
            await _studyGroupController.CreateStudyGroup(studyGroup2);
            await _studyGroupController.CreateStudyGroup(studyGroup3);

            // Act
            var result = await _studyGroupController.SearchStudyGroups("sort=CreateDate asc");

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var resultStudyModel = (List<StudyGroup>)((OkObjectResult)result).Value;
            resultStudyModel.Should().NotBeNull().And.BeOfType<List<StudyGroup>>();
            resultStudyModel.Count.Should().Be(3);
            resultStudyModel.Should().BeInAscendingOrder(e => e.CreateDate);
        }

        [Test]
        public async Task SortStudyGroupsDescending_ShouldReturn200_ReturnBody()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());
            var studyGroup2 = new StudyGroup(112, "Test Study Group 2", Subject.Chemistry, new DateTime(2024, 6, 21, 9, 47, 00, 123), new List<User>());
            var studyGroup3 = new StudyGroup(113, "Test Study Group 3", Subject.Physics, new DateTime(2024, 6, 21, 9, 48, 00, 123), new List<User>());

            await _studyGroupController.CreateStudyGroup(studyGroup);
            await _studyGroupController.CreateStudyGroup(studyGroup2);
            await _studyGroupController.CreateStudyGroup(studyGroup3);

            // Act
            var result = await _studyGroupController.SearchStudyGroups("sort=CreateDate desc");

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var resultStudyModel = (List<StudyGroup>)((OkObjectResult)result).Value;
            resultStudyModel.Should().NotBeNull().And.BeOfType<List<StudyGroup>>();
            resultStudyModel.Count.Should().Be(3);
            resultStudyModel.Should().BeInDescendingOrder(e => e.CreateDate);
        }

        #endregion

        #region JoinStudyGroup

        [Test]
        public async Task Join_1StudyGroup_ShouldReturnOkObjectResult()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());

            await _studyGroupController.CreateStudyGroup(studyGroup);

            // Act
            var result = await _studyGroupController.JoinStudyGroup(111, 1);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkResult>();
        }

        [Test]
        public async Task Join_3StudyGroups_ShouldReturnOkObjectResult()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, DateTime.UtcNow, new List<User>());
            var studyGroup2 = new StudyGroup(112, "Test Study Group 2", Subject.Chemistry, DateTime.UtcNow, new List<User>());
            var studyGroup3 = new StudyGroup(113, "Test Study Group 3", Subject.Physics, DateTime.UtcNow, new List<User>());

            await _studyGroupController.CreateStudyGroup(studyGroup);
            await _studyGroupController.CreateStudyGroup(studyGroup2);
            await _studyGroupController.CreateStudyGroup(studyGroup3);

            // Act
            var result = await _studyGroupController.JoinStudyGroup(111, 1);
            var result2 = await _studyGroupController.JoinStudyGroup(112, 1);
            var result3 = await _studyGroupController.JoinStudyGroup(113, 1);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkResult>();
            result2.Should().NotBeNull().And.BeOfType<OkResult>();
            result3.Should().NotBeNull().And.BeOfType<OkResult>();
        }

        [Test]
        public async Task JoinStudyGroup_ShouldReturnOkObjectResult()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());

            await _studyGroupController.CreateStudyGroup(studyGroup);

            // Act
            var result = await _studyGroupController.JoinStudyGroup(111, 1);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkResult>();
        }

        #endregion

        #region LeaveStudyGroup

        [Test]
        public async Task Leave_AllStudyGroup_ShouldReturnOkObjectResult()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());

            await _studyGroupController.CreateStudyGroup(studyGroup);
            await _studyGroupController.JoinStudyGroup(111, 1);

            // Act
            var result = await _studyGroupController.LeaveStudyGroup(111, 1);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkResult>();

            var savedStudyGroup = _fakeDbContext.StudyGroups.FirstAsync(x => x.Name == "Test Study Group 1");
            savedStudyGroup.Should().BeNull();
        }

        [Test]
        public async Task Leave_2Of3StudyGroup3_ShouldReturnOkObjectResult()
        {
            // Arrange
            var studyGroup = new StudyGroup(111, "Test Study Group 1", Subject.Math, DateTime.UtcNow, new List<User>());
            var studyGroup2 = new StudyGroup(112, "Test Study Group 2", Subject.Chemistry, DateTime.UtcNow, new List<User>());
            var studyGroup3 = new StudyGroup(113, "Test Study Group 3", Subject.Physics, DateTime.UtcNow, new List<User>());

            await _studyGroupController.CreateStudyGroup(studyGroup);
            await _studyGroupController.CreateStudyGroup(studyGroup2);
            await _studyGroupController.CreateStudyGroup(studyGroup3);

            await _studyGroupController.JoinStudyGroup(111, 1);
            await _studyGroupController.JoinStudyGroup(112, 1);
            await _studyGroupController.JoinStudyGroup(113, 1);

            // Act
            var result = await _studyGroupController.LeaveStudyGroup(111, 1);
            var result2 = await _studyGroupController.LeaveStudyGroup(112, 1);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkResult>();
            result2.Should().NotBeNull().And.BeOfType<OkResult>();

            var savedStudyGroups = await _fakeDbContext.StudyGroups.ToListAsync();
            savedStudyGroups.Count.Should().Be(1);
            var savedStudyGroup = savedStudyGroups.Find(x => x.Name == studyGroup3.Name);
            savedStudyGroup.Should().NotBeNull().And.BeEquivalentTo(studyGroup3,
                option => option.Excluding(p => p.Users));
            savedStudyGroup!.Users.Should().HaveCount(1);
        }

        #endregion
    }
}