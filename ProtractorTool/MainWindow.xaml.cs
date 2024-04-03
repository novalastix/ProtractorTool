using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProtractorTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPoint1Moving = false;
        private bool isPoint2Moving = false;

        private bool debug = false;

        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            Loaded += Ready;
        }

        private void Ready(object sender, RoutedEventArgs e)
        {
            TaskbarIcon.Visibility = Visibility.Collapsed;

            //Set Starting Position
            MoveObject(UnitCircle, 0, 0);
            MoveObject(Circle1, -(UnitCircle.ActualHeight / 2), 0);
            MoveObject(CirclePoint1, -(UnitCircle.ActualHeight / 2), 0);
            MoveObject(Circle2, UnitCircle.ActualHeight / 2, 0);
            MoveObject(CirclePoint2, UnitCircle.ActualHeight / 2, 0);

           
            Pos2.Text = GetX(Circle2) + "," + GetY(Circle2);

            UpdateText(Pos1, Circle1);
            UpdateText(Pos2, Circle2);

            Line1.X1 = (ProtractorCanvas.ActualWidth / 2);
            Line1.Y1 = (ProtractorCanvas.ActualHeight / 2);
            Line2.X1 = (ProtractorCanvas.ActualWidth / 2);
            Line2.Y1 = (ProtractorCanvas.ActualHeight / 2);

            SetLine(CirclePoint1, Line1);
            SetLine(CirclePoint2, Line2);

            UpdateAngle();

            if(debug)
            {

            }
        }

        private double GetX(FrameworkElement obj)
        {
            return Canvas.GetLeft(obj) + (obj.ActualWidth / 2) - (ProtractorCanvas.ActualWidth / 2);
        }

        private double GetY(FrameworkElement obj)
        {
            return Canvas.GetTop(obj) + (obj.ActualHeight / 2) - (ProtractorCanvas.ActualHeight / 2);
        }

        private void MoveObject(FrameworkElement obj, double X, double Y)
        {
            Canvas.SetLeft(obj, X-(obj.ActualWidth/2) + (ProtractorCanvas.ActualWidth / 2));
            Canvas.SetTop(obj, Y- (obj.ActualHeight / 2) + (ProtractorCanvas.ActualHeight / 2));
        }

        private void CloseButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void HideButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            TaskbarIcon.Visibility = Visibility.Visible;
            this.Hide();
        }


        private void MouseDown(Button Circle,ref bool moving)
        {
            moving = true;
        }
        private void MouseUp(Button Circle, ref bool moving, Ellipse CirclePoint, TextBlock Pos)
        {
            moving = false;

            MoveObject(Circle, GetX(CirclePoint), GetY(CirclePoint));

            UpdateText(Pos, Circle);
        }

        private void MouseMove(Button Circle, ref bool moving, TextBlock Pos, Ellipse CirclePoint, Line line)
        {
            if (!moving) return;

            var mousePoint = Mouse.GetPosition(ProtractorCanvas);

            var X = mousePoint.X - (ProtractorCanvas.ActualWidth / 2);
            var Y = mousePoint.Y - (ProtractorCanvas.ActualHeight / 2);

            MoveObject(Circle, X, Y);
            PositionCircle(CirclePoint, Circle);
            SetLine(CirclePoint, line);

            UpdateText(Pos, Circle);
            UpdateAngle();
        }

        private void UpdateAngle()
        {
            var Point1X = GetX(CirclePoint1);
            var Point1Y = GetY(CirclePoint1);
            var Point2X = GetX(CirclePoint2);
            var Point2Y = GetY(CirclePoint2);

            double dotProduct = Point1X * Point2X + Point1Y * Point2Y;

            double magnitude1 = Math.Sqrt(Math.Pow(Point1X, 2) + Math.Pow(Point1Y, 2));
            double magnitude2 = Math.Sqrt(Math.Pow(Point2X, 2) + Math.Pow(Point2Y, 2));

            double angle = Math.Acos(dotProduct / (magnitude1 * magnitude2));

            angle *= (180.0 / Math.PI);

            Angle.Text = Math.Round(angle,2) + "°" ;
        }

        private void UpdateText(TextBlock pos, Button circle)
        {
            pos.Text = (int) GetX(circle) + "," + (int)GetY(circle);
        }

        private void PositionCircle(Ellipse circle, Button button)
        {
            var X = GetX(button);
            var Y = GetY(button);
            
            var angle = Math.Atan(X/Y);

            var radius = UnitCircle.ActualHeight / 2;

            var newY = Math.Cos(angle) * radius;
            var newX = Math.Sin(angle) * radius;

            if(Y<0)
            {
                newX *= -1;
                newY *= -1;
            }

            MoveObject(circle, newX, newY);
        }
       
        private void SetLine(Ellipse circle, Line line)
        {
            line.X2 = GetX(circle) + (ProtractorCanvas.ActualWidth / 2);
            line.Y2 = GetY(circle) + (ProtractorCanvas.ActualHeight / 2);
        }

        private void Circle1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown(Circle1, ref isPoint1Moving);
        }

        private void Circle1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUp(Circle1, ref isPoint1Moving,CirclePoint1, Pos1);
        }

        private void Circle1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMove(Circle1, ref isPoint1Moving, Pos1,CirclePoint1,Line1);
        }

        private void Circle2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown(Circle2, ref isPoint2Moving);
        }

        private void Circle2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUp(Circle2, ref isPoint2Moving,CirclePoint2, Pos2);
        }

        private void Circle2_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMove(Circle2, ref isPoint2Moving, Pos2,CirclePoint2,Line2);
        }

        private void TaskbarOpen(object sender, RoutedEventArgs e)
        {
            this.Show();
            Ready(sender,e);
        }

        private void TaskbarExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
