using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace ShadowWatcher
{
    public class Countdown : INotifyPropertyChanged
    {
        private DispatcherTimer timer;
        private DateTime startTime;

        private double progress1 = 100;
        public double Progress1 {
            get => progress1;
            set => SetProperty(ref progress1, value);
        }

        private double progress2 = 100;
        public double Progress2
        {
            get => progress2;
            set => SetProperty(ref progress2, value);
        }

        private string progressText = "90s";
        public string ProgressText
        {
            get => progressText;
            set => SetProperty(ref progressText, value);
        }

        private bool isVisible = false;
        public bool IsVisible
        {
            get => isVisible && Settings.ShowCountdown;
            set => SetProperty(ref isVisible, value);
        }

        public Countdown()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;
        }

        public void Start()
        {
            startTime = DateTime.Now;
            timer.Stop();
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                NotifyPropertyChanged(propertyName);
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void Timer_Tick(object sender, EventArgs e)
        {
            var duration = TimeSpan.FromSeconds(90) - (DateTime.Now - startTime);
            if (duration <= TimeSpan.Zero)
            {
                timer.Stop();
                Progress1 = 0;
                Progress2 = 0;
                ProgressText = "0s";
            }
            else if (duration <= TimeSpan.FromSeconds(15))
            {
                Progress1 = 0;
                Progress2 = duration.TotalMilliseconds / 150;
                ProgressText = $"{Math.Round(duration.TotalMilliseconds / 1000)}s";
            }
            else
            {
                Progress1 = (duration - TimeSpan.FromSeconds(15)).TotalMilliseconds / 750;
                Progress2 = 100;
                ProgressText = $"{Math.Round(duration.TotalMilliseconds / 1000)}s";
            }
        }
    }
}
