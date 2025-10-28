"use strict";
let mainContent;
(function () {
    let html = document.querySelector('html');
    mainContent = document.querySelector('.app-content');

    // theme color picker
    const dynamicPrimaryLight = document.querySelectorAll('input.color-primary-light');
    const dynamicBackground = document.querySelectorAll('input.color-bg-transparent');
    dynamicPrimaryColor(dynamicPrimaryLight);
    dynamicBackgroundColor(dynamicBackground);

    localStorageBackup();


    
    //RTL 
    if (!localStorage.getItem("dashlotrtl")) {
        // html.setAttribute("dir" , "rtl") // for rtl version 
    }

    //Light Theme Style
    if (!localStorage.getItem("dashlotlighttheme")) {
        // html.setAttribute("data-theme-color" , "light") // for light theme 
    }

    //Dark Theme Style
    if (!localStorage.getItem("dashlotdarktheme")) {
        // html.setAttribute("data-theme-color" , "dark") // for dark theme 
    }

    //Menu Layout
    if (!localStorage.getItem("dashlotlayout")) {
        // html.setAttribute("data-layout" , "vertical") // for Vertical layout 
        // html.setAttribute("data-layout" , "horizontal") // for horizontal layout 
    }
    
    //Menu Styles
    if (!localStorage.getItem("dashlotMenu")) {
        // html.setAttribute("data-menu-style" , "light") // for light menu style 
        html.setAttribute("data-menu-style" , "dark") // for dark menu style 
        // html.setAttribute("data-menu-style" , "color") // for color menu style
        // html.setAttribute("data-menu-style" , "gradient") // for gradient menu style 
    }
    
    //Header Styles
    if (!localStorage.getItem("dashlotHeader")) {
        // html.setAttribute("data-header-style" , "light") // for light header style 
        // html.setAttribute("data-header-style" , "dark") // for dark header style 
        // html.setAttribute("data-header-style" , "color") // for color header style 
        // html.setAttribute("data-header-style" , "gradient") // for gradient header style 
    }
    
    //Default Layout Styles
    if (!localStorage.getItem("dashlotverticalstyles")) {
        // html.setAttribute("data-vertical-style" , "default") // for Vertical default style 
    }

    //Closed Layout Styles
    if (!localStorage.getItem("dashlotverticalstyles")) {
        // html.setAttribute("data-vertical-style" , "closed") // for Vertical closed style 
        // $('body').addClass('sidenav-toggled');
    }

    //IconText Layout Styles
    if (!localStorage.getItem("dashlotverticalstyles")) {
        // html.setAttribute("data-vertical-style" , "icontext") // for Vertical icontext style 
        // textLayoutFn();
    }

    //Overlay Layout Styles
    if (!localStorage.getItem("dashlotverticalstyles")) {
        // html.setAttribute("data-vertical-style" , "overlay") // for Vertical overlay style 
        // $('body').addClass('sidenav-toggled');
    }

    //Hover Submenu Layout Styles
    if (!localStorage.getItem("dashlotverticalstyles")) {
        // html.setAttribute("data-vertical-style" , "hover") // for Vertical hover style 
        // hoverLayoutFn();
    }

    //Hover Submenu1 Layout Styles
    if (!localStorage.getItem("dashlotverticalstyles")) {
        // html.setAttribute("data-vertical-style" , "hover1") // for Vertical hover1 style 
        // hoverLayoutFn();
    }

    //Double Menu Layout Styles
    if (!localStorage.getItem("dashlotverticalstyles")) {
        // html.setAttribute("data-vertical-style" , "doublemenu") // for Vertical doublemenu style 
        // doubleLayoutFn();
    }

    //Double Menu Tabs Layout Styles
    if (!localStorage.getItem("dashlotverticalstyles")) {
        // html.setAttribute("data-vertical-style" , "doublemenu-tabs") // for Vertical doublemenu-tabs style 
        // doubleLayoutFn();
    }

    //horizontalmenu Layout Styles
    if (!localStorage.getItem("dashlotlayout") === "horizontal" || localStorage.getItem("dashlotlayout") == null) {
        // html.setAttribute("data-hor-style" , "hor-click") // for horizontal click style 
        // html.setAttribute("data-hor-style" , "hor-hover") // for horizontal hover style 
    } 

    //Boxed styles
    if (!localStorage.getItem("dashlotboxed")) {
        // html.setAttribute("data-width" , "boxed") // for boxed style
    }

    //Scrollabel styles
    if (!localStorage.getItem("dashlotscrollable")) {
        // html.setAttribute("data-position" , "scrollable") // for scrollable style
    }

    //No-Shadow styles
    if (!localStorage.getItem("dashlotnoshadow")) {
        // html.setAttribute("data-skins" , "no-shadow") // for boxed style
    }

    //Centerlogo For Horizontal
    if (!localStorage.getItem("dashlotcenterlogo")) {
        // html.setAttribute("data-logo" , "centerlogo") // for Horizontal Centerlogo
    }



    /*RTL Start*/
    if (html.getAttribute('dir') === "rtl") {
        rtlFn();
    }
    /*RTL End*/

    /*Horizontal Start*/
    if (html.getAttribute('data-hor-style') === "hor-click") {
        horizontalClickFn();
    }
    /*Horizontal End*/

    /*Horizontal-Hover Start*/
    if (html.getAttribute('data-hor-style') === "hor-hover") {
        horizontalHoverFn();
    }
    /*Horizontal-Hover End*/

    if (document.querySelector(".sidebar-right1")) {
        switcherClick();
    }

    checkOptions();

})();

