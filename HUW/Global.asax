<%@ Application Language="C#" %>
<%@ Import Namespace="System.ServiceModel.Activation" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Services.Description" %>
<%@ Import Namespace="log4net.Config" %>

<script RunAt="server">


    void Application_BeginRequest(object sender, EventArgs e)
    {
        //// Get the current path
        //string CurrentURL_Path = Request.Path.ToLower();

        //if (CurrentURL_Path.StartsWith("/news"))
        //{
        //    //CurrentURL_Path = CurrentURL_Path.Trim("/");
        //    //string NewsID = CurrentPath.Substring(CurrentPath.IndexOf("/"));
        //    HttpContext MyContext = HttpContext.Current;
        //    MyContext.RewritePath("/index.aspx");
        //}         
    }

    void RegisterRoutes(RouteCollection routes)
    {
        //routes.Ignore("{*allimages}", new { allimages = @".*\.jpg(/.*)?" });
        //routes.Ignore("{*allimagespng}", new { allimagespng = @".*\.png(/.*)?" });
        //routes.Ignore("{*allasmx}", new { allasmx = @".*\.asmx(/.*)?" });
        //routes.Ignore("{*alljs}", new { alljs = @".*\.js(/.*)?" });
        //routes.Ignore("{*allcss}", new { allcss = @".*\.css(/.*)?" });
        //routes.Ignore("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
        //routes.Ignore("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
        //routes.MapPageRoute("SuperCategoryRoute", "{supercategory}/{scid}", "~/SuperCategory.aspx");
        //routes.MapPageRoute("CategoryRoute", "{supercategory}/{category}/{scid}/{cid}", "~/Category.aspx");
        //routes.MapPageRoute("SubCategory", "{supercategory}/{category}/{subcategory}/{scid}/{cid}/{sbcid}", "~/Category.aspx");
        //routes.MapPageRoute("DetailRoute", "product/{productname}/{productid}", "~/Details.aspx");
    }

    void Application_Start(object sender, EventArgs e)
    {
        RouteTable.Routes.MapHttpRoute(
           name: "DefaultApi",
           routeTemplate: "api/{controller}/{action}/{id}/{param}",
           defaults: new { id = System.Web.Http.RouteParameter.Optional, param = System.Web.Http.RouteParameter.Optional }
           ).RouteHandler = new MyHttpControllerRouteHandler();
        // RouteTable.Routes.Add(new ServiceRoute("",
        //new WebServiceHostFactory(),
        //typeof(Service)));

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
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

        Exception ex = Server.GetLastError();
        throw ex;

        //if (ex.GetType() == typeof(System.Web.HttpUnhandledException))
        //{
        //    Server.ClearError();
        //}

    }

    void Session_Start(object sender, EventArgs e)
    {
        //Response.Redirect("~/Index.aspx");
        if (BAL.BalUtility.GetSession(Utility.Shared.Sessions.Currency) == null)
        {
            Utility.CustomCurrency CurrentCurrency = new Utility.CustomCurrency();
            CurrentCurrency.FromCurrency = Utility.Shared.CountryCurrency.INR.ToString();
            CurrentCurrency.ToCurrency = Utility.Shared.CountryCurrency.INR.ToString();
            CurrentCurrency.Symbol = "₹";
            CurrentCurrency.Value = 1;
            BAL.BalUtility.CreateSession(CurrentCurrency, Utility.Shared.Sessions.Currency);
        }

        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    //protected void Application_BeginRequest(object sender, EventArgs e)
    //{

    //    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //    HttpContext.Current.Response.Cache.SetNoStore();

    //    EnableCrossDmainAjaxCall();
    //}

    //private void EnableCrossDmainAjaxCall()
    //{
    //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin",
    //                  "http://localhost:50120");

    //    if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
    //    {
    //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods",
    //                      "GET, POST");
    //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers",
    //                      "Content-Type, Accept");
    //        HttpContext.Current.Response.AddHeader("Access-Control-Max-Age",
    //                      "1728000");
    //        HttpContext.Current.Response.End();
    //    }
    //}

</script>
