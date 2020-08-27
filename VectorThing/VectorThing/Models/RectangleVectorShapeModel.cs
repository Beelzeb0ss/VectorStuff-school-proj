using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using VectorThing.Utility.ViewModelStuff;

namespace VectorThing.Models
{
    class RectangleVectorShapeModel : VectorShapeModel
    {
        protected double width;
        protected double height;

        public double Width
        {
            get
            {
                return width * scale;
            }
            set
            {
                if (width == value)
                {
                    return;
                }

                width = value;

                OnPropertyChanged("Width");
            }
        }

        public double Height
        {
            get
            {
                return height * scale;
            }
            set
            {
                if (height == value)
                {
                    return;
                }

                height = value;

                OnPropertyChanged("Height");
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
                if(scale == value)
                {
                    return;
                }

                scale = value;

                OnPropertyChanged("Scale");
                OnPropertyChanged("Height");
                OnPropertyChanged("Width");
            }
        }

        public override VectorShapeModel DeepCopy()
        {
            RectangleVectorShapeModel copy = new RectangleVectorShapeModel(x, y, rot, width, height, color, layer);
            copy.Name = name + " copy";
            copy.Scale = scale;
            return copy;
        }

        public RectangleVectorShapeModel(double x, double y, double rot, double width, double height, Color color, Layer layer) : base(x, y, rot, color, layer)
        {
            this.width = width;
            this.height = height;
        }
    }
}
