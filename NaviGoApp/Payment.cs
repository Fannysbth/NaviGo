using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{
    public enum PaymentStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
    public class Payment
    {
        public string PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public PaymentStatus Status { get; set; }

        public Payment(string paymentId, DateTime paymentDate, double amount, PaymentStatus status)
        {
            PaymentId = paymentId;
            PaymentDate = paymentDate;
            Amount = amount;
            Status = status;
        }

        public void ProcessPayment()
        {
            // Simulate payment processing
            try
            {
                Console.WriteLine($"Processing payment {PaymentId} for amount {Amount:C}");

                // Simulate some processing time
                System.Threading.Thread.Sleep(1000);

                // Assume payment is successful for demo
                Status = PaymentStatus.Confirmed;
                Console.WriteLine($"Payment {PaymentId} processed successfully");
            }
            catch (Exception ex)
            {
                Status = PaymentStatus.Cancelled;
                Console.WriteLine($"Payment {PaymentId} failed: {ex.Message}");
            }
        }
    }
}
