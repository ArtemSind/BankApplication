using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public interface IAccount
    {
        // Положить деньги на счёт
        void Put(decimal sum);
        // Взять со счёта
        decimal Withdraw(decimal sum);
    }
}
