using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GraphicPrimitives
{
    public partial class FormGP : Form
    {
        private DrawingCanvas drawingCanvas; // Объявление переменной для хранения экземпляра класса DrawingCanvas

        public FormGP()
        {
            InitializeComponent();

            drawingCanvas = new DrawingCanvas(); // Создание экземпляра класса DrawingCanvas
            drawingCanvas.Dock = DockStyle.Fill; // Заполнение всего доступного пространства формы
            Controls.Add(drawingCanvas); // Добавление DrawingCanvas на форму
        }

        // Кнопка удаления соединений примитивов линиями
        private void buttonClearLines_Click(object sender, EventArgs e)
        {
            drawingCanvas.ClearLines(); // Удаление линий
            drawingCanvas.Invalidate(); // Перерисовка холста
        }
    }

    // Класс компонента, в котором будут рисоваться примитивы
    public class DrawingCanvas : Control
    {
        private List<Shape> shapes; // Список примитивов
        private Shape selectedShape; // Выбранный примитив
        private Point lastMouseLocation; // Последнее расположение мыши

        private List<Line> lines; // Список линий 
        private Line drawingLine; // Рисуемая линия

        private bool isResizing; // Флаг изменения размера примитива
        private bool isMoving; // Флаг перемещения примитива

        // Конструктор класса холста
        public DrawingCanvas()
        {
            lines = new List<Line>(); // Инициализация списка линий

            // Инициализация списка примитивов
            shapes = new List<Shape>
            {
                new Circle(340, 130, 50, Brushes.DeepPink, Pens.Black),
                new Rectangle(200, 150, 80, 80, Brushes.SlateBlue, Pens.Black),
                new Triangle(400, 150, 100, Brushes.GreenYellow, Pens.Black)
            };

            // Обработчики событий мыши
            MouseDown += DrawingCanvas_MouseDown;
            MouseMove += DrawingCanvas_MouseMove;
            MouseUp += DrawingCanvas_MouseUp;
        }

        // Обработчик нажатия кнопки мыши
        private void DrawingCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            // Рисование начала линии при нажатии колесика мыши
            if (e.Button == MouseButtons.Middle)
            {
                drawingLine = new Line(e.Location, e.Location, Pens.Black);
            }

            // Проверка находится ли курсор мыши в примитиве
            foreach (Shape shape in shapes)
            {
                if (shape.ContainsPoint(e.Location))
                {
                    selectedShape = shape; // Установка выбранной фигуры
                    lastMouseLocation = e.Location; // Текущее положение мыши

                    // Проверка для определения начала изменения размера
                    if (e.Button == MouseButtons.Left)
                    {
                        isResizing = true;
                    }

                    // Проверка для определения начала перемещения
                    if (e.Button == MouseButtons.Right)
                    {
                        isMoving = true;
                    }
                }
            }
        }

        // Обработчик движения мыши
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Перемещение и изменение размера выбранного примитива
            if (selectedShape != null)
            {
                int deltaX = e.X - lastMouseLocation.X;
                int deltaY = e.Y - lastMouseLocation.Y;

                // Изменение размера примитивов
                if (isResizing)
                {
                    // Прямоугольник
                    if (selectedShape is Rectangle rectangle)
                    {
                        int newWidth = Math.Max(rectangle.Width + deltaX, 0);
                        int newHeight = Math.Max(rectangle.Height + deltaY, 0);
                        rectangle.Resize(newWidth, newHeight);
                    }

                    // Круг
                    else if (selectedShape is Circle circle)
                    {
                        int newRadius = Math.Max(circle.Radius + deltaX, 0);
                        circle.Resize(newRadius);
                    }

                    // Равносторонний треугольник
                    else if (selectedShape is Triangle triangle)
                    {
                        int newSideLength = Math.Max(triangle.SideLength + deltaX, 0);
                        triangle.Resize(newSideLength);
                    }

                    Invalidate(); // Перерисовка холста
                }

                // Перемещение примитива
                else if (isMoving)
                {
                    selectedShape.Move(deltaX, deltaY);
                    Invalidate(); // Перерисовка холста
                }

                lastMouseLocation = e.Location; // Обновление последней позиции мыши
            }

            // Конец линии
            if (drawingLine != null && e.Button == MouseButtons.Middle)
            {
                drawingLine.EndPoint = e.Location;
                Invalidate(); // Перерисовка холста
            }
        }

        private void DrawingCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            // Завершение рисования линии при отпускании колесика мыши
            if (drawingLine != null && e.Button == MouseButtons.Middle)
            {
                lines.Add(drawingLine); // Добавление линии в список
                drawingLine = null; // Обнуление текущей линии
                Invalidate(); // Перерисовка холста
            }

            // Освобождение захвата мыши
            selectedShape = null; // Обнуляем выбранный примитив
            isResizing = false; // Сброс флага изменения размера
            isMoving = false; // Сброс флага перемещения
        }

        // Метод отрисовки 
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); // Вызов метода для отрисовки

            e.Graphics.Clear(Color.White); // Очистка области рисования

            foreach (Shape shape in shapes)
            {
                shape.Draw(e.Graphics); // Отрисовка всех примитивов из списка
            }

            foreach (Line line in lines)
            {
                line.Draw(e.Graphics); // Отрисовка всех линий из списка
            }
        }

        // Метод для очистки списка линий
        public void ClearLines()
        {
            lines.Clear(); // Очищаем список линий
        }
    }

    // Базовый класс для графических примитивов
    abstract class Shape
    {
        protected Brush FillColor { get; set; } // Кисть для заливки
        protected Pen BorderColor { get; set; } // Перо для отрисовки рамки
        public int Width { get; set; } // Ширина
        public int Height { get; set; } // Высота

        public abstract void Draw(Graphics graphics); // Метод для отрисовки примитива
        public abstract bool ContainsPoint(Point point); // Метод для проверки находится ли точка внутри примитива
        public abstract void Move(int deltaX, int deltaY); // Метод для перемещения фигур
    }

    // Класс прямоугольника
    class Rectangle : Shape
    {
        private Point Location { get; set; } // Расположение 

        // Конструктор класса прямоугольника
        public Rectangle(int x, int y, int width, int height, Brush fillColor, Pen borderColor)
        {
            Location = new Point(x, y); // Расположение верхней левой точки
            Width = width; // Ширина прямоугольника
            Height = height; // Высота прямоугольника 
            FillColor = fillColor; // Цвет заливки
            BorderColor = borderColor; // Цвет рамки
        }

        // Метод отрисовки прямоугольника
        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(FillColor, Location.X, Location.Y, Width, Height); // Рисует заливку
            graphics.DrawRectangle(BorderColor, Location.X, Location.Y, Width, Height); // Рисует рамку
        }

        // Проверка находится ли точка внутри прямоугольника
        public override bool ContainsPoint(Point point)
        {
            return point.X >= Location.X && point.X <= Location.X + Width && point.Y >= Location.Y && point.Y <= Location.Y + Height;
        }

        // Перемещение прямоугольника
        public override void Move(int deltaX, int deltaY)
        {
            Location = new Point(Location.X + deltaX, Location.Y + deltaY);
        }

        // Изменение размера прямоугольника
        public void Resize(int newWidth, int newHeight)
        {
            Width = newWidth;
            Height = newHeight;
        }
    }

    // Класс круга
    class Circle : Shape
    {
        public int Radius { get; set; } // Радиус
        private Point Location { get; set; } // Расположение

        // Конструктор класса круга
        public Circle(int x, int y, int radius, Brush fillColor, Pen borderColor)
        {
            Location = new Point(x, y); // Центр круга
            Radius = radius; // Радиус
            FillColor = fillColor; // Цвет заливки
            BorderColor = borderColor; // Цвет рамки
        }

        // Отрисовка круга
        public override void Draw(Graphics graphics)
        {
            graphics.FillEllipse(FillColor, Location.X - Radius, Location.Y - Radius, Radius * 2, Radius * 2); // Рисует заливку
            graphics.DrawEllipse(BorderColor, Location.X - Radius, Location.Y - Radius, Radius * 2, Radius * 2); // Рисует рамку
        }

        // Проверяет лежит ли точка внутри круга
        public override bool ContainsPoint(Point point)
        {
            int distanceFromCenter = (int)Math.Sqrt(Math.Pow(point.X - Location.X, 2) + Math.Pow(point.Y - Location.Y, 2));
            return distanceFromCenter <= Radius;
        }

        // Перемещение круга
        public override void Move(int deltaX, int deltaY)
        {
            Location = new Point(Location.X + deltaX, Location.Y + deltaY);
        }

        // Изменение размера круга
        public void Resize(int newRadius)
        {
            Radius = newRadius;
        }
    }

    // Класс равностороннего треугольника
    class Triangle : Shape
    {
        private Point[] Points { get; set; } // Массив для вершин треугольника
        public int SideLength { get; set; } // Сторона треугольника

        // Конструктор класса треугольника
        public Triangle(int x, int y, int sideLength, Brush fillColor, Pen borderColor)
        {
            SideLength = sideLength;
            FillColor = fillColor;
            BorderColor = borderColor;
            CalculateTrianglePoints(x, y);
        }

        // Вычисление координат вершин треугольника
        private void CalculateTrianglePoints(int x, int y)
        {
            Points = new Point[3];
            int halfSideLength = SideLength / 2;

            // Вершины треугольника
            Points[0] = new Point(x, y);
            Points[1] = new Point(x + SideLength, y);
            Points[2] = new Point(x + halfSideLength, y + (int)(Math.Sqrt(3) * halfSideLength));

        }

        // Отрисовка треугольника
        public override void Draw(Graphics graphics)
        {
            graphics.FillPolygon(FillColor, Points); // Отрисовка заливки
            graphics.DrawPolygon(BorderColor, Points); // Отрисовка рамки
        }

        // Проверка лежит ли точка внутри треугольника
        public override bool ContainsPoint(Point point)
        {
            using (GraphicsPath path = new GraphicsPath()) // Используется объект GraphicsPath для работы с путями 
            {
                path.AddPolygon(Points); // Добавление полигона с вершинами примитива в GraphicsPath
                return path.IsVisible(point); // Возвращается результат проверки видимости точки относительно пути фигуры
            }
        }

        // Перемещение треугольника
        public override void Move(int deltaX, int deltaY)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = new Point(Points[i].X + deltaX, Points[i].Y + deltaY);
            }
        }

        // Изменение размера треугольника
        public void Resize(int newSideLength)
        {
            SideLength = newSideLength;
            CalculateTrianglePoints(Points[0].X, Points[0].Y);
        }
    }

    // Класс линии
    class Line : Shape
    {
        public Point StartPoint { get; set; } // Точка начала линии
        public Point EndPoint { get; set; } // Точка конца линии

        // Конструктор класса линии
        public Line(Point startPoint, Point endPoint, Pen borderColor)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            BorderColor = borderColor;
        }

        // Метод для отрисовки линии 
        public override void Draw(Graphics graphics)
        {
            graphics.DrawLine(BorderColor, StartPoint, EndPoint);
        }

        public override bool ContainsPoint(Point point)
        {
            return false;
        }

        public override void Move(int deltaX, int deltaY)
        {

        }
    }
}
