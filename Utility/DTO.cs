using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    [Serializable]
  public  class ProductListDTO
    {
                                

    public   long ProductId{set;get;} 
     public bool IsFeaturedProduct { get; set; }
     public bool IsActive { get; set; }
     public bool IsDeleted { get; set; }
     public bool IsSold { get; set; }
   
      //public ICollection<NoftifyMe> NoftifyMes { get; set; }
      public decimal ProductCost { get; set; }
      public decimal ProductDiscountPercentage { get; set; }
      public decimal ProductOriginalCost { get; set; }
      public string ProductImgUrl { get; set; }
      public string ProductName { get; set; }
      //public ICollection<ProductSpecification> ProductSpecifications { get; set; }
      public int Quantity { get; set; }
      public DateTime UpdatedOn { get; set; }
      public string Brand { get; set; }
      public DateTime CreatedOn { get; set; }
      public long SubCategoryId { get; set; }
      public string SubCategoryName { get; set; }
      public int CategoryProductCount { get; set; }
      public int SubCategoryProductCount { get; set; }
      public string CategoryName { get; set; }
      public string currency { get; set; }
      public long SuperCategoryId { get; set; }
      public long CategoryId { get; set; }
      public string SuperCategoryName { get; set; }
      public string breadcrumb { get; set; }

      public string sidelist { get; set; }
    }
}
