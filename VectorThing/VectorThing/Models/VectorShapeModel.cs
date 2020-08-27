using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VectorThing.Utility.ViewModelStuff;

namespace VectorThing.Models
{
    class VectorShapeModel : INotifyPropertyChanged
    {
        protected string name;
        protected double x;
        protected double y;
        protected double scale = 1;
        protected double rot;
        protected Color color;
        protected Layer layer;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if(name == value)
                {
                    return;
                }

                name = value;

                OnPropertyChanged("Name");
            }
        }

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (x == value)
                {
                    return;
                }

                x = value;

                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (y == value)
                {
                    return;
                }

                y = value;

                OnPropertyChanged("Y");
            }
        }

        public double Rot
        {
            get
            {
                return rot;
            }
            set
            {
                if (rot == value)
                {
                    return;
                }

                rot = value;

                OnPropertyChanged("Rot");
            }
        }

        public virtual double Scale
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
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                if (color == value)
                {
                    return;
                }

                color = value;

                OnPropertyChanged("Color");
            }
        }

        public Layer Layer
        {
            get
            {
                return layer;
            }
            set
            {
                if (layer == value)
                {
                    return;
                }

                layer = value;

                OnPropertyChanged("Layer");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public virtual VectorShapeModel DeepCopy(){
            Debug.WriteLine("Using base deep copy");
            return null;
        }

        public VectorShapeModel() { }

        public VectorShapeModel(double x, double y, double rot, Color color, Layer layer)
        {
            name = "";
            this.x = x;
            this.y = y;
            this.rot = rot;
            this.color = color;
            this.layer = layer;
        }

    }
}
