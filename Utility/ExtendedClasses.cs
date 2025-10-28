using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Utility
{
  [MetadataTypeAttribute(typeof(SubCategoryMetaData))]
    public partial class SubCategory
    {
        public string CategoryName;
    }

    [MetadataTypeAttribute(typeof(StateMetaData))]
    public partial class State
    {
        public string CountryName;
    }

    [MetadataTypeAttribute(typeof(ProductMetaData))]
    public partial class Product
    {
 

        public HttpPostedFile ProductImgs;
        public HttpPostedFile ProductImgss;
        public HttpFileCollection ProductGalleryImgs;
      
        public string CurrencyCode { get; set; }
        public decimal CurrencyValue { get; set; }
        public string CurrencySymbol { get; set; }

    }

    [MetadataTypeAttribute(typeof(PaymentTransactionMetaData))]
    public partial class PaymentTransaction
    {
        public int rows { get; set; }
    }


     [MetadataTypeAttribute(typeof(CategoryMetaData))]
    public partial class Category
    {
       
        public string SuperCategoryName { get; set; }
    }


     [MetadataTypeAttribute(typeof(FeaturesCategoryMetaData))]
    public partial class FeaturesCategory {
        public string BusinessName { get; set; }
    }



    [MetadataTypeAttribute(typeof(FeaturesSubCategoryMetaData))]
    public partial class FeaturesSubCategory
    {
        public string FeaturesCategoryName { get; set; }
    }

    [MetadataType(typeof(ProductGalleryMetaData))]
    public partial class ProductGallery
    { 
    
    }

    [MetadataType(typeof(ProductFreatureMetaData))]
    public partial class ProductFeature
    { 
    
    }

    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        public int Redirectvalue { get; set; }
    }

   // public class Cart
   // {
   //     public Product  CartProduct { get; set; }
   //     public User CartUser { get; set; }
   //    public  int Quantity { get; set; }
   //}
    [MetadataType(typeof(CustomCurrencyMetaData))]
    public class CustomCurrency
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public string Symbol { get; set; }
        public decimal Value { get; set; }
    }

    [MetadataType(typeof(ProductSpecificationMetaData))]
    public partial class ProductSpecification
    {

    }


    [MetadataType(typeof(RoleMetaData))]
    public partial class Role
    {

    }


    [MetadataType(typeof(SpecificationTypeMetaData))]
    public partial class SpecificationType
    {

    }


    //[MetadataType(typeof(StateMetaData))]
    //public partial class State
    //{

    //}

    [MetadataType(typeof(UserAddressMetaData))]
    public partial class UserAddress
    {

    }

   
    [MetadataType(typeof(UserProductTransactionMetaData))]
    public partial class UserProductTransaction
    {

    }

     [MetadataType(typeof(UserProductTransactionSpecificationMetaData))]
    public partial class UserProductTransactionSpecification
    {

    }



#region metadata classes

    public class SubCategoryMetaData
    {
        [JsonIgnore]
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }

    public class CategoryMetaData
    {
        [JsonIgnore]
        public virtual SuperCategory SuperCategory { get; set; }
    }

    public class ProductMetaData
    {
        [JsonIgnore]
        public virtual ICollection<ProductFeature> ProductFeatures { get; set; }
        [JsonIgnore]
        public virtual SubCategory SubCategory { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductsGallery> ProductsGalleries { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProductTransaction> UserProductTransactions { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductReview> ProductReviews { get; set; }
        [JsonIgnore]
        public virtual ICollection<RelatedProduct> RelatedProducts { get; set; }
        [JsonIgnore]
        public virtual ICollection<RelatedProduct> RelatedProducts1 { get; set; }
        [JsonIgnore]
        public virtual ICollection<SubProduct> SubProducts { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductInventory> ProductInventories { get; set; }
        [JsonIgnore]
        public virtual ICollection<NoftifyMe> NoftifyMes { get; set; }
    }

    public class ProductGalleryMetaData
    {
        [JsonIgnore]
        public virtual Product Product { get; set; }
    }


    public class ProductFreatureMetaData
    {
        [JsonIgnore]
        public virtual Product Product { get; set; }
        [JsonIgnore]
        public virtual FeaturesSubCategory FeaturesSubCategory { get; set; }
    }

    public class ProductSpecificationMetaData
    {
        [JsonIgnore]
        public virtual Product Product { get; set; }
        [JsonIgnore]
        public virtual SpecificationType SpecificationType { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProductTransactionSpecification> UserProductTransactionSpecifications { get; set; }
    }

    public class RoleMetaData
    {
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
    public class SpecificationTypeMetaData
    {
        [JsonIgnore]
        public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; }
    }

    public class StateMetaData
    {
        [JsonIgnore]
        public virtual Country Country { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
    }

    public class UserMetaData
    {
        [JsonIgnore]
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        [JsonIgnore]
        public virtual Role Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProductTransaction> UserProductTransactions { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductReview> ProductReviews { get; set; }
    }

    public class UserAddressMetaData
    {
        [JsonIgnore]
        public virtual Country Country { get; set; }
        [JsonIgnore]
        public virtual State State { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProductTransaction> UserProductTransactions { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProductTransaction> UserProductTransactions1 { get; set; }
    }

    public class UserProductTransactionMetaData
    {
        [JsonIgnore]
        public virtual PaymentTransaction PaymentTransaction { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
        [JsonIgnore]
        public virtual SubProduct SubProduct { get; set; }

        [JsonIgnore]
        public virtual UserAddress UserAddress { get; set; }
        [JsonIgnore]
        public virtual UserAddress UserAddress1 { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProductTransactionSpecification> UserProductTransactionSpecifications { get; set; }
    }

    public class UserProductTransactionSpecificationMetaData
    {
        [JsonIgnore]
        public virtual ProductSpecification ProductSpecification { get; set; }
        [JsonIgnore]
        public virtual UserProductTransaction UserProductTransaction { get; set; }
    }

    public class PaymentTransactionMetaData 
    {
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProductTransaction> UserProductTransactions { get; set; }
    }

    public class FeaturesCategoryMetaData
    {
        [JsonIgnore]
        public virtual BusinessType BusinessType { get; set; }
        [JsonIgnore]
        public virtual ICollection<FeaturesSubCategory> FeaturesSubCategories { get; set; }
    }

    public class FeaturesSubCategoryMetaData
    {
        [JsonIgnore]
        public virtual FeaturesCategory FeaturesCategory { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductFeature> ProductFeatures { get; set; }
    }

    public class CustomCurrencyMetaData
    { 
    
    }
#endregion
    

}
