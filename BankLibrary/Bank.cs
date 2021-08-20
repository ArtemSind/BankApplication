using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    // тип счёта
    public enum AccountType
    {
        Ordinary,
        Deposit
    }

    public class Bank<T> where T : Account
    {
        List<T> accounts = new List<T>();
        
        public string Name { get; private set; }

        public Bank(string name)
        {
            this.Name = name;
        }

        //метод создания счёта
        public void Open(AccountType accountType, decimal sum,
            AccountStateHandler addSumHandler, AccountStateHandler withdrawSumHandler,
            AccountStateHandler calculationHandler, AccountStateHandler closeAccountHandler,
            AccountStateHandler openAccountHandler)
        {
            T newAccount = null;

            switch (accountType)
            {
                case AccountType.Ordinary:
                    newAccount = new DemandAccount(sum, 1) as T;
                    break;
                case AccountType.Deposit:
                    newAccount = new DepositAccount(sum, 40) as T;
                    break;
            }

            if (newAccount == null)
                throw new Exception("Ошибка создания счёта");
            // добавляем новый счёт в массив счетов
            if (accounts == null)
                accounts = new List<T>() { newAccount };
            else
                accounts.Add(newAccount);
            // установка обработчиков событий счёта
            newAccount.Added += addSumHandler;
            newAccount.Withdrawed += withdrawSumHandler;
            newAccount.Closed += closeAccountHandler;
            newAccount.Opened += openAccountHandler;
            newAccount.Calculated += calculationHandler;

            newAccount.Open();
        }
        //добавление средств на счёт
        public void Put(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Счёт не найден");
            account.Put(sum);
        }
        // вывод средств
        public void Withdraw(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Счёт не найден");
            account.Withdraw(sum);
        }
        // закрытие счёта
        public void Close(int id)
        {
            int index;
            T account = FindAccount(id, out index);
            if (account == null)
                throw new Exception("Счёт не найден");

            account.Close();

            if (accounts.Count <= 1)
                accounts = null;
            else
                accounts.RemoveAt(index);
        }
        
        // Начисление процентов по счета
        public void CalculatePercentage()
        {
            if (accounts == null) // если массив не создан, выходим из метода
                return;

            foreach (var account in accounts)
            {
                account.IncrementDays();
                account.Calculate();
            }
        }
        
        // поиск счёта по id
        public T FindAccount(int id)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].Id == id)
                    return accounts[i];
            }
            return null;
        }
        // перегруженная версия поиска счёта
        public T FindAccount(int id, out int index)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].Id == id)
                {
                    index = i;
                    return accounts[i];
                }
            }
            index = -1;
            return null;
        }
    }
}
