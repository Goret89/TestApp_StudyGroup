using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TestApp;

namespace UnitTests
{
    public class StudyGroupTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StudyGroup_Should_SetThroughConstructor_And_Get_Id()
        {
            // Arrange
            var expectedId = 1;

            // Act
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Math, new DateTime(), new List<User>());

            // Assert
            studyGroup.StudyGroupId.Should().Be(expectedId);
        }

        [Test]
        public void StudyGroup_Should_SetThroughConstructor_And_Get_Name()
        {
            // Arrange
            var expectedName = "Test Study Group 1";

            // Act
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Math, new DateTime(), new List<User>());

            // Assert
            studyGroup.Name.Should().Be(expectedName);
        }

        [Test]
        public void StudyGroup_Should_SetThroughConstructor_And_Get_SubjectMath()
        {
            // Arrange
            var expectedSubject = Subject.Math;

            // Act
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Math, new DateTime(), new List<User>());

            // Assert
            studyGroup.Subject.Should().Be(expectedSubject);
        }

        [Test]
        public void StudyGroup_Should_SetThroughConstructor_And_Get_SubjectChemistry()
        {
            // Arrange
            var expectedSubject = Subject.Chemistry;

            // Act
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Chemistry, new DateTime(), new List<User>());

            // Assert
            studyGroup.Subject.Should().Be(expectedSubject);
        }

        [Test]
        public void StudyGroup_Should_SetThroughConstructor_And_Get_SubjectPhysics()
        {
            // Arrange
            var expectedSubject = Subject.Physics;

            // Act
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Physics, new DateTime(), new List<User>());

            // Assert
            studyGroup.Subject.Should().Be(expectedSubject);
        }

        [Test]
        public void StudyGroup_Should_SetThroughConstructor_And_Get_DateTime()
        {
            // Arrange
            var expectedDateTime = new DateTime(2024, 6, 21, 9, 49, 00, 123);

            // Act
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Physics, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());

            // Assert
            studyGroup.CreateDate.Should().Be(expectedDateTime);
        }

        [Test]
        public void StudyGroup_Should_SetThroughConstructor_And_Get_Users_Zero()
        {
            // Act
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Physics, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());

            // Assert
            studyGroup.Users.Count.Should().Be(0);
        }

        [Test]
        public void StudyGroup_Should_SetThroughAddUser_And_Get_SetUser()
        {
            // Arrange
            var expectedUser = new List<User> { new User { FullName = "Test User" } };
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Physics, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());

            // Act
            var user = new User { FullName = "Test User" };
            studyGroup.AddUser(user);

            // Assert
            studyGroup.Users.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public void StudyGroup_Should_SetThroughAddUser_And_Get_MultipleUsers()
        {
            // Arrange
            var expectedUser = new List<User> { new User { FullName = "Test User" }, new User { FullName = "Test User 2" } };
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Physics, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User> { new User { FullName = "Test User" } });

            // Act
            var user = new User { FullName = "Test User 2" };
            studyGroup.AddUser(user);

            // Assert
            studyGroup.Users.Should().BeEquivalentTo(expectedUser);
            studyGroup.Users.Count.Should().Be(expectedUser.Count);
        }

        [Test]
        public void StudyGroup_Should_SetThroughConstructor_And_Get_AlreadySetUsers()
        {
            // Arrange
            var expectedUser = new List<User> { new User { FullName = "Test User" } };

            // Act
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Physics, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User> { new User { FullName = "Test User" } });

            // Assert
            studyGroup.Users.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public void StudyGroup_Should_RemoveUser_And_Get_RestUsers()
        {
            // Arrange
            var user = new User { FullName = "Test User" };
            var user2 = new User { FullName = "Test User 2" };
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Physics, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());
            studyGroup.AddUser(user);
            studyGroup.AddUser(user2);

            var expectedUser = new List<User> { new User { FullName = "Test User 2" } };

            // Act
            studyGroup.RemoveUser(user);

            // Assert
            studyGroup.Users.Should().BeEquivalentTo(expectedUser);
            studyGroup.Users.Count.Should().Be(1);
        }

        [Test]
        public void StudyGroup_Should_RemoveAllUsers_And_Get_ZeroUsers()
        {
            // Arrange
            var studyGroup = new StudyGroup(1, "Test Study Group 1", Subject.Physics, new DateTime(2024, 6, 21, 9, 49, 00, 123), new List<User>());
            var user = new User { FullName = "Test User 2" };
            var user2 = new User { FullName = "Test User" };

            studyGroup.AddUser(user);
            studyGroup.AddUser(user2);

            // Act
            studyGroup.RemoveUser(user);
            studyGroup.RemoveUser(user2);

            // Assert
            studyGroup.Users.Count.Should().Be(0);
        }
    }
}