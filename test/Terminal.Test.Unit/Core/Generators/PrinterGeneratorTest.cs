using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Terminal.Core.Generators;
using Terminal.Core.Managers;
using Terminal.Infrastructure.Storage;
using Xunit;

namespace Terminal.Test.Unit.Core.Generators
{
    public class PrinterGeneratorTest
    {
        [Fact(DisplayName = "Prints a statement format")]
        public void PrintTest()
        {
            // Setup
            var account = new List<AccountManager.Account>{new AccountManager.Account("UnitTest", 1000)};
            var storageMock = new Mock<IStorage>();
            storageMock
                .Setup(s => s.Get<List<AccountManager.Account>>(It.IsAny<string>()))
                .Returns(account);

            var accountStub = new AccountManager(storageMock.Object);
            var printer = new PrinterGenerator(accountStub);
            
            // Act
            var statement = printer.GenerateStatement();
            
            // Assert
            statement.Should().Contain("UnitTest").And.Contain("1000");
        }
    }
}