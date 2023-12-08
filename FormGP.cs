using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GraphicPrimitives
{
    public partial class FormGP : Form
    {
        private DrawingCanvas drawingCanvas;

        public FormGP()
        {
            InitializeComponent();

            drawingCanvas = new DrawingCanvas();
            drawingCanvas.Dock = DockStyle.Fill;
            Controls.Add(drawingCanvas);
        }
    }

    public class DrawingCanvas : Control
    {

        private Circle circle;
        private Rectangle rectangle;
        private Triangle triangle;
 

        public DrawingCanvas()
        {
            circle = new Circle(100, 100, 50, Brushes.Red, Pens.Black);
            rectangle = new Rectangle(60, 80, Brushes.Blue, Pens.Black);
            triangle = new Triangle(100, Brushes.Green, Pens.Black);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.White);

            circle.Draw(e.Graphics);
            rectangle.Draw(e.Graphics);
            triangle.Draw(e.Graphics);
        }
    }

    abstract class Shape
    {
        protected Brush FillColor { get; set; }
        protected Pen BorderColor { get; set; }
        protected int Width { get; set; }
        protected int Height { get; set; }

        public abstract void Draw(Graphics graphics);
        public abstract bool ContainsPoint(Point point);
    }

    class Circle : Shape
    {
        private int Radius { get; set; }
        public Circle(int width, int height, int radius, Brush fillColor, Pen borderColor)
        {
            Width = width;
            Height = height;
            Radius = radius;
            FillColor = fillColor;
            BorderColor = borderColor;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.FillEllipse(FillColor, 100, 100, Radius * 2, Radius * 2);
            graphics.DrawEllipse(BorderColor, 100, 100, Radius * 2, Radius * 2);
        }

        public override bool ContainsPoint(Point point)
        {
            int distanceFromCenter = (int)Math.Sqrt(Math.Pow(point.X - Radius, 2) + Math.Pow(point.Y - Radius, 2));
            return distanceFromCenter <= Radius;
        }
    }

    class Rectangle : Shape
    {
        public Rectangle(int width, int height, Brush fillColor, Pen borderColor)
        {
            Width = width;
            Height = height;
            FillColor = fillColor;
            BorderColor = borderColor;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(FillColor, 250, 200, Width, Height);
            graphics.DrawRectangle(BorderColor, 250, 200, Width, Height);
        }

        public override bool ContainsPoint(Point point)
        {
            return point.X >= 0 && point.X <= Width && point.Y >= 0 && point.Y <= Height;
        }
    }

    class Triangle : Shape
    {
        private int SideLength { get; set; }
        public Triangle(int sideLength, Brush fillColor, Pen borderColor)
        {
            SideLength = sideLength;
            FillColor = fillColor;
            BorderColor = borderColor;
        }

        public override void Draw(Graphics graphics)
        {
            Point[] points = CalculateTrianglePoints();
            graphics.FillPolygon(FillColor, points);
            graphics.DrawPolygon(BorderColor, points);
        }

        public override bool ContainsPoint(Point point)
        {
            Point[] points = CalculateTrianglePoints();

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddPolygon(points);
                return path.IsVisible(point);
            }
        }

        private Point[] CalculateTrianglePoints()
        {
            Point[] points = new Point[3];
            int halfSideLength = SideLength / 2;

            points[0] = new Point(0, 0);
            points[1] = new Point(SideLength, 0);
            points[2] = new Point(halfSideLength, (int)(Math.Sqrt(3) * halfSideLength));

            return points;
        }
    }
}
