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
        private Circle circle;
        private Rectangle rectangle;
        //private Triangle triangle;

        public FormGP()
        {
            InitializeComponent();
        }

        private void FormGP_Paint(object sender, PaintEventArgs e)
        {

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
}
