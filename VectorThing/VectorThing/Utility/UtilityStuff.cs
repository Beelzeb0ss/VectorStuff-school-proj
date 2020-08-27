using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VectorThing.Models;
using VectorThing.Utility.ViewModelStuff;

namespace VectorThing.Utility
{
    static class UtilityStuff
    {
        private static readonly Regex numRegex = new Regex("^[0-9.-]+$");

        public static bool IsTextNumbersOnly(string text)
        {
            return numRegex.IsMatch(text);
        }

        public static Point GetAveragePosOfShapes(List<VectorShapeModel> list)
        {
            Point point = new Point();
            double x = 0;
            double y = 0;
            foreach(VectorShapeModel shape in list)
            {
                x += shape.X;
                y += shape.Y;
            }
            x /= list.Count;
            y /= list.Count;
            point.X = (int)x;
            point.Y = (int)y;
            return point;
        }

        public static double Vector2Dist(Vector2 p1, Vector2 p2)
        {
            double dist = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            return dist;
        }

        public static double GetAngleFromOrbit(Vector2 center, Vector2 pos)
        {
            //return ConvertRadiansToDegrees( Math.Pow(Math.Sin(ConvertToRadians(-(center.Y - pos.Y) / Vector2Dist(center, pos))), -1));
            double deg = Math.Asin(Math.Abs((pos.Y - center.Y)) / Vector2Dist(center, pos));
            deg = ConvertRadiansToDegrees(deg);
            if (pos.X > center.X && pos.Y > center.Y)
            {
                //q1
            }
            else if(pos.X < center.X && pos.Y > center.Y)
            {
                //q2
                deg = 180 - deg;
            }
            else if(pos.X < center.X && pos.Y < center.Y)
            {
                //q3
                deg = 180 + deg;
            }
            else if(pos.X > center.X && pos.Y < center.Y)
            {
                //q4
                deg = 360 - deg;
            }
            return deg;
        }

        public static Vector2 GetOrbitPos(double angle, double radius)
        {
            angle = NormalizeAngleTo360(angle);
            angle = ConvertToRadians(angle);
            Vector2 pos = new Vector2();

            if (angle <= 90)
            {
                pos.X = Math.Cos(angle) * radius;
                pos.Y = Math.Sin(angle) * radius;
            }
            else if(angle <= 180)
            {
                pos.X = Math.Cos(180 - angle) * radius;
                pos.Y = Math.Sin(180 - angle) * radius;
            }
            else if(angle <= 270)
            {
                pos.X = Math.Cos(angle - 180) * radius;
                pos.Y = Math.Sin(angle - 180) * radius;
            }
            else
            {
                pos.X = Math.Cos(360 - angle) * radius;
                pos.Y = Math.Sin(360 - angle) * radius;
            }

            return pos;
        }

        private static double NormalizeAngleTo360(double angle)
        {
            while(angle > 360)
            {
                angle -= 360;
            }
            return angle;
        }

        public static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public static double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }

    }
}
