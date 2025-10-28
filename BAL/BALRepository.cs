using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Text;
using DAL;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Utility;
using BAL;
//using Dapper;

namespace BAL
{

    //public class OfflineSalesRepository : RepositoryBase<OfflineSale>
    //{
    //}
    public class SuperCategoryRepository : RepositoryBase<SuperCategory>
    {
        public void Save(string Name, long empid)
        {
            SuperCategory sc = new SuperCategory();
            sc.SuperCategoryName = Name;
            sc.IsActive = true;
            sc.IsDeleted = false;
            sc.CreatedOn = DateTime.Now;
            sc.UpdatedOn = DateTime.Now;
            sc.UpdatedBy = empid;
            sc.CreatedBy = empid;
            Insert(sc);
        }
        public IEnumerable<Object> GetSuperCatByGroup()
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {

                IEnumerable<Object> data = (from a in Context.SuperCategories
                                            select new
                                            {
                                                Categories = (from c in Context.Categories
                                                              where c.SuperCategoryId == a.SuperCategoryId
                                                              select new
                                                              {
                                                                  CategoryId = c.CategoryId,
                                                                  CategoryName = c.CategoryName,

                                                                  SubCategories = (from sc in Context.SubCategories
                                                                                   where sc.CategoryId == c.CategoryId
                                                                                   select new
                                                                                   {
                                                                                       SubCategoryId = sc.SubCategoryId,
                                                                                       SubCategoryName = sc.SubCategoryName,
                                                                                       Product = (from p in Context.Products
                                                                                                  where sc.SubCategoryId == p.SubCategoryId
                                                                                                  select new
                                                                                                  {
                                                                                                      ProductId = p.ProductId,
                                                                                                      ProductName = p.ProductName,
                                                                                                  }),
                                                                                       Count = Context.Products.Where(p => p.SubCategoryId == sc.SubCategoryId && p.IsDeleted == false).Count()
                                                                                       //Count = (from p in Context.Products where p.IsDeleted == false && p.SubCategoryId == sc.SubCategoryId select p.SubCategoryId).Count(),
                                                                                   }),

                                                              }),
                                                SuperCategoryId = a.SuperCategoryId,
                                                SuperCategoryName = a.SuperCategoryName,
                                            }).ToList();
                return data;
            }
        }

