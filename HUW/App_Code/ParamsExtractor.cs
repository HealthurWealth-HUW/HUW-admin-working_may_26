using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for ParamsExtractor
/// </summary>


/// <summary>
/// This class is used to return url parameter values to the caller
/// </summary>
public class UrlParamsExtractor
{
    public static object lockObject;
    private static UrlParamsExtractor obj;
    private UrlParamsExtractor()
    {
    }
    /// <summary>
    /// Used to return Instance
    /// </summary>
    public static UrlParamsExtractor GetInstance
    {
        get
        {
            if (obj == null)
                obj = new UrlParamsExtractor();
            return obj;
        }
    }
    /// <summary>
    /// Used to return value for given param
    /// </summary>
    /// <param name="paramName"> key name</param>
    /// <returns></returns>
    public dynamic getParam(string paramName, Page objPageRef)
    {
        switch (paramName)
        {
            case "supercategory":
                return objPageRef.RouteData.Values["supercategory"] != null ? objPageRef.RouteData.Values["supercategory"].ToString() : "";

            case "category":
                return objPageRef.RouteData.Values["category"] != null ? objPageRef.RouteData.Values["category"].ToString() : "";
            case "subcategory":
                return objPageRef.RouteData.Values["subcategory"] != null ? objPageRef.RouteData.Values["subcategory"].ToString() : "";
            case "scid":
                return objPageRef.RouteData.Values["scid"] != null ? Convert.ToInt32(objPageRef.RouteData.Values["scid"].ToString()) : 0;
            case "cid":
                return objPageRef.RouteData.Values["cid"] != null ? Convert.ToInt32(objPageRef.RouteData.Values["cid"].ToString()) : 0;
            case "sbcid":
                return objPageRef.RouteData.Values["sbcid"] != null ? Convert.ToInt32(objPageRef.RouteData.Values["sbcid"].ToString()) : 0;
            case "productid":
                return objPageRef.RouteData.Values["productid"] != null ? Convert.ToInt32(objPageRef.RouteData.Values["productid"].ToString()) : 0;
            case "productname":
                return objPageRef.RouteData.Values["productname"] != null ? objPageRef.RouteData.Values["productname"].ToString() : "";

        }
        return null;
    }
}