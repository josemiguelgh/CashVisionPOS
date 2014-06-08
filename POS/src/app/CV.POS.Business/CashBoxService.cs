using System;
using System.Collections.Generic;
using System.Linq;
using CV.POS.Business.Helpers;
using CV.POS.Business.Interfaces;
using CV.POS.Entities;

namespace CV.POS.Business
{
    public sealed class CashBoxService : ICashBoxService
    {
        public IUow Uow { get; set; }

        private byte defaultCashBoxId = 1;

        public CashBoxService(IUow uow)
        {
            Uow = uow;
        }

        public bool IsOpenedForOtherUsers(short currentUserId)
        {
            var defaultCashboxId = GetDefatultCashbox().CashboxId;
            var cashboxCountOpened = (from cashboxStatus in Uow.CashBoxStatusRepository.GetAll()
                                        where cashboxStatus.CashboxId == defaultCashboxId
                                            && cashboxStatus.UserId != currentUserId
                                            && cashboxStatus.EndDate == null
                                        select cashboxStatus.CBStatusId).Count();

            return cashboxCountOpened > 0;
        }

        public bool IsOpened()
        {
            var defaultCashboxId = GetDefatultCashbox().CashboxId;
            var cashboxCountOpened = (from cashboxStatus in Uow.CashBoxStatusRepository.GetAll()
                                      where cashboxStatus.CashboxId == defaultCashboxId
                                          && cashboxStatus.EndDate == null
                                      select cashboxStatus.CBStatusId).Count();

            return cashboxCountOpened > 0;
        }

        public Cashbox GetDefatultCashbox()
        {
            return Uow.CashBoxRepository.GetById(defaultCashBoxId); //return the unique cashbox available, should have a value of 1
        }

        public decimal GetAmountForCashBox()
        {
            return GetDefatultCashbox().CurrentAmount;
        }

        public OperationResult OpenCashBox(short userId, int sessionId, decimal verifiedAmount)
        {
            if (GetAmountForCashBox() == verifiedAmount)
            {
                var cashboxStatus = new CashboxStatus
                {
                    BeginDate = DateTime.Now,
                    StartAmount = verifiedAmount,
                    CashboxId = defaultCashBoxId,
                    UserId = userId,
                    CashboxTestify = new List<CashboxTestify>()
                };
                cashboxStatus.CashboxTestify.Add(
                    new CashboxTestify
                    {
                        CashboxId = defaultCashBoxId, 
                        TestifyDate = DateTime.Now,
                        UserId = userId,
                        AmountChecked = verifiedAmount,
                        Difference = GetAmountForCashBox() - verifiedAmount, //should always be zero
                        SessionId = sessionId
                    }
                );
                Uow.CashBoxStatusRepository.Insert(cashboxStatus);
                Uow.Commit();
                return new OperationResult {Succeed = true};
            }
            else
            {
                return new OperationResult{Succeed = false, Message = "La cantidad verificada no coincide con la cantidad que se tiene registrada en la caja, comuníquese con un administrador."};   
            }
        }
    }

    public interface ICashBoxService
    {
        bool IsOpenedForOtherUsers(short currentUserId);
        bool IsOpened();
        decimal GetAmountForCashBox();
        OperationResult OpenCashBox(short userId, int sessionId, decimal verifiedAmount);
        Cashbox GetDefatultCashbox();
    }

}
