using System;
using System.Linq;
using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;
using System.Data.Entity;

namespace CV.POS.Infrastructure
{
    public class ProductBaseRepository 
        : BaseRepository<ProductBase>, IProductBaseRepository
    {
        public ProductBaseRepository(PosDbContext dbContext)
            : base(dbContext)
        {
            
        }

        public IQueryable<ProductBase> GetProductsWithDependenciesByName(int premiseId, string productName)
        {
            IQueryable<ProductBase> result;            
            
            if (string.IsNullOrEmpty(productName) || string.IsNullOrWhiteSpace(productName))
                result = GetProductsBaseByPremiseId(premiseId);
            else
                result = from pb in GetProductsBaseByPremiseId(premiseId)
                    where pb.Name.Contains(productName)
                    select pb;

            return result.Include(x => x.Product.Select(y => y.ProductUnit))
                .Take(30);
        }

        public ProductBase GetProductsWithGeneralInfoById(int premiseId, short productBaseId)
        {
            return (from pb in GetProductsBaseByPremiseId(premiseId)
                where pb.ProductBaseId == productBaseId
                select pb).Single();
        }

        public ProductPremise GetStock(short productBaseId, int premiseId)
        {
            //return GetProductsBaseByPremiseId(premiseId)
            //    .Single(x => x.ProductBaseId == productBaseId)
            //    .Product.Single()
            //    .ProductPremise.Single();

            int currentProductId = GetProductId(productBaseId);

            return (from pp in Db.ProductPremise
                     where pp.PremiseId == premiseId
                           && pp.ProductId == currentProductId
                     select pp).Single();
        }

        private int GetProductId(short productBaseId)
        {
            return (from pb in Db.ProductBase
                join p in Db.Product
                    on pb.ProductBaseId equals p.ProductBaseId
                where pb.ProductBaseId == productBaseId
                select p.ProductId).First();
        }

        private IQueryable<ProductBase> GetProductsBaseByPremiseId(int premiseId)
        {
            return (from pb in Db.ProductBase
                    join p in Db.Product
                        on pb.ProductBaseId equals p.ProductBaseId
                    join pp in Db.ProductPremise
                        on p.ProductId equals pp.ProductId
                    where pp.PremiseId == premiseId
                    select pb)
                    .Include(x => x.Product.Select(y => y.ProductPremise));
        }

        
    }

    //public static class Extensions
    //{
    //    public static IQueryable<ProductBase> ProductByPremiseId(this PosDbContext context, int premiseId)
    //    {
    //        return (from pb in context.ProductBase
    //                join p in context.Product
    //                    on pb.ProductBaseId equals p.ProductBaseId
    //                join pp in context.ProductPremise
    //                    on p.ProductId equals pp.ProductId
    //                where pp.PremiseId == premiseId
    //                select pb)
    //                .Include(x => x.Product.Select(y => y.ProductPremise));

    //        //return context.Companies
    //        //    .Include("Employee.Employee_Car")
    //        //    .Include("Employee.Employee_Country")
    //        //    .FirstOrDefault(c => c.Id == companyID);
    //    }

    //}
}
