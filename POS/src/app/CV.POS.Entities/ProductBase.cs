//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CV.POS.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductBase
    {
        public ProductBase()
        {
            this.Product = new HashSet<Product>();
        }
    
        public short ProductBaseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SaleDefaultUnitAbbr { get; set; }
    
        public virtual ICollection<Product> Product { get; set; }
    }
}