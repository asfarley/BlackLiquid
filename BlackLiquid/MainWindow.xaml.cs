﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Threading;

namespace BlackLiquid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Image state;



        public Image State
        {
            get { return state; }
            set { state = value; NotifyPropertyChanged(); }
        }

        private SimulationWorld world = new SimulationWorld();

        public SimulationWorld World
        {
            get {  return world; }
            set { world = value; NotifyPropertyChanged(); }
        }

        public int ImageWidth
        {
            get
            {
                return GlobalConstants.Width;
            }
        }

        public int ImageHeight
        {
            get
            {
                return GlobalConstants.Width;
            }
        }

        private DispatcherTimer updateTimer;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            World.Initialize();

            updateTimer = new System.Windows.Threading.DispatcherTimer();
            updateTimer.Tick += updateTimer_Tick;
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            updateTimer.Start();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            // code goes here
            World.Update();

            NotifyPropertyChanged("World");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            updateTimer.Stop();
            World.Initialize();
            updateTimer.Start();
        }
    }
}