function switcherClick() {
    let ltrBtn, rtlBtn, verticalBtn, horiBtn, horiHoverBtn, lightBtn, darkBtn, boxedBtn, fullwidthBtn, scrollableBtn, fixedBtn, lightHeaderBtn, darkHeaderBtn, colorHeaderBtn, gradientHeaderBtn, lightMenuBtn, darkMenuBtn, colorMenuBtn, gradientMenuBtn, shadowBtn, NoshadowBtn, defaultBtn, closedBtn, iconTextBtn, hoversubBtn, hoversub1Btn, overlayBtn, doubleBtn, doubleTabsBtn, defaultlogoBtn, centerlogoBtn, resetBtn;
    let html = document.querySelector('html');
    lightBtn = document.querySelector('#switchbtn-light');
    darkBtn = document.querySelector('#switchbtn-dark');
    ltrBtn = document.querySelector('#switchbtn-ltr');
    rtlBtn = document.querySelector('#switchbtn-rtl');
    verticalBtn = document.querySelector('#switchbtn-vertical');
    horiBtn = document.querySelector('#switchbtn-horizontal');
    horiHoverBtn = document.querySelector('#switchbtn-horizontalHover');
    boxedBtn = document.querySelector('#switchbtn-boxed');
    fullwidthBtn = document.querySelector('#switchbtn-fullwidth');
    scrollableBtn = document.querySelector('#switchbtn-scrollable');
    fixedBtn = document.querySelector('#switchbtn-fixed');
    lightHeaderBtn = document.querySelector('#switchbtn-lightheader');
    darkHeaderBtn = document.querySelector('#switchbtn-darkheader');
    colorHeaderBtn = document.querySelector('#switchbtn-colorheader');
    gradientHeaderBtn = document.querySelector('#switchbtn-gradientheader');
    lightMenuBtn = document.querySelector('#switchbtn-lightmenu');
    darkMenuBtn = document.querySelector('#switchbtn-darkmenu');
    colorMenuBtn = document.querySelector('#switchbtn-colormenu');
    gradientMenuBtn = document.querySelector('#switchbtn-gradientmenu');
    shadowBtn = document.querySelector('#switchbtn-shadow');
    NoshadowBtn = document.querySelector('#switchbtn-noshadow');
    defaultBtn = document.querySelector('#switchbtn-defaultmenu');
    closedBtn = document.querySelector('#switchbtn-closed');
    iconTextBtn = document.querySelector('#switchbtn-text');
    hoversubBtn = document.querySelector('#switchbtn-hoversub');
    hoversub1Btn = document.querySelector('#switchbtn-hoversub1');
    overlayBtn = document.querySelector('#switchbtn-overlay');
    doubleBtn = document.querySelector('#switchbtn-doublemenu');
    doubleTabsBtn = document.querySelector('#switchbtn-doublemenu-tabs');
    defaultlogoBtn = document.querySelector('#switchbtn-defaultlogo');
    centerlogoBtn = document.querySelector('#switchbtn-centerlogo');
    resetBtn = document.querySelector('#resetbtn');

    /*Light Layout Start*/
    let lightThemeVar = lightBtn.addEventListener('click', () => {
        html.setAttribute('data-theme-color', 'light');
        html.setAttribute('data-header-style', 'light');
        html.setAttribute('data-menu-style', 'light');
        
        $('#switchbtn-lightmenu').prop('checked', true);
        $('#switchbtn-lightheader').prop('checked', true);

        localStorage.setItem("dashlotlighttheme", true);
        localStorage.removeItem("dashlotdarktheme");
        localStorage.removeItem("dashlotbgColor");
        localStorage.removeItem("dashlotheaderbg");
        localStorage.removeItem("dashlotbgwhite");
        localStorage.removeItem("dashlotmenubg");

        localStorage.setItem("dashlotHeader", 'light');
        localStorage.setItem("dashlotMenu", 'light');

        checkOptions();
        const root = document.querySelector(':root');
        root.style = "";
        names();

        if (!document.body.classList.contains('auth-page')) {
            let mainHeader = document.querySelector('.app-header');
            mainHeader.style = "";
            let appSidebar = document.querySelector('.app-sidebar');
            appSidebar.style = "";
        }

    })
    /*Light Layout End*/

    /*Dark Layout Start*/
    let darkThemeVar = darkBtn.addEventListener('click', () => {
        html.setAttribute('data-theme-color', 'dark');
        html.setAttribute('data-header-style', 'dark');
        html.setAttribute('data-menu-style', 'dark');
        
        $('#switchbtn-darkmenu').prop('checked', true);
        $('#switchbtn-darkheader').prop('checked', true);
       
        localStorage.setItem("dashlotdarktheme", true);
        localStorage.removeItem("dashlotlighttheme");
        localStorage.removeItem("dashlotbgColor");
        localStorage.removeItem("dashlotheaderbg");
        localStorage.removeItem("dashlotbgwhite");
        localStorage.removeItem("dashlotmenubg");
        //
        

        localStorage.setItem("dashlotHeader", 'dark');
        localStorage.setItem("dashlotMenu", 'dark');

        checkOptions();

        const root = document.querySelector(':root');
        root.style = "";
        names();

        if (!document.body.classList.contains('auth-page')) {
            let mainHeader = document.querySelector('.app-header');
            mainHeader.style = "";
            let appSidebar = document.querySelector('.app-sidebar');
            appSidebar.style = "";
        }

    });
    /*Dark Layout End*/

    /*Light Menu Start*/
    let lightMenuVar = lightMenuBtn?.addEventListener('click', () => {
        html.setAttribute('data-menu-style', 'light');
        let appSidebar = document.querySelector('.app-sidebar');
        appSidebar.style = "";
        localStorage.setItem("dashlotMenu", 'light');
    });
    /*Light Menu End*/

    /*Color Menu Start*/
    let colorMenuVar = colorMenuBtn?.addEventListener('click', () => {
        html.setAttribute('data-menu-style', 'color');
        let appSidebar = document.querySelector('.app-sidebar');
        appSidebar.style = "";
        localStorage.setItem("dashlotMenu", 'color');
    });
    /*Color Menu End*/

    /*Dark Menu Start*/
    let darkMenuVar = darkMenuBtn?.addEventListener('click', () => {
        html.setAttribute('data-menu-style', 'dark');
        let appSidebar = document.querySelector('.app-sidebar');
        appSidebar.style = "";
        localStorage.setItem("dashlotMenu", 'dark');
    });
    /*Dark Menu End*/

    /*Gradient Menu Start*/
    let gradientMenuVar = gradientMenuBtn?.addEventListener('click', () => {
        html.setAttribute('data-menu-style', 'gradient');
        let appSidebar = document.querySelector('.app-sidebar');
        appSidebar.style = "";
        localStorage.setItem("dashlotMenu", 'gradient');
    });
    /*Gradient Menu End*/

    /*Light Header Start*/
    let lightHeaderVar = lightHeaderBtn?.addEventListener('click', () => {
        html.setAttribute('data-header-style', 'light');
        let mainHeader = document.querySelector('.app-header');
        mainHeader.style = "";
        localStorage.setItem("dashlotHeader", 'light');
    });
    /*Light Header End*/

    /*Color Header Start*/
    let colorHeaderVar = colorHeaderBtn?.addEventListener('click', () => {
        html.setAttribute('data-header-style', 'color');
        let mainHeader = document.querySelector('.app-header');
        mainHeader.style = "";
        localStorage.setItem("dashlotHeader", 'color');
    });
    /*Color Header End*/

    /*Dark Header Start*/
    let darkHeaderVar = darkHeaderBtn?.addEventListener('click', () => {
        html.setAttribute('data-header-style', 'dark');
        let mainHeader = document.querySelector('.app-header');
        mainHeader.style = "";
        localStorage.setItem("dashlotHeader", 'dark');
    });
    /*Dark Header End*/

    /*Gradient Header Start*/
    let gradientHeaderVar = gradientHeaderBtn?.addEventListener('click', () => {
        html.setAttribute('data-header-style', 'gradient');
        let mainHeader = document.querySelector('.app-header');
        mainHeader.style = "";
        localStorage.setItem("dashlotHeader", 'gradient');
    });
    /*Gradient Header End*/

    /*Full Width Layout Start*/
    let fullwidthVar = fullwidthBtn?.addEventListener('click', () => {
        html.setAttribute('data-width', 'fullwidth');
        if (html.getAttribute('data-layout') === "horizontal") {
            checkHoriMenu();
        }
        localStorage.setItem("dashlotfullwidth", true);
        localStorage.removeItem("dashlotboxed");
    });
    /*Full Width Layout End*/

    /*Boxed Layout Start*/
    let boxedVar = boxedBtn?.addEventListener('click', () => {
        html.setAttribute('data-width', 'boxed');
        if (html.getAttribute('data-layout') === "horizontal") {
            checkHoriMenu();
        }
        localStorage.setItem("dashlotboxed", true);
        localStorage.removeItem("dashlotfullwidth");
    });
    /*Boxed Layout End*/

    /*Shadow Layout Start*/
    let shadowVar = shadowBtn?.addEventListener('click', () => {
        html.setAttribute('data-skins', 'shadow');
        localStorage.setItem("dashlotshadow", true);
        localStorage.removeItem("dashlotnoshadow");
    });
    /*Shadow Layout End*/

    /*No Shadow Layout Start*/
    let noShadowVar = NoshadowBtn?.addEventListener('click', () => {
        html.setAttribute('data-skins', 'no-shadow');
        localStorage.setItem("dashlotnoshadow", true);
        localStorage.removeItem("dashlotshadow");
    });
    /*No Shadow Layout End*/

    /*Header-Position Styles Start*/
    let fixedVar = fixedBtn?.addEventListener('click', () => {
        html.setAttribute('data-position', 'fixed');
        localStorage.setItem("dashlotfixed", true);
        localStorage.removeItem("dashlotscrollable");
    });

    let scrollableVar = scrollableBtn?.addEventListener('click', () => {
        html.setAttribute('data-position', 'scrollable');
        localStorage.setItem("dashlotscrollable", true);
        localStorage.removeItem("dashlotfixed");
    });
    /*Header-Position Styles End*/

    /*Default Sidemenu Start*/
    let defaultVar = defaultBtn?.addEventListener('click', () => {
        html.setAttribute('data-vertical-style', 'default');
        document.body.classList.remove('sidenav-toggled');
        localStorage.removeItem("dashlotverticalstyles");

        hovermenu();
    });
    /*Default Sidemenu End*/

    /*Closed Sidemenu Start*/
    let closedVar = closedBtn?.addEventListener('click', () => {
        html.setAttribute('data-vertical-style', 'closed');
        localStorage.setItem("dashlotverticalstyles", 'closed');

        hoverLayoutFn();
    });
    /*Closed Sidemenu End*/

    /*Hover Submenu Start*/
    let hoverSubVar = hoversubBtn?.addEventListener('click', () => {
        html.setAttribute('data-vertical-style', 'hover');
        localStorage.setItem("dashlotverticalstyles", 'hover');
        document.body.classList.remove("show-sidenav");

        hoverLayoutFn();
    });
    /*Hover Submenu End*/

    /*Hover Submenu 1 Start*/
    let hoverSub1Var = hoversub1Btn?.addEventListener('click', () => {
        html.setAttribute('data-vertical-style', 'hover1');
        localStorage.setItem("dashlotverticalstyles", 'hover1');
        document.body.classList.remove("show-sidenav");

        hoverLayoutFn();
    });
    /*Hover Submenu 1 End*/

    /*Icon Text Sidemenu Start*/
    let iconTextVar = iconTextBtn?.addEventListener('click', () => {
        html.setAttribute('data-vertical-style', 'icontext');
        localStorage.setItem("dashlotverticalstyles", 'icontext');

        textLayoutFn();
    });
    /*Icon Text Sidemenu End*/

    /*Icon Overlay Sidemenu Start*/
    let overlayVar = overlayBtn?.addEventListener('click', () => {
        html.setAttribute('data-vertical-style', 'overlay');
        localStorage.setItem("dashlotverticalstyles", 'overlay');

        hoverLayoutFn();
    });
    /*Icon Overlay Sidemenu End*/

    /*Double Menu Sidemenu Start*/
    let doubleVar = doubleBtn?.addEventListener('click', () => {
        html.setAttribute('data-vertical-style', 'doublemenu');
        localStorage.setItem("dashlotverticalstyles", 'doublemenu');

        doubleLayoutFn();
    });
    /*Double Menu Sidemenu End*/

    /*Double Menu Sidemenu Start*/
    let doubleTabsVar = doubleTabsBtn?.addEventListener('click', () => {
        html.setAttribute('data-vertical-style', 'doublemenu-tabs');
        localStorage.setItem("dashlotverticalstyles", 'doublemenu-tabs');

        doubleLayoutFn();
    });
    /*Double Menu Sidemenu End*/

    /* Sidemenu start*/
    let verticalVar = verticalBtn?.addEventListener('click', () => {
        // local storage
        localStorage.removeItem("dashlotlayout");
        localStorage.setItem("dashlotverticalstyles", 'default');

        verticalFn();
        document.querySelectorAll(".side-menu>li>.active")[0].classList.add("open")
        document.querySelectorAll(".side-menu>li>.active")[0].nextElementSibling.style.display = "block"
    });
    /* Sidemenu end*/

    /* horizontal click start*/
    let horiVar = horiBtn?.addEventListener('click', () => {

        //    local storage 
        localStorage.setItem("dashlotlayout", 'horizontal');
        localStorage.removeItem("dashlotverticalstyles");

        horizontalClickFn();
    });
    /* horizontal click end*/

    /* horizontal hover start*/
    let horiHoverVar = horiHoverBtn?.addEventListener('click', () => {

        //    local storage 
        localStorage.setItem("dashlotlayout", 'horizontalhover');
        localStorage.removeItem("dashlotverticalstyles");

        horizontalHoverFn();
    });
    /* horizontal hover end*/
    /* rtl start*/
    let rtlVar = rtlBtn?.addEventListener('click', () => {
        localStorage.setItem("dashlotrtl", true);
        localStorage.removeItem("dashlotltr");
        rtlFn();
    });
    /* rtl end*/
    /* ltr start*/
    let ltrVar = ltrBtn?.addEventListener('click', () => {
        //    local storage 
        localStorage.setItem("dashlotltr", true);
        localStorage.removeItem("dashlotrtl");

        ltrFn();
    });
    /* ltr end*/


    /*Horizontal Logo Position Start*/
    let defaultlogoVar = defaultlogoBtn?.addEventListener('click', () => {
        html.setAttribute('data-logo', 'defaultlogo');
        localStorage.setItem("dashlotdefaultlogo", true);
        localStorage.removeItem("dashlotcenterlogo");
    });

    let centerlogoVar = centerlogoBtn?.addEventListener('click', () => {
        html.setAttribute('data-logo', 'centerlogo');
        localStorage.setItem("dashlotcenterlogo", true);
        localStorage.removeItem("dashlotdefaultlogo");
    });
    /*Horizontal Logo Position End*/
}

