using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
//using DAL;
using Utility;

public class IpHostValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(HttpActionContext actionContext)
    {
        var context = actionContext.Request.Properties["MS_HttpContext"] as System.Web.HttpContextBase;
        string userIp = context.Request.UserHostAddress;
        try
        {
          // AuthorizedIpRepository.GetAuthorizedIPs().First(x => x == userIp);
        }
        catch (Exception)
        {
            actionContext.Response =new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
               {
                   Content = new StringContent("Unauthorized IP Address")
               };
            return;
        }
    }
}

public class AuthorizedIpRepository
{
    public static IQueryable<string> GetAuthorizedIPs()
    {
         var ips = new List<string> { "127.0.0.1", "::1" };//::1 is the IPv6 loopback address. Equivalent to 127.0.0.1 for IPv4.
        return ips.AsQueryable();
    }
}

//HTTPS Authentication

public class CustomHttpsAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(HttpActionContext actionContext)
    {
        if (!String.Equals(actionContext.Request.RequestUri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
        {
            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            {
                Content = new StringContent("HTTPS Required")
            };
            return;
        }
    }
}


//Tokens based on Public/Private Keys

public class RSAClass
{
    private static string _privateKey = "<RSAKeyValue><Modulus>s6lpjspk+3o2GOK5TM7JySARhhxE5gB96e9XLSSRuWY2W9F951MfistKRzVtg0cjJTdSk5mnWAVHLfKOEqp8PszpJx9z4IaRCwQ937KJmn2/2VyjcUsCsor+fdbIHOiJpaxBlsuI9N++4MgF/jb0tOVudiUutDqqDut7rhrB/oc=</Modulus><Exponent>AQAB</Exponent><P>3J2+VWMVWcuLjjnLULe5TmSN7ts0n/TPJqe+bg9avuewu1rDsz+OBfP66/+rpYMs5+JolDceZSiOT+ACW2Neuw==</P><Q>0HogL5BnWjj9BlfpILQt8ajJnBHYrCiPaJ4npghdD5n/JYV8BNOiOP1T7u1xmvtr2U4mMObE17rZjNOTa1rQpQ==</Q><DP>jbXh2dVQlKJznUMwf0PUiy96IDC8R/cnzQu4/ddtEe2fj2lJBe3QG7DRwCA1sJZnFPhQ9svFAXOgnlwlB3D4Gw==</DP><DQ>evrP6b8BeNONTySkvUoMoDW1WH+elVAH6OsC8IqWexGY1YV8t0wwsfWegZ9IGOifojzbgpVfIPN0SgK1P+r+kQ==</DQ><InverseQ>LeEoFGI+IOY/J+9SjCPKAKduP280epOTeSKxs115gW1b9CP4glavkUcfQTzkTPe2t21kl1OrnvXEe5Wrzkk8rA==</InverseQ><D>HD0rn0sGtlROPnkcgQsbwmYs+vRki/ZV1DhPboQJ96cuMh5qeLqjAZDUev7V2MWMq6PXceW73OTvfDRcymhLoNvobE4Ekiwc87+TwzS3811mOmt5DJya9SliqU/ro+iEicjO4v3nC+HujdpDh9CVXfUAWebKnd7Vo5p6LwC9nIk=</D></RSAKeyValue>";
    private static string _publicKey = "<RSAKeyValue><Modulus>s6lpjspk+3o2GOK5TM7JySARhhxE5gB96e9XLSSRuWY2W9F951MfistKRzVtg0cjJTdSk5mnWAVHLfKOEqp8PszpJx9z4IaRCwQ937KJmn2/2VyjcUsCsor+fdbIHOiJpaxBlsuI9N++4MgF/jb0tOVudiUutDqqDut7rhrB/oc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
    private static UnicodeEncoding _encoder = new UnicodeEncoding();

    public static string Decrypt(string data)
    {
        var rsa = new RSACryptoServiceProvider();
        var dataArray = data.Split(new char[] { ',' });
        byte[] dataByte = new byte[dataArray.Length];
        for (int i = 0; i < dataArray.Length; i++)
        {
            dataByte[i] = Convert.ToByte(dataArray[i]);
        }

        rsa.FromXmlString(_privateKey);
        var decryptedByte = rsa.Decrypt(dataByte, false);
        return _encoder.GetString(decryptedByte);
    }

    public static string Encrypt(string data)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(_publicKey);
        var dataToEncrypt = _encoder.GetBytes(data);
        var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
        var length = encryptedByteArray.Count();
        var item = 0;
        var sb = new StringBuilder();
        foreach (var x in encryptedByteArray)
        {
            item++;
            sb.Append(x);

            if (item < length)
                sb.Append(",");
        }

        return sb.ToString();
    }
}

//Example from asp.net

//WebRequest getRequest(string method, string contentType, string endPoint)
//{
// var request = WebRequest.Create(endPoint);
// request.Method = method;
// request.ContentType = contentType;
 
// ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
 
// request.Headers.Add("Authorization-Token", "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");
 
// return request;
//}

public class AuthorizedUserRepository
{
    public static IQueryable<Utility.User> GetUsers()
    {

        IList<User> users = new List<User>();
        //users.Add(new User("User1"));
        //users.Add(new User("User2"));
        //users.Add(new User("User3"));
        //users.Add(new User("Administrator"));

        return users.AsQueryable();
    }
}

public class TokenValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(HttpActionContext actionContext)
    {
        string token;

        try
        {
            token = actionContext.Request.Headers.GetValues("Authorization-Token").First();
        }
        catch (Exception)
        {
            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Missing Authorization-Token")
            };
            return;
        }

        try
        {
            AuthorizedUserRepository.GetUsers().First(x => x.FirstName  == RSAClass.Decrypt(token));
            base.OnActionExecuting(actionContext);
        }
        catch (Exception)
        {
            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
            {
                Content = new StringContent("Unauthorized User")
            };
            return;
        }
    }
}
