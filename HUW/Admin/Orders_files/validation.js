var errormessage;

function emailval(val)
	{
	    var eid;
	    eid = new RegExp('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$');
	    if (!eid.test(val))
	        {
	            errMsg('Invalid email address',val);
	            return false;
	        }
	    else
	        {
	            return true;
	        }
	}
function reqFval(val,msg)
	{
	  if (val.length < 1) 
	    {
	        errMsg(msg,val);
	        return false;
	     } 
	     else 
	     {
			 return true;
	     }
	}
	function monthstr(mnothval)
{
if (mnothval=='Jan')
    {return 1}
else if (mnothval=='Feb')
    {return 2}
else if (mnothval=='Mar')
    {return 3}
else if (mnothval=='Apr')
    {return 4}
else if (mnothval=='May')
    {return 5}
else if (mnothval=='Jun')
    {return 6}
else if (mnothval=='Jul')
    {return 7}
else if (mnothval=='Aug')
    {return 8}
else if (mnothval=='Sep')
    {return 9}
else if (mnothval=='Oct')
    {return 10}
else if (mnothval=='Nov')
    {return 11}
else if (mnothval=='Dec')
    {return 12}
}
function datecompare(val,img,topposition,leftposition)
{
//return true;
//alert('comingdatecompare');
	var val2=txttodate.value;
var date1;
var date2;
var monthtemp1;
var monthtemp2;
var month1;
var month2;
var year1;
var year2;
	if(subreqfieldval(val,'Please enter From  Date',1))
		{
			if(datevalidation(val,'Invalid From  Date',img,topposition,leftposition))
			{
			try
				{

								date1 = val.substring (0, val.indexOf ("-"));
            				monthtemp1 = val.substring (val.indexOf ("-")+1, val.lastIndexOf ("-"));
            				month1=monthstr(monthtemp1);
			            	year1 = val.substring (val.lastIndexOf ("-")+1, val.length);
	                    date2 = val2.substring (0, val2.indexOf ("-"));
                    monthtemp2 = val2.substring (val2.indexOf ("-")+1, val2.lastIndexOf ("-"));
                    month2 = monthstr(monthtemp2);
                  //  alert(month2);
				    year2 = val2.substring (val2.lastIndexOf ("-")+1, val2.length);
			    if (year1 > year2) {errMsg('From date should be greater than To date'); return false;}
			    if (month1 > month2)  {errMsg('From date should be greater than To date'); return false;} 
			    if (date1 > date2)  {errMsg('From date should be greater than To date'); return false;}
        	    else return true;
    				}
				    catch (s)
				    {
    					alert(s);
				    }
			}

		}
}
function Todate(val,iframe)
{
if(subreqfieldval(val,'Please enter To  Date',1))
	{
	if(datevalidation(val,'Invalid To  Date'))
							{
		return true;
							}
	}
}
function datevalidation(val,msg,img,topposition,leftposition)
  {
	try
	{
	 var reg;
	    //reg = new RegExp('^(([1-9])|(0[1-9])|(1[0-2]))\/(([0-9])|([0-2][0-9])|(3[0-1]))\/(([0-9][0-9])|([1-2][0,9][0-9][0-9]))$');
	    //reg = new RegExp('^(([1-9])|(0[1-9])|(1[0-2]))\/((0[1-9])|([1-31]))\/((\d{2})|(\d{4}))$');
        reg = new RegExp('^(0?[1-9]|[12][0-9]|3[01])-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-([0-9]{0,4})$');
        //^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\/](Jan|JAN|Feb|FEB|Mar|MAR|Apr|APR|May|MAY|Jun|JUN|Jul|JUL|Aug|AUG|Sep|SEP|Oct|OCT|Nov|NOV|Dec|DEC)[\/]\d{4}$
	    if (!reg.test(val))
	        {
	            errMsg(msg,val);
	            return false;
	        }
	    else
	        {
	            return true;
	        }
	

		
	}
	catch (e)
	{
		//alert(e);
	}
  

}
function ordernoval(val)
{
if(subreqfieldval(val,'Please enter Order Number',1))
	{

		var ono;
	    ono = new RegExp('^[0-9]{0,20}$');
	    if (!ono.test(val))
	        {
				errMsg('ordernumber should be numeric',val); return false;	        }
	    else
	        {
			////alert('in two order');
	            return true;
	        }
	} 
}
function numeric(val,msg)
{
	var ono;
	    ono = new RegExp('^[0-9]{0,20}$');
	    if (!ono.test(val))
	        {
				errMsg(msg,val); return false;	        }
	    else
	        {
		
	            return true;
	        }
}
function subreqfieldval(val,msg,len)
	{
		////alert('in subreqfieldval'+len+'length='+val.length);
	    if (val.length < parseInt(len)) 
	    {
		    errMsg(msg,val);
	        return false;				
			
	     } 
	     else 
	     {
			 return true;
	     }
	}
