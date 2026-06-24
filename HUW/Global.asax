<%@ Application Language="C#" %>
<%@ Import Namespace="System.ServiceModel.Activation" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web.Http.WebHost" %>
<%@ Import Namespace="System.Web.Http.Controllers" %>
<%@ Import Namespace="System.Web.Http.Filters" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.SessionState" %>
<%@ Import Namespace="System.Web.Services.Description" %>
<%@ Import Namespace="System.Net.Http" %>
<%@ Import Namespace="log4net.Config" %>

<script RunAt="server">

    // Route handler that enables session state for Web API controllers
    public class MyHttpControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public MyHttpControllerHandler(RouteData routeData) : base(routeData) { }
    }

    public class MyHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MyHttpControllerHandler(requestContext.RouteData);
        }
    }

    // IP validation filter
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
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    Content = new StringContent("Unauthorized IP Address")
                };
                return;
            }
        }
    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
    }

    void RegisterRoutes(RouteCollection routes)
    {
    }

    void Application_Start(object sender, EventArgs e)
    {
        RouteTable.Routes.MapHttpRoute(
           name: "DefaultApi",
           routeTemplate: "api/{controller}/{action}/{id}/{param}",
           defaults: new { id = System.Web.Http.RouteParameter.Optional, param = System.Web.Http.RouteParameter.Optional }
           ).RouteHandler = new MyHttpControllerRouteHandler();

        var config = GlobalConfiguration.Configuration;
        // config.Filters.Add(new TokenValidationAttribute());
        // config.Filters.Add(new CustomHttpsAttribute());
        config.Filters.Add(new IpHostValidationAttribute());
        //log4net
        //    XmlConfigurator.Configure();
        GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

        RegisterRoutes(RouteTable.Routes);
    }

    void Application_End(object sender, EventArgs e)
    {
    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();
        throw ex;
    }

    void Session_Start(object sender, EventArgs e)
    {
        if (BAL.BalUtility.GetSession(Utility.Shared.Sessions.Currency) == null)
        {
            Utility.CustomCurrency CurrentCurrency = new Utility.CustomCurrency();
            CurrentCurrency.FromCurrency = Utility.Shared.CountryCurrency.INR.ToString();
            CurrentCurrency.ToCurrency = Utility.Shared.CountryCurrency.INR.ToString();
            CurrentCurrency.Symbol = "₹";
            CurrentCurrency.Value = 1;
            BAL.BalUtility.CreateSession(CurrentCurrency, Utility.Shared.Sessions.Currency);
        }
    }

    void Session_End(object sender, EventArgs e)
    {
    }

</script>
