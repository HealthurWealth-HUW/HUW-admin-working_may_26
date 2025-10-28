<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invoice.aspx.cs" Inherits="Admin_Invoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script src="js/custom.js" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {
        GenrateInvoice();
    });
</script>
<body>
    <form id="form1" runat="server">
    <div>
   <input type="hidden" id="lblPtrnsId" />
    </div>
    </form>
</body>
</html>
