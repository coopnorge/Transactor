using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Terminal.Core.Managers;
using Terminal.Infrastructure.Storage;
using Xunit;

namespace Terminal.Test.Unit.Core.Managers
{
    public class AccountManagerTest
    {
        private const string ExpectedStorageKey = "BankAccounts";
        private const string ExpectedBank = "UnitTest";
        private const long ExpectedBalance = 42;
        
        private readonly Mock<IStorage> _storageMock;

        public AccountManagerTest()
        {
            _storageMock = new Mock<IStorage>();
            
            var expectedAccounts = new List<AccountManager.Account> {new AccountManager.Account(ExpectedBank, ExpectedBalance)};
            _storageMock
                .Setup(s => s.Get<List<AccountManager.Account>>("BankAccounts"))
                .Returns(expectedAccounts);
        }

        [Fact(DisplayName = "Add new bank account")]
        public void TestAddBankAccount()
        {
            // Setup
            _storageMock.Setup(s => s.Save(ExpectedStorageKey, 999));

            var manager = new AccountManager(_storageMock.Object);
            
            // Act
            manager.AddBankAccount("UnitTest", 999);
            
            // Assert
            _storageMock
                .Verify(s => s.Save(ExpectedStorageKey, It.IsAny<List<AccountManager.Account>>()), Times.Once);
        }
        
        [Fact(DisplayName = "Get all bank accounts")]
        public void TestGetBankAccounts()
        {
            // Setup
            var manager = new AccountManager(_storageMock.Object);
            
            // Act
            var bankAccounts = manager.GetBankAccounts();
            
            // Assert
            bankAccounts.Should().BeOfType<AccountManager.Account[]>();
            bankAccounts.Should().NotBeEmpty();
            bankAccounts.First().Bank.Should().Match("UnitTest");
            bankAccounts.First().Balance.Should().Be(42);
        }

        [Fact(DisplayName = "Try pay and it must be ok")]
        public void TestPayOk()
        {
            // Setup
            var manager = new AccountManager(_storageMock.Object);
            
            // Act
            var isPaid = manager.Pay(ExpectedBank, ExpectedBalance);

            // Assert
            isPaid.Should().BeTrue();
        }
        
        [Fact(DisplayName = "Try pay and but not enough money")]
        public void TestPayFail()
        {
            // Setup
            var manager = new AccountManager(_storageMock.Object);
            
            // Act
            var isPaid = manager.Pay(ExpectedBank, 9999);

            // Assert
            isPaid.Should().BeFalse();
        }
    }
}