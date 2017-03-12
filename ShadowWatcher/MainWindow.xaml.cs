using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ShadowWatcher
{
    public partial class MainWindow : Window
    {
        private bool isAttached = false;

        public CardList List { get; set; } = new CardList();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void attachObserver()
        {
            var process = Process.Start("injector\\mono-assembly-injector.exe", "-dll Observer.dll -target Shadowverse.exe -namespace ShadowWatcher -class Loader -method Load");
            if (process != null && process.WaitForExit(10000))
            {
                var exitCode = process.ExitCode;
                if (exitCode != 0)
                {
                    MessageBox.Show("Failed to load.");
                    return;
                }
            }

            Receiver.OnReceived = Receiver_OnReceived;
            isAttached = true;
        }

        private void detachObserver()
        {
            var process = Process.Start("injector\\mono-assembly-injector.exe", "-dll Observer.dll -target Shadowverse.exe -namespace ShadowWatcher -class Loader -method Unload");
            if (process != null && process.WaitForExit(10000))
            {
                var exitCode = process.ExitCode;
                if (exitCode != 0)
                {
                    MessageBox.Show("Failed to unload.");
                    return;
                }
            }

            Receiver.OnReceived = null;
            isAttached = false;
        }

        private void Receiver_OnReceived(string action, string data)
        {
            var str = "";
            switch (action)
            {
                case "PlayHand":
                    var cards = data.Split(';');
                    var list = new List<CardInfo>();
                    foreach (var card in cards)
                    {
                        var info = card.Split(',');
                        var cardInfo = new CardInfo
                        {
                            ID = int.Parse(info[0]),
                            Name = info[1],
                            Cost = int.Parse(info[2]),
                        };
                        if (info.Length > 3)
                        {
                            cardInfo.Atk = int.Parse(info[3]);
                            cardInfo.Life = int.Parse(info[4]);
                        }
                        list.Add(cardInfo);
                    }
                    str = $"PlayHand:{list.Select(e => e.Name).Aggregate((a, b) => $"{a},{b}")}";

                    Dispatcher.Invoke(() =>
                    {
                        List.Add(list);
                    });
                    break;
                case "BattleReady":
                    str = action;

                    Dispatcher.Invoke(() =>
                    {
                        List.Clear();
                    });
                    break;
                case "Load":
                case "Unload":
                case "Win":
                case "Lose":
                    str = action;
                    break;
                default:
                    str = $"{action}:{data}";
                    break;
            }
            Dispatcher.Invoke(() =>
            {
                LogText.Text = $"{str}\r\n{LogText.Text}";
            });
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!isAttached)
                attachObserver();
            else
                detachObserver();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (isAttached)
                detachObserver();
        }
    }

    public class CardList : IEnumerable<CardInfo>, INotifyCollectionChanged
    {
        private List<CardInfo> list = new List<CardInfo>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(CardInfo elem, bool notify = true)
        {
            if (list.Contains(elem))
            {
                list.Find(e => e == elem).Amount += elem.Amount;
            }
            else
            {
                list.Add(elem);
                list.Sort();

                if (notify)
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void Add(IEnumerable<CardInfo> elems)
        {
            foreach (var elem in elems)
            {
                Add(elem, false);
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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
            get { return $"{Cost}{(Atk.HasValue ? $",{Atk.Value},{Life.Value}" : "")}"; }
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

        public static bool operator > (CardInfo op1, CardInfo op2)
        {
            return op1.CompareTo(op2) > 0;
        }

        public static bool operator < (CardInfo op1, CardInfo op2)
        {
            return op1.CompareTo(op2) < 0;
        }

        public static bool operator >= (CardInfo op1, CardInfo op2)
        {
            return op1.CompareTo(op2) >= 0;
        }

        public static bool operator <= (CardInfo op1, CardInfo op2)
        {
            return op1.CompareTo(op2) <= 0;
        }

        public bool Equals(CardInfo other)
        {
            return CompareTo(other) == 0;
        }

        public static bool operator == (CardInfo op1, CardInfo op2)
        {
            if (ReferenceEquals(op1, null))
            {
                return ReferenceEquals(op2, null);
            }
            return op1.Equals(op2);
        }

        public static bool operator != (CardInfo op1, CardInfo op2)
        {
            return !(op1 == op2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CardInfo);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
