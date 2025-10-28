// JScript File



function ConvertToStandardDate(objDateString,DF,DS)
{
 var arry = DF.toLowerCase();
 var idate;
 var imonth;
 var iyear;
 var MonthName;
 var Date;
 var Year;
 var MonthNumber;
 var ConvertedDate;

    var idate = arry.indexOf("d");
    var imonth = arry.indexOf("m");
    var iyear = arry.indexOf("y");
     
        
          var temp1=objDateString.substring(idate,objDateString.length);
          var temp3= objDateString.substring(imonth,objDateString.length);
          var temp2=objDateString.substring(iyear,objDateString.length);
          
          
          var temp4=arry.substring(imonth,arry.length)
          var MonthFormat=temp4.split(DS);
                   
          MonthName=temp3.split(DS); 
          Date=temp1.split(DS);
          Year=temp2.split(DS);
                                                 
          if(MonthFormat[0]=="MMM" ||MonthFormat[0]=="mmm")
          {
          MonthNumber=GetMonthNumber(MonthName[0]);
          }
          else
          {
          MonthNumber=MonthName[0];
          }
                    
          ConvertedDate=MonthNumber+"/"+Date[0]+"/"+Year[0];
          return ConvertedDate;
}

function GetMonthNumber(objMonth)
{

 var tempMonth;
        if(objMonth=="Jan")
        {
        tempMonth="01";
        }
        if(objMonth=="Feb")
        {
        tempMonth="02";
        }
        if(objMonth=="Mar")
        {
        tempMonth="03";
        }
        if(objMonth=="Apr")
        {
        tempMonth="04";
        }
        if(objMonth=="May")
        {
        tempMonth="05";
        }
        if(objMonth=="Jun")
        {
        tempMonth="06";
        }
        if(objMonth=="Jul")
        {
        tempMonth="07";
        }
        if(objMonth=="Aug")
        {
        tempMonth="08";
        }
        if(objMonth=="Sep")
        {
        tempMonth="09";
        }
        if(objMonth=="Oct")
        {
        tempMonth="10";
        }
        if(objMonth=="Nov")
        {
        tempMonth="11";
        }
        if(objMonth=="Dec")
        {
        tempMonth="12";
        }
        return tempMonth;
}
function getDateObject(dateString,dateSeperator)
{
	var curValue=dateString;
	var sepChar=dateSeperator;
	var curPos=0;
	var cDate,cMonth,cYear;

	curPos=dateString.indexOf(sepChar);
	cMonth=dateString.substring(0,curPos);
		
	endPos=dateString.indexOf(sepChar,curPos+1);			
	cDate=dateString.substring(curPos+1,endPos);

			
	curPos=endPos;
	endPos=curPos+5;			
	cYear=curValue.substring(curPos+1,endPos);
	cMonth=parseInt(cMonth,0)-1;
  	dtObject=new Date();
  	dtObject.setFullYear(cYear,cMonth,cDate);
  
	return dtObject;
}