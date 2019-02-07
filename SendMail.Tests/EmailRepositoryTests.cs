using Microsoft.EntityFrameworkCore;
using Moq;
using SendMail.Data;
using SendMail.Entities;
using SendMail.Models;
using SendMail.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SendMail.Tests
{
    public class EmailRepositoryTests
    {
        [Fact]
        public void GetEmailAddress_EmailAddressShouldReturnFromDb()
        {
            var sampleData = GetSampleEmailAddresses().AsQueryable();
            var mockSet = new Mock<DbSet<EmailAddress>>();
            mockSet.As<IQueryable<EmailAddress>>()
                .Setup(m => m.Provider).Returns(sampleData.Provider);
            mockSet.As<IQueryable<EmailAddress>>()
                .Setup(m => m.Expression).Returns(sampleData.Expression);
            mockSet.As<IQueryable<EmailAddress>>()
                .Setup(m => m.ElementType).Returns(sampleData.ElementType);
            mockSet.As<IQueryable<EmailAddress>>()
                .Setup(m => m.GetEnumerator()).Returns(sampleData.GetEnumerator());

            var mockContext = new Mock<EmailDbContext>(null);
            mockContext.Setup(m => m.EmailAddresses).Returns(mockSet.Object);

            var repository = new EmailRepository(mockContext.Object);

            // Arrange values
            EmailAddress expectedEmailAddress = new EmailAddress()
            {
                Name = "Michael Choi",
                Email = "MC@gmail.com"
            };

            // Act 
            var testEmail = new EmailAddressModel()
            {
                Id = 1,
                Name = "Michael Choi",
                Email = "MC@gmail.com"
            };

            EmailAddress actualEmailAddress = repository.GetEmailAddress(testEmail);

            // Assert
            Assert.True(actualEmailAddress != null);
            Assert.Equal(expectedEmailAddress.Email, actualEmailAddress.Email);
            Assert.Equal(expectedEmailAddress.Name, actualEmailAddress.Name);
        }

        public List<EmailAddress> GetSampleEmailAddresses()
        {
            return new List<EmailAddress>() {
                new EmailAddress()
                {
                    Id = 1,
                    Name = "Michael C",
                    Email = "MC@gmail.com"
                },
                new EmailAddress()
                {
                    Id = 2,
                    Name = "Jim Garner",
                    Email = "Jimbo@gmail.com"
                },
               new EmailAddress()
               {
                   Id = 3,
                   Name = "Timothy Smith",
                   Email = "Tim105@outlook.com"
               },
                new EmailAddress()
                {
                    Id = 4,
                    Name = "Sara B",
                    Email = "Sara96@yahoo.com"
                }
            };
        }
    }
}