function ltrFn() {
    let html = document.querySelector('html');
    html.setAttribute("dir", "ltr");
    let select2Cont = document.querySelectorAll(".select2-container")
    select2Cont.forEach(e => e.setAttribute("dir", "ltr"))
    document.querySelector("#style")?.setAttribute("href", "../assets/plugins/bootstrap/css/bootstrap.min.css");
    var carousel = $('.owl-carousel');
    $.each(carousel, function (index, element) {
        // element == this
        var carouselData = $(element).data('owl.carousel');
        carouselData.settings.rtl = false; //don't know if both are necessary
        carouselData.options.rtl = false;
        $(element).trigger('refresh.owl.carousel');
    });
    if (html.getAttribute('data-layout') === "horizontal") {
        checkHoriMenu();
    }
    //
    checkOptions();
}

function checkHoriMenu(){}

function rtlFn() {
    let html = document.querySelector('html');
    html.setAttribute("dir", "rtl");
    let select2Cont = document.querySelectorAll(".select2-container")
    select2Cont.forEach(e => e.setAttribute("dir", "rtl"))
    document.querySelector("#style")?.setAttribute("href", "../assets/plugins/bootstrap/css/bootstrap.rtl.min.css");
    var carousel = $('.owl-carousel');
    $.each(carousel, function (index, element) {
        setTimeout(() => {
            var carouselData = $(element).data('owl.carousel');
            carouselData.settings.rtl = true; //don't know if both are necessary
            carouselData.options.rtl = true;
            $(element).trigger('refresh.owl.carousel');
        }, 100);
    });
    if (html.getAttribute('data-layout') === "horizontal") {
        checkHoriMenu();
    }
    //
    checkOptions();
}

