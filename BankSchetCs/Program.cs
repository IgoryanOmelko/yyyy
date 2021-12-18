using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSchetCs
{

    public struct Fio
    {
        public String sName;
        public String fName;
        public String tName;
    }

    class Program
    {
        public static BankSchetClass[] BankSchets = new BankSchetClass[2];
        public static BankCard[] BankCards = new BankCard[2];
        public static BankVklad[] bankVklads = new BankVklad[1];
        public static Random rand = new Random();
        public static Fio FIO;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < BankSchets.Length; i++)
            {
                OpenSchet(ref BankSchets[i]);
                OpenCard(ref BankCards[i], BankSchets[i], BankSchets[i].Balance);

            }
            bankVklads[0] = new BankVklad(0, DateTime.Now, FIO, 0, 0, 0);
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                uint selectSchet = ControlUInt("Введите номер счёта " + GetNumbers(BankSchets));
                Console.ForegroundColor = ConsoleColor.White;
                string choice = "";
                for (int i = 0; i < BankSchets.Length; i++)
                {
                    if (BankSchets[i].Number == selectSchet)
                    {
                       while(choice != "0")
                        {
                            BankSchets[i].Balance = SumForSchet(BankCards, BankSchets[i]);
                            choice = WriteMessage("Выберите опцию:\n1) Вывести информацию о счёте\n2) Перевод\nПерегрузка операция\n3) Объединение двух счетов\n4) Сравнение счетов\n5) Добавление нового счета\n6) Вывести информацию о всех картах\n7) Создать карту\n8) Управлять картой\n0) Выйти из аккаунта");
                            switch (choice)
                            {
                                case "1": Console.WriteLine(BankSchets[i].ToString());break;
                                case "2": TransferSchet(ref BankSchets, BankSchets[i], choice);break;
                                case "3": TransferSchet(ref BankSchets, BankSchets[i], choice); break;
                                case "4": TransferSchet(ref BankSchets, BankSchets[i], choice); break;
                                case "5": Array.Resize(ref BankSchets, BankSchets.Length + 1); OpenSchet(ref BankSchets[BankSchets.Length - 1]);break;
                                case "6":
                                    for (int j = 0; j < BankCards.Length; j++)
                                    {
                                        if (BankCards[j].Number == BankSchets[i].Number)
                                        {
                                            Console.WriteLine(BankCards[j].ToString());
                                        }
                                    }break;
                                case "7": Array.Resize(ref BankCards, BankCards.Length + 1); OpenCard(ref BankCards[BankCards.Length - 1], BankSchets[i], 0);  break;
                                case "8": CardOperation(BankCards, ChoiceCard(BankCards, BankSchets[i]), BankSchets);break;
                            }
                        }
                    }
                }
            }
        }


        static void OpenSchet(ref BankSchetClass bankSchets)
        {
            FIO.sName = WriteMessage("Введите Фамилию");
            FIO.fName = WriteMessage("Введите имя");
            FIO.tName = WriteMessage("Введите отчество");

            bankSchets = new BankSchetClass(GenerateNumber(), GenerateDate(), FIO, ControlBalance("Введите баланс"), ControlInt("Введите кол-во месяцев"));
        }

        static void OpenCard(ref BankCard bankCard, BankSchetClass bankSchet, double money)
        {

            bankCard = new BankCard(bankSchet.Number, GenerateNumberCard(), ControlPin("Придумайте ПИН"), DateTime.Now, bankSchet.FIO, money, 36);
        }

        static void OpenVklad(ref BankVklad bankVklad, BankCard bankCard)
        {
            bankVklad = new BankVklad(UInt32.Parse(bankCard.NumCard.Substring(0, 4) + GenerateNumber().ToString()), DateTime.Now, bankCard.FIO, 0, ControlInt("Введите количество месяцев"), ControlBalance("Введите количество процентов"));
        }

        static void TransferSchet(ref BankSchetClass[] bankSchets, BankSchetClass schet, string choice)
        {
            uint selectSchet = ControlUInt("Введите номер счёта");

            for (int i = 0; i < bankSchets.Length; i++)
            {
                if (selectSchet == bankSchets[i].Number)
                    switch (choice)
                    {
                        case "2": schet.Transfer(bankSchets[i], ControlBalance("Введите сумму для вывода"));break;
                        case "3": schet = schet + bankSchets[i]; break;
                        case "4": if (bankSchets[i] == schet)
                                Console.WriteLine("Баланс выбранных счетов одинаков");
                            if (bankSchets[i] != schet)
                                Console.WriteLine("Баланс выбранных счетов разные");break;

                    }
            }
        }

        static void CardOperation(BankCard[] bankCards, BankCard bankCard, BankSchetClass[] bankSchets) //операции с картой
        {
            string choice = "";
            while (choice != "0")
            {
                choice = WriteMessage("Выберите опцию:\n1) Вывести информацию о счёте\n2) Добавить средств на счёт\n3) Вывести средства \n4) Перевод \n5) Перевод по номеру телефона \n6) Присоединить к карте \n7) Поменять ПИН \n8) Присоединить телефон \n9) Кэшбек с покупки \n10) Рассчитаться кэшбеком \n11) Открыть счет \n12) Управлять счетом");
                switch (choice)
                {
                    case "1": Console.WriteLine(bankCard.ToString());break;
                    case "2": bankCard.ImportSchet(ControlBalance("Введите сумму для пополнения"));break;
                    case "3": bankCard.Export(ControlBalance("Введите сумму для вывода"), ControlPin("Введите PIN"));break;
                    case "4": string num = WriteMessage("Введите номер карты получателя");
                        for (int i = 0; i < bankCards.Length; i++)
                        {
                            if (num == bankCards[i].NumCard)
                                bankCard.CTransfer(bankCards[i], ControlBalance("Введите сумму для перевода"), ControlPin("Введите ПИН"));
                        }break;
                    case "5": string numph = WriteMessage("Введите номер телефона получателя");
                        for (int i = 0; i < bankCards.Length; i++)
                        {
                            if (numph == bankCards[i].NumPhone)
                                bankCard.CTransfer(bankCards[i], ControlBalance("Введите сумму для перевода"), ControlPin("Введите ПИН"));
                        }break;
                    case "6": uint numtf = ControlUInt("Введите номер счета");
                        for (int i = 0; i < bankSchets.Length; i++)
                        {
                            if (numtf == bankSchets[i].Number)
                                bankCard.Connect(bankSchets[i]);
                        }
                        return;
                        break;
                    case "7": bankCard.ChangePin(ControlPin("Введите старый ПИН"), ControlPin("Введите новый ПИН"));break;
                    case "8": bankCard.ConnectPhone(WriteMessage("Введите номер телефона"));break;
                    case "9": bankCard.BuyWithGetCashBank(ControlBalance("Введите цену товара"), ControlBalance("Введите процент кэшбека"));break;
                    case "10": bankCard.BuyWithCashBack(ControlBalance("Введите цену товара"));break;
                    case "11": if (bankVklads[0].Number == 0) OpenVklad(ref bankVklads[0], bankCard); else { Array.Resize(ref bankVklads, bankVklads.Length + 1); OpenVklad(ref bankVklads[bankVklads.Length - 1], bankCard); } break;
                    case "12": if (bankVklads[0].Number != 0)
                            VkladOperation(bankVklads, ChoiceVklad(bankVklads, bankCard)); break;
                }
            }
            return;
        }

        static void VkladOperation(BankVklad[] bankVklads, BankVklad bankVklad)
        {
            string choice = "";
            while (choice != "0")
            {
                choice = WriteMessage("Выберите опцию:\n1) Вывести информацию о счёте\n2) Добавить средств на счёт");
                switch (choice)
                {
                    case "1": Console.WriteLine(bankVklad.ToString()); break;
                    case "2": ImportInVklad()
                        
                }
            }
        }

        static BankCard ChoiceCard(BankCard[] bankCard, BankSchetClass bankSchet) //выбрать карту для счета
        {
            do
            {
                for (int j = 0; j < bankCard.Length; j++)
                {
                    if (bankCard[j].Number == bankSchet.Number)
                    {
                        string choice = WriteMessage($"Если вы хотите выбрать карту {bankCard[j].NumCard}, отправьте 1");
                        if (choice == "1")
                            return bankCard[j];
                    }
                }
            }
            while (true);
        }

        static BankVklad ChoiceVklad(BankVklad[] bankVklads, BankCard bankCard) //выбрать карту для счета
        {

            do
            {
                for (int j = 0; j < bankVklads.Length; j++)
                {
                    if (bankVklads[j].Number.ToString().Substring(0, 4) == bankCard.NumCard.ToString().Substring(0,4))
                    {
                        string choice = WriteMessage($"Если вы хотите выбрать вклад {bankVklads[j].Number}, отправьте 1");
                        if (choice == "1")
                            return bankVklads[j];
                    }
                }
            }
            while (true);
        }

        static void ImportInVklad(BankVklad bankVklad, BankCard bankCard)
        {
            
            double money = bankCard.Balance; double chmoney = ControlBalance("Введите сумму для снятия");
            bankCard.Export(ControlBalance("Введите сумму для пополнения вклада"), ControlPin("Введите ПИН"));
            if (money != bankCard.Balance)
            {
                if (bankVklad.Type.abilityExport && bankVklad.Month == 0)
                    bankVklad.ImportSchet(chmoney);
                else
                    bankCard.ImportSchet(chmoney);
            }
        }

        static double SumForSchet(BankCard[] bankCards, BankSchetClass BankSchets) //сумма всех банковских карт
        {
            double money = 0;
            for (int j = 0; j < bankCards.Length; j++)
            {
                if (BankCards[j].Number == BankSchets.Number)
                    money += BankCards[j].Balance;
            }
            return money;
        }

        static DateTime GenerateDate()
        {
            int minday, maxday, days;
            if (DateTime.Now.Day > 5)
            {
                minday = DateTime.Now.Day - 5;
                maxday = DateTime.Now.Day;
            }
            else
            {
                minday = DateTime.Now.Day;
                maxday = DateTime.Now.Day + 5;
            }
            days = rand.Next(minday, maxday);
            DateTime dat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
            return dat;

        }

        static string GetNumbers(BankSchetClass[] bankSchets)
        {
            string result = "";
            for (int i = 0; i < bankSchets.Length; i++)
            {
                result += bankSchets[i].Number.ToString() + " ";
            }
            return result;
        }

        private static uint GenerateNumber()
        {
            Random ran = new Random();
            uint key; string genKey = "";
            for (int j = 0; j < 3; j++)
            {
                genKey += ran.Next(0, 10).ToString();
            }
            key = Convert.ToUInt32(genKey);

            return key;
        }

        private static string GenerateNumberCard()
        {
            Random ran = new Random();
            string genKey = "";
            for (int j = 0; j < 4; j++)
            {
                genKey += ran.Next(1000, 10000).ToString();
            }
            return genKey;
        }

        static string WriteMessage(string mes)
        {

            Console.WriteLine(mes);
            string result = Console.ReadLine();
            return result;
        }

        static uint ControlUInt(string mess)
        {
            uint rezult; string temp;
            do
            {
                Console.WriteLine(mess);
                temp = Console.ReadLine();
            }
            while (!uint.TryParse(temp, out rezult));
            return (rezult);
        }

        static Int16 ControlInt(string mess)
        {
            Int16 rezult; string temp;
            do
            {
                do
                {
                    Console.WriteLine(mess);
                    temp = Console.ReadLine();
                }
                while (!Int16.TryParse(temp, out rezult));
            }
            while (rezult > 10000);
            return (rezult);
        }

        static Int16 ControlPin(string mess)
        {
            Int16 rezult; string temp;
            do
            {
                do
                {
                    Console.WriteLine(mess);
                    temp = Console.ReadLine();
                }
                while (!Int16.TryParse(temp, out rezult));
            }
            while (rezult > 10000 || rezult < 999);
            return (rezult);
        }

        static double ControlBalance(string mess)
        {
            double rezult; string temp;
            do
            {
                do
                {
                    Console.WriteLine(mess);
                    temp = Console.ReadLine();
                }
                while (!double.TryParse(temp, out rezult));
            }
            while (rezult < 0);
            rezult = Math.Round(rezult, 2);
            return (rezult);
        }
    }
}
