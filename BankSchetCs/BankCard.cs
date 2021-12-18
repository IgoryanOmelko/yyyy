using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSchetCs
{
    
    class BankCard : BankSchetClass
    {
        private string numCard;
        private Int16 pin;
        private string numPhone;
        private double cashback;

        public string NumCard { get { return numCard; } }
        public Int16 Pin { get { return pin; } set { if (value < 10000) pin = value; } }
        public string NumPhone { get { return numPhone; } set { numPhone = value; } }
        public double Cashback { get { return cashback; }set { cashback = value; } }

        public BankCard(uint num, string numc, Int16 pincode, DateTime date, Fio fioo, double blnc, int mon) : base(num, date, fioo, blnc, mon)
        {
            number = num;
            numCard = numc;
            pin = pincode;
            numPhone = "0";
            dateOpen = date;
            fio = fioo;
            balance = blnc;
            cashback = 0;
            month = mon;
        }

        public void Connect(BankSchetClass bankSchet)
        {
            number = bankSchet.Number;
        }

        public void Export(double money, Int16 pn)
        {
            if (Pin == pn && Balance >= money)
                Balance -= money;

        }

        public void ChangePin(Int16 pinold, Int16 pinnew)
        {
            if (pinold == Pin)
                Pin = pinnew;
        }

        public void ConnectPhone(string phone)
        {
            if (phone.Length == 11)
                NumPhone = phone;
        }

        public void BuyWithGetCashBank(double cost, double cash)
        {
            if (Cashback <= 30 && Balance >= cost)
            {
                Cashback += cost / 100 * cash;
                Balance -= cost;
            }
        }

        public void BuyWithCashBack(double price)
        {
            if (price <= Cashback)
                Cashback -= price;
        }

        public void CTransfer(BankCard bankcard, double money, Int16 Pin)
        {
            if (money <= balance && Pin == pin)
            {
                bankcard.balance += money;
                balance -= money;
                MessageWrite("Операция выполнена", ConsoleColor.Green);
            }
        }

        public override string ToString()
        {
            if (numPhone != "0")
                return $"Номер карты: {numCard} " +
                $"Номер счета: {number}" +
                $"\nФИО: {fio.sName} {fio.fName} " +
                $"{fio.tName}\nДата открытия карты:{dateOpen.Date} " +
                $"\nСрок действия карты: {month}" +
                $"\nДата закрытия карты: {InfoClose()}" +
                $"\nБаланс: {balance.ToString("F2")}" +
                $"\nНомер телефона владельца: {numPhone}" +
                $"\nСумма кэшбека: {cashback}\n";
            return $"Номер карты: {numCard} " +
                $"Номер счета: {number}" +
                $"\nФИО: {fio.sName} {fio.fName} " +
                $"{fio.tName}\nДата открытия карты:{dateOpen.Date} " +
                $"\nСрок действия карты: {month}" +
                $"\nДата закрытия карты: {InfoClose()}" +
                $"\nБаланс: {balance.ToString("F2")}" +
                $"\nСумма кэшбека: {cashback}\n";
        }
    }
}