function verticalFn() {
    $('#switchbtn-vertical').prop('checked', true);
    let html = document.querySelector('html');
    html.setAttribute('data-layout', 'vertical');
    html.setAttribute('data-vertical-style', 'default');
    html.removeAttribute('data-hor-style');
    if (!document.body.classList.contains('auth-page')) {
        document.body.classList.add('sidebar-mini');
        document.querySelector(".main-content").classList.add("app-content");
        let mainContainer = document.querySelectorAll(".main-container")
        mainContainer.forEach(e => e.classList.add("container-fluid"))
        mainContainer.forEach(e => e.classList.remove("container"))
        document.querySelector(".main-content").classList.remove("horizontal-content");
        document.querySelector(".app-header").classList.remove("hor-header");
        document.querySelector(".app-sidebar").classList.remove("horizontal-main");
        document.querySelector(".main-sidemenu").classList.remove("container");
        document.querySelector('#slide-left').classList.remove('d-none');
        document.querySelector('#slide-right').classList.remove('d-none');
        if (html.getAttribute('data-layout') === "horizontal") {
            checkHoriMenu();
        }
        responsive();
        menuClick();
        mainContent.removeEventListener('click', slideClick);
        //
        checkOptions();
    }
}

function horizontalClickFn() {
    $('#switchbtn-horizontal').prop('checked', true);
    let html = document.querySelector('html');
    html.setAttribute('data-layout', 'horizontal');
    html.setAttribute('data-hor-style', 'hor-click');
    html.removeAttribute('data-vertical-style');
    if (!document.body.classList.contains('auth-page')) {
        
        ActiveSubmenu();
        document.querySelector(".main-content").classList.add("horizontal-content");
        let mainContainer = document.querySelectorAll(".main-container")
        mainContainer.forEach(e => e.classList.add("container"))
        mainContainer.forEach(e => e.classList.remove("container-fluid"))
        document.querySelector(".app-header").classList.add("hor-header");
        document.querySelector(".app-sidebar").classList.add("horizontal-main");
        document.querySelector(".main-sidemenu").classList.add("container");

        document.querySelector(".main-content").classList.remove("app-content");
        document.body.classList.remove('sidebar-mini');
        document.body.classList.remove('sidenav-toggled');
        responsive();
        menuClick();
        mainContent.addEventListener('click', slideClick);
        setTimeout(() => {
            if (window.innerWidth >= 992) {
                slideClick()
            }
            checkHoriMenu();
        }, 800)
        //
        checkOptions();
    }
}

