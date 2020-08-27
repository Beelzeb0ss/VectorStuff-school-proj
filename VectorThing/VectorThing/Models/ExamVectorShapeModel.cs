using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using VectorThing.Utility.ViewModelStuff;

namespace VectorThing.Models
{
    class ExamVectorShapeModel : VectorShapeModel
    {
        protected double radius;

        protected double vertLineX1;
        protected double vertLineY1;
        protected double vertLineX2;
        protected double vertLineY2;

        //mid line
        protected double horzLine1X1;
        protected double horzLine1Y1;
        protected double horzLine1X2;
        protected double horzLine1Y2;

        //top line
        protected double horzLine2X1;
        protected double horzLine2Y1;
        protected double horzLine2X2;
        protected double horzLine2Y2;

        public double Radius
        {
            get
            {
                return radius * scale;
            }
            set
            {
                if (radius == value)
                {
                    return;
                }

                radius = value;

                OnPropertyChanged("Radius");
                OnPropertyChanged("VertLineX1");
                OnPropertyChanged("VertLineX2");
                OnPropertyChanged("VertLineY1");
                OnPropertyChanged("VertLineY2");

                OnPropertyChanged("HorzLine1X1");
                OnPropertyChanged("HorzLine1X2");
                OnPropertyChanged("HorzLine1Y1");
                OnPropertyChanged("HorzLine1Y2");

                OnPropertyChanged("HorzLine2X1");
                OnPropertyChanged("HorzLine2X2");
                OnPropertyChanged("HorzLine2Y1");
                OnPropertyChanged("HorzLine2Y2");
            }
        }

        public override double Scale
        {
            get
            {
                return scale;
            }
            set
            {
                if (scale == value)
                {
                    return;
                }

                scale = value;

                OnPropertyChanged("Scale");
                OnPropertyChanged("Radius");
            }
        }

        public double VertLineX1
        {
            get
            {
                return X + Radius / 2;
            }
        }

        public double VertLineY1
        {
            get
            {
                return Y;
            }
        }

        public double VertLineX2
        {
            get
            {
                return VertLineX1;
            }
        }

        public double VertLineY2
        {
            get
            {
                return Y + Radius;
            }
        }

        public double HorzLine1X1
        {
            get
            {
                return X;
            }
        }

        public double HorzLine1Y1
        {
            get
            {
                return Y + Radius / 2;
            }
        }

        public double HorzLine1X2
        {
            get
            {
                return X + Radius;
            }
        }

        public double HorzLine1Y2
        {
            get
            {
                return HorzLine1Y1;
            }
        }

        public double HorzLine2X1
        {
            get
            {
                return X + Radius / 10;
            }
        }

        public double HorzLine2Y1
        {
            get
            {
                return Y + Radius / 4;
            }
        }

        public double HorzLine2X2
        {
            get
            {
                return X + (Radius / 10) * 9;
            }
        }

        public double HorzLine2Y2
        {
            get
            {
                return HorzLine2Y1;
            }
        }

        public Color LineColor()
        {
            return Color.FromRgb(0, 0, 0);
        }

        public override VectorShapeModel DeepCopy()
        {
            CircleVectorShapeModel copy = new CircleVectorShapeModel(x, y, rot, color, layer);
            copy.Name = name + " copy";
            copy.Scale = scale;
            copy.Radius = radius;
            return copy;
        }

        public ExamVectorShapeModel(double x, double y, double rot, Color color, Layer layer) : base(x, y, rot, color, layer)
        {

        }
    }
}
