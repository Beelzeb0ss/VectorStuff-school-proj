using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using VectorThing.Utility.ViewModelStuff;

namespace VectorThing.Models
{
    class CircleVectorShapeModel : VectorShapeModel
    {
        protected double radius;

        public double Radius
        {
            get
            {
                return radius * scale;
            }
            set
            {
                if(radius == value)
                {
                    return;
                }

                radius = value;

                OnPropertyChanged("Radius");
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

        public override VectorShapeModel DeepCopy()
        {
            CircleVectorShapeModel copy = new CircleVectorShapeModel(x, y, rot, color, layer);
            copy.Name = name + " copy";
            copy.Scale = scale;
            copy.Radius = radius;
            return copy;
        }

        public CircleVectorShapeModel(double x, double y, double rot, Color color, Layer layer): base(x, y, rot, color, layer)
        {

        }
    }
}
