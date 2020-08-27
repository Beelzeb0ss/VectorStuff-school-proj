using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VectorThing.Utility.ViewModelStuff
{
    class Layer:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private string name;
        public String Name
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

        private int depth;
        public int Depth
        {
            get
            {
                return depth;
            }
            set
            {
                if (depth == value)
                {
                    return;
                }

                depth = value;
                OnPropertyChanged("Depth");
            }
        }

        private bool isEditable;
        public bool IsEditable
        {
            get
            {
                return isEditable;
            }
            set
            {
                if(isEditable == value)
                {
                    return;
                }

                isEditable = value;
                OnPropertyChanged("IsEditable");
            }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if(isVisible == value)
                {
                    return;
                }

                isVisible = value;
                OnPropertyChanged("IsVisible");
                OnPropertyChanged("Visibility");
            }
        }
        public Visibility Visibility
        {
            get
            {
                if (IsVisible)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }

        public Layer(String name, int depth, bool isEditable, bool isVisible)
        {
            Name = name;
            this.depth = depth;
            this.isEditable = isEditable;
            this.isVisible = isVisible;
        }
    }
}
