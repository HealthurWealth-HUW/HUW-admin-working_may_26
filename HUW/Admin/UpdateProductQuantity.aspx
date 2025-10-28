<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="UpdateProductQuantity.aspx.cs" Inherits="Admin_UpdateProductQuantity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script src="js/custom.js" type="text/javascript"></script>
<script type="text/javascript">
    SubProductCostCaluclations().empty;
    SubProductCostCaluclations();

    CostCaluclations().empty;
    CostCaluclations();
</script>
    <script type="text/javascript">
        UpdteQtyClick();
    </script>
     
<div id ="UpdateQtyTable">
</div>
 <div>
<asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="true"
            EnablePartialRendering="true" />

            <div>
                <asp:Label id="lblMsg" runat="server" />
            </div>
<table>
<tr>
<td> Product Id:</td>
<td>
    <asp:TextBox runat="server" ID="txtProductId" readonly="true"/></td>
</tr>

<tr>
<td> Product Quantity:</td>
<td>
    <asp:TextBox runat="server" ID="txtProductQuantity" ClientIDMode="Static" /></td>
</tr>
</table>
</div>

 <div class="section">
        <div class="">
            <div class="">
                <asp:Label ID="lblSubprdctsTitle" Text="Sub Products" runat="server" />
               <span class="hide"></span>
            </div>
            <div class="content">
                <div class="row">
                   <div class="right">                  
                        <asp:UpdatePanel ID="udpSubProducts" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:GridView ID="GVSubProducts" runat="server" ClientIDMode="Static" AutoGenerateColumns="False" ShowFooter="True" >
                                      <Columns>
                                      <asp:TemplateField HeaderText="SubProductId">
                                            <ItemTemplate>
                                            <asp:TextBox runat="server" Style="height: 25px;" readonly="true" ID="txtSubProductId" Text='<%#Eval("SubProductId") %>' ></asp:TextBox>
                                               </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="SPName">
                                            <ItemTemplate>
                                            <asp:HiddenField  runat="server"  ID="hdnSubProductId" Value ='<%#Eval("SubProductId") %>' />
                                                <asp:TextBox runat="server" Style="height: 25px;"  ID="txtSPName" Text='<%#Eval("SPName") %>' readonly="true" ></asp:TextBox>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" Style="height: 25px;"  ID="txtSubProductQuantity" Text='<%#Eval("Quantity") %>'  ClientIDMode="Static" onkeyup="SubProductCostCaluclations();" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                       
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

 <div>
        <asp:Button ID="btnUpdateProduct" CssClass="orange" Text="Update" runat="server" OnClick = "btnUpdateProduct_Click" />
          </div>
        
</asp:Content>

