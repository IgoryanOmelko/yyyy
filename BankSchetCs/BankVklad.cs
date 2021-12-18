using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSchetCs
{
    class BankVklad : BankSchetClass
    {
        public struct TypeVklad
        {
            public bool duration;
            public double procent;
            public bool abilityExport;
            public bool prangaz;
        }
        public TypeVklad Type;
        public BankVklad(uint num, DateTime date, Fio fioo, double blnc, int mon, double proc) : base(num, date, fioo, blnc, mon)
        {
            number = num;
            dateOpen = date;
            fio = fioo;
            balance = blnc;
            month = mon;
            Type.procent = proc;
            if (num != 0)
            {
                Type.duration = Choice("Вклад рассчитан на определенный срок?");
                if (Type.duration == true)
                    Type.abilityExport = Choice("Есть ли возможность досрочного вывода/пополнения вклада?");
                else
                    Type.abilityExport = true;
                Type.prangaz = Choice("Есть ли возможность продления срока вклада?");
            }
        }

        



        public override void ImportSchet(double money)
        {
            if (Type.abilityExport || Month == 0)
                Balance += money;
            else
                MessageWrite("Вы не можете добавить средств, пока не истек срок вклада", ConsoleColor.Red);
        }

        public override void ExportSchet(double money)
        {
            if (Type.abilityExport || Month == 0)
            {
                if (money < Balance)
                    Balance -= money;
                else
                    MessageWrite("Запрошенного кол-ва средств не обнаружено", ConsoleColor.Red);
            }
            else
            {
                MessageWrite("Вы не можете снять средства, пока не истек срок вклада", ConsoleColor.Red);
            }
        }

        public void ProcAdd (int m)
        {
            if (m <= month || Type.duration == false)
            {
                Balance *= Math.Pow((1.0 + Type.procent / 100.0 / 365.0), 30);
                Month -= m;
            }
        }

        public void Prolangazia (int m)
        {
            if (Type.duration == false && Type.prangaz == true)
                Month = Month + m;
        }

        private bool Choice(string mes)
        {
            Console.WriteLine(mes + "\nОтправьте 1, если это верное утверждение\n" +
                "Отправьте 2, если ложное");
            switch(Console.ReadLine())
            {
                case "1": return true;
                case "2": return false;
                default: break;
            }
            return false;
        }

        private string ChoiceToString(bool tf)
        {
            if (tf)
                return "ДА";
            return "НЕТ";
        }

        public override string ToString()
        {
            return $"Номер счёта: {number} " +
                $"\nФИО: {fio.sName} {fio.fName} " +
                $"{fio.tName}\nДата открытия счета:{dateOpen.Date} " +
                $"\nСрок действия счета: {month}" +
                $"\nДата закрытия счета: {InfoClose()}" +
                $"\nПроцент: {Type.procent}" +
                $"\nБаланс: {balance.ToString("F2")}" +
                "\nВклад ограничен по времени: " + ChoiceToString(Type.duration) +
                "\nСредства можно вывести до окончания срока: " + ChoiceToString(Type.abilityExport) +
                "\nПролонгация: " + ChoiceToString(Type.prangaz);
        }
    }
}
