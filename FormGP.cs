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

}
