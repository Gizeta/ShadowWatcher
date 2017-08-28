using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using static CardBasePrm;

namespace ShadowWatcher.Contract
{
    public class CardData : IComparable<CardData>, IEquatable<CardData>, INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int Cost { get; set; }
        public int? Atk { get; set; } = null;
        public int? Life { get; set; } = null;
        public string Name { get; set; }

        private int amount = 1;
        public int Amount
        {
            get { return amount; }
            set
            {
                if (value != amount)
                {
                    amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        public string CostText => $"<{Cost}>";
        public string NameText => $"{(Atk.HasValue ? $"({Atk.Value},{Life.Value})" : "")}{Name}";

        public static CardData Parse(string data, int amount = 1)
        {
            var info = data.Split('\t');
            var card = new CardData
            {
                ID = int.Parse(info[0]),
                Name = info[1],
                Cost = int.Parse(info[2]),
                Amount = amount,
            };
            if (info.Length > 3)
            {
                card.Atk = int.Parse(info[3]);
                card.Life = int.Parse(info[4]);
            }
            return card;
        }

        public static CardData Parse(BattleCardBase card, bool realCost = false, int amount = 1)
        {
            var param = card.BaseParameter;
            var d = param.CardId.ToString();
            var id = int.Parse($"{d[3]}{d[5]}{d[2]}{d[4]}{d[6]}{d[7]}{d[0]}");
            var data = new CardData
            {
                ID = id,
                Name = Regex.Replace(param.CardName, @"(\[[a-zA-Z0-9\/\-]*(rub\<[^\>]*\>)*\])", ""),
                Cost = realCost ? card.Cost : param.Cost,
                Amount = amount
            };
            if (param.CharType == CharaType.NORMAL)
            {
                data.Atk = param.Atk;
                data.Life = param.Life;
            }
            return data;
        }

        public override string ToString() => $"{ID}\t{Name}\t{Cost}{(Atk.HasValue ? $"\t{Atk.Value}\t{Life.Value}" : "")}";

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(CardData other)
        {
            if (other == null) return 1;

            if (Cost - other.Cost == 0)
                return ID - other.ID;
            else
                return Cost - other.Cost;
        }

        public static bool operator >(CardData op1, CardData op2) => op1.CompareTo(op2) > 0;
        public static bool operator <(CardData op1, CardData op2) => op1.CompareTo(op2) < 0;
        public static bool operator >=(CardData op1, CardData op2) => op1.CompareTo(op2) >= 0;
        public static bool operator <=(CardData op1, CardData op2) => op1.CompareTo(op2) <= 0;

        public bool Equals(CardData other) => CompareTo(other) == 0;

        public static bool operator ==(CardData op1, CardData op2)
        {
            if (ReferenceEquals(op1, null))
            {
                return ReferenceEquals(op2, null);
            }
            return op1.Equals(op2);
        }

        public static bool operator !=(CardData op1, CardData op2) => !(op1 == op2);

        public override bool Equals(object obj) => Equals(obj as CardData);

        public override int GetHashCode() => Cost * 10000000 + ID;
    }
}