function btnclick(divid)
	{
			  return $(divid).jVal();
	}
function errMsg(msg,val)
	{
//	   $("#bubble").html("<div style='width:140px;font-family:Arial,Helvetica,sans-serif;font-size:11px;text-align:left;padding-top:15px;padding-left:25px;color:red;'>"+msg+"</div>"); 
//	   $("#bubble").html("<div style='width:140px;font-family:Arial,Helvetica,sans-serif;font-size:11px;text-align:left;padding-top:15px;padding-left:25px;color:red;'>"+msg+"</div>"); 

//$("#bubble").html("<div class='maindiv'><img src='http://www.sample2.martjack.com/Images/Bubbleimages/close.jpg'  align='right'/><span class='left'>&nbsp;</span>"+msg+"</div>");

//  <span class="right">&nbsp;</span>
errormessage=msg;
	}
function bulkquantity(val)
{
if(subreqfieldval(val,'Please enter the Order Quantity',1))
	{
		if(numeric(val,'Order Quantity should be numeric'))
		{
			  comparebulkquantity(val,lblYAMLQty.value);
		}
    }
}
function comparebulkquantity(val1,val2)
{

    	var idComp1=0;
        var idComp2=0;
        idComp1=parseInt(val1);
        idComp2=parseInt(val2);

            if(idComp1 < idComp2)
            {
				return true;
            }
            else
            {
    				errMsg('Order Quantity should be greater than equal to Bulk Quantity',val);
					return false;
            }
}
function comparestring(par1,par2,msg)
{
	////alert(par1+par2);
        if(par1==par2)
            {
			////alert('1');
				return true;
            }
            else
            {
				//			//alert('2');
    				errMsg(msg);
					return false;
            }
}

function newpasswordcompare(val)
{
		if(subreqfieldval(val,'Password should have mimimun 6 charector length',6))
		{
		var txtConPassword1;
		var txtConPassword1=txtConPassword.value;
////alert(txtConPassword1);
		if (comparestring(val,txtConPassword1,'Confirm password must match password.'))
		{
			return true;
		}
		}

}
function mobileno(val)
{

    if (subreqfieldval(val,'Enter Mobile Number',1))
    {
     var ono;
    ono = new RegExp('^[0-9]{0,13}$');
    if (!ono.test(val))
    {
        errMsg('Enter valid Number',val); return false;	        
    }
    else
    {
        return true;
    }
    }
}



function firstname(val)
{
    if (subreqfieldval(val,'Enter First Name',1))
    {
        var ono;
        ono = new RegExp('^[a-zA-Z]{0,50}$');
        if (!ono.test(val))
        {
            errMsg('Enter Valid First Name',val); return false;	        
        }
        else
        {

            return true;
        }

    }
}

function lastname(val)
{

    if (subreqfieldval(val,'Enter Last Name',1))
    {
        var ono;
        ono = new RegExp('^[a-zA-Z]{0,50}$');
        if (!ono.test(val))
        {
            errMsg('Enter Valid Last Name',val); return false;	        
        }
        else
        {
            return true;
        }

    }
} 

	//		try
//			{
//				month1 = val.substring (0, val.indexOf ("/"));
//				date1 = val.substring (val.indexOf ("/")+1, val.lastIndexOf ("/"));
//				year1 = val.substring (val.lastIndexOf ("/")+1, val.length);
//	            month2 = val2.substring (0, val2.indexOf ("/"));
  //              date2 = val2.substring (val2.indexOf ("/")+1, val2.lastIndexOf ("/"));
//				year2 = val2.substring (val2.lastIndexOf ("/")+1, val2.length);
//			if (year1 > year2) {errMsg('date year'); return false;}
//			if (month1 > month2)  {errMsg('date month'); return false;} 
//			if (date1 > date2)  {errMsg('date date'); return false;}
  //      	else return true;

	//		}
//catch (err)
//{
//alert(err);
//}
 
//	return false;


function numericvalues(val,msg)
{  
	var ono;
	    ono = new RegExp('^[0-9]{0,20}$');
	    
	   
	    if(val.length<1)
	    {
	     errMsg(msg)
	     return false
	    }
	    //if (!ono.test(val))
	     else if(isNaN(val))
	        {
				errMsg(msg,val); return false;	        }
		
	    else
	        {
		
	            return true;
	        }
}


