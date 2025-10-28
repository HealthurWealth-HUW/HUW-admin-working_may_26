<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AddEmployee.aspx.cs" Inherits="Admin_AddEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css" />

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <style type="text/css">
        .toolti-inner {
            max-width: 200px;
            padding: .25rem .5rem;
            color: #fff;
            text-align: center;
            background-color: #000;
            border-radius: .25rem;
        }

        .toolti .arrow {
            position: absolute;
            display: block;
            width: .8rem;
            height: .4rem;
            left: 46px;
            bottom: 0;
        }

            .toolti .arrow::before {
                position: absolute;
                content: "";
                border-color: transparent;
                border-style: solid;
                top: 4px;
                border-width: .4rem .4rem 0;
                border-top-color: #000;
            }

        .toolti {
            position: absolute;
            z-index: 1070;
            display: block;
            margin: 0;
            top: -16px;
            right: 55px;
            font-family: -apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
            font-style: normal;
            font-weight: 400;
            line-height: 1.5;
            text-align: left;
            text-align: start;
            text-decoration: none;
            text-shadow: none;
            text-transform: none;
            letter-spacing: normal;
            word-break: normal;
            word-spacing: normal;
            white-space: normal;
            line-break: auto;
            font-size: 11px;
            word-wrap: break-word;
            opacity: 0;
        }

        .form_fields_container {
            width: 88%;
        }

        table tbody td {
            border: 0
        }

        table tbody tr {
            float: left;
            min-width: 185px
        }
    </style>
    <script type="text/javascript">
        function myfunction() {

            if ($('#ContentPlaceHolder1_txtfirst').val().trim() == '') {

                $('#ContentPlaceHolder1_txtfirst').css({ 'border': '1px solid #d41723' });
                $("#tstreet1").show();
                $("#tstreet1").css({ 'opacity': '1' });
            }
            if ($('#ContentPlaceHolder1_txtsecond').val().trim() == '') {

                $('#ContentPlaceHolder1_txtsecond').css({ 'border': '1px solid #d41723' });
                $("#tstreet2").show();
                $("#tstreet2").css({ 'opacity': '1' });
            }

            if ($('#ContentPlaceHolder1_txtemail').val().trim() == '') {

                $('#ContentPlaceHolder1_txtemail').css({ 'border': '1px solid #d41723' });
                $("#tstreet3").show();
                $("#tstreet3").css({ 'opacity': '1' });

            }
            if ($('#ContentPlaceHolder1_txtpass').val().trim() == '') {

                $('#ContentPlaceHolder1_txtpass').css({ 'border': '1px solid #d41723' });
                $("#tstreet4").show();
                $("#tstreet4").css({ 'opacity': '1' });

            }
            if ($('#ContentPlaceHolder1_txtmbl').val().trim() == '') {

                $('#ContentPlaceHolder1_txtmbl').css({ 'border': '1px solid #d41723' });
                $("#tstreet5").show();
                $("#tstreet5").css({ 'opacity': '1' });
            }
            if ($('#ContentPlaceHolder1_txtfirst').val().trim() != '' && $('#ContentPlaceHolder1_txtsecond').val().trim() != '' && $('#ContentPlaceHolder1_txtemail').val().trim() != '' && $('#ContentPlaceHolder1_txtpass').val().trim() != '' && $('#ContentPlaceHolder1_txtmbl').val().trim() != '') {

                $('#ContentPlaceHolder1_step2').show().animate({
                    'marginLeft': "-=30px",//moves left
                    'marginRight': "+=15px"//moves right
                });
                $('#ContentPlaceHolder1_step1').hide();

            }
            if ($('#ContentPlaceHolder1_txtfirst').val().trim() != '') {
                $('#ContentPlaceHolder1_txtfirst').css({ 'border': '1px solid #ccc' });
                $("#tstreet1").css({ 'opacity': '0' });
            }
            if ($('#ContentPlaceHolder1_txtsecond').val().trim() != '') {

                $('#ContentPlaceHolder1_txtsecond').css({ 'border': '1px solid #ccc' });
                $("#tstreet2").css({ 'opacity': '0' });
            }
            if ($('#ContentPlaceHolder1_txtemail').val().trim() != '') {

                $('#ContentPlaceHolder1_txtemail').css({ 'border': '1px solid #ccc' });
                $("#tstreet3").css({ 'opacity': '0' });
            }
            if ($('#ContentPlaceHolder1_txtpass').val() != '') {

                $('#ContentPlaceHolder1_txtpass').css({ 'border': '1px solid #ccc' });

                $("#tstreet4").css({ 'opacity': '0' });
            }
            if ($('#ContentPlaceHolder1_txtmbl').val() != '') {

                $('#ContentPlaceHolder1_txtmbl').css({ 'border': '1px solid #ccc' });

                $("#tstreet5").css({ 'opacity': '0' });
            }

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="step1" runat="server">
        <div id="" class="">
            <div class="one_wrap " id="" style="">
                <div id="" class="widget" style="">
                    <div class="widget_title">
                        <%--<span style="font-family: iconSweets;"></span>--%>
                        <h5>
                            <label id="">
                                <b>&nbsp;&nbsp;&nbsp;Step 1: Add Employee</b>
                            </label>
                        </h5>
                    </div>
                    <label id="lblerror" runat="server" style="color: red; padding: 10px 0;"></label>
                    <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="">

                        <div class="search_contentpad">

                            <div class="" id="">
                                <ul class="form_fields_container">
                                    <li>
                                        <label class="search_label" id="lblPrdctID">First Name </label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtfirst" runat="server" required></asp:TextBox>
                                        </div>
                                        <div class="toolti" id="tstreet1">
                                            <div class="arrow"></div>
                                            <div class="toolti-inner">
                                                Please fill this field
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblstoresku">Last Name</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtsecond" runat="server" required></asp:TextBox>
                                        </div>
                                        <div class="toolti" id="tstreet2">
                                            <div class="arrow"></div>
                                            <div class="toolti-inner">
                                                Please fill this field
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblPrdctID">Email ID</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtemail" runat="server" required></asp:TextBox>
                                        </div>
                                        <div class="toolti" id="tstreet3">
                                            <div class="arrow"></div>
                                            <div class="toolti-inner">
                                                Please fill this field
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblstoresku">Password</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtpass" runat="server" required TextMode="Password"></asp:TextBox>
                                        </div>
                                        <div class="toolti" id="tstreet4">
                                            <div class="arrow"></div>
                                            <div class="toolti-inner">
                                                Please fill this field
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblstoresku">MobileNo</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtmbl" runat="server" required></asp:TextBox>
                                        </div>
                                        <div class="toolti" id="tstreet5">
                                            <div class="arrow"></div>
                                            <div class="toolti-inner">
                                                Please fill this field
                                            </div>
                                        </div>
                                    </li>

                                    <div class="globalaction_bar text_right">
                                        <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="">*</span>
                                        <asp:Button ID="con" Style="margin-top: 2px;" class="button_small greyishBtn" runat="server" OnClientClick="myfunction(); return false;" Text="Continue to Step 2" />
                                    </div>
                                </ul>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>

    <div id="step2" runat="server" style="display: none; margin-left: 30px; margin-right: 0;">
        <div id="" class="">
            <div class="one_wrap " id="divSearchResult" style="">
                <div id="" class="widget" style="">
                    <div class="widget_title">
                        <%--<span style="font-family: iconSweets;"></span>--%>
                        <h5>
                            <label id="ctl00_ctl00_Main_Main_lblProductList">
                                <b>&nbsp;&nbsp;&nbsp;Step 2: Permission Restrictions</b>
                            </label>
                        </h5>
                    </div>

                    <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="StoreTab">

                        <div class="search_contentpad">

                            <div class="margin_10" id="margin_10">
                                <ul class="form_fields_container">
                                    <li>
                                        <asp:CheckBoxList ID="chkaccess" runat="server"></asp:CheckBoxList>
                                    </li>

                                    <div class="globalaction_bar text_right">
                                        <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="ctl00_ctl00_Main_Main_SelectProductUserControl_CustomValidator1">*</span>
                                        <%--<input type="submit" class="button_small greyishBtn" id="btnSearch" onclick=" return makeSearch();" value="Search" name="btnSearch">--%>
                                        <asp:Button ID="btnSavedetails" Style="margin-top: 2px;" class="button_small greyishBtn" runat="server" Text="Save" OnClick="btnSavedetails_Click" />
                                    </div>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>





    <%--<h1>Add Employee</h1>
    <div>
        <asp:Label ID="lblfirst" runat="server" Text="Firstname:"></asp:Label>
        <asp:TextBox ID="txtfirst" runat="server" required Style="width: 420px;"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="lblsecond" runat="server" Text="Lastname:"></asp:Label>
        <asp:TextBox ID="txtsecond" runat="server" required Style="width: 420px;"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="lblemail" runat="server" Text="EmailID:"></asp:Label>
        <asp:TextBox ID="txtemail" runat="server" required Style="width: 420px;"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="lblpass" runat="server" Text="Password:"></asp:Label>
        <asp:TextBox ID="txtpass" runat="server" required TextMode="Password" Style="width: 420px;"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="lblmbl" runat="server" Text="MobileNo:"></asp:Label>
        <asp:TextBox ID="txtmbl" runat="server" required Style="width: 420px;"></asp:TextBox>
    </div>
    <h1>Permissions Restriction For pages</h1>
    <asp:CheckBoxList ID="chkaccess" runat="server"></asp:CheckBoxList>

    <asp:Button ID="btnSavedetails" Style="margin-top: 2px;" class="button_small greyishBtn" runat="server" Text="Save" OnClick="btnSavedetails_Click" />--%>
</asp:Content>
