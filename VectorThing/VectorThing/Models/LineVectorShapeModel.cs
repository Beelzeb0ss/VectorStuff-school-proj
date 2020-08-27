using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using VectorThing.Utility.ViewModelStuff;

namespace VectorThing.Models
{
    class LineVectorShapeModel : VectorShapeModel
    {
        protected double x1;
        protected double y1;
        protected double x2;
        protected double y2;

        public double X1
        {
            get
            {
                return x1;
            }
            set
            {
                if (x1 == value)
                {
                    return;
                }

                x1 = value;

                OnPropertyChanged("X1");
            }
        }

        public double Y1
        {
            get
            {
                return y1;
            }
            set
            {
                if (y1 == value)
                {
                    return;
                }

                y1 = value;

                OnPropertyChanged("Y1");
            }
        }

        public double X2
        {
            get
            {
                return x2;
            }
            set
            {
                if(x2 == value)
                {
                    return;
                }

                x2 = value;

                OnPropertyChanged("X2");
            }
        }

        public double Y2
        {
            get
            {
                return y2;
            }
            set
            {
                if (y2 == value)
                {
                    return;
                }

                y2 = value;

                OnPropertyChanged("Y2");
            }
        }
        public override VectorShapeModel DeepCopy()
        {
            LineVectorShapeModel copy = new LineVectorShapeModel(x, y, x2, y2, rot, color, layer);
            copy.Name = name + " copy";
            copy.Scale = scale;
            return copy;
        }

        public LineVectorShapeModel(double x, double y, double x2, double y2, double rot, Color color, Layer layer): base(x, y, rot, color, layer)
        {
            this.x2 = x2;
            this.y2 = y2;
        }
    }
}
