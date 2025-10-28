<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ProductAnalysis.aspx.cs" Inherits="Admin_ProductAnalysis" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:Chart ID="Chart1" runat="server" Width="500" Height="300">
    <Series>
        <asp:Series Name="Series1" ChartType="Column">
        </asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
            <AxisX Title="Months">
            </AxisX>
            <AxisY Title="Product Count">
            </AxisY>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>--%>
<div class="row py-4 mt-3 mb-4 bg-light card me-4 shadow" style="flex-direction: inherit;">

 <div class="col-xl-6"><asp:Label ID="lblname" runat="server" style="color:#ff5f68;font-weight:bold;font-size:16px;"></asp:Label></div>
<div class="col-xl-6">
 <div class="form-check form-check-inline">
<div class="d-flex align-items-center">
  <input class="form-check-input h5" type="radio" value="3" onclick="reload(3)" name="inlineRadioOptions" id="inlineRadio1" value="option1" >
  <span class="form-check-label h5 mb-0" for="inlineRadio1">&nbsp;3 Months</span>
</div>
</div>
<div class="form-check form-check-inline">
<div class="d-flex align-items-center">
  <input class="form-check-input h5" type="radio"  value="6"  onclick="reload(6)" name="inlineRadioOptions" id="inlineRadio2" value="option2">
  <span class="form-check-label h5 mb-0" for="inlineRadio2">&nbsp;6 Months</span>
</div>
</div>
<div class="form-check form-check-inline">
<div class="d-flex align-items-center">
  <input class="form-check-input h5" type="radio"  value="12"  onclick="reload(12)" name="inlineRadioOptions" id="inlineRadio3" value="option3">
  <span class="form-check-label h5 mb-0" for="inlineRadio3">&nbsp;12 Months</span>
</div>
</div>
</div>
</div>

    <asp:Chart runat="server" ID="Chart1"  Width="800">
        <Series>
            <asp:Series Name="Series1" ChartType="Column"></asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            <AxisX Title="Months">
            </AxisX>
            <AxisY Title="Product Count">
            </AxisY>
        </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
<script>
function reload(months){
 var currentUrl = window.location.href;

    // Create a URL object to work with URL parameters
    var url = new URL(currentUrl);

    // Set the new parameter value
    url.searchParams.set("mn", months);

    // Reload the page with the updated URL
    window.location.href = url.toString();
}
function checkRadioButtonBasedOnURLParam() {

    var currentUrl = window.location.href;
    var url = new URL(currentUrl);
    var paramValue = url.searchParams.get('mn'); // Replace 'parameterName' with your parameter name

    if (paramValue) {
        var radioButtons = document.querySelectorAll('input[type=radio][name=inlineRadioOptions]');
        radioButtons.forEach(function(radioButton) {
            if (radioButton.value === paramValue) {
                radioButton.checked = true;
            }
        });
    }
}
checkRadioButtonBasedOnURLParam();
</script>
</asp:Content>

