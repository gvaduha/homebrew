using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Rusal.Emission.Framework
{
    public class GeometryUtils
    {
        public static List<PointF> GetStrangeBoundingBoxRectangleOfFivePoints(List<PointF> shape)
        {
            var minX = shape.Min(p => p.X);
            var maxX = shape.Max(p => p.X);
            var minY = shape.Min(p => p.Y);
            var maxY = shape.Max(p => p.Y);

            return new List<PointF>
            {
                new PointF(minX, minY),
                new PointF(maxX, minY),
                new PointF(maxX, maxY),
                new PointF(minX, maxY),
                new PointF(minX, minY) //Same as first!
            };
        }

        public static Rectangle GetBoundingBoxRectangle(List<PointF> shape)
        {
            var minX = (int)shape.Min(p => p.X);
            var maxX = (int)shape.Max(p => p.X);
            var minY = (int)shape.Min(p => p.Y);
            var maxY = (int)shape.Max(p => p.Y);

            return new Rectangle(new Point(minX, minY), new Size(maxX - minX, maxY - minY));
        }

        public static bool IsIntersect(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Right  > rect2.Left &&
                   rect1.Left   < rect2.Right &&
                   rect1.Bottom > rect2.Top &&
                   rect1.Top    < rect2.Bottom;
        }

        public static bool IsIntersect(List<PointF> points, Rectangle rect)
        {
            var minX = points.Min(p => p.X);
            var maxX = points.Max(p => p.X);
            var minY = points.Min(p => p.Y);
            var maxY = points.Max(p => p.Y);

            return !(rect.Right <= minX) && !(rect.Left >= maxX) &&
                   !(rect.Bottom <= minY) && !(rect.Top >= maxY);
        }

        public static bool IsIntersect(List<PointF> shape1, List<PointF> shape2)
        {
            var minX = shape1.Min(p => p.X);
            var maxX = shape1.Max(p => p.X);
            var minY = shape1.Min(p => p.Y);
            var maxY = shape1.Max(p => p.Y);

            var boundingBoxShape = GetBoundingBoxRectangle(shape2);

            return !(boundingBoxShape.Right <= minX) && !(boundingBoxShape.Left >= maxX) &&
                   !(boundingBoxShape.Bottom <= minY) && !(boundingBoxShape.Top >= maxY);
        }

        public static double GetIntersectPercentage(List<PointF> shape1, List<PointF> shape2)
        {
            var area = 0d;
            var boundingBoxIgnoredShape = GetBoundingBoxRectangle(shape2);

            var markedPath = new GraphicsPath();
            markedPath.AddLines(shape1.ToArray());
            var markedRegion = new Region(markedPath);
            if (markedRegion.IsVisible(boundingBoxIgnoredShape))
            {
                markedRegion.Intersect(boundingBoxIgnoredShape);
                area = GetArea(markedRegion);
            }

            markedRegion.Dispose();
            markedPath.Dispose();

            return area * 100 / GetArea(boundingBoxIgnoredShape);
        }

        public static double GetIntersectPercentage(List<PointF> shape, Rectangle rect)
        {
            var area = 0d;

            var markedPath = new GraphicsPath();
            markedPath.AddLines(shape.ToArray());
            var markedRegion = new Region(markedPath);
            if (markedRegion.IsVisible(rect))
            {
                markedRegion.Intersect(rect);
                area = GetArea(markedRegion);
            }

            markedRegion.Dispose();
            markedPath.Dispose();

            return area * 100 / GetArea(rect);
        }

        public static double GetIntersectPercentage(Rectangle rect1, Rectangle rect2)
        {
            var intersectRec = Rectangle.Intersect(rect1, rect2);
            var minRectSquare = Math.Min(rect1.Width * rect1.Height, rect2.Width * rect2.Height);
            var percentage = intersectRec.Width * intersectRec.Height * 100f / minRectSquare;
            return percentage;
        }

        public static double GetAvgIntersectPercentage(Rectangle rect1, Rectangle rect2)
        {
            var intersectRec = Rectangle.Intersect(rect1, rect2);
            var percentage = intersectRec.Width * intersectRec.Height * 100f * 2 /
                             (rect1.Width * rect1.Height + rect2.Width * rect2.Height);
            return percentage;
        }

        public static double GetVerticalIntersect(Rectangle rect1, Rectangle rect2, double heightFault)
        {
            if (rect1.Left > rect2.Right || rect1.Right < rect2.Left)
            {
                return 0;
            }

            var boxes = new[] { rect1, rect2 };
            boxes = boxes.OrderBy(b => b.Bottom).ToArray();
            var upBox = new Rectangle(boxes[0].X, boxes[0].Y, boxes[0].Width,
                (int)(boxes[0].Height * (1 + heightFault)));
            var bottomBox = new Rectangle((int)(boxes[1].X - boxes[1].Height * heightFault), boxes[1].Y,
                boxes[1].Width,
                boxes[1].Height);
            return GetIntersectPercentage(upBox, bottomBox);
        }

        public static double GetArea(Rectangle rect)
        {
            return rect.Height * rect.Width;
        }

        public static double GetArea(Region region)
        {
            return region.GetRegionScans(new Matrix())
                .Sum(regionScan => regionScan.Height * regionScan.Width);
        }
    }
}
