using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility;

/// <summary>
/// Summary description for GenericPool
/// </summary>
public static class PaymentTransactionPool
{
    public static List<PaymentTransaction> _AvailablePool = new List<PaymentTransaction>();
    public static List<PaymentTransaction> _InProcessPool = new List<PaymentTransaction>();
    public static PaymentTransaction GetObject()
    {
        PaymentTransaction objPaymentTransaction;

        lock (_AvailablePool)
        {

            if (_AvailablePool.Count != 0)
            {
                objPaymentTransaction = _AvailablePool[0];
                _InProcessPool.Add(objPaymentTransaction);
                _AvailablePool.Remove(objPaymentTransaction);
                return objPaymentTransaction;
            }
            else
            {
                objPaymentTransaction = new PaymentTransaction();
                _InProcessPool.Add(objPaymentTransaction);
                return objPaymentTransaction;
            }
        }

        

    }

    public static void ReleaseObject(PaymentTransaction objpayment)
    {

        CleanObject(objpayment);
        _AvailablePool.Add(objpayment);
      
    }

    private static void CleanObject(PaymentTransaction objpayment)
    {
        objpayment.Amount = null;
        objpayment.AmountFromMyAccount = false;
        objpayment.Authorized = false;
        objpayment.Comments = null;
        objpayment.CourierName = null;
        objpayment.CreatedOn = DateTime.Now;
        objpayment.CurrencyCode = null;
        objpayment.CurrencySymbol = null;
        objpayment.CurrencyValue = 0;
        objpayment.Delivered = false;
        objpayment.DeliveredDate = null;
        objpayment.DeliveredID = null;
        objpayment.Dispatched = false;
        objpayment.DispatchedDate = null;
        objpayment.DispatchedID = null;
        objpayment.Location = null;
        objpayment.OrderCurrentStatus = 0;
        objpayment.OrdersReturnAction = null;
        objpayment.OrdersReturnReason = null;
        objpayment.OtherCharges = null;
        objpayment.PaymentMode = null;
        objpayment.PaymentStatus = 0;
        objpayment.PaymentTransactionId = 0;
        objpayment.PGTxnId = null;
        objpayment.Pickup = false;
        objpayment.PickupDate = DateTime.Now;
        objpayment.PickupID = null;
        objpayment.ReceivedBy = null;
        objpayment.ServiceTax = null;
        objpayment.ShipmentDate = DateTime.Now;
        objpayment.ShipmentId = null;
        objpayment.ShipmentType = null;
        objpayment.ShipmentURL = null;
        objpayment.ShippingCharges = null;
        objpayment.TxnAmount = null;
        objpayment.TxnMessage = null;
        objpayment.TxnRefNo = null;
        objpayment.TxnStatus = null;
        objpayment.UpdatedOn = DateTime.Now;
        objpayment.UserId = 0;
        objpayment.User = null;
        objpayment.UserProductTransactions = null;
        objpayment.VAT = null;
    }
}