function horizontalHoverFn() {
    $('#switchbtn-horizontalHover').prop('checked', true);
    let html = document.querySelector('html');
    html.setAttribute('data-layout', 'horizontal');
    html.setAttribute('data-hor-style', 'hor-hover');
    html.removeAttribute('data-vertical-style');
    let li = document.querySelectorAll('.side-menu li')

    if (!document.body.classList.contains('auth-page')) {

        document.querySelector(".main-content").classList.add("horizontal-content");
        document.querySelector(".main-content").classList.remove("app-content");
        let mainContainer = document.querySelectorAll(".main-container")
        mainContainer.forEach(e => e.classList.add("container"))
        mainContainer.forEach(e => e.classList.remove("container-fluid"))
        document.querySelector(".app-header").classList.add("hor-header");
        document.querySelector(".app-sidebar").classList.add("horizontal-main")
        document.querySelector(".main-sidemenu").classList.add("container")
        document.body.classList.remove('sidebar-mini');
        document.body.classList.remove('sidenav-toggled');
        responsive();
        menuClick();
        mainContent.removeEventListener('click', slideClick);
        //
        setTimeout(() => {
            if (window.innerWidth >= 992) {
                slideClick()
            }
            checkHoriMenu();
        }, 500)
        checkOptions();
    }
}

function resetData() {
    let html = document.querySelector('html');
    $('#switchbtn-ltr').prop('checked', true);
    $('#switchbtn-rtl').prop('checked', false);
    $('#switchbtn-light').prop('checked', true);
    $('#switchbtn-lightmenu').prop('checked', true);
    $('#switchbtn-lightheader').prop('checked', true);
    $('#switchbtn-fullwidth').prop('checked', true);
    $('#switchbtn-fixed').prop('checked', true);
    $('#switchbtn-defaultmenu').prop('checked', true);
    $('#switchbtn-shadow').prop('checked', true);
    $('#switchbtn-defaultlogo').prop('checked', true);
    html.setAttribute('data-theme-color', 'light');
    html.setAttribute('data-header-style', 'light');
    html.setAttribute('data-menu-style', 'dark');
    html.setAttribute('data-width', 'fullwidth');
    html.setAttribute('data-position', 'fixed');
    html.setAttribute('data-logo', 'defaultlogo');
    html.setAttribute('data-skins', 'shadow');
    html.setAttribute('data-layout', 'vertical');
    html.setAttribute('data-vertical-style', 'default');
    document.body.classList.remove('sidenav-toggled');
    verticalFn();
    ltrFn();
    localStorage.clear();
    if (!document.body.classList.contains('auth-page')) {
        let mainHeader = document.querySelector('.app-header');
        mainHeader.style = "";
        let appSidebar = document.querySelector('.app-sidebar');
        appSidebar.style = "";
    
        //
        checkOptions();
        menuClick();
    }
    names();
}