function dropdownvaldite(val,type,msg)
	{
	  if(type=="Country")
	  {
	    if(val==0)
	    {
	       errMsg(msg,val)
	       return false;
	     }
	     else 
	       {
	         return true;
	       }
	   }
	   else if(type=="State")
	   {
	     if(val=="--Select State--")
	     {
	      errMsg(msg,val);
	      return false;
	      }
	      else 
	       {
	         return true;
	       }
	    }
	    else if(type=="City")
	    {
	      if(val=="--Select City--")
	       {
	         errMsg(msg,val);
	         return false;
	       } 
	       else 
	       {
	         return true;
	       }
	     }
	}
	
	
	function zipcodeValidate(val)
	{ 
	  if (val.length < 5) 
	    {
	        errMsg('ZipCode Should be More than 5 Digits' ,val);
	  
	        return false;
	     } 
	  else if(val.length >10)
	  {
	        errMsg('ZipCode Should Not Exceed 10 Digits',val);
	        return false;
	   }
	     else 
	     {
			 return true;
	     }
	}
	
	//return month in integer format
	function getMonthNew(tempMonth) {
	    if (tempMonth != undefined) {
	        var tmpMonth = tempMonth
	        if (tmpMonth.toLowerCase() == 'jan') { tmpMonth = 1; } else if (tmpMonth.toLowerCase() == 'feb') { tmpMonth = 2; } else if (tmpMonth.toLowerCase() == 'mar') { tmpMonth = 3; } else if (tmpMonth.toLowerCase() == 'apr') { tmpMonth = 4; } else if (tmpMonth.toLowerCase() == 'may') { tmpMonth = 5; } else if (tmpMonth.toLowerCase() == 'jun') { tmpMonth = 6; }
	        else if (tmpMonth.toLowerCase() == 'jul') { tmpMonth = 7; } else if (tmpMonth.toLowerCase() == 'aug') { tmpMonth = 8; } else if (tmpMonth.toLowerCase() == 'sep') { tmpMonth = 9; } else if (tmpMonth.toLowerCase() == 'oct') { tmpMonth = 10; } else if (tmpMonth.toLowerCase() == 'nov') { tmpMonth = 11; } else if (tmpMonth.toLowerCase() == 'dec') { tmpMonth = 12; }
	        return tmpMonth
	    }
       
    }
    
//    //validate 2 dates
    function validateDatesNew(fromDate, Todate, dtSeperator, dtformat) {
        var dtSeperator = dtSeperator;
        var dtformat = dtformat;
        var dtFromatSplit = dtformat.split(" ");
        var dayIndex = 0;
        var monthIndex = 0;
        var yearIndex = 0;
        for (var i = 0; i < dtFromatSplit.length; i++) {
            if (dtFromatSplit[i].substring(0, 1).toLowerCase() == 'd')
            { dayIndex = 0; }
            else if (dtFromatSplit[i].substring(0, 1).toLowerCase() == 'm')
            { monthIndex = 1; }
            else if (dtFromatSplit[i].substring(0, 1).toLowerCase() == 'y')
            { yearIndex = 2; }
        }
        var spFmDate = fromDate.split(dtSeperator);
        var spToDate = Todate.split(dtSeperator);
        var fromDay, fromMonth, fromYear, toDay, toMonth, toYear;
        if (fromDate != undefined && fromDate != '') {
            if (Todate != undefined && Todate != '') {
                fromDay = spFmDate[dayIndex];
                fromMonth = spFmDate[monthIndex];
                fromYear = spFmDate[yearIndex];
                toDay = spToDate[dayIndex];
                toMonth = spToDate[monthIndex];
                toYear = spToDate[yearIndex];
            }
        }

        fromMonth = getMonthNew(fromMonth);
        toMonth = getMonthNew(toMonth);
        var myFromDate = new Date();
        var myToDate = new Date();
        if (toYear > fromYear) //comparing year first
        { return true; }
        else if (toYear == fromYear) //comparing year first
        {
            if (toMonth > fromMonth) //comparing month
            { return true; }
            else if (toMonth == fromMonth) //comparing month
            {
                if (toDay >= fromDay) //comparing day
                { return true; }
                else { return false; }
            }
            else { return false; }
        } else { return false; }
    }


    //added by shahid 2-8-2010 prevent chars in textbox
    function OnlyNumbers(e) {

        var isNN = (navigator.appName.indexOf("Netscape") != -1);
        var keyCode = (isNN) ? e.which : e.keyCode;
        if (isNN) {
            if (keyCode == 0)
                return true;
        }
        //keycode 8 is for backspace in mozilla
        if (keyCode == 8) {
            return true;
        }
        if (keyCode > 47 && keyCode < 58) {
            return true;
        }
        else {
            if (e.returnValue) {
                e.returnValue = false;
                return false;
            }
            else if (e.preventDefault) {
                e.preventDefault();
                return false;
            }
            this.event.returnValue = false;
            return false;
        }
    }
