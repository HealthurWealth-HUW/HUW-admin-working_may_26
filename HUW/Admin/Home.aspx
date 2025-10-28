<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_Home" CodeFile="Home.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <link rel="Stylesheet" href="//cdn.datatables.net/1.10.21/css/jquery.dataTables.min.css" />
 <!-- bootstrap -->
    <%--<link rel="stylesheet" href="Orders_files/bootstrap.min.css" />--%>
 <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
<script src="js/custom.js" type="text/javascript"></script>
<script type="text/javascript" src="//cdn.datatables.net/1.10.21/js/jquery.dataTables.min.js"></script>
    <style>
        #activity_stats h1 {
            margin-bottom: 20px;
            border-bottom: solid 1px #cdd1d3;
            color: #3c4143;
            font-size: 22px;
            font-family: 'CorbelBold';
            padding-bottom: 6px;
        }

        h1 {
            color: #6c6c6c;
            font-size: 16px;
        }
        .table tbody th
        {
                background: #0f7a80;
                color: #fff;
                text-align: center;
                font-weight: 100;
                font-size: 14px;
                padding: 11px 0px;
        }
        .table tbody td
        {
            text-align:center;
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#gridViewTasks").prepend( $("<thead></thead>").append( $(this).find("tr:first") ) ).DataTable() ;} );
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <div class="row" id="content_wrap">
        <div id="cp_placeholder">
            <div id="activity_stats">
                <h1>Statistics</h1>
            </div>
        </div>
        <div class="col-lg-12 col-md-12">
            <h1>Order Process</h1>
            <div class="row top-summary">


                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>AUTHORIZED</b> ORDERS</p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000"><a href="AuthorizedOrders.aspx">
                                    <asp:Label ID="lblAuthorizedOrders" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>PICKUP</b> ORDERS</p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000"><a href="PickupOrders.aspx">
                                    <asp:Label ID="lblPickupOrders" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>In Packing</b> ORDERS</p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000"><a href="Inpacking.aspx">
                                    <asp:Label ID="lblinpacking" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>DISPATCHED</b> ORDERS</p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000"><a href="DispatchedOrders.aspx">
                                    <asp:Label ID="lblDispatchedOrders" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>DELIVERED</b> ORDERS</p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000"><a href="DeliveredOrders.aspx">
                                    <asp:Label ID="lblDeliveredOrders" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <%--<div class="col-lg-3 col-md-6">
						<div class="widget lightblue-1">
							<div class="widget-content padding">
								<div class="widget-icon">

								</div>
								<div class="text-box">
									<p class="maindata">PENDING <b>PRODUCTS</b></p>
									<h2><span class="animate-number" data-value="18648" data-duration="3000"><asp:Label  ID="lbldeliverPendingproducts" runat="server" /></span></h2>
									<div class="clearfix"></div>
								</div>
							</div>
							<div class="widget-footer">
								<div class="row">
									<div class="col-sm-12">
										<i class="fa fa-caret-up rel-change"></i> <b></b>
									</div>
								</div>
								<div class="clearfix"></div>
							</div>
						</div>
					</div>--%>
            </div>

            <h1 id="hdrOrderStatistics" runat="server">Order Statistics</h1>
            <div class="row top-summary col-offset-2" id="divOrderStatistics" runat="server">
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata">USER DROPPED <b>ORDERS</b></p>
                                <h2><span class="animate-number" data-value="25153" data-duration="3000"><a href="UserCancelledOrders.aspx">
                                    <asp:Label ID="lblUserCancelledOrders" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata">ADMIN CANCELED <b>ORDERS</b></p>
                                <h2><span class="animate-number" data-value="25153" data-duration="3000"><a href="AdminCancelledOrders.aspx">
                                    <asp:Label ID="lblAdminCancelledOrders" runat="server" Text="0" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata">UN-AUTHORIZED <b>ORDERS</b></p>
                                <h2><span class="animate-number" data-value="25153" data-duration="3000"><a href="Un-AuthorizedOrders.aspx">
                                    <asp:Label ID="lblUnauthorizedorders" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
            <h1>Product Statistics</h1>
            <div class="row top-summary col-offset-2">
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata">TOTAL <b>PRODUCTS</b></p>
                                <h2><span class="animate-number" data-value="25153" data-duration="3000">
                                    <a href="ProductSearch.aspx">
                                        <asp:Label ID="lblTotalProducts" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>ACTIVE</b> PRODUCTS</p>
                                <h2><span class="animate-number" data-value="6399" data-duration="3000">
                                    <a href="ProductSearch.aspx?Range=Active">
                                        <asp:Label ID="lblActiveProducts" runat="server" /></a></span></h2>

                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-down rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>Deleted</b> PRODUCTS</p>
                                <h2><span class="animate-number" data-value="70389" data-duration="3000">
                                    <a href="ProductSearch.aspx?Range=InActive">
                                        <asp:Label ID="lbldeletedProducts" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-down rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>In-ACTIVE</b> PRODUCTS</p>
                                <h2><span class="animate-number" data-value="70389" data-duration="3000">
                                    <a href="ProductSearch.aspx?Range=InActive">
                                        <asp:Label ID="lblinactive" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-down rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>


                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>OUT OF STOCK</b> PRODUCTS</p>
                                <h2><span class="animate-number" data-value="70389" data-duration="3000">
                                    <a href="ProductSearch.aspx?Range=Outofstock">
                                        <asp:Label ID="lblOutOfStockProducts" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-down rel-change"></i><b></b>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
            <h1 id="hdrUserStatistics" runat="server">User Statistics</h1>
            <div class="row top-summary" id="divUserStatistics" runat="server">
                <div class="col-lg-4 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata">TOTAL <b>USERS</b></p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000">
                                    <asp:Label ID="lblTotalUSers" runat="server" /></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>ALL USERS
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-4 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata">REGISTERED <b>USERS</b></p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000">
                                    <asp:Label ID="lblRegisterUsersmonth" runat="server" /></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>FROM LAST ONE MONTH
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>


                <div class="col-lg-4 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata">REGISTERED <b>USERS</b></p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000">
                                    <asp:Label ID="lblthisweek" runat="server" /></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>FROM LAST ONE WEEK
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
            <h1 id="hdrSaleStatistics" runat="server">Sale Statistics</h1>
            <div class="row top-summary" id="divSaleStatistics" runat="server">
                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>PRODUCT SALES</b></p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000">
                                    <asp:Label ID="lblSalesofthismonth" runat="server" /></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>THIS MONTH
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>


                <div class="col-lg-3 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>PRODUCT SALES</b></p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000">
                                    <asp:Label ID="lblSalesofthisweek" runat="server" /></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>THIS WEEK
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
            <h1 id="hdrAmountStatistics" runat="server">Sale Amount Statistics</h1>
            <div class="row top-summary" id="divAmountStatistics" runat="server">
                <div class="col-lg-6 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>TODAY SALES</b></p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000"><a href="SalesReport.aspx?Range=Today">
                                    <asp:Label ID="lblTodaySales" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>SALES AMOUNT
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-6 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>THIS WEEK</b></p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000"><a href="SalesReport.aspx?Range=Week">
                                    <asp:Label ID="lblsalesweek" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>SALES AMOUNT
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-6 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>THIS MONTH</b></p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000"><a href="SalesReport.aspx?Range=Month">
                                    <asp:Label ID="lblsalesmonth" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>SALES AMOUNT
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-6 col-md-6">
                    <div class="widget lightblue-1">
                        <div class="widget-content padding">
                            <div class="widget-icon">
                            </div>
                            <div class="text-box">
                                <p class="maindata"><b>TOTAL SALES</b></p>
                                <h2><span class="animate-number" data-value="18648" data-duration="3000"><a href="SalesReport.aspx">
                                    <asp:Label ID="lblsalestotal" runat="server" /></a></span></h2>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="widget-footer">
                            <div class="row">
                                <div class="col-sm-12">
                                    <i class="fa fa-caret-up rel-change"></i><b></b>SALES AMOUNT
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>

                      
                    </div>
                </div>
                
            </div>
             <h1 id="h2" runat="server" hidden>Notify List</h1>
             <p>&nbsp;</p>
             <div class="table-responsive" hidden>
                   <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-widget">
                            <Columns>
                                <%--<asp:BoundField ItemStyle-Width="150px" DataField="SchoolId" HeaderText="S.No" />--%>
                                  <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <a href="../../Admin/updateProducts.aspx?ID=<%# Eval("Productid")%>"  class="g"><%# Eval("Productid")%></a>
                                                                </ItemTemplate>
                                      <HeaderTemplate>
                Product id
         </HeaderTemplate>
                                                            </asp:TemplateField>
                                <%--<asp:BoundField ItemStyle-Width="150px" DataField="Productid" HeaderText="Product ID" />--%>
                                <asp:BoundField ItemStyle-Width="150px" DataField="Soldoutdate" HeaderText="Sold out Date" />
                                <asp:BoundField ItemStyle-Width="150px" DataField="ProductName" HeaderText="Product Name" />
                                <asp:BoundField ItemStyle-Width="150px" DataField="Notifymecount" HeaderText="No. Of Request" />
                            
                            </Columns>
                        </asp:GridView>
                 </div>

        </div>
    </div>


</asp:Content>