function checkOptions() {

    let html= document.querySelector('html')

    // light
    if (html.getAttribute('data-theme-color') === "light") {
        $('#switchbtn-light').prop('checked', true);
        $('#switchbtn-darkmenu').prop('checked', true);
        $('#switchbtn-lightheader').prop('checked', true);
    }

    // dark
    if (html.getAttribute('data-theme-color') === "dark") {
        $('#switchbtn-dark').prop('checked', true);
        $('#switchbtn-darkmenu').prop('checked', true);
        $('#switchbtn-darkheader').prop('checked', true);
    }

    // horizontal
    if (html.getAttribute('data-hor-style') === "hor-click") {
        $('#switchbtn-horizontal').prop('checked', true);
    }

    // horizontal-hover
    if (html.getAttribute('data-hor-style') === "hor-hover")  {
        $('#switchbtn-horizontalHover').prop('checked', true);
    }

    //RTL 
    if (html.getAttribute('dir') === "rtl") {
        $('#switchbtn-rtl').prop('checked', true);
    }

    //LTR
    if (html.getAttribute('dir') === "ltr") {
        $('#switchbtn-ltr').prop('checked', true);
    }

    // light header 
    if (html.getAttribute('data-header-style') === "light") {
        $('#switchbtn-lightheader').prop('checked', true);
    }

    // color header 
    if (html.getAttribute('data-header-style') === "color") {
        $('#switchbtn-colorheader').prop('checked', true);
    }

    // gradient header 
    if (html.getAttribute('data-header-style') === "gradient") {
        $('#switchbtn-gradientheader').prop('checked', true);
    }

    // dark header 
    if (html.getAttribute('data-header-style') === "dark") {
        $('#switchbtn-darkheader').prop('checked', true);
    }

    // light menu
    if (html.getAttribute('data-menu-style') === 'light') {
        $('#switchbtn-lightmenu').prop('checked', true);
    }

    // color menu
    if (html.getAttribute('data-menu-style') === 'color') {
        $('#switchbtn-colormenu').prop('checked', true);
    }

    // gradient menu
    if (html.getAttribute('data-menu-style') === 'gradient') {
        $('#switchbtn-gradientmenu').prop('checked', true);
    }

    // dark menu
    if (html.getAttribute('data-menu-style') === 'dark') {
        $('#switchbtn-darkmenu').prop('checked', true);
    }

    //boxed 
    if (html.getAttribute('data-width') === 'boxed') {
        $('#switchbtn-boxed').prop('checked', true);
    }

    //scrollable 
    if (html.getAttribute('data-position') === 'scrollable') {
        $('#switchbtn-scrollable').prop('checked', true);
    }

    //noshadow 
    if (html.getAttribute('data-skins') === 'no-shadow') {
        $('#switchbtn-noshadow').prop('checked', true);
    }

    //centerlogo 
    if (html.getAttribute('data-logo') === 'centerlogo') {
        $('#switchbtn-centerlogo').prop('checked', true);
    }

    //vertical menus
   
    let verticalStyles = html.getAttribute('data-vertical-style');
    switch (verticalStyles) {
        case 'default':
            $('#switchbtn-defaultmenu').prop('checked', true);
            break;
        case 'closed':
            $('#switchbtn-closed').prop('checked', true);
            break;
        case 'icontext':
            $('#switchbtn-text').prop('checked', true);
            break;
        case 'overlay':
            $('#switchbtn-overlay').prop('checked', true);
            break;
        case 'hover':
            $('#switchbtn-hoversub').prop('checked', true);
            break;
        case 'hover1':
            $('#switchbtn-hoversub1').prop('checked', true);
            break;
        case 'doublemenu':
            $('#switchbtn-doublemenu').prop('checked', true);
            break;
        case 'doublemenu-tabs':
            $('#switchbtn-doublemenu-tabs').prop('checked', true);
            break;
        default:
            $('#switchbtn-defaultmenu').prop('checked', true);
            break;

    }
}

const handleThemeUpdate = (cssVars) => {
    const root = document.querySelector(':root');
    const keys = Object.keys(cssVars);
    keys.forEach(key => {
        root.style.setProperty(key, cssVars[key]);
    });
}

// to check the value is hexa or not
const isValidHex = (hexValue) => /^#([A-Fa-f0-9]{3,4}){1,2}$/.test(hexValue)

const getChunksFromString = (st, chunkSize) => st.match(new RegExp(`.{${chunkSize}}`, "g"))
// convert hex value to 256
const convertHexUnitTo256 = (hexStr) => parseInt(hexStr.repeat(2 / hexStr.length), 16)
// get alpha value is equla to 1 if there was no value is asigned to alpha in function
const getAlphafloat = (a, alpha) => {
    if (typeof a !== "undefined") { return a / 255 }
    if ((typeof alpha != "number") || alpha < 0 || alpha > 1) {
        return 1
    }
    return alpha
}
// convertion of hex code to rgba code
function hexToRgba(hexValue, alpha) {
    if (!isValidHex(hexValue)) { return null }
    const chunkSize = Math.floor((hexValue.length - 1) / 3)
    const hexArr = getChunksFromString(hexValue.slice(1), chunkSize)
    const [r, g, b, a] = hexArr.map(convertHexUnitTo256)
    return `rgba(${r}, ${g}, ${b}, ${getAlphafloat(a, alpha)})`
}

