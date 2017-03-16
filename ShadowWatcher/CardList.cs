using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShadowWatcher
{
    public class CardList : IEnumerable<CardInfo>, INotifyCollectionChanged
    {
        private List<CardInfo> list = new List<CardInfo>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(CardInfo elem)
        {
            if (list.Contains(elem))
            {
                list.Find(e => e == elem).Amount += elem.Amount;
            }
            else
            {
                list.Add(elem);
                list.Sort();

                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void Clear()
        {
            list.Clear();

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<CardInfo> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CardInfo : IComparable<CardInfo>, IEquatable<CardInfo>, INotifyPropertyChanged
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
                    NotifyPropertyChanged();
                }
            }
        }

        public string CostText
        {
            get { return $"<{Cost}{(Atk.HasValue ? $",{Atk.Value},{Life.Value}" : "")}>"; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(CardInfo other)
        {
            if (other == null) return 1;

            if (Cost - other.Cost == 0)
                return ID - other.ID;
            else
                return Cost - other.Cost;
        }

        public static bool operator >(CardInfo op1, CardInfo op2)
        {
            return op1.CompareTo(op2) > 0;
        }

        public static bool operator <(CardInfo op1, CardInfo op2)
        {
            return op1.CompareTo(op2) < 0;
        }

        public static bool operator >=(CardInfo op1, CardInfo op2)
        {
            return op1.CompareTo(op2) >= 0;
        }

        public static bool operator <=(CardInfo op1, CardInfo op2)
        {
            return op1.CompareTo(op2) <= 0;
        }

        public bool Equals(CardInfo other)
        {
            return CompareTo(other) == 0;
        }

        public static bool operator ==(CardInfo op1, CardInfo op2)
        {
            if (ReferenceEquals(op1, null))
            {
                return ReferenceEquals(op2, null);
            }
            return op1.Equals(op2);
        }

        public static bool operator !=(CardInfo op1, CardInfo op2)
        {
            return !(op1 == op2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CardInfo);
        }

        public override int GetHashCode()
        {
            return Cost * 1000000000 + ID;
        }
    }
}
