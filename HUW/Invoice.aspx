<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invoice.aspx.cs" Inherits="Invoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8" />
    <script type="text/javascript" src="catalog/view/theme/leisure/js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="catalog/view/theme/leisure/js/jquery.flexslider.js"></script>
    <script type="text/javascript" src="catalog/view/theme/leisure/js/jquery.jcarousel.js"></script>
    <script type="text/javascript" src="catalog/view/theme/leisure/js/form_elements.js"></script>
    <script type="text/javascript" src="catalog/view/theme/leisure/js/custom.js"></script>
    <link href="Validation/invoice.css" rel="stylesheet" type="text/css" />
    <link href="Validation/stylesheet.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/js/custom.js" type="text/javascript"></script>
    <script src="Scripts/js/jquery-1.6.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Admin/js/custom.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
    <style type="text/css">
        table tr:last-child td:first-child {
            -moz-border-radius-bottomleft: 5px;
            -webkit-border-bottom-left-radius: 5px;
            border-bottom-left-radius: 5px;
        }

        table tr:last-child td:last-child {
            -moz-border-radius-bottomright: 5px;
            -webkit-border-bottom-right-radius: 5px;
            border-bottom-right-radius: 5px;
        }
    </style>

</head>


<script type="text/javascript" language="javascript">
    var sum = 0;
    $(document).ready(function () {
        FillUserTransctionDetails();
        setTimeout(function () {
            var sum = 0;
            var gstsum = 0;
            $('.prices').each(function () {
                var prevClass = $(this).prev().attr('class');
                sum += parseFloat($(this).text());
                $('#producttotal').text(sum.toFixed(2));
            })
            $('.gstprices').each(function () {
                var prevClass = $(this).prev().attr('class');
                gstsum += parseFloat($(this).text());
                $('#gsttotal').text(gstsum.toFixed(2));
            })
        }, 1500);
        setTimeout(function () {
            if ($('#Statename').val() != "Telangana") {
                //$('#IGST').remove();
                //$('#CGST').html('IGST');

            }
        }, 500);
        
        

            
    });
    
    
</script>


<body>
    <div id="divInvoice"></div>
    
</body>
</html>


