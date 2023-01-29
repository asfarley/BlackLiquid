using System;
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
using System.Collections.ObjectModel;
using System.Xml.Linq;

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
            set { state = value; OnPropertyChanged(); }
        }

        private SimulationWorld world = new SimulationWorld();

        public SimulationWorld World
        {
            get {  return world; }
            set { world = value; OnPropertyChanged(); }
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


            private double zoomFactor = 1.0;

            public double ZoomFactor
            {
                get { return zoomFactor; }
                set
                {
                    zoomFactor = value;
                    OnPropertyChanged();
                }
            }

            private double canvasWidth = 10000;
            public double CanvasWidth
            {
                get { return canvasWidth; }
                set { canvasWidth = value; OnPropertyChanged(); }
            }

            private double canvasHeight = 6000;
            public double CanvasHeight
            {
                get { return canvasHeight; }
                set { canvasHeight = value; OnPropertyChanged(); }
            }

            private double viewWidth = 1920;
            public double ViewWidth
            {
                get { return viewWidth; }
                set { viewWidth = value; OnPropertyChanged();  }
            }

            private double viewHeight = 1080;
            public double ViewHeight
            {
                get { return viewHeight; }
                set { viewHeight = value; OnPropertyChanged();  }
            }

            private double mouseXScreenCoordinates = 0;
            public double MouseXScreenCoordinates
            {
                get { return mouseXScreenCoordinates; }
                set { mouseXScreenCoordinates = value; OnPropertyChanged(); }
            }

            private double mouseYScreenCoordinates = 0;
            public double MouseYScreenCoordinates
            {
                get { return mouseYScreenCoordinates; }
                set { mouseYScreenCoordinates = value; OnPropertyChanged(); }
            }

            private double mouseXCanvasCoordinates = 0;
            public double MouseXCanvasCoordinates
            {
                get { return mouseXCanvasCoordinates; }
                set { mouseXCanvasCoordinates = value; OnPropertyChanged(); }
            }

            private double mouseYCanvasCoordinates = 0;
            public double MouseYCanvasCoordinates
            {
                get { return mouseYCanvasCoordinates; }
                set { mouseYCanvasCoordinates = value; OnPropertyChanged(); }
            }

            private int translateX = 0;

            public int TranslateX
            {
                get { return translateX; }
                set
                {
                    translateX = value;
                    OnPropertyChanged();
                }
            }

            private int translateY = 0;

            public int TranslateY
            {
                get { return translateY; }
                set
                {
                    translateY = value;
                    OnPropertyChanged();
                }
            }

            private int snapPixels = 32;

            public int SnapPixels
            {
                get { return snapPixels; }
                set
                {
                    snapPixels = value;
                    OnPropertyChanged();
                }
            }

            private int translateXStartDrag = 0;

            private int translateYStartDrag = 0;

            private double dragOffsetX = 0.0;

            public double DragOffsetX
            {
                get { return dragOffsetX; }
                set
                {
                    dragOffsetX = value;
                    OnPropertyChanged();
                }
            }

            private double dragOffsetY = 0.0;

            public double DragOffsetY
            {
                get { return dragOffsetY; }
                set
                {
                    dragOffsetY = value;
                    OnPropertyChanged();
                }
            }

            private bool middleMouseDragging = false;

            public bool MiddleMouseDragging
            {
                get { return middleMouseDragging; }
                set
                {
                    middleMouseDragging = value;
                    OnPropertyChanged();
                }
            }

            private bool leftMouseDragging = false;

            public bool LeftMouseDragging
            {
                get { return leftMouseDragging; }
                set
                {
                    leftMouseDragging = value;
                    OnPropertyChanged();
                }
            }


            private Canvas mainCanvas;

            private System.Windows.Point dragStartClickPointView;
            private System.Windows.Point dragStartClickPointCanvas;

            private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
            {
                var viewHeightCanvasCoordinates = ViewHeight / ZoomFactor;
                var viewWidthCanvasCoordinates = ViewWidth / ZoomFactor;

                var canvasCenterXBefore = (viewWidthCanvasCoordinates / 2.0) - TranslateX;
                var canvasCenterYBefore = (viewHeightCanvasCoordinates / 2.0) - TranslateY;

                ZoomFactor *= (e.Delta > 0) ? 1.05 : 1.0 / 1.05;

                viewHeightCanvasCoordinates = ViewHeight / ZoomFactor;
                viewWidthCanvasCoordinates = ViewWidth / ZoomFactor;

                var canvasCenterX = (viewWidthCanvasCoordinates / 2.0) - TranslateX;
                var canvasCenterY = (viewHeightCanvasCoordinates / 2.0) - TranslateY;

                var canvasCenterXOffset = canvasCenterX - canvasCenterXBefore;
                var canvasCenterYOffset = canvasCenterY - canvasCenterYBefore;

                var xAdjustment = canvasCenterXOffset;
                var yAdjustment = canvasCenterYOffset;

                TranslateX = TranslateX + (int)xAdjustment;
                TranslateY = TranslateY + (int)yAdjustment;
            }

            private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
            {
                e.Handled = true;

                var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                e2.RoutedEvent = ListBox.MouseWheelEvent;
                e2.Source = e.Source;

                CanvasListBox.RaiseEvent(e2);
            }

            private void ListBox_MouseMove(object sender, MouseEventArgs e)
            {
                var mousePos = e.GetPosition(CanvasListBox);
                MouseXScreenCoordinates = mousePos.X;
                MouseYScreenCoordinates = mousePos.Y;

                var mousePosition = e.GetPosition(mainCanvas);
                MouseXCanvasCoordinates = mousePosition.X;
                MouseYCanvasCoordinates = mousePosition.Y;

                if (MiddleMouseDragging)
                {
                    DragOffsetX = MouseXScreenCoordinates - dragStartClickPointView.X;
                    DragOffsetY = MouseYScreenCoordinates - dragStartClickPointView.Y;
                    var dragOffsetXCanvasCoordinates = DragOffsetX / ZoomFactor;
                    var dragOffsetYCanvasCoordinates = DragOffsetY / ZoomFactor;
                    TranslateX = translateXStartDrag + (int)dragOffsetXCanvasCoordinates;
                    TranslateY = translateYStartDrag + (int)dragOffsetYCanvasCoordinates;
                }
            }

            private void Canvas_MouseMove(object sender, MouseEventArgs e)
            {
                var canvas = (Canvas)sender;
                var mousePosition = e.GetPosition(canvas);
                MouseXCanvasCoordinates = mousePosition.X;
                MouseYCanvasCoordinates = mousePosition.Y;

                if (MiddleMouseDragging)
                {
                    DragOffsetX = MouseXScreenCoordinates - dragStartClickPointView.X;
                    DragOffsetY = MouseYScreenCoordinates - dragStartClickPointView.Y;
                    var dragOffsetXCanvasCoordinates = DragOffsetX / ZoomFactor;
                    var dragOffsetYCanvasCoordinates = DragOffsetY / ZoomFactor;
                    TranslateX = translateXStartDrag + (int)dragOffsetXCanvasCoordinates;
                    TranslateY = translateYStartDrag + (int)dragOffsetYCanvasCoordinates;
                }
            }

            private void Canvas_MouseUp(object sender, MouseEventArgs e)
            {
                ReleaseAll();
            }

            private void Canvas_MouseDown(object sender, MouseEventArgs e)
            {
                dragStartClickPointView.X = MouseXScreenCoordinates;
                dragStartClickPointView.Y = MouseYScreenCoordinates;

                dragStartClickPointCanvas.X = MouseXCanvasCoordinates;
                dragStartClickPointCanvas.Y = MouseYCanvasCoordinates;

                translateXStartDrag = TranslateX;
                translateYStartDrag = TranslateY;

                CanvasListBox.UnselectAll();

                if (e.MiddleButton == MouseButtonState.Pressed)
                {
                    MiddleMouseDragging = true;
                }
            }

            

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }

            private void ReleaseAll()
            {
               
                LeftMouseDragging = false;
                MiddleMouseDragging = false;
            }

            private void MainCanvas_Loaded(object sender, RoutedEventArgs e)
            {
                mainCanvas = (Canvas)sender;
            }

          
           
            private int RoundToSnappedValue(int pixel)
            {
                return SnapPixels * (int)Math.Round(pixel / (float)SnapPixels);
            }

    private void updateTimer_Tick(object sender, EventArgs e)
        {
            // code goes here
            World.Update();

            OnPropertyChanged("World");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            updateTimer.Stop();
            World.Initialize();
            updateTimer.Start();
        }
    }
}
