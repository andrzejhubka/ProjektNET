// ProjektZaliczeniowyNET.Tests/Models/ServiceOrderTests.cs
using NUnit.Framework;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Tests.Models
{
    [TestFixture]
    public class ServiceOrderTests
    {
        [Test]
        public void ServiceOrder_ShouldCreateInstance()
        {
            // Arrange & Act
            var serviceOrder = new ServiceOrder
            {
                Status = ServiceOrderStatus.Pending
            };

            // Assert
            Assert.That(serviceOrder, Is.Not.Null);
        }

        [Test]
        public void ServiceOrder_ShouldSetId()
        {
            // Arrange
            var serviceOrder = new ServiceOrder
            {
                Status = ServiceOrderStatus.Pending,
                Id = 1
            };

            // Act & Assert
            Assert.That(serviceOrder.Id, Is.EqualTo(1));
        }

        [Test]
        public void ServiceOrder_ShouldSetStatus()
        {
            // Arrange
            var serviceOrder = new ServiceOrder
            {
                Status = ServiceOrderStatus.InProgress
            };

            // Act & Assert
            Assert.That(serviceOrder.Status, Is.EqualTo(ServiceOrderStatus.InProgress));
        }

        [Test]
        public void ServiceOrder_ShouldHaveServiceTasksList()
        {
            // Arrange
            var serviceOrder = new ServiceOrder
            {
                Status = ServiceOrderStatus.Pending
            };

            // Act & Assert
            Assert.That(serviceOrder.ServiceTasks, Is.Not.Null);
        }

        [Test]
        public void ServiceOrder_ShouldHaveCommentsList()
        {
            // Arrange
            var serviceOrder = new ServiceOrder
            {
                Status = ServiceOrderStatus.Pending
            };

            // Act & Assert
            Assert.That(serviceOrder.Comments, Is.Not.Null);
        }
    }
}