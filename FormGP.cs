using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
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
        private List<Shape> shapes;
        private Shape selectedShape;
        private Point lastMouseLocation;

        //private Circle circle;
        //private Rectangle rectangle;
        //private Triangle triangle;

        //штучка на попозже
        private bool isResizing;



        public DrawingCanvas()
        {
            shapes = new List<Shape>
            {
                new Circle(10, 10, 100, 100, 50, Brushes.Red, Pens.Black),
                new Rectangle(60, 60, 60, 80, Brushes.Blue, Pens.Black),
                new Triangle(200, 200, 100, Brushes.Green, Pens.Black)
            };


            MouseDown += DrawingCanvas_MouseDown;
            MouseMove += DrawingCanvas_MouseMove;
            MouseUp += DrawingCanvas_MouseUp;
        }

        private void Shape_OnShapeClicked(object sender, EventArgs e)
        {
            selectedShape = (Shape)sender;
            lastMouseLocation = Cursor.Position;
        }

        private void DrawingCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (Shape shape in shapes)
            {
                if (shape.ContainsPoint(e.Location))
                {
                    selectedShape = shape;
                    isResizing = true;
                    Capture = true;
                    lastMouseLocation = e.Location;
                    return; // Выходим, чтобы не проверять другие фигуры после того, как одна была найдена
                }
            }
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Capture && selectedShape != null)
            {
                int deltaX = e.X - lastMouseLocation.X;
                int deltaY = e.Y - lastMouseLocation.Y;

                if (isResizing)
                {
                    if (selectedShape is Circle circle)
                    {
                        int newRadius = Math.Max(circle.Radius + deltaX, 0);
                        circle.Resize(newRadius);
                    }

                    else if (selectedShape is Rectangle rectangle)
                    {
                        int newWidth = Math.Max(rectangle.Width + deltaX, 0);
                        int newHeight = Math.Max(rectangle.Height + deltaY, 0);
                        rectangle.Resize(newWidth, newHeight);
                    }

                    Invalidate(); // Перерисовываем контрол после перемещения
                }
                else
                {
                    // Перемещение
                    selectedShape.Move(deltaX, deltaY);
                    Invalidate();
                }

                lastMouseLocation = e.Location;

            }
            
        }

        private void DrawingCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            // Освобождаем захват мыши
            Capture = false;
            selectedShape = null;
            isResizing = false;
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.White);

            foreach (Shape shape in shapes)
            {
                shape.Draw(e.Graphics);
            }
        }
    }




    abstract class Shape
    {
        protected Brush FillColor { get; set; }
        protected Pen BorderColor { get; set; }

        //вот это я зачем сделала?? 
        public int Width { get; set; }
        public int Height { get; set; }

        public abstract void Draw(Graphics graphics);
        public abstract bool ContainsPoint(Point point);

        public abstract void Move(int deltaX, int deltaY);
    }

    class Circle : Shape
    {
        public int Radius { get; set; }
        private Point Location { get; set; }

        public Circle(int x, int y, int width, int height, int radius, Brush fillColor, Pen borderColor)
        {
            Location = new Point(x, y);
            Width = width;
            Height = height;
            Radius = radius;
            FillColor = fillColor;
            BorderColor = borderColor;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.FillEllipse(FillColor, Location.X - Radius, Location.Y - Radius, Radius * 2, Radius * 2);
            graphics.DrawEllipse(BorderColor, Location.X - Radius, Location.Y - Radius, Radius * 2, Radius * 2);

        }

        public override bool ContainsPoint(Point point)
        {
            int distanceFromCenter = (int)Math.Sqrt(Math.Pow(point.X - Location.X, 2) + Math.Pow(point.Y - Location.Y, 2));
            return distanceFromCenter <= Radius;
        }

        public override void Move(int deltaX, int deltaY)
        {
            Location = new Point(Location.X + deltaX, Location.Y + deltaY);
        }

        public void Resize(int newRadius)
        {
            Radius = newRadius;
        }
    }

    class Rectangle : Shape
    {
        private Point Location { get; set; }
        public Rectangle(int x, int y, int width, int height, Brush fillColor, Pen borderColor)
        {
            Location = new Point(x, y);
            Width = width;
            Height = height;
            FillColor = fillColor;
            BorderColor = borderColor;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(FillColor, Location.X, Location.Y, Width, Height);
            graphics.DrawRectangle(BorderColor, Location.X, Location.Y, Width, Height);
        }

        public override bool ContainsPoint(Point point)
        {
            return point.X >= Location.X && point.X <= Location.X + Width && point.Y >= Location.Y && point.Y <= Location.Y + Height;
        }

        public override void Move(int deltaX, int deltaY)
        {
            Location = new Point(Location.X + deltaX, Location.Y + deltaY);
        }
        public void Resize(int newWidth, int newHeight)
        {
            Width = newWidth;
            Height = newHeight;
        }

    }

    class Triangle : Shape
    {
        private Point[] Points { get; set; }
        private int SideLength { get; set; }

        public Triangle(int x, int y, int sideLength, Brush fillColor, Pen borderColor)
        {
            SideLength = sideLength;
            FillColor = fillColor;
            BorderColor = borderColor;
            CalculateTrianglePoints(x, y);
        }

        private void CalculateTrianglePoints(int x, int y)
        {
            Points = new Point[3];
            int halfSideLength = SideLength / 2;

            Points[0] = new Point(x, y);
            Points[1] = new Point(x + SideLength, y);
            Points[2] = new Point(x + halfSideLength, y + (int)(Math.Sqrt(3) * halfSideLength));
        }

        public override void Draw(Graphics graphics)
        {
            graphics.FillPolygon(FillColor, Points);
            graphics.DrawPolygon(BorderColor, Points);
        }

        public override bool ContainsPoint(Point point)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddPolygon(Points);
                return path.IsVisible(point);
            }
        }

        public override void Move(int deltaX, int deltaY)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = new Point(Points[i].X + deltaX, Points[i].Y + deltaY);
            }
        }
    }
}