        //public IEnumerable<Object> GetSubCatByCatByGroup(int id)
        //{
        //    using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
        //    {
        //        IEnumerable<Object> data = (from a in Context.Categories
        //                                    where a.CategoryId == id
        //                                    select new
        //                                    {
        //                                        SubCategories = (from sc in Context.SubCategories
        //                                                         where sc.CategoryId == a.CategoryId
        //                                                         select new
        //                                                         {
        //                                                             SubCategoryId = sc.SubCategoryId,
        //                                                             SubCategoryName = sc.SubCategoryName,
        //                                                             Count = (from p in Context.Products where p.IsSold == false && p.SubCategoryId == sc.SubCategoryId select p.SubCategoryId).Count(),
        //                                                         }),
        //                                        CategoryId = a.CategoryId,
        //                                        CategoryName = a.CategoryName,
        //                                    }).ToList();
        //        return data;
        //    }
        //}

    }

    public class OfflineSalesRepository : RepositoryBase<OfflineSale>
    {
    }

    public class NotifyMeRepository : RepositoryBase<NoftifyMe>
    {
    }
    public class GSTRepository : RepositoryBase<Deff_GST>
    {

    }
    public class CategoryRepository : RepositoryBase<Category>
    {
        public void Save(string Name, int scid, long empid)
        {
            Category sc = new Category();
            sc.SuperCategoryId = scid;
            sc.CategoryName = Name;

            sc.IsActive = true;
            sc.IsDeleted = false;
            sc.CreatedOn = DateTime.Now;
            sc.UpdatedOn = DateTime.Now;
            sc.UpdatedBy = empid;
            sc.CreatedBy = empid;
            Insert(sc);
        }
        public IEnumerable<Object> GetCatByGroup()
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                IEnumerable<Object> data = (from a in Context.Categories
                                            select new
                                            {
                                                SubCategories = (from sc in Context.SubCategories
                                                                 where sc.CategoryId == a.CategoryId
                                                                 select new
                                                                 {
                                                                     SubCategoryId = sc.SubCategoryId,
                                                                     SubCategoryName = sc.SubCategoryName,
                                                                     Product = (from p in Context.Products
                                                                                where sc.SubCategoryId == p.SubCategoryId
                                                                                select new
                                                                                {
                                                                                    ProductId = p.ProductId,
                                                                                    ProductName = p.ProductName,
                                                                                }),
                                                                     Count = (from p in Context.Products where p.IsSold == false && p.SubCategoryId == sc.SubCategoryId select p.SubCategoryId).Count(),
                                                                 }),
                                                CategoryId = a.CategoryId,
                                                CategoryName = a.CategoryName,
                                            }).ToList();
                return data;
            }
        }


        public IEnumerable<Object> GetProductsByCatGroup()
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                IEnumerable<Object> data = (from a in Context.Categories
                                            join b in Context.SuperCategories
                                            on a.SuperCategoryId equals b.SuperCategoryId
                                            where (from q in Context.Products
                                                   where q.SubCategory.CategoryId == a.CategoryId
                                                   select q.ProductId).Count() > 0
                                            select new
                                            {
                                                CategoryId = a.CategoryId,
                                                CategoryName = a.CategoryName.Replace(" ", "-"),
                                                SuperCategoryId = a.SuperCategoryId,
                                                SuperCategoryName = b.SuperCategoryName.Replace(" ", "-"),
                                                products = (from p in Context.Products
                                                            where p.IsDeleted == false &&
                                                                p.SubCategory.CategoryId == a.CategoryId
                                                            select p
                                                ).Take(4)
                                            }).ToList();
                return data;
            }
        }

        public IEnumerable<Object> GetSubCatByCatByGroup(int id)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                IEnumerable<Object> data = (from a in Context.Categories
                                            where a.CategoryId == id
                                            select new
                                            {
                                                SubCategories = (from sc in Context.SubCategories
                                                                 where sc.CategoryId == a.CategoryId
                                                                 select new
                                                                 {
                                                                     SubCategoryId = sc.SubCategoryId,
                                                                     SubCategoryName = sc.SubCategoryName,
                                                                     Product = (from p in Context.Products
                                                                                where sc.SubCategoryId == p.SubCategoryId
                                                                                select new
                                                                                {
                                                                                    ProductId = p.ProductId,
                                                                                    ProductName = p.ProductName,
                                                                                }),
                                                                     Count = Context.Products.Where(p => p.SubCategoryId == sc.SubCategoryId && p.IsDeleted == false).Count()

                                                                     //Count = (from p in Context.Products where p.IsSold && p.IsDeleted == false && p.SubCategoryId == sc.SubCategoryId select p.SubCategoryId).Count(),
                                                                 }),
                                                CategoryId = a.CategoryId,
                                                CategoryName = a.CategoryName,
                                            }).ToList();
                return data;
            }
        }

        public IEnumerable<Object> GetProductsBySuperCatGroup(int SuperCategoryID)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                List<Category> categories = Context.Categories.Where(x => x.SuperCategoryId == SuperCategoryID && x.IsActive == true).ToList();

                IEnumerable<Object> data = (from a in categories
                                            join b in Context.SuperCategories
                                            on a.SuperCategoryId equals b.SuperCategoryId
                                            where (from q in Context.Products
                                                   where q.SubCategory.CategoryId == a.CategoryId
                                                   select q.ProductId).Count() > 0
                                            select new
                                            {
                                                CategoryId = a.CategoryId,
                                                CategoryName = a.CategoryName,
                                                SuperCategoryId = a.SuperCategoryId,
                                                SuperCategoryName = b.SuperCategoryName,
                                                products = (from p in Context.Products
                                                            where p.IsDeleted == false &&
                                                                p.SubCategory.CategoryId == a.CategoryId
                                                            select p
                                                ).Take(4).ToList()
                                            }).ToList();
                return data;
            }
        }
    }
    public class ReviewRepository : RepositoryBase<ProductReview>
    {
    }
    public class SubCategoryRepository : RepositoryBase<SubCategory>
    {
        public void Save(string Name, int scid, int cid, long empid)
        {
            SubCategory sc = new SubCategory();
            sc.CategoryId = cid;
            sc.SubCategoryName = Name;
            sc.IsActive = true;
            sc.IsDeleted = false;
            sc.CreatedOn = DateTime.Now;
            sc.UpdatedOn = DateTime.Now;
            sc.UpdatedBy = empid;
            sc.CreatedBy = empid;
            Insert(sc);
        }
    }
    public class StatesRepository : RepositoryBase<State>
    {
    }
    public class AddressRepository : RepositoryBase<UserAddress>
    {
        public UserAddress UserAddress(long userId)
        {
            UserAddress address = new UserAddress();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                address = context.UserAddresses.Where(x => x.AddressTypeId == 2 && x.UserId == userId).FirstOrDefault();
                if (address == null)
                {
                    address = context.UserAddresses.Where(x => x.AddressTypeId == 1 && x.UserId == userId).FirstOrDefault();
                }
            }
            return address;
        }

    }
    public class UserRepository : RepositoryBase<User>
    {
        public int UserDetails(DateTime frmdt, DateTime todt)
        {
            //using (IDbConnection context = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_HuwEntities1"].ConnectionString))
            //{
            //    int usercount = context.Query<User>("select * from Users where CreatedOn<'" + frmdt.ToShortDateString() + "' and CreatedOn > '" + todt.ToShortDateString() + "'").Count();
            //    return usercount;
            //}

            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {

                int usercount = (from user in context.Users
                                 where user.CreatedOn < frmdt && user.CreatedOn > todt
                                 select new
                                 {
                                 }).ToList().Count();
                return usercount;

            }
        }
    }
    public class RoleRepository : RepositoryBase<Role>
    {
    }
    public class CountryRepository : RepositoryBase<Country>
    {
    }
    public class AccessRepository : RepositoryBase<def_accesss>
    {
        public void Updatedata(def_accesss access)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                def_accesss dbaccess = (from a in Context.def_accesss where a.AccessID == access.AccessID select a).FirstOrDefault();
                dbaccess.PageUrl = access.PageUrl;
                dbaccess.Pagename = access.Pagename;
                dbaccess.IsPage = access.IsPage;
                dbaccess.UpdatedOn = access.UpdatedOn;
                //dbaccess.MobileNo = access.MobileNo;
                //dbaccess.UpDatedOn = DateTime.Now;
                //dbaccess.IsActive = access.IsActive;
                Context.SaveChanges();
            }
        }
    }
    public class CouponRepository : RepositoryBase<User>
    {
        public void Updatedata(Cashback_Coupons coups)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                Cashback_Coupons dbProduct = (from a in Context.Cashback_Coupons where a.Coup_ID == coups.Coup_ID select a).FirstOrDefault();
                dbProduct.Is_Active = coups.Is_Active;

                Context.SaveChanges();
            }
        }
    }
    public class CBCRepository : RepositoryBase<CBC_Data_Tree>
    {
        public void Updateaccessss(CBC_Data_Tree acc)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                CBC_Data_Tree dbProduct = (from a in Context.CBC_Data_Tree where a.Prod_ID == acc.Prod_ID select a).FirstOrDefault();
                List<CBC_Data_Tree> dbProducts = (from a in Context.CBC_Data_Tree where a.Prod_ID == acc.Prod_ID select a).ToList();
                foreach (var db in dbProducts)
                {
                    db.Is_Active = acc.Is_Active;
                }
                Context.SaveChanges();
            }
        }
    }

    public class employeeRepository : RepositoryBase<User>
    {
        public void Updatedata(User user)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                User dbProduct = (from a in Context.Users where a.UserId == user.UserId select a).FirstOrDefault();
                dbProduct.FirstName = user.FirstName;
                dbProduct.LastName = user.LastName;
                dbProduct.EmailId = user.EmailId;
                dbProduct.PassCode = user.PassCode;
                dbProduct.MobileNo = user.MobileNo;
                dbProduct.UpDatedOn = DateTime.Now;
                dbProduct.IsActive = user.IsActive;


                Context.SaveChanges();


            }
        }
        public void Updateaccess(tbl_accesspages acc)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                tbl_accesspages dbProduct = (from a in Context.tbl_accesspages where a.Employeeid == acc.Employeeid select a).FirstOrDefault();
                List<tbl_accesspages> dbProducts = (from a in Context.tbl_accesspages where a.Employeeid == acc.Employeeid select a).ToList();
                foreach (var db in dbProducts)
                {
                    dbProduct.IsActive = acc.IsActive;
                    db.IsActive = false;
                }



                Context.SaveChanges();


            }
        }
    }
    public class Blockemployee : RepositoryBase<def_accesss>
    {
    }
    public class userproducttransactionRepository : RepositoryBase<UserProductTransaction>
    {

        public decimal productcost(DateTime frmdt, DateTime todt)
        {
            //using (IDbConnection context = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_HuwEntities1"].ConnectionString))
            //{
            //    decimal total = 0;
            //    if (frmdt != todt)
            //    {
            //        var productcst = context.Query<PaymentTransaction>("select TxnAmount from PaymentTransactions where CreatedOn <'" + frmdt + "' and CreatedOn>'" + todt + "' and TxnStatus='SUCCESS'").ToList();
            //        foreach (var item in productcst)
            //        {
            //            total = total + Convert.ToDecimal(item.TxnAmount);
            //        }
            //    }
            //    else
            //    {
            //        var productcst = context.Query<PaymentTransaction>("select TxnAmount from PaymentTransactions where TxnStatus='SUCCESS'").ToList();
            //        foreach (var item in productcst)
            //        {
            //            total = total + Convert.ToDecimal(item.TxnAmount);
            //        }
            //    }
            //    return total;
            //}

            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                decimal total = 0;
                if (frmdt != todt)
                {
                    var productcst = (from UserProductTransactions in context.PaymentTransactions
                                      where UserProductTransactions.CreatedOn < frmdt && UserProductTransactions.CreatedOn > todt
                                      && UserProductTransactions.TxnStatus == "SUCCESS"
                                      select new
                                      {
                                          UserProductTransactions.TxnAmount
                                      }).ToList();

                    foreach (var item in productcst)
                    {
                        total = total + Convert.ToDecimal(item.TxnAmount);
                    }
                }
                else
                {
                    var productcst = (from UserProductTransactions in context.PaymentTransactions
                                      where UserProductTransactions.TxnStatus == "SUCCESS"
                                      select new
                                      {
                                          UserProductTransactions.TxnAmount
                                      }).ToList();

                    foreach (var item in productcst)
                    {
                        total = total + Convert.ToDecimal(item.TxnAmount);
                    }
                }
                return total;
            }
        }


        public decimal Productcost()
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                decimal total = 0;

                var productcst = (from UserProductTransactions in context.PaymentTransactions
                                  where UserProductTransactions.TxnStatus == "SUCCESS"
                                  select new
                                  {
                                      UserProductTransactions.TxnAmount
                                  }).ToList();

                foreach (var item in productcst)
                {
                    total = total + Convert.ToDecimal(item.TxnAmount);
                }

                return total;
            }
        }
    }

    public class ProductRepository : RepositoryBase<Product>
    {
        public List<Soldproducts> Getsoldproducts(string fromdate, string todate, long? Productid, string Productname)
        {
            if (fromdate == null)
                fromdate = DateTime.Now.AddYears(-100).ToString();
            if (todate == null)
                todate = DateTime.Now.ToString();
            if (Productname != null)
                return Context.Database.SqlQuery<Soldproducts>("select p.productid,p.ProductName,sum(up.quantity) as totalquantity,count(up.paymenttransactionid) as totalorders from products p join userproducttransactions up join PaymentTransactions pt on pt.PaymentTransactionId=up.PaymentTransactionId and pt.TxnStatus='Success' on up.productid=p.ProductId where up.CreatedOn>='" + fromdate + "' and up.CreatedOn<='" + todate + "' and p.productname like '%" + Productname + "%' group by p.ProductId,p.ProductName").ToList();
            if (Productid.HasValue)
                return Context.Database.SqlQuery<Soldproducts>("select p.productid,p.ProductName,sum(up.quantity) as totalquantity,count(up.paymenttransactionid) as totalorders from products p join userproducttransactions up join PaymentTransactions pt on pt.PaymentTransactionId=up.PaymentTransactionId and pt.TxnStatus='Success' on up.productid=p.ProductId where up.CreatedOn>='" + fromdate + "' and up.CreatedOn<='" + todate + "' and up.productid=" + Productid + " group by p.ProductId,p.ProductName").ToList();

            return Context.Database.SqlQuery<Soldproducts>("select p.productid,p.ProductName,sum(up.quantity) as totalquantity,count(up.paymenttransactionid) as totalorders from products p join userproducttransactions up join PaymentTransactions pt on pt.PaymentTransactionId=up.PaymentTransactionId and pt.TxnStatus='Success' on up.productid=p.ProductId where up.CreatedOn>='" + fromdate + "' and up.CreatedOn<='" + todate + "' group by p.ProductId,p.ProductName").ToList();
        }
        public int ProductDetails(DateTime frmdt, DateTime todt)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                int productcount = (from UserProductTransactions in context.UserProductTransactions
                                    where UserProductTransactions.CreatedOn < frmdt && UserProductTransactions.CreatedOn > todt
                                    select new
                                    {
                                    }).ToList().Count();
                return productcount;

            }
        }



        public dynamic GetProductDetails(int ProductId)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                //  Context.Database.Connection.Open();
                var data = (from p in Context.Products
                            where p.ProductId == ProductId
                            select new
                            {
                                ProductId = p.ProductId,

                                subcategory =
                                             (from sc in Context.SubCategories
                                              where sc.SubCategoryId == p.SubCategoryId
                                              select new
                                              {
                                                  subcategoryId = sc.SubCategoryId,
                                                  subcategoryName = sc.SubCategoryName,

                                                  categoryId =
                                                      (from c in Context.Categories
                                                       where c.CategoryId == sc.CategoryId
                                                       select new
                                                       {
                                                           categoryId = c.CategoryId,
                                                           categoryName = c.CategoryName,

                                                           supercategory =
                                                                     (from suprc in Context.SuperCategories
                                                                      where suprc.SuperCategoryId == c.SuperCategoryId
                                                                      select new
                                                                      {
                                                                          supercategoryId = suprc.SuperCategoryId,
                                                                          supercategoryName = suprc.SuperCategoryName
                                                                      })
                                                       })

                                              }),

                                isfeaturedproduct = p.IsFeaturedProduct,
                                ProductName = p.ProductName,
                                productdescription = p.ProductDescription,
                                quantity = p.Quantity,
                                productoriginalcost = p.ProductOriginalCost,
                                ProductCost = p.ProductCost,
                                productdiscountpercentage = p.ProductDiscountPercentage,
                                productdiscountcost = p.ProductDiscountCost,
                                productimageurl = p.ProductImgUrl,
                                productfeaturescontent = p.ProductFeaturesContent,
                                isactive = p.IsActive,
                                isdeleted = p.IsDeleted,
                                createdon = p.CreatedOn,
                                updatedon = p.UpdatedOn,
                                createdby = p.CreatedBy,
                                updatedby = p.UpdatedBy,
                                issold = p.IsSold,
                                cancompare = p.CanCompare,
                                hasreviews = p.HasReviews,
                                canreviewsneedpermissions = p.CanReviewsNeedPermission,

                                subproduct = (from q in Context.SubProducts
                                              where q.ProductId == p.ProductId
                                              select new
                                              {
                                                  productId = q.ProductId,
                                                  subproductId = q.SubProductId,
                                                  spName = q.SPName,
                                                  subproductoriginalcost = q.ProductOriginalCost,
                                                  subproductdiscountpercentage = q.ProductDiscountPercentage,
                                                  subproductdiscountcost = q.ProductDiscountCost,
                                                  subproductcost = q.ProductCost,
                                                  subproductquantity = q.Quantity
                                              }),

                                relatedproduct = (from rp in Context.RelatedProducts
                                                  where rp.ProductId == p.ProductId
                                                  select new
                                                  {
                                                      productId = rp.ProductId,
                                                      id = rp.Id,
                                                      relatedproductId = rp.RelatedProductId
                                                  }),

                                ProductSpecifications = p.ProductSpecifications,
                                // ProductFeatures = p.ProductFeatures,
                                // ProductsGalleries = p.ProductsGalleries



                            }).ToList();

                return data.ToList();
            }
        }


        public void Save(Product product)
        {
            try
            {
                //Live Code
                //product.ProductImgUrl = product.ProductImgs.Upload(Shared.ProductImageTypes.ProductImg);
                //Test Code
                string ImgPath = product.ProductImgs.Upload(Shared.ProductImageTypes.ProductImg);
                string[] Img = ImgPath.Split('\\');
                product.ProductImgUrl = "https://www.healthurwealth.com/UploadFiles/" + Img[5];
                string ImgPaths = product.ProductImgss.Upload(Shared.ProductImageTypes.ProductImg);
                string[] Imgs = ImgPaths.Split('\\');
                product.ProductImgUrl2 = "https://www.healthurwealth.com/UploadFiles/" + Imgs[5];

                Insert(product);
            }
            catch (System.Data.SqlTypes.SqlTypeException sdex)
            {
                throw sdex;
            }


            catch (Exception ex)
            {
                // // Shared.Log.Error(ex);
                throw;
            }
        }
        public void UpdateAdditionalinfo(Tbl_AdditionalInfo info)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                ProductRepository rep = new ProductRepository();
                if (BalUtility.GetProductAddInfo(info.ProductId) == null)
                {
                    info.CreatedOn = DateTime.Now;
                    Context.Tbl_AdditionalInfo.Add(info);
                    Context.SaveChanges();
                }
                else
                {
                    Tbl_AdditionalInfo datatoupdate = Context.Tbl_AdditionalInfo.Where(x => x.ProductId == info.ProductId).FirstOrDefault();
                    datatoupdate.Marketerdetails = info.Marketerdetails;
                    datatoupdate.Best_Before_Date = info.Best_Before_Date;
                    datatoupdate.Country_Of_Origin = info.Country_Of_Origin;
                    datatoupdate.GTIN = info.GTIN;
                    datatoupdate.BatchNo = info.BatchNo;
                    datatoupdate.Manufacturer_Details = info.Manufacturer_Details;
                    datatoupdate.Manufacturer_Date = info.Manufacturer_Date;
                    Context.SaveChanges();
                }
            }
        }

        public void UpdateProduct(Product product)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                Product dbProduct = (from a in Context.Products where a.ProductId == product.ProductId select a).FirstOrDefault();
                dbProduct.Weight = product.Weight;
                dbProduct.IsPresciption = product.IsPresciption;
                dbProduct.ProductId = product.ProductId;
                dbProduct.IsFeaturedProduct = product.IsFeaturedProduct;
                dbProduct.CanCompare = product.CanCompare;
                dbProduct.HasReviews = product.HasReviews;
                dbProduct.CanReviewsNeedPermission = product.CanReviewsNeedPermission;
                dbProduct.Occasion = product.Occasion;
                dbProduct.ProductCode = product.ProductCode;
                dbProduct.ProductColor = product.ProductColor;
                dbProduct.ProductDescription = product.ProductDescription;
                dbProduct.ShortDescription = product.ShortDescription;
                dbProduct.ProductOriginalCost = product.ProductOriginalCost;
                dbProduct.ProductCost = product.ProductCost;
                dbProduct.ProductDiscountPercentage = product.ProductDiscountPercentage;
                dbProduct.ProductDiscountCost = product.ProductDiscountCost;
                dbProduct.Quantity = product.Quantity;
                dbProduct.ProductName = product.ProductName;
                dbProduct.HSNCode = product.HSNCode;
                dbProduct.GST = product.GST;
                dbProduct.Brand = product.Brand;
                dbProduct.ProductFeaturesContent = product.ProductFeaturesContent;
                dbProduct.Free_Product_ID = product.Free_Product_ID;
                dbProduct.Is_Free_Product_Active = product.Is_Free_Product_Active;
                dbProduct.SubCategoryId = product.SubCategoryId;
                dbProduct.Page_Title = product.Page_Title;
                dbProduct.Meta_Description = product.Meta_Description;
                dbProduct.Meta_Keywords = product.Meta_Keywords;
                dbProduct.H2Tag = product.H2Tag;
                if (product.ProductImgs != null && product.ProductImgUrl != null)
                {
                    dbProduct.ProductImgs = product.ProductImgs;
                    dbProduct.ProductImgUrl = product.ProductImgUrl;
                }
                if (product.ProductImgss != null && product.ProductImgUrl2 != null)
                {
                    dbProduct.ProductImgss = product.ProductImgss;
                    dbProduct.ProductImgUrl2 = product.ProductImgUrl2;
                }

                foreach (var item in product.ProductFeatures)
                {
                    ProductFeature dbProductFeature = new ProductFeature();
                    if (item.ProductFeatureId != 0)
                    {
                        dbProductFeature = (from a in Context.ProductFeatures where a.ProductFeatureId == item.ProductFeatureId select a).FirstOrDefault();
                    }
                    if (dbProductFeature == null)
                    {
                        dbProductFeature = new ProductFeature();
                    }

                    dbProductFeature.ProductId = item.ProductId;
                    dbProductFeature.FeatureInfo = item.FeatureInfo;
                    dbProductFeature.FeaturesSubCategoryId = item.FeaturesSubCategoryId;
                    if (item.ProductFeatureId == 0)
                    {
                        Context.ProductFeatures.Add(dbProductFeature);
                    }
                }

                foreach (var item in product.ProductSpecifications)
                {
                    ProductSpecification dbProductSpecification = new ProductSpecification();
                    if (item.ProductSpecificationId != 0)
                    {
                        dbProductSpecification = (from a in Context.ProductSpecifications where a.ProductSpecificationId == item.ProductSpecificationId select a).FirstOrDefault();
                    }
                    if (dbProductSpecification == null)
                    {
                        dbProductSpecification = new ProductSpecification();
                    }
                    dbProductSpecification.ProductId = item.ProductId;
                    dbProductSpecification.ProductSpecificationName = item.ProductSpecificationName;
                    dbProductSpecification.ProductSpecificationNameValues = item.ProductSpecificationNameValues;
                    dbProductSpecification.SpecificationTypeId = item.SpecificationTypeId;

                    if (item.ProductSpecificationId == 0)
                    {
                        Context.ProductSpecifications.Add(dbProductSpecification);
                    }
                }

                foreach (var item in product.SubProducts)
                {
                    SubProduct dbSubProduct = new SubProduct();
                    if (item.SubProductId != 0)
                    {
                        dbSubProduct = (from a in Context.SubProducts where a.SubProductId == item.SubProductId select a).FirstOrDefault();
                    }
                    if (dbSubProduct == null)
                    {
                        dbSubProduct = new SubProduct();
                    }
                    dbSubProduct.ProductId = item.ProductId;
                    dbSubProduct.SPName = item.SPName;
                    dbSubProduct.Quantity = item.Quantity;
                    dbSubProduct.ProductOriginalCost = item.ProductOriginalCost;
                    dbSubProduct.ProductDiscountPercentage = item.ProductDiscountPercentage;
                    dbSubProduct.ProductDiscountCost = item.ProductDiscountCost;
                    dbSubProduct.ProductCost = item.ProductCost;

                    if (item.SubProductId == 0)
                    {
                        Context.SubProducts.Add(dbSubProduct);
                    }
                }

                Context.Database.ExecuteSqlCommand("delete RelatedProducts where ProductId=" + product.ProductId);
                if (product.RelatedProducts.Count != 0)
                {
                    foreach (var item in product.RelatedProducts)
                    {
                        RelatedProduct cartitem = new RelatedProduct();
                        cartitem.ProductId = item.ProductId;
                        cartitem.RelatedProductId = item.RelatedProductId;
                        Context.RelatedProducts.Add(cartitem);
                    }

                    //foreach (var item in product.RelatedProducts)
                    //{
                    //     var cartitem = Context.RelatedProducts.Where(x => x.RelatedProductId = item.RelatedProductId && x.ProductId == product.ProductId).FirstOrDefault();
                    //        if (cartitem != null)
                    //        {
                    //            Context.RelatedProducts.Remove(cartitem);
                    //        }

                    //        if (Context.RelatedProducts.Where(x => x.RelatedProductId == item.RelatedProductId && x.ProductId == product.ProductId).FirstOrDefault() == null)
                    //        {
                    //            cartitem = new RelatedProduct();
                    //            cartitem.ProductId = item.ProductId;
                    //            cartitem.RelatedProductId = item.RelatedProductId;
                    //            Context.RelatedProducts.Add(cartitem);
                    //        }
                    //    //RelatedProduct dbRelatedProduct = new RelatedProduct();
                    //    //if (item.Id != 0)
                    //    //{
                    //    //    dbRelatedProduct = (from a in Context.RelatedProducts where a.Id == item.Id select a).FirstOrDefault();

                    //    //}



                    //    //if (dbRelatedProduct == null)
                    //    //{
                    //    //    dbRelatedProduct = new RelatedProduct();
                    //    //}

                    //    //dbRelatedProduct.ProductId = item.ProductId;
                    //    //dbRelatedProduct.RelatedProductId = item.RelatedProductId;

                    //    //if (item.Id == 0)
                    //    //{
                    //    //    Context.RelatedProducts.Add(dbRelatedProduct);
                    //    //}

                    //    }
                }
                //else
                //{
                //    var cartitem = Context.RelatedProducts.Where(x => x.ProductId == product.ProductId).FirstOrDefault();
                //    Context.RelatedProducts.Remove(cartitem);
                //}
                Context.SaveChanges();


            }
        }

        public void UpdateProductQtyAndInsert(Product product)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                Product dbProduct = (from a in Context.Products where a.ProductId == product.ProductId select a).FirstOrDefault();
                dbProduct.Quantity = product.Quantity;

                foreach (var item in product.SubProducts)
                {
                    SubProduct dbSubProduct = new SubProduct();
                    if (item.SubProductId != 0)
                    {
                        dbSubProduct = (from a in Context.SubProducts where a.SubProductId == item.SubProductId select a).FirstOrDefault();
                    }

                    dbSubProduct.ProductId = item.ProductId;
                    dbSubProduct.SPName = item.SPName;
                    dbSubProduct.Quantity = item.Quantity;

                }
                Context.SaveChanges();
            }
        }

        public bool OrderProducts(ICollection<UserProductTransaction> data)
        {
            try
            {
                foreach (var userProductTransaction in data)
                {
                    userProductTransaction.Product = null;

                    foreach (var item in userProductTransaction.UserProductTransactionSpecifications)
                        item.UserProductTransaction = null;
                }

                var repository = new UserProductTransactionRepository();
                repository.Insert(data);
                return true;
            }
            catch (Exception ex)
            {
                //// Shared.Log.Error(ex);
                throw;
            }
        }


        public List<Product> GetLatestProductList()
        {
            var repository = new ProductRepository();
            int totalRecords;
            return repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 4, (c => c.IsSold && c.IsActive && c.IsDeleted == false), null, "", true);
        }

        public List<Product> GetAllLatestProductList()
        {
            var repository = new ProductRepository();
            //using (IDbConnection context = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_HuwEntities1"].ConnectionString))
            //{
            //    List<Product> data = new List<Product>();
            //    data = context.Query<Product>("Select Top 4 * from Products where IsSold='false' and IsActive='true' and IsDeleted='false' order by  CreatedOn desc").ToList();
            //    return data;
            //}

            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                List<Product> data = new List<Product>();
                var dta = Context.Products.Where(f => f.IsSold == false && f.IsActive == true && f.IsDeleted == false).OrderByDescending(f => f.CreatedOn).Take(4);
                data = dta.ToList();
                return data;
            }
        }

        public dynamic GetDiscountProductList()
        {
            var repository = new ProductRepository();

            //using (IDbConnection context = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_HuwEntities1"].ConnectionString))
            //{
            //    var datas = context.Query<Product>("select Top 3 ProductDiscountCost,ProductId,ProductImgUrl from Products where IsSold='false' order by ProductDiscountCost desc").ToList();
            //    return datas;
            //}

            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                //List<Product> data = new List<Product>();
                //var dta = Context.Products.Where(f => f.IsSold == false).OrderByDescending(f => f.CreatedOn).ToList();
                //var dta = Context.Products.Where(f => f.IsSold && f.IsDeleted == false).OrderBy(f => f.ProductDiscountCost).Take(3);
                var datas = (from dis in Context.Products
                             where dis.IsSold == false
                             orderby dis.ProductDiscountCost descending
                             select new
                             {
                                 dis.ProductDiscountCost,
                                 dis.ProductId,
                                 dis.ProductImgUrl
                             }).Take(3);
                return datas.ToList();
            }
        }

        public dynamic GetProducts(int rows, long ProductId, string ProductName, int SuperCategory, int Category, int SubCategory, string ProductStatus, string Brand, int Quantity)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                List<Product> data = new List<Product>();

                bool status = true;
                if (ProductStatus == "1")
                {
                    status = true;
                }
                else if (ProductStatus == "-1")
                {
                    ProductStatus = null;
                }
                else
                {
                    status = false;
                }
                if (ProductStatus == "2")
                {
                    var datas = (from p in Context.Products
                                 join subc in Context.SubCategories on p.SubCategoryId equals subc.SubCategoryId
                                 join c in Context.Categories on subc.CategoryId equals c.CategoryId
                                 join sup in Context.SuperCategories on c.SuperCategoryId equals sup.SuperCategoryId
                                 where (ProductId == 0 ? true : (p.ProductId == ProductId))
                                 && (ProductName == null ? true : (p.ProductName.Contains(ProductName)))
                                 && (SuperCategory == 0 ? true : (sup.SuperCategoryId == SuperCategory))
                                 && (Category == 0 ? true : (c.CategoryId == Category))
                                 && (SubCategory == 0 ? true : (subc.SubCategoryId == SubCategory))
                                 && (Brand == null ? true : (p.Brand == Brand))
                                 && (Quantity == 0 ? true : ((Quantity == 1 ? p.Quantity <= 0 : (Quantity == 2 ? (p.Quantity >= 1 && p.Quantity < 5) : (p.Quantity >= 5)))))
                                 && (ProductStatus == null ? true : (p.IsDeleted == status))
                                 select new
                                 {
                                     ProductId = p.ProductId,
                                     IsActive = p.IsActive,
                                     IsDeleted = p.IsDeleted,
                                     IsSold = p.IsSold,
                                     ProductCost = p.ProductCost,
                                     ProductDiscountPercentage = p.ProductDiscountPercentage,
                                     ProductOriginalCost = p.ProductOriginalCost,
                                     ProductImgUrl = p.ProductImgUrl,
                                     ProductName = p.ProductName,
                                     Quantity = p.Quantity,
                                     UpdatedOn = p.UpdatedOn,
                                     Brand = p.Brand,
                                     CreatedOn = p.CreatedOn,
                                     SubCategoryName = p.SubCategory.SubCategoryName,
                                     SoldQty = p.UserProductTransactions.Where(x => x.ProductId == p.ProductId).Count()
                                 }).OrderByDescending(x => x.SoldQty).ToList().Take(100);

                    return datas;
                }
                else
                {
                    var datas = (from p in Context.Products
                                 join subc in Context.SubCategories on p.SubCategoryId equals subc.SubCategoryId
                                 join c in Context.Categories on subc.CategoryId equals c.CategoryId
                                 join sup in Context.SuperCategories on c.SuperCategoryId equals sup.SuperCategoryId
                                 where (ProductId == 0 ? true : (p.ProductId == ProductId))
                                 && (ProductName == null ? true : (p.ProductName.Contains(ProductName)))
                                 && (SuperCategory == 0 ? true : (sup.SuperCategoryId == SuperCategory))
                                 && (Category == 0 ? true : (c.CategoryId == Category))
                                 && (SubCategory == 0 ? true : (subc.SubCategoryId == SubCategory))
                                 && (Brand == null ? true : (p.Brand == Brand))
                                 && (Quantity == 0 ? true : ((Quantity == 1 ? p.Quantity <= 0 : (Quantity == 2 ? (p.Quantity >= 1 && p.Quantity < 5) : (p.Quantity >= 5)))))
                                 && (ProductStatus == null ? true : (p.IsDeleted == status))
                                 select new
                                 {
                                     ProductId = p.ProductId,
                                     IsActive = p.IsActive,
                                     IsDeleted = p.IsDeleted,
                                     IsSold = p.IsSold,
                                     ProductCost = p.ProductCost,
                                     ProductDiscountPercentage = p.ProductDiscountPercentage,
                                     ProductOriginalCost = p.ProductOriginalCost,
                                     ProductImgUrl = p.ProductImgUrl,
                                     ProductName = p.ProductName,
                                     Quantity = p.Quantity,
                                     UpdatedOn = p.UpdatedOn,
                                     Brand = p.Brand,
                                     CreatedOn = p.CreatedOn,
                                     SubCategoryName = p.SubCategory.SubCategoryName,
                                     SoldQty = p.UserProductTransactions.Where(x => x.ProductId == p.ProductId).Count()
                                 }).OrderByDescending(x => x.ProductId).ToList();

                    return datas;
                }
            }
        }

        public dynamic EverydayPerformanceGraphBR()
        {
            db_Zon_HuwEntities Context = new db_Zon_HuwEntities();
            var enddate = DateTime.Now;
            var startdate = enddate.AddDays(-31);
            object[] superarray = new object[4];
            int[] data1 = new int[31];
            int[] data2 = new int[31];
            int[] data3 = new int[31];
            int[] data4 = new int[31];
            //int count1 = 0;
            int count5 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            var datas = Context.PaymentTransactions.Where(x => x.CreatedOn >= startdate && x.CreatedOn <= enddate).ToList();
            //List<EverydayPerformanceGraph> graphtable = new List<EverydayPerformanceGraph>();
            //List<PaymentTransaction> currentdaydata = new List<PaymentTransaction>();
            var currentdaydate = enddate;
            for (var y = 0; y <= 31; y++)
            {

                foreach (var item in datas)
                {
                    if (item.CreatedOn.Date == currentdaydate.Date)
                    {
                        if (item.TxnStatus == "Pending")
                        {
                            count5 = count5 + 1;
                        }
                        if (item.TxnStatus == "SUCCESS")
                        {
                            count2 = count2 + 1;
                        }
                        if (item.TxnStatus == "RETURNED")
                        {
                            count3 = count3 + 1;
                        }
                        if (item.TxnStatus == "Cancelled")
                        {
                            count4 = count4 + 1;
                        }
                    }
                }
                data1[y] = count5;
                data2[y] = count2;
                data3[y] = count3;
                data4[y] = count4;
                count5 = 0;
                count2 = 0;
                count3 = 0;
                count4 = 0;
                currentdaydate = currentdaydate.AddDays(-1);
            }
            superarray[0] = data1;
            superarray[1] = data2;
            superarray[2] = data3;
            superarray[3] = data4;

            return superarray;
        }
        public dynamic GetRoutineCustomersData()
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                var datas = Context.Database.SqlQuery<Utility.RoutineCustomers>("select x.FirstName,x.Emailid,x.MobileNo,T.userid,T.ProductCost, T.Quantity, T.ProductCost * T.Quantity as Totalcost, T.CreatedOn, y.TxnStatus, T.productid,P.ProductName,T.quantity,g.Status from ( select T.userid, T.productid, T.quantity, T.ProductCost, t.CreatedOn, T.PaymentTransactionId, row_number() over(partition by T.userid,T.productid order by T.userid desc) as rn from dbo.UserProductTransactions as T ) as T inner JOIN(SELECT UserId, ProductId FROM[UserProductTransactions] GROUP BY UserId, ProductId HAVING count(*) > 1) b ON T.UserId = b.UserId AND T.ProductId = b.ProductId inner join Users as x on x.UserId = T.UserId inner join PaymentTransactions as y on y.PaymentTransactionId = T.PaymentTransactionId left join Routine_Customers_Table as g on g.User_ID=T.UserId and g.Product_ID=T.ProductId left join Products P on T.ProductId=P.ProductId where T.rn <= 2").ToList<Utility.RoutineCustomers>();
                return datas;
            }
        }
        public dynamic GetProductsfortopselling(int rows, long ProductId, string ProductName, int SuperCategory, int Category, int SubCategory, string ProductStatus, string Brand, int Quantity, int NoOfProds = -1, int NoOfPastDays = -1)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                List<Product> data = new List<Product>();

                bool status = true;
                if (ProductStatus == "1")
                {
                    status = true;
                }
                else if (ProductStatus == "-1")
                {
                    ProductStatus = null;
                }
                else
                {
                    status = false;
                }

                var NOOFDAYSDATE = Convert.ToDateTime(DateTime.Now.AddDays(-100000));
                var N = 1000000;

                if (NoOfProds != -1)
                {
                    N = NoOfProds;
                }
                if (NoOfPastDays != -1)
                {
                    NOOFDAYSDATE = Convert.ToDateTime(DateTime.Now.AddDays(-NoOfPastDays));
                }
                var datas = (from p in Context.Products
                             join subc in Context.SubCategories on p.SubCategoryId equals subc.SubCategoryId
                             join c in Context.Categories on subc.CategoryId equals c.CategoryId
                             join sup in Context.SuperCategories on c.SuperCategoryId equals sup.SuperCategoryId
                             where (ProductId == 0 ? true : (p.ProductId == ProductId))
                             && (ProductName == null ? true : (p.ProductName.Contains(ProductName)))
                             && (SuperCategory == 0 ? true : (p.SubCategoryId == SuperCategory))
                             && (Category == 0 ? true : (subc.CategoryId == Category))
                             && (SubCategory == 0 ? true : (c.SuperCategoryId == SubCategory))
                             && (Brand == null ? true : (p.Brand == Brand))
                             && (Quantity == 0 ? true : ((Quantity == 1 ? p.Quantity <= 0 : (Quantity == 2 ? (p.Quantity >= 1 && p.Quantity < 5) : (p.Quantity >= 5)))))
                             && (ProductStatus == null ? true : (p.IsDeleted == status))
                             && (NoOfPastDays == -1 ? true : (p.CreatedOn >= NOOFDAYSDATE))
                             select new
                             {
                                 ProductId = p.ProductId,
                                 IsActive = p.IsActive,
                                 IsDeleted = p.IsDeleted,
                                 IsSold = p.IsSold,
                                 ProductCost = p.ProductCost,
                                 ProductDiscountPercentage = p.ProductDiscountPercentage,
                                 ProductOriginalCost = p.ProductOriginalCost,
                                 ProductImgUrl = p.ProductImgUrl,
                                 ProductName = p.ProductName,
                                 Quantity = p.Quantity,
                                 UpdatedOn = p.UpdatedOn,
                                 Brand = p.Brand,
                                 CreatedOn = p.CreatedOn,
                                 SubCategoryName = p.SubCategory.SubCategoryName,
                                 SoldQty = p.UserProductTransactions.Where(x => x.ProductId == p.ProductId).Count()
                             }).OrderByDescending(x => x.SoldQty).ToList().Take(N);

                return datas = datas.OrderByDescending(x => x.SoldQty);
            }
        }
        public List<Product> GetFeatureProductList()
        {
            var repository = new ProductRepository();

            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                //int totalRecords;
                //return repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 4,
                //    (c => c.IsSold == false && c.IsFeaturedProduct == true), null, "", false);

                List<Product> data = new List<Product>();
                var dta = Context.Products.Where(f => f.IsFeaturedProduct == true && f.IsActive && f.IsDeleted == false).OrderByDescending(f => f.CreatedOn).Take(4);
                data = dta.ToList();
                return data;
            }

        }

        public List<Product> GetAllFeatureProductList()
        {
            var repository = new ProductRepository();

            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                //int totalRecords;
                //return repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 4,
                //    (c => c.IsSold == false && c.IsFeaturedProduct == true), null, "", false);

                List<Product> data = new List<Product>();
                var dta = Context.Products.Where(f => f.IsFeaturedProduct == true && f.IsDeleted == false).OrderByDescending(f => f.CreatedOn).ToList();
                data = dta.ToList();
                return data;
            }

        }

        public Product GetProduct(int productId)
        {
            var repository = new ProductRepository();
            int totalRecords;
            return repository.Single(p => p.ProductId == productId);

        }

        public dynamic GetBrandNames()
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                IEnumerable<Object> brands = (from s in Context.Products
                                              select new
                                              {
                                                  Brand = s.Brand
                                              }).Distinct().ToList();

                return brands;
            }

        }

        //modify by shashi

        public dynamic GetBoxNames()
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                //return (from s in Context.Tbl_Packing_Box
                //                           select new Tbl_Packing_Box
                //                           {
                //                               BoxName = s.BoxName,
                //                               BoxId = s.BoxId
                //                           }).Distinct().ToList();
                return Context.Database.SqlQuery<Boxs>("select Distinct BoxName,BoxId from Tbl_Packing_Box where isactive=1").ToList();

                //return Box;
            }
        }

        public dynamic GetProductsBySuperCategory(int SuperCatId, int sort, int value)
        {
            List<ProductListDTO> list = new List<ProductListDTO>();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                if (SuperCatId == 8)
                {
                    list = (from p in context.Products
                            where p.IsActive == true && p.ProductDiscountPercentage != 0
                            && p.Quantity > 0 && p.IsDeleted == false
                            select new ProductListDTO
                            {
                                Brand = p.Brand,
                                CategoryId = p.SubCategory.CategoryId,
                                CategoryName = p.SubCategory.Category.CategoryName,
                                IsActive = p.IsActive,
                                IsDeleted = p.IsDeleted,
                                IsFeaturedProduct = p.IsFeaturedProduct,
                                IsSold = p.IsSold,
                                ProductCost = p.ProductCost,
                                ProductDiscountPercentage = p.ProductDiscountPercentage,
                                ProductId = p.ProductId,
                                ProductImgUrl = p.ProductImgUrl,
                                ProductName = p.ProductName.Replace(" ", "-"),
                                ProductOriginalCost = p.ProductOriginalCost,
                                Quantity = p.Quantity,
                                SubCategoryId = p.SubCategoryId,
                                SubCategoryName = p.SubCategory.SubCategoryName,
                                UpdatedOn = p.UpdatedOn
                            }).OrderByDescending(x => x.UpdatedOn).ToList();
                }
                else
                {
                    list = (from p in context.Products
                            where p.SubCategory.Category.SuperCategoryId == SuperCatId && p.IsActive == true
                            && p.Quantity > 0 && p.IsDeleted == false
                            select new ProductListDTO
                            {
                                Brand = p.Brand,
                                CategoryId = p.SubCategory.CategoryId,
                                CategoryName = p.SubCategory.Category.CategoryName,
                                IsActive = p.IsActive,
                                IsDeleted = p.IsDeleted,
                                IsFeaturedProduct = p.IsFeaturedProduct,
                                IsSold = p.IsSold,
                                ProductCost = p.ProductCost,
                                ProductDiscountPercentage = p.ProductDiscountPercentage,
                                ProductId = p.ProductId,
                                ProductImgUrl = p.ProductImgUrl,
                                ProductName = p.ProductName.Replace(" ", "-"),
                                ProductOriginalCost = p.ProductOriginalCost,
                                Quantity = p.Quantity,
                                SubCategoryId = p.SubCategoryId,
                                SubCategoryName = p.SubCategory.SubCategoryName,
                                UpdatedOn = p.UpdatedOn
                            }).ToList();
                }

                if (sort == 1)
                {
                    list = list.OrderBy(x => x.ProductCost).ToList();
                }
                else if (sort == 2)
                {
                    list = list.OrderByDescending(x => x.ProductCost).ToList();
                }

                if (value == 0)
                {
                    list = list.ToList();
                }
                else
                {
                    int skip = 0;
                    if (value > 15)
                    {
                        skip = value - 15;
                    }
                    list = list.Skip(skip).Take(value - skip).ToList();
                }
                return list;
            }
        }


        public dynamic GetProductsBetweenPriceinSuperCategory(int SuperCatId, int sort, decimal MinPrice, decimal MaxPrice)
        {
            List<ProductListDTO> list = new List<ProductListDTO>();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                if (SuperCatId == 8)
                {
                    list = (from p in context.Products
                            where p.IsActive == true
                            && p.Quantity > 0 && p.IsDeleted == false
                            && p.ProductCost >= MinPrice && p.ProductCost <= MaxPrice
                            select new ProductListDTO
                            {
                                Brand = p.Brand,
                                CategoryId = p.SubCategory.CategoryId,
                                CategoryName = p.SubCategory.Category.CategoryName,
                                IsActive = p.IsActive,
                                IsDeleted = p.IsDeleted,
                                IsFeaturedProduct = p.IsFeaturedProduct,
                                IsSold = p.IsSold,
                                ProductCost = p.ProductCost,
                                ProductDiscountPercentage = p.ProductDiscountPercentage,
                                ProductId = p.ProductId,
                                ProductImgUrl = p.ProductImgUrl,
                                ProductName = p.ProductName.Replace(" ", "-"),
                                ProductOriginalCost = p.ProductOriginalCost,
                                Quantity = p.Quantity,
                                SubCategoryId = p.SubCategoryId,
                                SubCategoryName = p.SubCategory.SubCategoryName,
                                UpdatedOn = p.UpdatedOn
                            }).OrderByDescending(x => x.ProductDiscountPercentage).ToList();
                }
                else
                {
                    list = (from p in context.Products
                            where p.SubCategory.Category.SuperCategoryId == SuperCatId && p.IsActive == true
                            && p.Quantity > 0 && p.IsDeleted == false
                           && p.ProductCost >= MinPrice && p.ProductCost <= MaxPrice
                            select new ProductListDTO
                            {
                                Brand = p.Brand,
                                CategoryId = p.SubCategory.CategoryId,
                                CategoryName = p.SubCategory.Category.CategoryName,
                                IsActive = p.IsActive,
                                IsDeleted = p.IsDeleted,
                                IsFeaturedProduct = p.IsFeaturedProduct,
                                IsSold = p.IsSold,
                                ProductCost = p.ProductCost,
                                ProductDiscountPercentage = p.ProductDiscountPercentage,
                                ProductId = p.ProductId,
                                ProductImgUrl = p.ProductImgUrl,
                                ProductName = p.ProductName.Replace(" ", "-"),
                                ProductOriginalCost = p.ProductOriginalCost,
                                Quantity = p.Quantity,
                                SubCategoryId = p.SubCategoryId,
                                SubCategoryName = p.SubCategory.SubCategoryName,
                                UpdatedOn = p.UpdatedOn
                            }).OrderByDescending(x => x.ProductDiscountPercentage).ToList();
                }
            }
            return list;
        }

        public IEnumerable<Object> Getbrnds()
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                IEnumerable<Object> brands = (from s in Context.Products
                                              where s.IsActive == true && s.IsDeleted == false
                                              select new
                                              {
                                                  Brand = s.Brand

                                              }).Distinct().ToList();

                // orderby s.Brand ascending
                // select s.Brand).Distinct();
                return brands;
            }
        }

        //public IEnumerable<Object> GetbrndsByCatId(long pcategoryId)
        //{
        //    using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
        //    {
        //        // 
        //        var SubCatgries = (from s in Context.Categories //var SubCat
        //                                           select new
        //                                           {
        //                                               Categories = (from c in Context.SubCategories
        //                                                             where c.Category.CategoryId == pcategoryId
        //                                                             select new
        //                                                             {
        //                                                                 SubCategoryId = c.SubCategoryId,
        //                                                                 // SubCategoryName = c.SubCategoryName
        //                                                             }).Distinct()
        //                                           }).ToList();
        //        //IEnumerable<Object> totlbrands;
        //        //IEnumerable<Object> brands;
        //        // StringBuilder obj = new StringBuilder();
        //        //List<long> subcatlst = new List<long>();
        //        // int totalRecords;
        //        //foreach (var item in SubCatgries)
        //        //{
        //        //    subcatlst.Add(Convert.ToInt64(item));

        //            //long id = Convert.ToInt64(item.GetType().GetProperty("Brand").GetValue(item, null));
        //            //brands = (from s in Context.Products
        //            //          where s.SubCategoryId == id
        //            //                              select new
        //            //                              {
        //            //                                  Brand = s.Brand

        //            //                              }).Distinct().ToList();

        //            //obj.Append(brands);
        //            //totlbrands = totlbrands + brands;
        //       // }
        //        //ProductRepository repository = new ProductRepository();
        //        //var data = pcategoryId != 0 ? (repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
        //        //    (p => p.IsSold == false && p.IsActive && p.IsDeleted == false && p.SubCategory.CategoryId == pcategoryId && (subcatlst.Count == 0 ? true : subcatlst.Contains(p.SubCategoryId))), null, "").Distinct()) : null;

        //        //var h = obj;
        //        // IEnumerable<Object> brands ;
        //        //foreach (var item in SubCat)
        //        //{
        //        //    var bds = 
        //        //    brands = brands + 
        //        //}
        //        //List<long> SCat = new List<long>();
        //        //SCat.Add(SubCat);
        //        //return brands;
        //    }
        //}

        public dynamic GetProductsByCategory(int SuperCatId, int SubId, int CatId, int sort, string brand)
        {
            List<ProductListDTO> list = new List<ProductListDTO>();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<ProductListDTO> subProductlist = new List<ProductListDTO>();
                if (SubId == 0)
                {
                    subProductlist = (from p in context.Products
                                      where p.SubCategory.CategoryId == CatId && p.IsActive == true
                                      && p.Quantity > 0 && p.IsDeleted == false
                                      select new ProductListDTO
                                      {
                                          Brand = p.Brand,
                                          CategoryId = p.SubCategory.CategoryId,
                                          CategoryName = p.SubCategory.Category.CategoryName,
                                          IsActive = p.IsActive,
                                          IsDeleted = p.IsDeleted,
                                          IsFeaturedProduct = p.IsFeaturedProduct,
                                          IsSold = p.IsSold,
                                          ProductCost = p.ProductCost,
                                          ProductDiscountPercentage = p.ProductDiscountPercentage,
                                          ProductId = p.ProductId,
                                          ProductImgUrl = p.ProductImgUrl,
                                          ProductName = p.ProductName.Replace(" ", "-"),
                                          ProductOriginalCost = p.ProductOriginalCost,
                                          Quantity = p.Quantity,
                                          SubCategoryId = p.SubCategoryId,
                                          SubCategoryName = p.SubCategory.SubCategoryName,
                                          UpdatedOn = p.UpdatedOn
                                      }).ToList();
                }
                else
                {
                    subProductlist = (from p in context.Products
                                      where p.SubCategory.SubCategoryId == SubId && p.IsActive == true
                                      && p.Quantity > 0 && p.IsDeleted == false
                                      select new ProductListDTO
                                      {
                                          Brand = p.Brand,
                                          CategoryId = p.SubCategory.CategoryId,
                                          CategoryName = p.SubCategory.Category.CategoryName,
                                          IsActive = p.IsActive,
                                          IsDeleted = p.IsDeleted,
                                          IsFeaturedProduct = p.IsFeaturedProduct,
                                          IsSold = p.IsSold,
                                          ProductCost = p.ProductCost,
                                          ProductDiscountPercentage = p.ProductDiscountPercentage,
                                          ProductId = p.ProductId,
                                          ProductImgUrl = p.ProductImgUrl,
                                          ProductName = p.ProductName.Replace(" ", "-"),
                                          ProductOriginalCost = p.ProductOriginalCost,
                                          Quantity = p.Quantity,
                                          SubCategoryId = p.SubCategoryId,
                                          SubCategoryName = p.SubCategory.SubCategoryName,
                                          UpdatedOn = p.UpdatedOn
                                      }).ToList();
                }

                if (sort == 1)
                {
                    subProductlist = subProductlist.OrderBy(x => x.ProductCost).ToList();
                }
                else if (sort == 2)
                {
                    subProductlist = subProductlist.OrderByDescending(x => x.ProductCost).ToList();
                }

                list.AddRange(subProductlist);
            }
            return list.ToList();
        }

        public dynamic GetSubCategoryList(int CatId)
        {
            List<ProductListDTO> list = new List<ProductListDTO>();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {

                List<ProductListDTO> subCatlist = (from p in context.SubCategories
                                                   where p.CategoryId == CatId && p.IsActive == true
                                                   select new ProductListDTO
                                                   {
                                                       CategoryId = p.CategoryId,
                                                       CategoryName = p.Category.CategoryName,
                                                       SubCategoryId = p.SubCategoryId,
                                                       SubCategoryName = p.SubCategoryName,
                                                       SuperCategoryId = p.Category.SuperCategoryId.Value
                                                   }).ToList();

                list.AddRange(subCatlist);
            }
            return list.ToList();
        }

        public dynamic GetProductsBetweenPriceinCategory(int SuperCatId, int SubId, int CatId, int sort, string brand, decimal MinPrice, decimal MaxPrice)
        {
            List<ProductListDTO> list = new List<ProductListDTO>();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {

                list = (from p in context.Products
                        where p.SubCategory.CategoryId == CatId && p.IsActive == true
                        && p.Quantity > 0 && p.IsDeleted == false
                       && p.ProductCost >= MinPrice && p.ProductCost <= MaxPrice
                        select new ProductListDTO
                        {
                            Brand = p.Brand,
                            CategoryId = p.SubCategory.CategoryId,
                            CategoryName = p.SubCategory.Category.CategoryName,
                            IsActive = p.IsActive,
                            IsDeleted = p.IsDeleted,
                            IsFeaturedProduct = p.IsFeaturedProduct,
                            IsSold = p.IsSold,
                            ProductCost = p.ProductCost,
                            ProductDiscountPercentage = p.ProductDiscountPercentage,
                            ProductId = p.ProductId,
                            ProductImgUrl = p.ProductImgUrl,
                            ProductName = p.ProductName.Replace(" ", "-"),
                            ProductOriginalCost = p.ProductOriginalCost,
                            Quantity = p.Quantity,
                            SubCategoryId = p.SubCategoryId,
                            SubCategoryName = p.SubCategory.SubCategoryName,
                            UpdatedOn = p.UpdatedOn
                        }).ToList();

            }
            return list;
        }

        public dynamic GetBrandList(int SubId)
        {
            List<ProductListDTO> list = new List<ProductListDTO>();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {

                List<ProductListDTO> subCatlist = (from p in context.Products
                                                   where p.SubCategoryId == SubId && p.IsActive == true
                                                   && p.IsDeleted == false && p.Quantity > 0
                                                   select new ProductListDTO
                                                   {
                                                       Brand = p.Brand
                                                   }).ToList();

                list.AddRange(subCatlist);
            }
            return list.ToList();
        }

        public dynamic GetCategoryBrandList(int CatId)
        {
            List<ProductListDTO> list = new List<ProductListDTO>();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                list = (from p in context.Products
                        where p.SubCategory.CategoryId == CatId && p.IsActive == true
                        && p.IsDeleted == false && p.Quantity > 0
                        select new ProductListDTO
                        {
                            Brand = p.Brand
                        }).ToList();
            }
            return list;
        }

        public dynamic GetSuperCatBrandList(int SuperCatId)
        {
            List<ProductListDTO> list = new List<ProductListDTO>();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {

                list = (from p in context.Products
                        where p.SubCategory.Category.SuperCategoryId == SuperCatId && p.IsActive == true
                        && p.Quantity > 0 && p.IsDeleted == false
                        select new ProductListDTO
                        {
                            Brand = p.Brand
                        }).ToList();

            }
            return list;
        }

        public List<Product> GetAllLatestProducts()
        {
            var repository = new ProductRepository();

            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                List<Product> data = new List<Product>();
                var dta = Context.Products.Where(f => f.IsDeleted == false && f.IsActive == true && f.Quantity > 0).OrderByDescending(f => f.CreatedOn).ToList();
                data = dta.ToList();
                return data;
            }

        }
    }
    public class RelatedProductRepository : RepositoryBase<RelatedProduct>
    {
        public dynamic GetProdctDetailsByRelatedprodct(long RelatedPrdctId)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                var data = (from r in Context.RelatedProducts
                            where r.RelatedProductId == RelatedPrdctId
                            select new
                            {
                                Id = r.Id,
                                RelatedPrdctId = r.RelatedProductId,
                                Product = (from p in Context.Products
                                           where p.ProductId == r.ProductId
                                           select new
                                           {
                                               ProductId = p.ProductId,
                                               ProductName = p.ProductName
                                           })
                            }).ToList();
                return data.ToList();
            }
        }
        public dynamic getimages(int id)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                var data = Context.Database.SqlQuery<string>("select productimgurl from Products where ProductId=" + id).FirstOrDefault();
                return data;
            }
        }

        public IEnumerable<Object> GetImagesbyProductId(int ProductId)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                IEnumerable<Object> GetImg = (from img in Context.Products
                                              where img.ProductId == ProductId
                                              select new
                                              {
                                                  img,
                                                  Galleries = (from g in Context.ProductsGalleries
                                                               where g.ProductId == img.ProductId
                                                               select new
                                                               {
                                                                   g.LargeImgUrl,
                                                                   g.ImgUrl,
                                                               }),
                                              }).ToList();
                //var GetImg = Context.Products.Where(p => p.ProductId == ProductId);
                return GetImg.ToList();
            }
        }

    }

    public class SubProductRepository : RepositoryBase<SubProduct>
    {
    }

    public class ProductFeatureRepository : RepositoryBase<ProductFeature>
    {
        public List<ProductFeature> GetProductFeaturesToCompare()
        {
            var CompareItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCompareList) ??
                  new List<UserProductTransaction>();

            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                var items = (from a in Context.ProductFeatures
                             join b in CompareItems
                                 on a.ProductId equals b.ProductId
                             select new ProductFeature
                             {
                                 FeatureInfo = a.FeatureInfo,
                                 ProductFeatureId = a.ProductFeatureId,

                             }
                       ).ToList();
                return items;
            }
        }
    }

    public class ProductSpecificationRepository : RepositoryBase<ProductSpecification>
    {
    }
    public class ProductsGalleryRepository : RepositoryBase<ProductsGallery>
    {
    }
    public class LogRepository : RepositoryBase<tbl_Loghistory>
    {
    }
    public class ImagerespoRepository : RepositoryBase<Tbl_Presecription_Info>
    {
    }
    public class UserProductTransactionRepository : RepositoryBase<UserProductTransaction>
    {
        public dynamic GetOrderDetls(int TransactionId)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                //  Context.Database.Connection.Open();
                var data = (
                from a in Context.UserProductTransactions
                where a.PaymentTransactionId == TransactionId
                select new
                {
                    Quantity = a.Quantity,
                    TransactionId = a.TransactionId,
                    User = (from ur in Context.Users
                            where ur.UserId == a.UserAddress.UserId
                            select new
                            {
                                UserId = ur.UserId,
                                Role = ur.Role,
                                FirstName = ur.FirstName,
                                LastName = ur.LastName,
                                MobileNo = ur.MobileNo,
                                RoleName = ur.Role.RoleName,
                                EmailId = ur.EmailId,
                                RoleDescription = ur.Role.RoleDescription
                            }),
                    Products = (from p in Context.Products
                                where p.ProductId == a.ProductId
                                select new
                                {
                                    ProductFeatures = p.ProductFeatures,
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    ProductCost = p.ProductCost,
                                    ProductSpecifications = p.ProductSpecifications,
                                    ProductColor = a.Product.ProductColor,
                                    Occasion = a.Product.Occasion,
                                    ProductCode = a.Product.ProductCode,
                                    ProductsGalleries = p.ProductsGalleries,
                                }),
                    userProductTransactionSpecifications = (from c in Context.UserProductTransactionSpecifications
                                                            where c.TransactionId == a.TransactionId
                                                            select new
                                                            {
                                                                BillingAddressId = c.UserProductTransaction.BillingAddressId,
                                                                CurrencyCode = c.UserProductTransaction.PaymentTransaction.CurrencyCode,
                                                                CurrencySymbol = c.UserProductTransaction.PaymentTransaction.CurrencySymbol,
                                                                ProductSpecificationName = c.ProductSpecification.ProductSpecificationName,
                                                                ProductSpecificationId = c.ProductSpecificationId,
                                                                SpecificationValue = c.SpecificationValue
                                                            }),
                    ShipingAddress = (from ad in Context.UserAddresses
                                      where ad.UserAddressId == a.ShippingAddressId
                                      select new
                                      {
                                          StreetAddress1 = ad.StreetAddress1,
                                          StreetAddress2 = ad.StreetAddress2,
                                          StateName = ad.StateName,
                                          PinCode = ad.PinCode,
                                          LandMark = ad.LandMark,
                                          City = ad.City,
                                      }),
                    Country = (from c in Context.Countries
                               where c.CountryId == a.UserAddress1.CountryId
                               select new
                               {
                                   Country = c.CountryName
                               }),
                    BillingAddress = (from ad in Context.UserAddresses
                                      where ad.UserAddressId == a.BillingAddressId
                                      select new
                                      {
                                          StreetAddress1 = ad.StreetAddress1,
                                          StreetAddress2 = ad.StreetAddress2,
                                          StateName = ad.StateName,
                                          PinCode = ad.PinCode,
                                          LandMark = ad.LandMark,
                                          City = ad.City,
                                      }),
                    PaymentTransactionDetails = (from pt in Context.PaymentTransactions
                                                 where pt.PaymentTransactionId == a.PaymentTransactionId
                                                 select new
                                                 {
                                                     CurrencySymbol = pt.CurrencySymbol,
                                                     CreatedOn = pt.CreatedOn,
                                                     UpdatedOn = pt.UpdatedOn,
                                                     PaymentStatus = pt.PaymentStatus,
                                                     PaymentTransactionId = pt.PaymentTransactionId,
                                                     TxnAmount = pt.TxnAmount,
                                                     TxnRefNo = pt.TxnRefNo,
                                                     TxnStatus = pt.TxnStatus,
                                                     PGTxnId = pt.PGTxnId,
                                                     ServiceTax = pt.ServiceTax == null ? 0 : pt.ServiceTax,
                                                     ShippingCharges = pt.ServiceTax == null ? 0 : pt.ShippingCharges,
                                                     OtherCharges = pt.OtherCharges == null ? 0 : pt.ShippingCharges,
                                                     VAT = pt.VAT == null ? 0 : pt.VAT,
                                                     CurrencyValue = pt.CurrencyValue,
                                                     Paymode = pt.PaymentMode
                                                 })
                }).ToList();

                return data.ToList();
            }
        }
    }

    public class CheckoutUserProductTransactionRepository : RepositoryBase<CheckOutUserProductTransaction>
    {
        public dynamic GetOrderDetls(int TransactionId)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                //  Context.Database.Connection.Open();
                var data = (
                from a in Context.CheckOutUserProductTransactions
                where a.PaymentTransactionId == TransactionId
                select new
                {
                    Quantity = a.Quantity,
                    TransactionId = a.TransactionId,
                    User = (from ur in Context.Users
                            where ur.UserId == a.UserAddress.UserId
                            select new
                            {
                                UserId = ur.UserId,
                                Role = ur.Role,
                                FirstName = ur.FirstName,
                                LastName = ur.LastName,
                                MobileNo = ur.MobileNo,
                                RoleName = ur.Role.RoleName,
                                EmailId = ur.EmailId,
                                RoleDescription = ur.Role.RoleDescription
                            }),
                    Products = (from p in Context.Products
                                where p.ProductId == a.ProductId
                                select new
                                {
                                    ProductFeatures = p.ProductFeatures,
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    ProductCost = p.ProductCost,
                                    ProductSpecifications = p.ProductSpecifications,
                                    ProductColor = a.Product.ProductColor,
                                    Occasion = a.Product.Occasion,
                                    ProductCode = a.Product.ProductCode,
                                    ProductsGalleries = p.ProductsGalleries,
                                }),
                    //userProductTransactionSpecifications = (from c in Context.UserProductTransactionSpecifications
                    //                                        where c.TransactionId == a.TransactionId
                    //                                        select new
                    //                                        {
                    //                                            BillingAddressId = c.UserProductTransaction.BillingAddressId,
                    //                                            CurrencyCode = c.UserProductTransaction.PaymentTransaction.CurrencyCode,
                    //                                            CurrencySymbol = c.UserProductTransaction.PaymentTransaction.CurrencySymbol,
                    //                                            ProductSpecificationName = c.ProductSpecification.ProductSpecificationName,
                    //                                            ProductSpecificationId = c.ProductSpecificationId,
                    //                                            SpecificationValue = c.SpecificationValue
                    //                                        }),
                    ShipingAddress = (from ad in Context.UserAddresses
                                      where ad.UserAddressId == a.ShippingAddressId
                                      select new
                                      {
                                          StreetAddress1 = ad.StreetAddress1,
                                          StreetAddress2 = ad.StreetAddress2,
                                          StateName = ad.StateName,
                                          PinCode = ad.PinCode,
                                          LandMark = ad.LandMark,
                                          City = ad.City,
                                      }),
                    Country = (from c in Context.Countries
                               where c.CountryId == a.UserAddress1.CountryId
                               select new
                               {
                                   Country = c.CountryName
                               }),
                    BillingAddress = (from ad in Context.UserAddresses
                                      where ad.UserAddressId == a.BillingAddressId
                                      select new
                                      {
                                          StreetAddress1 = ad.StreetAddress1,
                                          StreetAddress2 = ad.StreetAddress2,
                                          StateName = ad.StateName,
                                          PinCode = ad.PinCode,
                                          LandMark = ad.LandMark,
                                          City = ad.City,
                                      }),
                    PaymentTransactionDetails = (from pt in Context.CheckOutPaymentTransactions
                                                 where pt.PaymentTransactionId == a.PaymentTransactionId
                                                 select new
                                                 {
                                                     CurrencySymbol = pt.CurrencySymbol,
                                                     CreatedOn = pt.CreatedOn,
                                                     UpdatedOn = pt.UpdatedOn,
                                                     PaymentStatus = pt.PaymentStatus,
                                                     PaymentTransactionId = pt.PaymentTransactionId,
                                                     TxnAmount = pt.TxnAmount,
                                                     TxnRefNo = pt.TxnRefNo,
                                                     TxnStatus = pt.TxnStatus,
                                                     PGTxnId = pt.PGTxnId,
                                                     ServiceTax = pt.ServiceTax == null ? 0 : pt.ServiceTax,
                                                     ShippingCharges = pt.ServiceTax == null ? 0 : pt.ShippingCharges,
                                                     OtherCharges = pt.OtherCharges == null ? 0 : pt.ShippingCharges,
                                                     VAT = pt.VAT == null ? 0 : pt.VAT,
                                                     CurrencyValue = pt.CurrencyValue,
                                                     Paymode = pt.PaymentMode
                                                 })
                }).ToList();

                return data.ToList();
            }
        }
    }

    public class TaxesConfigurationRepository : RepositoryBase<TaxesConfiguration>
    {

    }
    public class CashbackRepository : RepositoryBase<Cashback_Coupons>
    {

    }

    public class PaymentTransactionRepository : RepositoryBase<PaymentTransaction>
    {
        public PaymentTransaction MakePayment(ICollection<UserProductTransaction> data)
        {
            try
            {
                int? OfferProductId = null;
                if (HttpContext.Current.Session["OfferedProductID"] != null)
                {
                    OfferProductId = Convert.ToInt32(HttpContext.Current.Session["OfferedProductID"]);
                }

                long? freeProductId = null;
                foreach (var userProductTransaction in data)
                {
                    freeProductId = userProductTransaction.Product.Free_Product_ID;
                    userProductTransaction.Product = null;
                    if (userProductTransaction.UserProductTransactionSpecifications != null)
                        foreach (var item in userProductTransaction.UserProductTransactionSpecifications)
                            item.UserProductTransaction = null;
                }
                var calculations = BalUtility.GenerateOrderSummary();
                var orderSummary = (Caluclations)BalUtility.GetSession(Shared.Sessions.ShippingInfo);

                PaymentTransaction payment = new PaymentTransaction()
                {
                    UserId = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId,
                    PaymentStatus = (int)Utility.Shared.DelivarytStatus.Pending,
                    CurrencyCode = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).ToCurrency,
                    CurrencySymbol = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).Symbol,
                    CurrencyValue = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).Value,
                    CreatedOn = DateTime.Now,
                    UserProductTransactions = data,
                    Free_Product_ID = freeProductId,
                    Offer_Product_ID = OfferProductId,
                    OtherCharges = calculations.OtherCharges,
                    ServiceTax = calculations.ServiceTax,
                    ShippingCharges = calculations.ShippingCharges,
                    VAT = calculations.VAT,
                    TxnAmount = calculations.TotalPriceAfterDeductions,
                    PaymentMode = "Net Banking/Debit/Credit Card"
                };
                var repository = new PaymentTransactionRepository();
                repository.Insert(payment);
                return payment;
            }
            catch (Exception ex)
            {
                // // Shared.Log.Error(ex);
                throw;
            }
        }

        public PaymentTransaction MakePayment(ICollection<UserProductTransaction> data, string PaymentMode)
        {
            try
            {
                long? freeProductId = null;

                int? OfferProductId = null;
                if (HttpContext.Current.Session["OfferedProductID"] != null)
                {
                    OfferProductId = Convert.ToInt32(HttpContext.Current.Session["OfferedProductID"]);
                }
                foreach (var userProductTransaction in data)
                {

                    freeProductId = userProductTransaction.Product.Free_Product_ID;

                    userProductTransaction.Product = null;
                    if (userProductTransaction.UserProductTransactionSpecifications != null)
                        foreach (var item in userProductTransaction.UserProductTransactionSpecifications)
                            item.UserProductTransaction = null;
                }
                var calculations = BalUtility.GenerateOrderSummary();
                var orderSummary = (Caluclations)BalUtility.GetSession(Shared.Sessions.ShippingInfo);

                if (PaymentMode == "Cash On Delivery")
                {
                    if (calculations.TotalPriceAfterDeductions <= 2000 && calculations.TotalPriceAfterDeductions >= 1000)
                    {
                        calculations.ShippingCharges = 50;
                        calculations.TotalPriceAfterDeductions = calculations.TotalPriceAfterDeductions + calculations.ShippingCharges;
                    }
                    else if (calculations.TotalPriceAfterDeductions < 1000)
                    {
                        calculations.ShippingCharges = 99;
                        calculations.TotalPriceAfterDeductions = calculations.TotalPriceAfterDeductions + 49;
                    }
                    else
                    {
                        calculations.ShippingCharges = 0;
                    }
                }

                PaymentTransaction payment = new PaymentTransaction()
                {
                    UserId = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId,
                    PaymentStatus = (int)Utility.Shared.DelivarytStatus.Pending,
                    CurrencyCode = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).ToCurrency,
                    CurrencySymbol = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).Symbol,
                    CurrencyValue = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).Value,
                    CreatedOn = DateTime.Now,
                    UserProductTransactions = data,
                    Free_Product_ID = freeProductId,
                    Offer_Product_ID = OfferProductId,
                    OtherCharges = calculations.OtherCharges,
                    ServiceTax = calculations.ServiceTax,
                    ShippingCharges = calculations.ShippingCharges,
                    VAT = calculations.VAT,
                    TxnAmount = calculations.TotalPriceAfterDeductions,
                    PaymentMode = PaymentMode
                };

                var repository = new PaymentTransactionRepository();
                repository.Insert(payment);
                return payment;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public PaymentTransaction MakePaymentFromAdmin(ICollection<UserProductTransaction> data)
        {
            try
            {
                long? freeProductId = null;

                int? OfferProductId = null;
                if (HttpContext.Current.Session["OfferedProductID"] != null)
                {
                    OfferProductId = Convert.ToInt32(HttpContext.Current.Session["OfferedProductID"]);
                }
                foreach (var userProductTransaction in data)
                {
                    freeProductId = userProductTransaction.Product.Free_Product_ID;
                    userProductTransaction.Product = null;
                    if (userProductTransaction.UserProductTransactionSpecifications != null)
                        foreach (var item in userProductTransaction.UserProductTransactionSpecifications)
                            item.UserProductTransaction = null;
                }


                PaymentTransaction payment = new PaymentTransaction()
                {
                    UserId = data.FirstOrDefault().UserId,
                    PaymentStatus = (int)Utility.Shared.DelivarytStatus.Pending,
                    CurrencyCode = data.FirstOrDefault().CurrencyCode,
                    CurrencySymbol = data.FirstOrDefault().CurrencySymbol,
                    CurrencyValue = data.FirstOrDefault().CurrencyValue,
                    CreatedOn = System.DateTime.Now,
                    Free_Product_ID = freeProductId,
                    Offer_Product_ID = OfferProductId,
                    UserProductTransactions = data,

                    TxnAmount = data.FirstOrDefault().ProductCost
                };
                var repository = new PaymentTransactionRepository();
                repository.Insert(payment);

                return payment;
            }
            catch (Exception ex)
            {
                // // Shared.Log.Error(ex);
                throw;
            }
        }

        public void ComitPayment(PaymentTransaction trans)
        {
            //int status = (int)Utility.Shared.DelivarytStatus.Success;
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                PaymentTransaction paymentData = (from a in Context.PaymentTransactions where a.PaymentTransactionId == trans.PaymentTransactionId select a).FirstOrDefault();
                paymentData.PaymentStatus = trans.PaymentStatus;//(int)Utility.Shared.DelivarytStatus.Success;
                paymentData.TxnRefNo = trans.TxnRefNo;
                paymentData.PGTxnId = trans.PGTxnId;
                paymentData.TxnAmount = trans.TxnAmount;
                paymentData.TxnMessage = trans.TxnMessage;
                paymentData.TxnStatus = trans.TxnStatus;
                paymentData.OrderCurrentStatus = trans.OrderCurrentStatus;
                paymentData.Authorized = trans.Authorized;

                paymentData.UpdatedOn = DateTime.Now;

                var repository2 = new UserProductTransactionRepository();
                int totalRecords;

                (from a in Context.PaymentTransactions
                 where a.PaymentTransactionId == trans.PaymentTransactionId
                 select a).ToList().ForEach(u => u.PaymentStatus = paymentData.PaymentStatus);


                var cartItems = repository2.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0, (p => p.PaymentTransactionId == trans.PaymentTransactionId));
                var repository = new ProductRepository();
                Product product = null;

                foreach (var item in cartItems)
                {
                    if (item.ProductId != 0)
                    {
                        var data = repository2.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0, (p => p.ProductId == item.ProductId && p.PaymentTransaction.PaymentStatus == 0));
                        var soldoutQty = data.Sum(p => p.Quantity);

                        product = repository.Single(p => p.ProductId == item.ProductId);

                        var availableQuantity = product.Quantity - soldoutQty;

                        if (availableQuantity == 0)
                        {
                            product.IsSold = true;
                            repository.Update(product);
                        }

                    }
                }
                BalUtility.ClearSession(Shared.Sessions.CustomerCart);
                Context.SaveChanges();

            }
        }


        public string CourierName(long OrderID)
        {
            string courier = "";
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                courier = context.PaymentTransactions.Where(x => x.PaymentTransactionId == OrderID).First().CourierName;
            }
            return courier;
        }

        public dynamic Manifest(long OrderID)
        {
            PaymentTransaction payment = new PaymentTransaction();
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                payment = context.PaymentTransactions.Where(x => x.PaymentTransactionId == OrderID).First();
            }
            return payment;
        }


        public string MakeOrderStatusChange(int OrderId, string PgTxnId, string payMode)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                PaymentTransaction txn = context.PaymentTransactions.Where(x => x.PaymentTransactionId == OrderId).FirstOrDefault();
                if (txn.PaymentMode == payMode)
                {

                }
                else
                {
                    if (payMode == "Paytm")
                    {
                        decimal? total = txn.TxnAmount - txn.ShippingCharges;
                        if (total < 500)
                        {
                            txn.ShippingCharges = 50;
                            txn.TxnAmount = total + txn.ShippingCharges;
                        }
                        else if (total >= 500)
                        {
                            txn.ShippingCharges = 0;
                            txn.TxnAmount = total + txn.ShippingCharges;
                        }
                        txn.PaymentMode = payMode;
                    }
                    else if (payMode == "Cash On Delivery")
                    {
                        decimal? total = txn.TxnAmount - txn.ShippingCharges;
                        if (total <= 1000)
                        {
                            txn.ShippingCharges = 70;
                            txn.TxnAmount = total + txn.ShippingCharges;
                        }
                        else if (total > 1000 && total < 2000)
                        {
                            txn.ShippingCharges = 70;
                            txn.TxnAmount = total + txn.ShippingCharges;
                        }
                        else if (total >= 2000)
                        {
                            txn.ShippingCharges = 70;
                            txn.TxnAmount = total + txn.ShippingCharges;
                        }
                        txn.PaymentMode = payMode;
                    }
                }
                txn.TxnStatus = "SUCCESS";
                txn.TxnMessage = "Transaction Successful";
                txn.UpdatedOn = DateTime.Now;
                txn.Authorized = true;
                txn.OrderCurrentStatus = 6;
                txn.PGTxnId = PgTxnId;
                txn.TxnRefNo = PgTxnId;
                context.SaveChanges();
            }

            return "";
        }

        public string CheckoutOrderSuccess(int OrderId, string PgTxnId, string payMode)
        {
            try
            {
                db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                CheckOutPaymentTransaction txn = context.CheckOutPaymentTransactions.Where(x => x.PaymentTransactionId == OrderId).FirstOrDefault();
                List<CheckOutUserProductTransaction> checkOutUserProductTransactions = context.CheckOutUserProductTransactions.Where(x => x.PaymentTransactionId == OrderId).ToList();
                //List<UserProductTransaction> userProductTransactions = new List<UserProductTransaction>();
                if (txn != null)
                {
                    if (txn.PaymentMode == payMode)
                    {

                    }
                    else
                    {
                        if (payMode == "Paytm")
                        {
                            decimal? total = txn.TxnAmount - txn.ShippingCharges;
                            if (total <= 1000)
                            {
                                txn.ShippingCharges = 50;
                                txn.TxnAmount = total + txn.ShippingCharges;
                            }
                            else if (total > 1000)
                            {
                                txn.ShippingCharges = 0;
                                txn.TxnAmount = total + txn.ShippingCharges;
                            }
                            txn.PaymentMode = payMode;
                        }
                        else if (payMode == "Cash On Delivery")
                        {
                            decimal? total = txn.TxnAmount - txn.ShippingCharges;
                            if (total <= 1000)
                            {
                                txn.ShippingCharges = 99;
                                txn.TxnAmount = total + txn.ShippingCharges;
                            }
                            else if (total > 1000 && total < 2000)
                            {
                                txn.ShippingCharges = 50;
                                txn.TxnAmount = total + txn.ShippingCharges;
                            }
                            else if (total >= 2000)
                            {
                                txn.ShippingCharges = 0;
                                txn.TxnAmount = total + txn.ShippingCharges;
                            }
                            txn.PaymentMode = payMode;
                        }
                    }
                    txn.TxnStatus = "SUCCESS";
                    txn.TxnMessage = "Transaction Successful";
                    txn.UpdatedOn = DateTime.Now;
                    txn.Authorized = true;
                    txn.OrderCurrentStatus = 6;
                    txn.PGTxnId = "Checkout_" + PgTxnId;
                    txn.TxnRefNo = "Checkout_" + PgTxnId;

                    PaymentTransaction payment = new PaymentTransaction()
                    {
                        UserId = txn.UserId,
                        PaymentStatus = (int)Utility.Shared.DelivarytStatus.Success,
                        CurrencyCode = txn.CurrencyCode,
                        CurrencySymbol = txn.CurrencySymbol,
                        CurrencyValue = txn.CurrencyValue,
                        CreatedOn = DateTime.Now,
                        Free_Product_ID = txn.Free_Product_ID,
                        Offer_Product_ID = txn.Offer_Product_ID,
                        OtherCharges = txn.OtherCharges,
                        ServiceTax = txn.ServiceTax,
                        ShippingCharges = txn.ShippingCharges,
                        VAT = txn.VAT,
                        TxnAmount = txn.TxnAmount,
                        PaymentMode = txn.PaymentMode,
                        TxnStatus = txn.TxnStatus,
                        TxnMessage = txn.TxnMessage,
                        Authorized = txn.Authorized,
                        OrderCurrentStatus = txn.OrderCurrentStatus,
                        PGTxnId = txn.PGTxnId,
                        TxnRefNo = txn.TxnRefNo,
                        Promo_Code_ID = txn.Promo_Code_ID,
                        Promo_Code_Amount = txn.Promo_Code_Amount,
                        Has_Promo_Code = txn.Has_Promo_Code,
                        AmountFromMyAccount = txn.AmountFromMyAccount,
                        Comments = txn.Comments,
                        CourierName = txn.CourierName,

                    };
                    context.PaymentTransactions.Add(payment);
                    int i = context.SaveChanges();
                    if (i != 0)
                    {
                        foreach (var userProductTransaction in checkOutUserProductTransactions)
                        {
                            UserProductTransaction userproductTxn = new UserProductTransaction();
                            userproductTxn.ShippingAddressId = context.UserAddresses.Where(x => x.UserId == payment.UserId).FirstOrDefault().UserAddressId;
                            userproductTxn.BillingAddressId = context.UserAddresses.Where(x => x.UserId == payment.UserId).FirstOrDefault().UserAddressId;
                            userproductTxn.CreatedBy = payment.UserId;
                            userproductTxn.CreatedOn = DateTime.Now;
                            userproductTxn.CurrencyCode = payment.CurrencyCode;
                            userproductTxn.CurrencySymbol = payment.CurrencySymbol;
                            userproductTxn.CurrencyValue = payment.CurrencyValue;
                            userproductTxn.IsActive = true;
                            userproductTxn.IsDeleted = false;
                            userproductTxn.IsReplaced = false;
                            userproductTxn.OrdersReturnAction = null;
                            userproductTxn.OrdersReturnReason = null;
                            userproductTxn.PaymentTransactionId = payment.PaymentTransactionId;
                            userproductTxn.ProductCost = userProductTransaction.Product.ProductCost * userProductTransaction.Quantity;
                            userproductTxn.ProductDiscountCost = userProductTransaction.Product.ProductDiscountCost;
                            userproductTxn.ProductDiscountPercentage = userProductTransaction.Product.ProductDiscountPercentage;
                            userproductTxn.ProductId = userProductTransaction.Product.ProductId;
                            userproductTxn.Quantity = userProductTransaction.Quantity;
                            userproductTxn.ReplacementTransactionId = 0;
                            userproductTxn.Status = 0;
                            userproductTxn.SubProductId = null;
                            userproductTxn.UpdatedBy = payment.UserId;
                            userproductTxn.UpdatedOn = DateTime.Now;
                            userproductTxn.UserId = payment.UserId;
                            context.UserProductTransactions.Add(userproductTxn);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return "";

        }

        public string CheckoutOrderClose(int OrderId)
        {
            try
            {
                db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                CheckOutPaymentTransaction txn = context.CheckOutPaymentTransactions.Where(x => x.PaymentTransactionId == OrderId).FirstOrDefault();
                if (txn != null)
                {
                    txn.TxnStatus = "Closed";
                    txn.TxnMessage = "Order Closed";
                    txn.UpdatedOn = DateTime.Now;
                    txn.OrderCurrentStatus = 11;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
            return "";

        }
    }

    public class BusinessTypeRepository : RepositoryBase<BusinessType>
    {
        public IEnumerable<Object> GetBusinessCatByGroup()
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {

                IEnumerable<Object> data = (from a in Context.BusinessTypes
                                            select new
                                            {
                                                FeaturesCategories = (from c in Context.FeaturesCategories
                                                                      where c.BusinessId == a.BusinessId
                                                                      select new
                                                                      {
                                                                          FeaturesCategoryId = c.FeaturesCategoryId,
                                                                          FeaturesCategoryName = c.FeaturesCategoryName,

                                                                          FeaturesSubCategories = (from sc in Context.FeaturesSubCategories
                                                                                                   where sc.FeaturesCategoryId == c.FeaturesCategoryId
                                                                                                   select new
                                                                                                   {
                                                                                                       FeaturesSubCategoryId = sc.FeaturesSubCategoryId,
                                                                                                       FeaturesSubCategoryName = sc.FeaturesSubCategoryName,
                                                                                                       Count = (from p in Context.Products where p.IsSold == false && p.ProductFeatures.FirstOrDefault().FeaturesSubCategoryId == sc.FeaturesSubCategoryId select p.ProductFeatures.FirstOrDefault().FeaturesSubCategoryId).Count(),
                                                                                                   }),

                                                                      }),
                                                BusinessId = a.BusinessId,
                                                BusinessName = a.BusinessName,
                                            }).ToList();
                return data;
            }
        }
    }

    public class FeaturesCategoryRepository : RepositoryBase<FeaturesCategory>
    {
        public IEnumerable<Object> GetFeaturesCatByGroup()
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                IEnumerable<Object> data = (from a in Context.FeaturesCategories
                                            select new
                                            {
                                                FeaturesSubCategories = (from sc in Context.FeaturesSubCategories
                                                                         where sc.FeaturesCategoryId == a.FeaturesCategoryId
                                                                         select new
                                                                         {
                                                                             FeaturesSubCategoryId = sc.FeaturesSubCategoryId,
                                                                             SubCategoryName = sc.FeaturesSubCategoryName,
                                                                             Count = (from p in Context.Products where p.IsSold == false && p.SubCategoryId == sc.FeaturesSubCategoryId select p.SubCategoryId).Count(),
                                                                         }),
                                                FeaturesCategoryId = a.FeaturesCategoryId,
                                                FeaturesCategoryName = a.FeaturesCategoryName,
                                            }).ToList();
                return data;
            }
        }


        public IEnumerable<Object> GetFeaturesSubCatByFeaturesCatByGroup(int id)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                IEnumerable<Object> data = (from a in Context.FeaturesCategories
                                            where a.FeaturesCategoryId == id
                                            select new
                                            {
                                                FeaturesSubCategories = (from sc in Context.FeaturesSubCategories
                                                                         where sc.FeaturesCategoryId == a.FeaturesCategoryId
                                                                         select new
                                                                         {
                                                                             FeaturesSubCategoryId = sc.FeaturesSubCategoryId,
                                                                             FeaturesSubCategoryName = sc.FeaturesSubCategoryName,
                                                                             Count = (from p in Context.Products where p.IsSold == false && p.ProductFeatures.FirstOrDefault().FeaturesSubCategoryId == sc.FeaturesSubCategoryId select p.ProductFeatures.FirstOrDefault().FeaturesSubCategoryId).Count(),
                                                                         }),
                                                FeaturesCategoryId = a.FeaturesCategoryId,
                                                FeaturesCategoryName = a.FeaturesCategoryName,
                                            }).ToList();
                return data;
            }
        }

    }
    public class FeaturesSubCategoryRepository : RepositoryBase<FeaturesSubCategory>
    {
    }

    public class ProductInventoryRepository : RepositoryBase<ProductInventory>
    {
        public void InsertPrdctInventory(List<ProductInventory> ProductInventoryList)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {

                foreach (var item in ProductInventoryList)
                {
                    SubProduct dbSubProduct = new SubProduct();
                    if (item.SubProductId != 0)
                    {
                        dbSubProduct = (from a in Context.SubProducts where a.SubProductId == item.SubProductId select a).FirstOrDefault();
                    }

                    if (dbSubProduct.Quantity != item.SubProduct.Quantity)
                    {
                        var InvntryRepostry = new ProductInventoryRepository();
                        InvntryRepostry.Insert(ProductInventoryList);
                    }
                }
            }
        }
    }

    public class PinCodesInformationRepository : RepositoryBase<PinCodesInformation>
    {

    }
    public class ProductReviewsRepository : RepositoryBase<ProductReview>
    {

    }

    public class PinCodeInfoRepository : RepositoryBase<PincodeInfo>
    {

    }

    public class CommentRepository : RepositoryBase<Tbl_OrderComments>
    {

        public long OrderComment(int OrderID, string Comment, int p)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                try
                {
                    Tbl_OrderComments commentData = new Tbl_OrderComments();
                    commentData.Comment = Comment;
                    commentData.Comment_Date = DateTime.Now;
                    commentData.Order_Id = OrderID;
                    commentData.Status = true;
                    commentData.Commented_By = p;

                    context.Tbl_OrderComments.Add(commentData);
                    context.SaveChanges();
                    return commentData.Comment_ID;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public List<Tbl_OrderComments> GetOrderComments(int trnsId)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<Tbl_OrderComments> commentsList = context.Tbl_OrderComments.Where(x => x.Order_Id == trnsId).ToList();
                return commentsList;
            }
        }
    }
    public class CheckoutCommentRepository : RepositoryBase<Tbl_CheckoutOrderComments>
    {

        public long CheckoutOrderComment(int OrderID, string Comment, string OrderStatus, int p)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                try
                {
                    Tbl_CheckoutOrderComments commentData = new Tbl_CheckoutOrderComments();
                    commentData.Comment = Comment;
                    commentData.Comment_Date = DateTime.Now;
                    commentData.Order_Id = OrderID;
                    commentData.Status = true;
                    commentData.Commented_By = p;

                    context.Tbl_CheckoutOrderComments.Add(commentData);
                    context.SaveChanges();
                    return commentData.Comment_ID;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public List<Tbl_CheckoutOrderComments> GetCheckoutOrderComments(int trnsId)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<Tbl_CheckoutOrderComments> commentsList = context.Tbl_CheckoutOrderComments.Where(x => x.Order_Id == trnsId).ToList();
                return commentsList;
            }
        }
    }
    public class EmployeeRepository : RepositoryBase<tbl_accesspages>
    {
        public List<tbl_accesspages> Getaccesspages(long userid)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<tbl_accesspages> accesslist = context.tbl_accesspages.Where(x => x.Employeeid == userid && x.IsActive == true).ToList();
                return accesslist;
            }
        }
    }
    public class DocumnetMonthRepository : RepositoryBase<Delivery_Invoice_Excel_Upload_Details>
    {
        public List<Delivery_Invoice_Excel_Upload_Details> GetDocuemnts()
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<Delivery_Invoice_Excel_Upload_Details> accesslist = context.Delivery_Invoice_Excel_Upload_Details.ToList();
                return accesslist;
            }
        }
    }
    public class RPinRepository : RepositoryBase<Delivery_Invoice_Excel>
    {
        public List<Deliverypins> GetPin(int docid)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                //List<Delivery_Invoice_Excel> accesslist = context.Delivery_Invoice_Excel.Where(x => x.Document_Id == docid).Distinct().ToList();
                return Context.Database.SqlQuery<Deliverypins>("select R_Pin from Delivery_Invoice_Excel  where Document_Id = '" + docid + "' group by R_Pin").ToList();
                //return accesslist;
            }
        }
    }
    public class DocumentDetailsRepository : RepositoryBase<Delivery_Invoice_Excel>
    {
        public List<Delivery_Invoice_Excel> GetDetails(int RPin)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<Delivery_Invoice_Excel> accesslist = context.Delivery_Invoice_Excel.Where(x => x.R_Pin == RPin).Distinct().ToList();
                //return Context.Database.SqlQuery<Delivery_Invoice_Excel>("select * from  where R_Pin = '"+ RPin + "'").ToList();
                return accesslist;

            }
        }
    }
    public class DocumentNameRepository : RepositoryBase<Delivery_Invoice_Excel_Upload_Details>
    {
        public List<Delivery_Invoice_Excel_Upload_Details> GetName(string FileNm)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<Delivery_Invoice_Excel_Upload_Details> accesslist = context.Delivery_Invoice_Excel_Upload_Details.Where(x => x.File_Name == FileNm).ToList();
                return accesslist;
            }
        }
    }
    public class PdfNameRepository : RepositoryBase<Delivery_Invoice_Pdf>
    {
        public List<Delivery_Invoice_Pdf> GetPdfName(string PdfNm)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<Delivery_Invoice_Pdf> accesslist = context.Delivery_Invoice_Pdf.Where(x => x.Pdf_Name == PdfNm).ToList();
                return accesslist;
            }
        }
    }
    public class DocumnetPdfRepository : RepositoryBase<Delivery_Invoice_Pdf>
    {
        public List<Delivery_Invoice_Pdf> GetPdfDocuemnts()
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<Delivery_Invoice_Pdf> accesslist = context.Delivery_Invoice_Pdf.ToList();
                return accesslist;
            }
        }
    }
    public class SumRepository : RepositoryBase<Delivery_Invoice_Excel>
    {
        public List<Delivery_Invoice_Excel> GetSum(int PinCode)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<Delivery_Invoice_Excel> accesslist = context.Delivery_Invoice_Excel.Where(x => x.R_Pin == PinCode).ToList();
                return accesslist;
            }
        }
    }
    public class PinCodeRepository : RepositoryBase<Tbl_Prescriptionorders>
    {
        public List<Tbl_Prescriptionorders> Getcode()
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                List<Tbl_Prescriptionorders> accesslist = context.Tbl_Prescriptionorders.Distinct().ToList();
                return accesslist;
            }
        }
    }
    public class CheckoutStatusRepository : RepositoryBase<CheckOutPaymentTransaction>
    {

        public long CheckoutStatus(int OrderID, string OrderStatus, int p)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                try
                {
                    //Tbl_AdditionalInfo datatoupdate = Context.Tbl_AdditionalInfo.Where(x => x.ProductId == info.ProductId).FirstOrDefault();
                    CheckOutPaymentTransaction commentData = context.CheckOutPaymentTransactions.Where(x => x.PaymentTransactionId == OrderID).FirstOrDefault();
                    commentData.OrderStatus = OrderStatus;

                    //context.CheckOutUserProductTransactions.Add(commentData);
                    context.SaveChanges();
                    return commentData.PaymentTransactionId;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
    }

    // modify by shashi

    public class BoxInpackaingRepository : RepositoryBase<PaymentTransaction>
    {

        public long BoxInpackaing(int OrderID, int BoxId)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                try
                {
                    //Tbl_AdditionalInfo datatoupdate = Context.Tbl_AdditionalInfo.Where(x => x.ProductId == info.ProductId).FirstOrDefault();
                    PaymentTransaction InpackingData = context.PaymentTransactions.Where(x => x.PaymentTransactionId == OrderID).FirstOrDefault();
                    InpackingData.BoxId = BoxId;

                    //context.CheckOutUserProductTransactions.Add(commentData);
                    context.SaveChanges();
                    return InpackingData.PaymentTransactionId;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
    }

    //public class OfflineSaleRepository : RepositoryBase<OfflineSale>
    //{ }
}




