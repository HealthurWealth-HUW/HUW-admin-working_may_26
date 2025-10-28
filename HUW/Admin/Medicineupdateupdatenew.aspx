<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeFile="Medicineupdateupdatenew.aspx.cs" Inherits="Admin_Medicineupdateupdatenew" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript" src="Orders_files/jquery_006.js"></script>
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <!-- bootstrap -->
    <link rel="stylesheet" href="Orders_files/bootstrap.min.css" />

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
<script src="js/custom.js" type="text/javascript"></script>
     <script type="text/javascript">
        function updatestatus(selected) {
            var r = confirm("Are you sure to change status")
            if (r == true) {
                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    url: '../api/Master/Updatestatus?status=' + selected.value + '&orderid=' + $('#txtid').html(),
                    type: 'post',
                    dataType: 'json',
                    success: function (data) {
                        window.location.href = "../Admin/prescriptionupdatenew.aspx?transid="+$('#txtid').html();
                    },
                    error:
                    {
                        //Show error message
                    }
                });
            }
        }
    </script>
    <link href="../css/magnific-popup.css" rel="stylesheet" />
    <script src="../js/jquery.magnific-popup.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.popup-gallery').magnificPopup({
                delegate: 'a',
                type: 'image',
                tLoading: 'Loading image #%curr%...',
                mainClass: 'mfp-img-mobile',
                gallery: {
                    enabled: true,
                    navigateByImgClick: true,
                    preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
                }
            });
        });
        </script>
     </asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   
    <script type="text/javascript">
        
    </script>
    <div id="divOrdersOverviewGrd"></div>
   
    <!-- prescription image -->
    <%-- <div class="widget clearfix mt-2">
        <div class="widget_title">
            <span id="spnBillAddress" class="iconsweet">r</span>
            <h5>Prescription images
            </h5>
        </div>
        <div class="row popup-gallery pl-2" id="getimageshere">
        </div>

    </div>
    <div class="widget clearfix mt-2">
        <div class="widget_title">
            <span id="something" class="iconsweet">r</span>
            <h5>details
            </h5>
        </div>
        <div class="row popup-gallery pl-2" id="getdetailshere">
        </div>

    </div>--%>
    <!-- add product section -->
     <div class="widget">
        <div class='widget_title'>
            <h5>
                <label id='Label1'><b>&nbsp;&nbsp;&nbsp;&nbsp;Update Medicine Details</b></label>
            </h5>
        </div>
    </div>
  <div class="widget_body clearfix col-12" style="display: table;">
        <div>
            <!-- bootstrap form -->
            <div class="col-lg-6 pr-3 pl-3 mt-4 left-half">
                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Order ID:</label>
                        <asp:Label ID="txtid" runat="server" CssClass="text-success font-weight-bold"></asp:Label>
                    </div>
                </div>




                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Order Items:</label>
                        <asp:Label ID="txtorderitems" runat="server" CssClass="text-black font-weight-bold"></asp:Label>
                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group popup-gallery">
                        <label class="alert-heading">Image:</label>
                        <%--<asp:Image ID="txtimage" runat="server" /><br /><br />--%>
                        <asp:Repeater ID="RptImages" runat="server">
                            <ItemTemplate>
                                <a href="<%# Container.DataItem %>">
                                    <asp:Image ID="Img" runat="server" ImageUrl='<%# Container.DataItem %>' Style="width: 90px;" />
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>



                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Delivery Type:</label>
                        <asp:Label ID="txtdeliverytype" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading text-success font-weight-bold">Change Status:</label>
                        <asp:DropDownList ID="DDlstatus" runat="server" CssClass="form-control">
                            <asp:ListItem Enabled="true" Text="Select Status" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Approve" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Ready for pickup" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Dispatched" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Delivered" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Reject" Value="6"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


            </div>

            <div class="col-lg-6 pr-3 pl-3 mt-4 right-half">
                 <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Date:</label>
                        <asp:Label ID="Date" runat="server" CssClass="text-black font-weight-bold"></asp:Label>

                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Name:</label>
                        <asp:Label ID="txtName" runat="server" CssClass="text-black font-weight-bold"></asp:Label>
                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Address:</label>
                        <asp:Label ID="txtaddress" runat="server" CssClass="text-black font-weight-bold" Style="padding-right: 20px; word-break: break-all;"></asp:Label>
                    </div>
                </div>

                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Mobile:</label>
                        <asp:Label ID="txtMobile" runat="server" CssClass="text-black font-weight-bold"></asp:Label>
                    </div>
                </div>

                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Locality:</label>
                        <asp:Label ID="txtlocality" runat="server" CssClass="text-black font-weight-bold"></asp:Label>
                    </div>
                </div>

                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Pincode:</label>
                        <asp:Label ID="txtpincode" runat="server" CssClass="text-black font-weight-bold"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="cleafix col-12" style="width:100%;display:table;"></div>
            <div>
                <div class="col-lg-6 left-half">
                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Bill No:</label>
                        <asp:TextBox ID="txtbillno" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                </div>
                <div class="col-lg-6 right-half">
                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Discount (In Percentage):</label>
                       <asp:TextBox ID="txtdiscountamount" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                </div>
                <div class="col-lg-6 left-half">
                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Payment Mode:</label>
                        <asp:TextBox ID="txtpaymentmode" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                </div>
                <div class="col-lg-6 right-half">
                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Delivered By:</label>
                        <asp:TextBox ID="txtdeliveryby" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                </div>
                <div class="col-lg-6 left-half">
                <div class="col-12">
                    <div class="form-group">
                        <label class="alert-heading">Admin Comments:</label>
                        <textarea id="Admincomments" runat="server" cols="20" rows="4" placeholder="Write comments here..." style="width: 100%;"></textarea>
                    </div>
                </div>
                </div>
              <div class="col-lg-6 right-half">
                   <div class="col-12">
                       <div class="form-group">
                           <label class="alert-heading">User Comments:</label>
                <textarea id="Usercomments" runat="server" cols="20" rows="4"  placeholder="Write comments here..." style="width:100%;"></textarea>
                        </div>
                       </div>
                  </div>
                <div class="col-md-6 form-group left-half">
                    <div class="col-12">
                           <label class="alert-heading">Amount:</label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                     </div>
            </div>
            
                <div class="col-lg-12" style="display:table;">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Button runat="server" Text="Update" ID="btn_Submit" CssClass="btn btn-primary mt-2" OnClick="btn_Submit_Click" />
                        </div>
                    </div>
                </div>
            </div>
        <div class="form-group col-md-6">
        </div>
        <div class="form-group col-md-2">
        </div>
    </div>
    <script>

</script>
    <style>
        .textb {
            width: 120px;
        }
         label, span{
            font-size: 14px;
        }
         .left-half, .right-half {
            width: 50%;
            float: left;
        }
    </style>
     </asp:Content>