// convertion of hex code to rgb code
function hexToRgb(hexValue) {
    if (!isValidHex(hexValue)) { return null }
    const chunkSize = Math.floor((hexValue.length - 1) / 3)
    const hexArr = getChunksFromString(hexValue.slice(1), chunkSize)
    const [r, g, b] = hexArr.map(convertHexUnitTo256)
    return `${r}, ${g}, ${b}`
}

function themeSwitch(switchProperty) {
    switchProperty.forEach((item) => {
        item.addEventListener('click', (e) => {
            const primaryColor = e.target.getAttribute('data-bg-color')
            const primaryHoverColor = e.target.getAttribute('data-bg-hover')
            const primaryBorderColor = e.target.getAttribute('data-bg-border')
            const primaryTransparent = e.target.getAttribute('data-bg-transparent')

            handleThemeUpdate({
                '--primary-bg-color': primaryColor,
                '--primary-bg-hover': primaryHoverColor,
                '--primary-bg-border': primaryBorderColor,
                '--primary-transparentcolor': primaryTransparent,
            });

            $("input.input-color-picker[data-id='bg-color']").val(primaryColor)
            $("input.input-color-picker[data-id1='bg-hover']").val(primaryColor)
            $("input.input-color-picker[data-id2='bg-border']").val(primaryColor)
            $("input.input-color-picker[data-id3='transparentcolor']").val(primaryColor)
        });
    });
}

function dynamicPrimaryColor(primaryColor) {
    primaryColor.forEach((item) => {
        item.addEventListener('input', (e) => {
            const cssPropName = `--primary-${e.target.getAttribute('data-id')}`;
            const cssPropName1 = `--primary-${e.target.getAttribute('data-id1')}`;
            const cssPropName2 = `--primary-${e.target.getAttribute('data-id2')}`;
            const cssPropName3 = `--primary-${e.target.getAttribute('data-id3')}`;
            handleThemeUpdate({
                [cssPropName]: hexToRgba(e.target.value),
                // 95 is used as the opacity 0.95
                [cssPropName1]: hexToRgba(e.target.value, 0.99),
                [cssPropName2]: hexToRgba(e.target.value, 0.2),
                [cssPropName3]: hexToRgba(e.target.value, 0.2),
            });
        });
    });
}
function dynamicBackgroundColor(bgColor) {
    bgColor.forEach((item) => {
        item.addEventListener('input', (e) => {
            const cssPropName5 = `--${e.target.getAttribute('data-id5')}`;
            const cssPropName6 = `--${e.target.getAttribute('data-id6')}`;
            const cssPropName7 = `--${e.target.getAttribute('data-id7')}`;
            const cssPropName8 = `--${e.target.getAttribute('data-id8')}`;
            handleThemeUpdate({
                [cssPropName5]: hexToRgba(e.target.value, 0.9),
                // 95 is used as the opacity 0.95
                [cssPropName6]: e.target.value,
                [cssPropName7]: e.target.value,
                [cssPropName8]: e.target.value,
            });
            if (!document.body.classList.contains('auth-page')) {
                let mainHeader = document.querySelector('.app-header');
                mainHeader.setAttribute('style', `--header-bg: ${e.target.value}`)
                let appSidebar = document.querySelector('.app-sidebar');
                appSidebar.setAttribute('style', `--menu-bg: ${e.target.value}`)
            }
        });
    });
    
}



const hex2rgb = (hex) => {
    const r = parseInt(hex.slice(1, 3), 16)
    const g = parseInt(hex.slice(3, 5), 16)
    const b = parseInt(hex.slice(5, 7), 16)
    // return {r, g, b} // return an object
    return [ r, g, b ]
}
function dynamicPrimaryColor(primaryColor) {
    primaryColor.forEach((item) => {
        item.addEventListener('input', (e) => {
            document.querySelector('html').style.setProperty('--primary-bg-color-rgb', hex2rgb(e.target.value)) ;
        });
    });
}
function dynamicBackground(backgroundColor) {
    backgroundColor.forEach((item) => {
        item.addEventListener('input', (e) => {
            document.querySelector('html').style.setProperty('--background-rgb', hex2rgb(e.target.value)) ;
        });
    });
}

function changePrimaryColor() {
    var userColor = document.getElementById('colorID').value;
    localStorage.setItem('dashlotprimaryColor', hex2rgb(userColor));
    names()
}
function transparentBgColor() {
    var userColor1 = document.getElementById('transparentBgColorID').value;
    document.querySelector("#switchbtn-darkheader").checked = true
    localStorage.setItem('dashlotBackground', hex2rgb(userColor1));
    names()
        let html = document.querySelector('html');
        html.setAttribute('data-theme-color', 'dark');
        html.setAttribute('data-menu-style', 'dark');
        html.setAttribute('data-header-style', 'dark');
        $('#switchbtn-dark').prop('checked', true);
}

