using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace rawdata
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static Program program;
        internal static Stream stream;
        internal static Typeface typeface = new Typeface("Verdana");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            stream = File.Open(e.Args[0], FileMode.OpenOrCreate, FileAccess.ReadWrite);

            Program.Format format = Program.Format.RAW;
            if (e.Args.Contains("-f"))
                Enum.TryParse<Program.Format>(e.Args[Array.IndexOf(e.Args, "-f") + 1], true, out format);

            int size = 1;
            if (e.Args.Contains("-s"))
                int.TryParse(e.Args[Array.IndexOf(e.Args, "-s") + 1], out size);

            Program.BytesOrder order = Program.BytesOrder.NATIVE;
            if (e.Args.Contains("-b"))
                Enum.TryParse<Program.BytesOrder>(e.Args[Array.IndexOf(e.Args, "-b") + 1], true, out order);

            int offset = 0;
            if (e.Args.Contains("-o"))
                int.TryParse(e.Args[Array.IndexOf(e.Args, "-o") + 1], out offset);

            int count = -1;
            if (e.Args.Contains("-c"))
                int.TryParse(e.Args[Array.IndexOf(e.Args, "-c") + 1], out count);

            program = new Program(stream, size, format, order, offset, count);


            if (e.Args.Contains("-w"))
            {
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.Show();
            }
            else if (e.Args.Contains("-i"))
            {
                var drawing = new DrawingVisual();

                var drawingContext = drawing.RenderOpen();

                program.SeekTo(0);

                Pen gridPen = new Pen(Brushes.Black, 1);
                double itemHeight = 20;
                var p1 = new Point(0.5, 0.5);
                var p2 = new Point(200, 0.5);
                var p3 = new Point(3.5, 3.5);

                drawingContext.DrawLine(gridPen, p1, p2);

                while (program.ReadNext())
                {
                    p1.Y += itemHeight;
                    p2.Y += itemHeight;
                    drawingContext.DrawLine(gridPen, p1, p2);
                    drawingContext.DrawText(new FormattedText(App.program.Text.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 10, Brushes.Black), p3);
                    p3.Y += itemHeight;
                }

                drawingContext.Close();

                
                int nWidth = (int)drawing.ContentBounds.Width;

                int nHeight = (int)drawing.ContentBounds.Height;

                var bitmap = new RenderTargetBitmap(nWidth, nHeight, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(drawing);

                using var stream = new MemoryStream();

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);

                var bytes = stream.GetBuffer();
                stream.Seek(0, SeekOrigin.Begin);
                
                //stream.CopyTo(Console.OpenStandardOutput());
                var outputFileName = e.Args[Array.IndexOf(e.Args, "-i") + 1];
                using var fileStream = File.OpenWrite(outputFileName);
                stream.CopyTo(fileStream);

                Application.Current.Shutdown();
            }
            else
            {
                program.SeekTo(0);

                while (program.ReadNext())
                {
                    Console.WriteLine(program.currentText);
                }

                Application.Current.Shutdown();
            }
        }
    }

}
