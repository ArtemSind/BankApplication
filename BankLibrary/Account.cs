using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public abstract class Account : IAccount
    {
        // Событие, возникающее при выводе денег
        protected internal event AccountStateHandler Withdrawed;
        // Событие, возникающее при добавлении на счёт
        protected internal event AccountStateHandler Added;
        // Событие, возникающее при открытии счёта
        protected internal event AccountStateHandler Opened;
        // Событие, возникающее при закрытии счёта
        protected internal event AccountStateHandler Closed;
        // Событие, возникающее при начислении процентов
        protected internal event AccountStateHandler Calculated;

        static int counter = 0;
        protected int _days = 0; // время с момента открытия счёта

        public Account(decimal sum, int percentage)
        {
            Sum = sum;
            Percentage = percentage;
            Id = ++counter;
        }

        // Текущая сумма на счету
        public decimal Sum { get; private set; }
        // Процент начислений
        public int Percentage { get; private set; }
        // Уникальный модификатор счёта
        public int Id { get; private set; }
        // вызов событий
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null)
                handler?.Invoke(this, e);
        }
        // вызов отдельных событий. Для каждого события определяется свой виртуальный метод
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }
        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }
        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }
        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        public virtual void Put(decimal sum)
        {
            Sum += sum;
            OnAdded(new AccountEventArgs("На счёт поступило " + sum, sum));
        }
        // метод снятия со счёта, возвращает сколько снято со счёта
        
        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (Sum >= sum)
            {
                Sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs($"Сумма {sum} снята со счёта {Id}", sum));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs($"Недостаточно денег на счёте {Id}", 0));
            }
            return result;
        }
        // открытие счёта
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs($"Открыт новый счёт! Id счёта: {Id}", Sum));
        }
        // закрытие счёта
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs($"Счёт {Id} закрыт. Итоговая сумма: {Sum}", Sum));
        }

        protected internal void IncrementDays()
        {
            _days++;
        }
        // начисление процентов
        protected internal virtual void Calculate()
        {
            decimal increment = Sum * Percentage / 100;
            Sum = Sum + increment;
            OnCalculated(new AccountEventArgs($"На счёт с id {Id} начислены проценты в размере: {increment}", increment));
        }
    }
}
