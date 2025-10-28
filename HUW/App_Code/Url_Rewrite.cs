using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for Url_Rewrite
/// </summary>
public static class Url_Rewrite
{    
    public static void Route_URLs()
    {
        string Current_URL = HttpContext.Current.Request.Url.PathAndQuery.ToLower();

        // Page Name ----------------------------------------------
        if (Current_URL.Contains("/details"))
        {
            string pattern = "/details.aspx";
            string replacement = "/details";
            Regex rgx = new Regex(pattern);
            Current_URL = rgx.Replace(Current_URL, replacement);
        }

        // id -----------------------------------------------------
        string id_pattern = "/id/(.*)/";
        if (Regex.Match(Current_URL, id_pattern).Success == true)
        {
            string id = Regex.Match(Current_URL, id_pattern).Groups[1].Value;
            string replacement = "?id=" + id;
            Regex oRegex = new Regex(id_pattern);
            Current_URL = oRegex.Replace(Current_URL, replacement);
        }

        HttpContext.Current.RewritePath(Current_URL);
    }
}