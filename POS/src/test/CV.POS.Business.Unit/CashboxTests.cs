using System;
using System.Collections.Generic;
using System.Linq;
using CV.POS.Business.Interfaces;
using CV.POS.Entities;
using Moq;
using NUnit.Framework;

namespace CV.POS.Business.Unit
{
    [TestFixture]
    public class CashboxTests
    {
        [Test]
        public void IsOpened_WithOpenedCashbox_ReturnsTrue()
        {
            //Arrange
            var cashboxRepoStub = new Mock<ICashBoxRepository>();
            cashboxRepoStub.Setup(x => x.GetById(1)).Returns(new Cashbox() {CashboxId = 1});

            var queryableExpectedResult = new List<CashboxStatus>(){new CashboxStatus(){CashboxId = 1, UserId = 2, EndDate = null}}.AsQueryable();
            var cashboxStatusRepoStub = new Mock<ICashBoxStatusRepository>();
            cashboxStatusRepoStub.Setup(x => x.GetAll()).Returns(queryableExpectedResult);

            var uowStub = new Mock<IUow>();
            uowStub.SetupGet(x => x.CashBoxRepository).Returns(cashboxRepoStub.Object);
            uowStub.SetupGet(x => x.CashBoxStatusRepository).Returns(cashboxStatusRepoStub.Object);
            
            var cashBoxService = new CashBoxService(uowStub.Object);

            //Act, Assert
            Assert.AreEqual(true, cashBoxService.IsOpenedForOtherUsers(currentUserId:1));
        }

        [Test]
        public void IsOpened_WithClosedCashbox_ReturnsFalse()
        {
            //Arrange
            var cashboxRepoStub = new Mock<ICashBoxRepository>();
            cashboxRepoStub.Setup(x => x.GetById(1)).Returns(new Cashbox() { CashboxId = 1 });

            var queryableExpectedResult = new List<CashboxStatus>() { new CashboxStatus() { CashboxId = 1, UserId = 2, EndDate = DateTime.Now } }.AsQueryable();
            var cashboxStatusRepoStub = new Mock<ICashBoxStatusRepository>();
            cashboxStatusRepoStub.Setup(x => x.GetAll()).Returns(queryableExpectedResult);

            var uowStub = new Mock<IUow>();
            uowStub.SetupGet(x => x.CashBoxRepository).Returns(cashboxRepoStub.Object);
            uowStub.SetupGet(x => x.CashBoxStatusRepository).Returns(cashboxStatusRepoStub.Object);

            var cashBoxService = new CashBoxService(uowStub.Object);

            //Act, Assert
            Assert.AreEqual(false, cashBoxService.IsOpenedForOtherUsers(currentUserId:1));
        }
    }
}
