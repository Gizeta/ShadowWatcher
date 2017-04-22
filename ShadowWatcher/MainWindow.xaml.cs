using Microsoft.Win32;
using ShadowWatcher.Socket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace ShadowWatcher
{
    public partial class MainWindow : Window
    {
        private bool isAttached = false;

        public CardList EnemyDeckList { get; set; } = new CardList();
        public CardList PlayerDeckList { get; set; } = new CardList();

        public class BattleData
        {
            public string BattleId { get; set; }
            public string ViewerId { get; set; }
            public string Data { get; set; }

            public BattleData(string json)
            {
                Data = json;

                BattleId = new Regex(@"battle_id\D+(\d+)").Match(json).Groups[1].Value;
                ViewerId = new Regex(@"viewer_id\D+(\d+)").Match(json).Groups[1].Value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Receiver.Initialize();

            DataContext = this;
        }

        private void attachObserver()
        {
            Receiver.OnReceived = Receiver_OnReceived;

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
                        EnemyDeckList.Clear();
                    });
                    break;
                case "Load":
                    Sender.Initialize(int.Parse(data));
                    break;
                case "EnemyPlay":
                case "EnemyAdd":
                    var info = data.Split(',');
                    var cardInfo = new CardInfo
                    {
                        ID = int.Parse(info[0]),
                        Name = info[1],
                        Cost = int.Parse(info[2]),
                        Amount = action == "EnemyPlay" ? 1 : -1,
                    };
                    if (info.Length > 3)
                    {
                        cardInfo.Atk = int.Parse(info[3]);
                        cardInfo.Life = int.Parse(info[4]);
                    }

                    Dispatcher.Invoke(() =>
                    {
                        EnemyDeckList.Add(cardInfo);
                    });
                    break;
                case "PlayerDeck":
                    var cardList = new List<CardInfo>();
                    var cards = data.Split(';');
                    foreach (var card in cards)
                    {
                        info = card.Split(',');
                        cardInfo = new CardInfo
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
                        cardList.Add(cardInfo);
                    }                    

                    Dispatcher.Invoke(() =>
                    {
                        PlayerDeckList.Clear();
                        PlayerDeckList.Add(cardList);
                    });
                    break;
                case "PlayerDraw":
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
                        PlayerDeckList.Add(cardInfo);
                    });
                    break;
                case "ReplayDetail":
                    Dispatcher.Invoke(() =>
                    {
                        ReplayGrid.DataContext = new BattleData(data);
                    });
                    break;
            }
            Dispatcher.Invoke(() =>
            {
                LogText.Text = $"{action}:{data}\n{LogText.Text}";
            });
        }

        private void AttachButton_Click(object sender, RoutedEventArgs e)
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

        private void RepSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".json";
            dialog.Filter = "对战数据 (.json)|*.json";
            if (dialog.ShowDialog() == true)
            {
                var stream = new StreamWriter(dialog.FileName);
                stream.Write((ReplayGrid.DataContext as BattleData).Data);
                stream.Close();
            }
        }

        private void RepLoadButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("未实现");
        }
    }
}
