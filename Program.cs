using System;
using System.Drawing;
using System.Drawing.Imaging;
namespace Fractals_cmd;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Введите размер изображения.");
        Console.WriteLine("Введите высоту изображения (до 20к):");
        int height = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Введите ширину изображения (до 20к):");
        int width = Convert.ToInt32(Console.ReadLine());

        Bitmap bitmap = new(width, height);
        Graphics graphics = Graphics.FromImage(bitmap);
        here:
        Console.WriteLine("Выберите тип рисуемого фрактала:");
        Console.WriteLine("1 - Мандельброт;\n2 - Треугольник Серпинского;\n0 - Выход.");
        int choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 0:
                Environment.Exit(0);
                break;

            case 1:
                Mandelbrot(width, height, bitmap);
                Console.ReadKey();
                break;

            case 2:
                //стартовые точки треугольника
                Point p1 = new(width / 2, 0);
                Point p2 = new(0, height);
                Point p3 = new(width, height);

                Console.WriteLine("Введите глубину рекурсии (не более 14, оптимально 11): \t");
                int depth = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                DrawSierpinskiTriangle(graphics, p1, p2, p3, depth);
                Console.WriteLine("Завершено.");
                bitmap.Save("serpinsky.bmp", ImageFormat.Bmp);
                Console.Beep();
                Console.ReadKey();
                break;

            default:
                //Console.WriteLine("Введите корректное значение.");
                Console.Clear();
                goto here;
        }
    }

    private static void Mandelbrot(int width, int height, Bitmap bitmap)
    {
        int counter = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                double zx = 0, zy = 0;
                double cX = -2.0 + (x / (double)width) * 3.0;
                double cY = -1.5 + (y / (double)height) * 3.0;

                int iteration = 0;
                while (zx * zx + zy * zy <= 4 && iteration < 255)
                {
                    double tmp = zx * zx - zy * zy + cX;
                    zy = 2.0 * zx * zy + cY;
                    zx = tmp;
                    iteration++;
                }
                Color color = Color.FromArgb(iteration, iteration, iteration);
                bitmap.SetPixel(x, y, color);
            }
            counter++;
            if (counter % 200 == 0)
            {
                Console.Clear();
                Console.WriteLine($"Завершено {counter / 200} процентов.");
            }
        }
        Console.Clear();
        Console.WriteLine("Завершено.");
        bitmap.Save("mandelbrot.bmp", ImageFormat.Bmp);
        Console.Beep();
    }

    private static void DrawSierpinskiTriangle(Graphics graphics, Point p1, Point p2, Point p3, int depth)
    {
        if (depth == 0)
        {
            //нулевой случай
            Point[] points = [p1, p2, p3];
            graphics.FillPolygon(Brushes.White, points);
        }
        else
        {
            //поиск середин сторон треугольника
            Point mid1 = new((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            Point mid2 = new((p2.X + p3.X) / 2, (p2.Y + p3.Y) / 2);
            Point mid3 = new((p1.X + p3.X) / 2, (p1.Y + p3.Y) / 2);

            //три подтреугольника
            DrawSierpinskiTriangle(graphics, p1, mid1, mid3, depth - 1);
            DrawSierpinskiTriangle(graphics, mid1, p2, mid2, depth - 1);
            DrawSierpinskiTriangle(graphics, mid3, mid2, p3, depth - 1);
        }
    }
}
