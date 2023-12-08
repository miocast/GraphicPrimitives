using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            rectangle = new Rectangle(250, 200, Brushes.Blue, Pens.Black);
            triangle = new Triangle(100, 30, Brushes.Green, Pens.Black);
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
    }

    class Triangle : Shape
    {
        private int SideLength { get; set; }
        private int Angle { get; set; }
        public Triangle(int sideLength, int angle, Brush fillColor, Pen borderColor)
        {
            SideLength = sideLength;
            Angle = angle;
            FillColor = fillColor;
            BorderColor = borderColor;
        }

        public override void Draw(Graphics graphics)
        {
            Point[] points = CalculateTrianglePoints();
            graphics.FillPolygon(FillColor, points);
            graphics.DrawPolygon(BorderColor, points);
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
