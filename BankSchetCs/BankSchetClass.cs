using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSchetCs
{
    class BankSchetClass
    {
        protected uint number;
        protected DateTime dateOpen;
        protected Fio fio;
        protected double balance;
        protected int month;

        public uint Number { get { return number; } }
        public DateTime DateOpen { get { return dateOpen; } set { dateOpen = value; } }
        public Fio FIO { get { return fio; } }
        public double Balance { get { return balance; } set { balance = Math.Round(value,2); } }
        public int Month { get { return month; } set { month = value; } }

        public BankSchetClass(uint num, DateTime date, Fio fioo, double blnc, int mon)
        {
            number = num;
            dateOpen = date;
            fio = fioo;
            balance = blnc;
            month = mon;
        }

        public virtual void ImportSchet(double money)
        {

            balance += money;
        }

        public virtual void ExportSchet (double money)
        {
            if (money < balance)
                balance -= money;
            else
                MessageWrite("Запрошенного кол-ва средств не обнаружено", ConsoleColor.Red);
        }

        public void ExportAll ()
        {
            if (balance > 0)
                balance = 0;
            else
                MessageWrite("Средств не обнаружено", ConsoleColor.Red);
        }

        public override string ToString()
        {
            return $"Номер счета: {number} " +
                $"\nФИО: {fio.sName} {fio.fName} " +
                $"{fio.tName}\nДата открытия:{dateOpen.Date} " +
                $"\nСрок вклада: {month}" +
                $"\nДата закрытия: {InfoClose()}" +
                $"\nБаланс: {balance.ToString("F2")}\n";

        }

        public static BankSchetClass operator +(BankSchetClass bsc1, BankSchetClass bsc2)
        {
            if (bsc1 != bsc2)
            {

                bsc1.Balance = bsc1.Balance + bsc2.Balance;
                MessageWrite("Операция выполнена", ConsoleColor.Green);
            }
            else
            {
                MessageWrite("Операция отклонена. Невозможно объеденить счёт с тем же счетом", ConsoleColor.Red);
            }
            return bsc1;
        }

        public static BankSchetClass operator%(BankSchetClass bsc, double proc)
        {
            if (proc <= 30)
            {
                bsc.balance *= Math.Pow((1 + proc / 100 / 365), bsc.month * 30);
                MessageWrite("Операция выполнена", ConsoleColor.Green);
            }
            else
            {
                MessageWrite("Операция отклонена. На больше, чем 30% годовых мы не согласны", ConsoleColor.Red);
            }
           
            return bsc;
        }

        public static BankSchetClass operator++(BankSchetClass bsc)
        {
            bsc.balance *= Math.Pow((1.0 + 4.7 / 100.0 / 365.0), 30);
            MessageWrite("Операция выполнена", ConsoleColor.Green);
            return bsc;
        }

        public static bool operator==(BankSchetClass bsc1, BankSchetClass bsc2)
        {
            if (bsc1.Balance == bsc2.Balance)
                return true;

            return false;
        }

        public static bool operator!=(BankSchetClass bsc1, BankSchetClass bsc2)
        {
            if (bsc1.Balance == bsc2.Balance)
                return false;
                return true;
        }

        public static BankSchetClass operator-(BankSchetClass bsc, double money)
        {
            if (money <= bsc.balance)
            {
                bsc.balance -= money;
                MessageWrite("Операция выполнена", ConsoleColor.Green);
            }
            return bsc;
        }

        public static BankSchetClass operator-(BankSchetClass bsc1, BankSchetClass bsc2)
        {
            double money = bsc1.Balance ;
            bsc1.balance -= money;
            bsc2.balance += money;
            MessageWrite("Операция выполнена", ConsoleColor.Green);
            return bsc2;
        }

        public static BankSchetClass operator+(BankSchetClass bsc, double money)
        {
            bsc.balance += money;
            MessageWrite("Операция выполнена", ConsoleColor.Green);
            return bsc;
        }

        protected static void MessageWrite(string mes, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(mes);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Transfer (BankSchetClass bankSchet, double money)
        {
            if (money <= balance)
            {
                bankSchet.balance += money;
                balance -= money;
                MessageWrite("Операция выполнена", ConsoleColor.Green);
            }
        }

        protected DateTime InfoClose ()
        {
            DateTime data = dateOpen.AddMonths(month);
            return data.Date;
        }
    }
}
