using System;
using System.Diagnostics;
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
            switch (action)
            {
                case "BattleReady":
                    Dispatcher.Invoke(() =>
                    {
                        List.Clear();
                    });
                    break;
                case "Load":
                case "Unload":
                case "Win":
                case "Lose":
                    break;
                case "EnemyPlay":
                    var info = data.Split(',');
                    var cardInfo = new CardInfo
                    {
                        ID = int.Parse(info[0]),
                        Name = info[0] + info[1],
                        Cost = int.Parse(info[2]),
                    };
                    if (info.Length > 3)
                    {
                        cardInfo.Atk = int.Parse(info[3]);
                        cardInfo.Life = int.Parse(info[4]);
                    }

                    Dispatcher.Invoke(() =>
                    {
                        List.Add(cardInfo);
                    });
                    break;
                case "EnemyAdd":
                    info = data.Split(',');
                    cardInfo = new CardInfo
                    {
                        ID = int.Parse(info[0]),
                        Name = info[1],
                        Cost = int.Parse(info[2]),
                        Amount = -1,
                    };
                    if (info.Length > 3)
                    {
                        cardInfo.Atk = int.Parse(info[3]);
                        cardInfo.Life = int.Parse(info[4]);
                    }

                    Dispatcher.Invoke(() =>
                    {
                        List.Add(cardInfo);
                    });
                    break;
            }
            Dispatcher.Invoke(() =>
            {
                LogText.Text = $"{action}:{data}\n{LogText.Text}";
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
}
