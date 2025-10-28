using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Utility
{
    public static class Validations
    {

        /************************************************

        * Topic : Usage of RegularExpression in .Net

        * Reference System.Text.RegularExpressions.

        * Author : kalit sikka

        * Summary: This regex helper class is design to provide set of static methods to validate commonly used patterns like Email-ID, URL, IP Address, Social Security number, Zip codes etc.

        * You just need to add this .cs file in your project to use any of the method in it.

        * For : http://eggheadcafe.com

        * **********************************************/

        /// <summary>

        /// This helper class contain general regex static methods which in today to today coding tasks

        /// </summary>


        public static bool IsValidEmailId(this string sEmailId)
        {

            Regex oEmail =
            new Regex(@"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" + @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" + @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$");
            //new Regex(@"/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i");
            return oEmail.IsMatch(sEmailId);

        }

        public static bool IsValidUrl(this string sUrl)
        {

            var oURL =

            new Regex(@"^[a-zA-Z0-9\-\.]+\.(com|org|net|mil|edu|COM|ORG|NET|MIL|EDU)$"); // you add more here like au, in

            return oURL.IsMatch(sUrl);

        }

        public static bool IsValidPhoneNumber( this string sPhone)
        {

            Regex oPhone =

            new Regex(@"^[2-9]\d{2}-\d{3}-\d{4}$"); // US Phone - like 800-555-5555 | 333-444-5555 | 212-666-1234

            return oPhone.IsMatch(sPhone);

        }

        public static bool IsValidIndianMobile(this string sMobile)
        {

            Regex oMobile =

            new Regex(@"^([1-9]{1})([0-9]{9})$"); // Indian Mobile - like +919847444225 | +91-98-44111112 | 98 44111116

            return oMobile.IsMatch(sMobile);

        }



        public static bool IsValidUKMobile(this string sMobile)
        {

            Regex oMobile =

            new Regex(@"^07([\d]{3})[(\D\s)]?[\d]{3}[(\D\s)]?[\d]{3}$"); // UK Mobile - like 07976444333 | 07956-514333 | 07988-321-213

            return oMobile.IsMatch(sMobile);

        }

        public static bool IsValidUSZipCode(this string sZipCode)
        {

            Regex oZipCode =

            new Regex(@"^\d{6}$"); // ZipCode - like 333333 | 555555 | 234455

            return oZipCode.IsMatch(sZipCode);

        }

        public static bool IsValidIPAddress(this string sIPAddress)
        {

            Regex oIP =

            new Regex(@"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$"); // IP Address - like 127.0.0.1 | 255.255.255.0 | 192.168.0.1

            return oIP.IsMatch(sIPAddress);

        }
      public static bool IsValidTime(this string sTime)
        {

            Regex oTime =

            new Regex(@"^(20|21|22|23|[01]d|d)(([:][0-5]d){1,2})$");

            return oTime.IsMatch(sTime);

        }

        public static bool Is24HourTimeFormat(this string sTime)
        {

            Regex oTime =

            new Regex(@"^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$"); // like - 12:15 | 10:26:59 | 22:01:15 - Seconds are optional here

            return oTime.IsMatch(sTime);

        }

        public static bool Is12HourTimeFormat(this string sTime)
        {

            Regex oTime =

            new Regex(@" ^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$"); // like - 12:00am | 1:00 PM | 12:59 pm

            return oTime.IsMatch(sTime);

        }

        public static bool DataFormat( this string sDate)
        {

            // dd/MM/yyyy format with leap year validations

            Regex oDate =
             new Regex(@"^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$");
            //new Regex(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$");

            return oDate.IsMatch(sDate);

        }


        public static bool isNumber(this string txt)
        {
            Regex rObj = new Regex("^[0-9][0-9]*$");
            return rObj.IsMatch(txt);
        }

        public static bool IsNumeric(this string sNum)
        {
            // Natural Number

            Regex oNum =

            new Regex(@"0*[1-9][0-9]*");

            return oNum.IsMatch(sNum);

        }

        public static bool IsAlpha(this string sValue)
        {

            // Alpha value

            Regex oValue =

            new Regex(@"[^a-zA-Z ]");

            return oValue.IsMatch(sValue);

        }

        public static bool IsAlphaNumeric(this string strToCheck)
        {

            Regex oCheck = new Regex("[^a-zA-Z0-9]");

            return oCheck.IsMatch(strToCheck);

        }



        public static bool IsStrongPassword(this string sPassword)
        {

            // Check Password with atleast 8 characters, no more than 15 characters, and must include atleast one upper case letter, one lower case letter, and one numeric digit.

            Regex oPassword =

            //new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,15}$");
            new Regex(@"^(?=.*\d)(?=.*[a-z]).{6,15}$");

            return oPassword.IsMatch(sPassword);

        }

        public static bool IsSocialSecurityNumber(this string sValue)
        {

            // U.S. social security numbers, within the range of numbers that have been currently allocated

            Regex oValue =

            new Regex(@"^(?!000)([0-6]\d{2}|7([0-6]\d|7[012]))([ -]?)(?!00)\d\d\3(?!0000)\d{4}");

            return oValue.IsMatch(sValue);

        }

        public static bool IsVISACreditCard(this string sValue)
        {

            // Validate against a visa card number

            Regex oValue =

            new Regex(@"^([4]{1})([0-9]{12,15})$");

            return oValue.IsMatch(sValue);

        }

        public static bool IsISBNumber(this string sValue)
        {

            // ISBN validation expression

            Regex oValue =

            new Regex(@"^\d{9}[\d|X]$");

            return oValue.IsMatch(sValue);

        }

        public static bool IsDollarAmount(this string sAmt)
        {

            // Dollar decimal amount with or without dollar sign

            Regex oAmt =

            new Regex(@"^\$?\d+(\.(\d{2}))?$");

            return oAmt.IsMatch(sAmt);

        }

        public static bool IsPrice(this string sPrice)
        {
            // this will allow the price as either 100  or 100.00
            int count = 0;
            Regex priceExpr = null;
            priceExpr = new Regex("^[0-9][0-9]+[.][0-9]{2}$");

            if (priceExpr.IsMatch(sPrice))
                count++;

            priceExpr = new Regex("^[0-9][0-9]+$");
            if (priceExpr.IsMatch(sPrice))
                count++;

            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>

        /// Method to return string array of splitted values from string

        /// </summary>

        /// <param name="value">string value [Kabir|Sachin|John|Mark|David|kevin|Nash]</param>

        /// <param name="separator">separator in the string value</param>

        /// <returns></returns>

        public static string[] mSplitString(string value, string separator)
        {

            string[] values = new string[25];

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(separator))
            {

                values = System.Text.RegularExpressions.Regex.Split(value, separator);

                return values;

            }

            return null;

        }

        /// <summary>

        /// To Remove specfic string from Collection of string

        /// </summary>

        /// <param name="strNames">Kabir|Sachin|John|Mark|David|kevin|Nash</param>

        /// <param name="sep">separator in the string value</param>

        /// <returns></returns>

        public static string MRemoveStringFromCollection(string strNames, string sep, string specificStr)
        {

            var list = new List<string>();

            string strValue = String.Empty;

            if (!string.IsNullOrEmpty(strNames))
            {

                list.AddRange(Regex.Split(strNames, sep));

                foreach (string str in list)
                {

                    if (!str.Contains(specificStr))
                    {

                        if (string.IsNullOrEmpty(strValue))
                        {

                            strValue = str;

                        }

                        else
                        {

                            strValue = strValue + sep + str;

                        }

                    }

                }

                return strValue;

            }

            return strNames;

        }



    }

 }
