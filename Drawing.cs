using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace rawdata
{
    internal static class Drawing
    {
        internal static Typeface typeface = new Typeface("Verdana");
        internal static Pen gridPen = new Pen(Brushes.Black, 1);
        internal static double itemHeight = 20;

        internal static Geometry MakeGrid(int rows, double height, double[] widths)
        {
            var geometry = new StringBuilder();
            var totalWidth = Enumerable.Sum(widths);

            var p1 = new Point(0.5, 0.5);
            var p2 = new Point(totalWidth - 0.5, 0.5);

            geometry.Append("M ");
            geometry.Append(p1);
            geometry.Append(",");
            geometry.Append(p2);

            while (rows-- > 0)
            {
                p1.Y += itemHeight;
                p2.Y += itemHeight;

                geometry.Append(" M ");
                geometry.Append(p1);
                geometry.Append(",");
                geometry.Append(p2);
            }

            p1 = new Point(0.0, 0.5);
            p2 = new Point(0.0, height - 0.5);

            geometry.Append("M ");
            geometry.Append(p1);
            geometry.Append(",");
            geometry.Append(p2);

            foreach (var width in widths)
            {
                p1.X += width;
                p2.X += width;

                geometry.Append(" M ");
                geometry.Append(p1);
                geometry.Append(",");
                geometry.Append(p2);
            }

            return Geometry.Parse(geometry.ToString());
        }

        internal static void DrawGrid(DrawingContext drawingContext, int rows, double height, double[] widths)
        {
            var totalWidth = Enumerable.Sum(widths);

            var p1 = new Point(0.5, 0.5);
            var p2 = new Point(totalWidth - 0.5, 0.5);

            drawingContext.DrawLine(gridPen, p1, p2);

            while (rows-- > 0)
            {
                p1.Y += itemHeight;
                p2.Y += itemHeight;
                drawingContext.DrawLine(gridPen, p1, p2);
            }

            p1 = new Point(0.0, 0.5);
            p2 = new Point(0.0, height - 0.5);

            drawingContext.DrawLine(gridPen, p1, p2);

            foreach (var width in widths)
            {
                p1.X += width;
                p2.X += width;
                drawingContext.DrawLine(gridPen, p1, p2);
            }

            /*p1 = new Point(totalWidth - 0.5, 0.5);
            p2 = new Point(totalWidth - 0.5, height - 0.5);

            drawingContext.DrawLine(gridPen, p1, p2);*/
        }
    }
}
