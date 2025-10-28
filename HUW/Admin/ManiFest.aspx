<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManiFest.aspx.cs" Inherits="Admin_ManiFest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:center;height:50px;">
            <h4>
                <b><u>HEALTH UR WEALTH Manifest</u></b>
            </h4>

        </div>
        <div style="height: 800px">
            <asp:GridView ID="gdvManifest" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="50px" HeaderText="Shipment ID" HeaderStyle-HorizontalAlign="Center" DataField="ShipmentId" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField ItemStyle-Width="100px" HeaderText="Address" HeaderStyle-HorizontalAlign="Center" DataField="Address" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField ItemStyle-Width="25px" HeaderText="Mobile" DataField="MobileNumber" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField ItemStyle-Width="25px" HeaderText="Shipment Date" DataField="ShortDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                    <%--<asp:BoundField ItemStyle-Width="25px" HeaderText="Weight" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />--%>
                    <asp:BoundField ItemStyle-Width="100px" HeaderText="Product" DataField="ProductName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <p>
                For Courier Personnel :
            </p>
            <p>
                I confirm that I have collected the packages as per the AWBs mention on this Manifest.
            </p>
            <table width="100%">
                <tr>
                    <td align="center">
                        <b>Name</b>
                    </td>
                    <td align="center">
                        <b>Signature</b>
                    </td>
                    <td align="center">
                        <b>Date and Time</b>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td align="center">
                        <b>________________</b>
                    </td>
                    <td align="center">
                        <b>_______________</b></td>
                    <td align="center">
                        <b>_______________</b>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
