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
        private ProtractorPoint point1 = new ProtractorPoint();
        private ProtractorPoint point2 = new ProtractorPoint();

        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
            Loaded += Ready;
        }

        private void Ready(object sender, RoutedEventArgs e)
        {

            Pos1.Text = Circle1.TransformToAncestor(ProtractorGrid).Transform(new Point(0, 0)).ToString();
            Pos2.Text = Circle2.TransformToAncestor(ProtractorGrid).Transform(new Point(0, 0)).ToString();
            MouseDown(Circle1, point1);
            MouseUp(Circle1, point1,CirclePoint1);
        }

        private void CloseButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void MouseDown(Button Circle, ProtractorPoint point)
        {
            if (point.pos == null)
                point.pos = Circle.TransformToAncestor(ProtractorGrid).Transform(new Point(0, 0));
            var mousePosition = Mouse.GetPosition(ProtractorGrid);
            point.deltaX = mousePosition.X - point.pos.Value.X;
            point.deltaY = mousePosition.Y - point.pos.Value.Y;
            point.isMoving = true;
        }
        private void MouseUp(Button Circle, ProtractorPoint point, Ellipse CirclePoint)
        {
            point.isMoving = false;

            var circlePos = CirclePoint.TransformToAncestor(ProtractorGrid).Transform(new Point(0, 0));

            var offsetX = point.pos.Value.X - circlePos.X + (22);
            var offsetY = point.pos.Value.Y - circlePos.Y + (10);

            Circle.RenderTransform = new TranslateTransform(-offsetX, -offsetY);

            point.currentTT = Circle.RenderTransform as TranslateTransform;


        }
        private void MouseMove(Button Circle, ProtractorPoint point, TextBlock Pos, Ellipse CirclePoint)
        {
            if (!point.isMoving) return;

            var mousePoint = Mouse.GetPosition(ProtractorGrid);

            var offsetX = (point.currentTT == null ? point.pos.Value.X : point.pos.Value.X - point.currentTT.X) + point.deltaX - mousePoint.X;
            var offsetY = (point.currentTT == null ? point.pos.Value.Y : point.pos.Value.Y - point.currentTT.Y) + point.deltaY - mousePoint.Y;

            var trueX = point.pos.Value.X - offsetX;
            var trueY = point.pos.Value.Y - offsetY;
        
            var min = 0;
            var max = 450;
            if (trueX > max) { offsetX += (trueX - max); trueX = max; }
            if (trueY > max) { offsetY += (trueY - max); trueY = max; }
            if (trueX < min) { offsetX += (trueX); trueX = min; }
            if (trueY < min) { offsetY += (trueY); trueY = min; }


            trueX -= (450 / 2);
            trueY -= (450 / 2);

            Pos.Text = trueX.ToString() + " , " + trueY.ToString() +": "+ ProtractorGrid.ActualHeight;

            Circle.RenderTransform = new TranslateTransform(-offsetX, -offsetY);

            PositionCircle(CirclePoint, trueX, trueY);
        }

        private void PositionCircle(Ellipse circle, double X, double Y)
        {
            
            var angle = Math.Atan(X/Y);

            var radius = UnitCircle.ActualHeight / 2;

            var newY = Math.Cos(angle) * radius;
            var newX = Math.Sin(angle) * radius;

            if(Y<0)
            {
                newX *= -1;
                newY *= -1;
            }

            circle.RenderTransform = new TranslateTransform(newX, newY);
        }
       //TODO: Draw Line

        private void Circle1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown(Circle1, point1);
        }

        private void Circle1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUp(Circle1, point1,CirclePoint1);
        }

        private void Circle1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMove(Circle1, point1, Pos1,CirclePoint1);
        }

        private void Circle2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown(Circle2, point2);
        }

        private void Circle2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUp(Circle2, point2,CirclePoint2);
        }

        private void Circle2_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMove(Circle2, point2, Pos2,CirclePoint2);
        }
    }

    public partial class ProtractorPoint
    {
        public bool isMoving;
        public Point? pos;
        public double deltaX;
        public double deltaY;
        public TranslateTransform currentTT;

        public ProtractorPoint()
        {
            isMoving = false;
            pos = null;
            deltaX = 0;
            deltaY = 0;
            currentTT = null;
        }
    }
}