// chart colors
let myVarVal,primaryRGB
function names() {
    'use strict'
    primaryRGB = getComputedStyle(document.documentElement).getPropertyValue('--primary-bg-color').trim();
    myVarVal = localStorage.getItem("dashlotprimaryColor") || primaryRGB;

    if (document.querySelector('#totalRevenueChart') !== null) {
        totalRevenueChart();
    }

    if (document.querySelector('#crypto') !== null) {
        var chart = new ApexCharts(document.querySelector("#crypto"), options);
        chart.render();
        chart.updateOptions({ 
            plotOptions: {
                candlestick: {
                    colors: {
                        upward: "rgb(" + myVarVal + ")",
                        downward: "#fdc530"
                    }
                }
            },
        })
        // cryptoCurrency();
    }

    if (document.querySelector('#ecommerceChart') !== null) {
        ecommerceChart();
    }
}
names()


function localStorageBackup() {
    // if there is a value stored, update color picker and background color
    // Used to retrive the data from local storage


    if (localStorage.dashlotprimaryColor) {
        if (document.getElementById('colorID')) {
            document.getElementById('colorID').value = localStorage.dashlotprimaryColor;
        }
         document.querySelector('html').style.setProperty('--primary-bg-color-rgb', localStorage.dashlotprimaryColor);
    }
    if(localStorage.dashlotBackground) {
        if (document.getElementById('transparentBgColorID')) {
            document.getElementById('transparentBgColorID').value = localStorage.dashlotBackground;
        }
        document.querySelector('html').style.setProperty('--background', "rgba(" + localStorage.dashlotBackground + ",0.9)");
        document.querySelector('html').style.setProperty('--white', "rgb(" + localStorage.dashlotBackground + ")");
        document.querySelector('html').style.setProperty('--header-bg', "rgb(" + localStorage.dashlotBackground + ")");
        document.querySelector('html').style.setProperty('--menu-bg', "rgb(" + localStorage.dashlotBackground + ")");
        let html = document.querySelector('html');
        html.setAttribute('data-theme-color', 'dark');
        html.setAttribute('data-menu-style', 'dark');
        html.setAttribute('data-header-style', 'dark');
        $('#switchbtn-dark').prop('checked', true);
    }
    if (localStorage.dashlotdarktheme) {
        let html = document.querySelector('html');
        html.setAttribute('data-theme-color', 'dark');
    }
    if (localStorage.dashlotrtl) {
        let html = document.querySelector('html');
        html.setAttribute('dir', 'rtl');
    }
    if (localStorage.dashlotlayout) {
        let html = document.querySelector('html');
        let layoutValue = localStorage.getItem('dashlotlayout');
        html.setAttribute('data-layout', 'horizontal');
        switch (layoutValue) {
            case 'horizontal':
                html.setAttribute('data-hor-style', 'hor-click');
                break;
            case 'horizontalhover':
                html.setAttribute('data-hor-style', 'hor-hover');
                break;
        }
    }
    if (localStorage.dashlotverticalstyles) {
        let html = document.querySelector('html');
        let verticalStyles = localStorage.getItem('dashlotverticalstyles');
        if (!(document.body.classList.contains('auth-page'))) {
            switch (verticalStyles) {
                case 'closed':
                    hoverLayoutFn();
                    html.setAttribute('data-vertical-style', 'closed');
                    break;
                case 'icontext':
                    textLayoutFn();
                    html.setAttribute('data-vertical-style', 'icontext');
                    break;
                case 'overlay':
                    hoverLayoutFn();
                    html.setAttribute('data-vertical-style', 'overlay');
                    break;
                case 'hover':
                    hoverLayoutFn();
                    html.setAttribute('data-vertical-style', 'hover');
                    break;
                case 'hover1':
                    html.setAttribute('data-vertical-style', 'hover1');
                    hoverLayoutFn();
                    break;
                case 'doublemenu':
                    html.setAttribute('data-vertical-style', 'doublemenu');
                    doubleLayoutFn();
                    break;
                case 'doublemenu-tabs':
                    html.setAttribute('data-vertical-style', 'doublemenu-tabs');
                    doubleLayoutFn();
                    break;

            }
        }
    }
    if (localStorage.dashlotnoshadow) {
        let html = document.querySelector('html');
        html.setAttribute('data-skins', 'no-shadow');
    }
    if (localStorage.dashlotboxed) {
        let html = document.querySelector('html');
        html.setAttribute('data-width', 'boxed');
    }
    if (localStorage.dashlotscrollable) {
        let html = document.querySelector('html');
        html.setAttribute('data-position', 'scrollable');
    }
    if (localStorage.dashlotcenterlogo) {
        let html = document.querySelector('html');
        html.setAttribute('data-logo', 'centerlogo');
    }
    if (localStorage.dashlotMenu) {
        let html = document.querySelector('html');
        let menuValue = localStorage.getItem('dashlotMenu');
        switch (menuValue) {
            case 'light':
                html.setAttribute('data-menu-style', 'light');
                break;
            case 'dark':
                html.setAttribute('data-menu-style', 'dark');
                break;
            case 'color':
                html.setAttribute('data-menu-style', 'color');
                break;
            case 'gradient':
                html.setAttribute('data-menu-style', 'gradient');
                break;

            default:
                break;
        }
    }
    if (localStorage.dashlotHeader) {
        let html = document.querySelector('html');
        let headerValue = localStorage.getItem('dashlotHeader');
        switch (headerValue) {
            case 'light':
                html.setAttribute('data-header-style', 'light');
                break;
            case 'dark':
                html.setAttribute('data-header-style', 'dark');
                break;
            case 'color':
                html.setAttribute('data-header-style', 'color');
                break;
            case 'gradient':
                html.setAttribute('data-header-style', 'gradient');
                break;

            default:
                break;
        }
    }
}