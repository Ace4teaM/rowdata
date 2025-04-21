using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace rawdata
{
    /// <summary>
    /// Logique d'interaction pour DataView.xaml
    /// </summary>
    public partial class DataView : UserControl
    {
        public DataView()
        {
            InitializeComponent();
        }

        private Pen gridPen = new Pen(Brushes.Black, 1);
        private double itemHeight = 20;
        private Typeface typeface = new Typeface("Verdana");

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var p1 = new Point(0, 0);
            var p2 = new Point(ActualWidth, 0);

            for (double y = 0; y < ActualHeight / itemHeight; y++)
            {
                drawingContext.DrawLine(gridPen, p1, p2);
                p1.Y += itemHeight;
                p2.Y += itemHeight;
            }

            App.program.SeekTo(0);
            p1 = new Point(3, 3);

            while (App.program.ReadNext() && p1.Y < ActualHeight)
            {
                drawingContext.DrawText(new FormattedText(App.program.Text.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 10, Brushes.Black), p1);
                p1.Y += itemHeight;
            }

        }
    }
}
