using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsPlay_app.Classes
{
    internal class UserInfo
    {
        // users table
        public int UserNr { get; set; }

        public string RegisteredDate { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImgUrl { get; set; }
        public string WebsiteUrl { get; set; }

        // balance table
        /// example insert:  INSERT INTO `balance` (`BalanceNr`, `UserNr`, `TransactionDate`, `SendTo`, `ReceivedFrom`, `Amount`, `TransactionTax`) VALUES ('3', '3', '', '', '', '5.0', '0');
        public int BalanceNr { get; set; }
        public string TransactionDate {  get; set; }
        public string SendTo { get; set; }
        public string ReceivedFrom { get; set; }
        public double Amount { get; set; }
        public double TransactionTax { get; set; }
    }
}
