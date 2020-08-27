using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using VectorThing.Models;
using VectorThing.Serialization;
using VectorThing.Utility;
using VectorThing.Utility.ViewModelStuff;
using Xceed.Wpf.Toolkit;

namespace VectorThing.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties And Shit
        private ObservableCollection<VectorShapeModel> vectorShapes = new ObservableCollection<VectorShapeModel>();
        public ObservableCollection<VectorShapeModel> VectorShapes
        {
            get
            {
                return vectorShapes;
            }
        }

        private ObservableCollection<Layer> layers = new ObservableCollection<Layer>();
        public ObservableCollection<Layer> Layers
        {
            get
            {
                return layers;
            }
        }

        private ToolsEnum selectedTool;
        public ToolsEnum SelectedTool
        {
            get
            {
                return selectedTool;
            }
            set
            {
                selectedTool = value;
                DeselectElement(null);
                OnPropertyChanged("SelectedTool");
            }
        }

        private VectorShapeModel selectedElement;
        private bool multipleSelectedElements = false;
        private List<VectorShapeModel> selectedElements;

        public VectorShapeModel SelectedElement
        {
            get
            {
                return selectedElement;
            }
            set
            {
                selectedElement = value;
                OnPropertyChanged("SelectedElement");
            }
        }
        public string SelectedElementName
        {
            get
            {
                if(selectedElement == null)
                {
                    return "";
                }
                return selectedElement.Name;
            }
            set
            {
                if (multipleSelectedElements)
                {
                    return;
                }
                selectedElement.Name = value;
                OnPropertyChanged("SelectedElementName");
            }
        }
        public double SelectedElementX
        {
            get
            {
                if (selectedElement == null)
                {
                    return 0;
                }
                return selectedElement.X;
            }
            set
            {
                if (multipleSelectedElements)
                {
                    TransformSelectedElementsXY(value, selectedElement.Y);
                }
                selectedElement.X = value;
                OnPropertyChanged("SelectedElementX");
            }
        }
        public double SelectedElementY
        {
            get
            {
                if (selectedElement == null)
                {
                    return 0;
                }
                return selectedElement.Y;
            }
            set
            {
                if (multipleSelectedElements)
                {
                    TransformSelectedElementsXY(selectedElement.X, value);
                }
                selectedElement.Y = value;
                OnPropertyChanged("SelectedElementY");
            }
        }
        public double SelectedElementRot
        {
            get
            {
                if (selectedElement == null)
                {
                    return 0;
                }
                return selectedElement.Rot;
            }
            set
            {
                if (multipleSelectedElements)
                {
                    TransformSelectedElementsRot(selectedElement.Rot - value);
                }
                selectedElement.Rot = value;
                OnPropertyChanged("SelectedElementRot");
            }
        }
        public double SelectedElementScale
        {
            get
            {
                if (selectedElement == null)
                {
                    return 0;
                }
                return selectedElement.Scale;
            }
            set
            {
                if (multipleSelectedElements)
                {
                    TransformSelectedElementsScale(value);
                }
                selectedElement.Scale = value;
                OnPropertyChanged("SelectedElementScale");
            }
        }
        public Layer SelectedElementLayer
        {
            get
            {
                if(selectedElement == null)
                {
                    return null;
                }
                return selectedElement.Layer;
            }
            set
            {
                if(selectedElement == null)
                {
                    return;
                }
                selectedElement.Layer = value;
                OnPropertyChanged("SelectedElementLayer");
            }
        }

        private Visibility transformVisibility = Visibility.Hidden;
        public Visibility TransformVisibility
        {
            get { return transformVisibility; }
        }
        public Visibility TransformNameVisibility
        {
            get
            {
                if (multipleSelectedElements)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        private Color color = Color.FromRgb(255, 0, 0);
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                if (selectedTool == ToolsEnum.Select && selectedElement != null)
                {
                    selectedElement.Color = color;
                }
                OnPropertyChanged("Color");
            }
        }

        private double canvasWidth = 1920;
        public double CanvasWidth
        {
            get
            {
                return canvasWidth;
            }
            set
            {
                canvasWidth = value;
                OnPropertyChanged("CanvasWidth");
            }
        }
        private double canvasHeight = 1080;
        public double CanvasHeight
        {
            get
            {
                return canvasHeight;
            }
            set
            {
                canvasHeight = value;
                OnPropertyChanged("CanvasHeight");
            }
        }
        private Color canvasColor = Color.FromRgb(255, 255, 255);
        public Color CanvasColor 
        {
            get
            {
                return canvasColor;
            }
            set
            {
                canvasColor = value;
                OnPropertyChanged("CanvasColor");
            }
        }
        #endregion

        #region Commands
        private CommandHandler canvasMouseDownCommand;
        public ICommand CanvasMouseDownCommand
        {
            get
            {
                if (canvasMouseDownCommand == null) {
                    canvasMouseDownCommand = new CommandHandler(CanvasMouseDown, null);
                }
                return canvasMouseDownCommand;
            }
        }

        private CommandHandler canvasMouseMoveCommand;
        public ICommand CanvasMouseMoveCommand
        {
            get
            {
                if (canvasMouseMoveCommand == null)
                {
                    canvasMouseMoveCommand = new CommandHandler(CanvasMouseMove, null);
                }
                return canvasMouseMoveCommand;
            }
        }

        private CommandHandler canvasMouseUpCommand;
        public ICommand CanvasMouseUpCommand
        {
            get
            {
                if (canvasMouseUpCommand == null)
                {
                    canvasMouseUpCommand = new CommandHandler(CanvasMouseUp, null);
                }
                return canvasMouseUpCommand;
            }
        }

        private CommandHandler deselectCommand;
        public ICommand DeselectCommand
        {
            get
            {
                if (deselectCommand == null)
                {
                    deselectCommand = new CommandHandler(DeselectElement, null);
                }
                return deselectCommand;
            }
        }

        private CommandHandler createLayerCommand;
        public ICommand CreateLayerCommand
        {
            get
            {
                if(createLayerCommand == null)
                {
                    createLayerCommand = new CommandHandler(CreateLayer, null);
                }
                return createLayerCommand;
            }
        }

        private CommandHandler deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if(deleteCommand == null)
                {
                    deleteCommand = new CommandHandler(DeleteShape, null);
                }
                return deleteCommand;
            }
        }

        private CommandHandler saveJsonCommand;
        public ICommand SaveJsonCommand
        {
            get
            {
                if(saveJsonCommand == null)
                {
                    saveJsonCommand = new CommandHandler(SaveJson, null);
                }
                return saveJsonCommand;
            }
        }

        private CommandHandler openCommand;
        public ICommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new CommandHandler(OpenFile, null);
                }
                return openCommand;
            }
        }

        private CommandHandler savePngCommand;
        public ICommand SavePngCommand
        {
            get
            {
                if(savePngCommand == null)
                {
                    savePngCommand = new CommandHandler(SavePNG, null);
                }
                return savePngCommand;
            }
        }

        private CommandHandler clearCanvasCommand;
        public ICommand ClearCanvasCommand
        {
            get
            {
                if(clearCanvasCommand == null)
                {
                    clearCanvasCommand = new CommandHandler(ClearCanvas, null);
                }
                return clearCanvasCommand;
            }
        }
        #endregion

        #region Internal values and stuff
        private Point mouseDownPoint;
        private Point mouseUpPoint;
        private VectorShapeModel currentShape;
        #endregion

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void CanvasMouseDown(Object element)
        {
            mouseDownPoint = Mouse.GetPosition((IInputElement)element);
            mouseUpPoint = mouseDownPoint;
            switch (SelectedTool)
            {
                case ToolsEnum.CreateRect:
                    currentShape = CreateRect();
                    break;
                case ToolsEnum.CreateLine:
                    currentShape = CreateLine();
                    break;
                case ToolsEnum.CreateCircle:
                    currentShape = CreateCircle();
                    break;
                case ToolsEnum.CreateEllipse:
                    currentShape = CreateEllipse();
                    break;
                case ToolsEnum.CreateExam:
                    currentShape = CreateExam();
                    break;
            }
            Debug.WriteLine("Mouse down pos: " + mouseDownPoint.ToString());
        }

        private void CanvasMouseMove(Object element)
        {
            if (currentShape == null) { return; }
            mouseUpPoint = Mouse.GetPosition((IInputElement)element);
            switch (SelectedTool)
            {
                case ToolsEnum.CreateRect:
                    ResizeRect((RectangleVectorShapeModel)currentShape);
                    break;
                case ToolsEnum.CreateLine:
                    ResizeLine((LineVectorShapeModel)currentShape);
                    break;
                case ToolsEnum.CreateCircle:
                    ResizeCircle((CircleVectorShapeModel)currentShape);
                    break;
                case ToolsEnum.CreateEllipse:
                    ResizeEllipse((EllipseVectorShapeModel)currentShape);
                    break;
                case ToolsEnum.CreateExam:
                    ResizeExam((ExamVectorShapeModel)currentShape);
                    break;
            }
        }

        private void CanvasMouseUp(Object element)
        {
            currentShape = null;
            mouseUpPoint = Mouse.GetPosition((IInputElement)element);
            Debug.WriteLine("Mouse up pos: " + mouseUpPoint.ToString());
            switch (SelectedTool) {
                case ToolsEnum.Select:
                    SetSelectedElement(element);
                    break;
                case ToolsEnum.CopyPaste:
                    Paste();
                    break;
            }
        }
        #region Selection
        private void SetSelectedElement(Object element)
        {
            ListBox listBox = (ListBox)element;
            if (listBox.SelectedItems.Count > 1)
            {
                SelectMultipleElements(listBox);
            }
            else
            {
                SelectSingleElement(listBox);
            }
        }

        private void SelectSingleElement(ListBox listBox)
        {
            multipleSelectedElements = false;

            selectedElement = listBox.SelectedItem as VectorShapeModel;
            if (selectedElement == null)
            {
                Debug.WriteLine("selected element is null");
                return;
            }
            Debug.WriteLine("Selected element color: " + selectedElement.Color.R.ToString() + "/" + selectedElement.Color.G.ToString() + "/" + selectedElement.Color.B.ToString());

            OnPropetyChangedSelectedElement();

            transformVisibility = Visibility.Visible;
            OnPropertyChanged("TransformVisibility");

            color = selectedElement.Color;
            OnPropertyChanged("Color");
        }

        private void SelectMultipleElements(ListBox listBox)
        {
            Debug.WriteLine(listBox.SelectedItems.Count);
            selectedElements = new List<VectorShapeModel>();
            foreach (VectorShapeModel shape in listBox.SelectedItems)
            {
                selectedElements.Add(shape);
            }
            if (selectedElements == null || selectedElements.Count == 0)
            {
                Debug.WriteLine("selected elementS is null");
                return;
            }
            Debug.WriteLine("Selected elements count: " + selectedElements.Count);

            System.Drawing.Point avg = UtilityStuff.GetAveragePosOfShapes(selectedElements);
            selectedElement = new VectorShapeModel(avg.X, avg.Y, 0, Color.FromRgb(255, 0, 0), layers[0]);

            multipleSelectedElements = true;
            OnPropetyChangedSelectedElement();

            transformVisibility = Visibility.Visible;
            OnPropertyChanged("TransformVisibility");
        }

        private void DeselectElement(Object e)
        {
            SelectedElement = null;
            selectedElements = null;
            multipleSelectedElements = false;

            transformVisibility = Visibility.Hidden;
            OnPropertyChanged("TransformVisibility");
        }

        private void OnPropetyChangedSelectedElement()
        {
            OnPropertyChanged("SelectedElement");
            OnPropertyChanged("SelectedElementName");
            OnPropertyChanged("SelectedElementX");
            OnPropertyChanged("SelectedElementY");
            OnPropertyChanged("SelectedElementRot");
            OnPropertyChanged("SelectedElementScale");
            OnPropertyChanged("SelectedElementLayer");
            OnPropertyChanged("TransformNameVisibility");
        }
        #endregion

        #region TransformSelection
        private void TransformSelectedElementsXY(double x, double y)
        {
            double offsetX = x - selectedElement.X;
            double offsetY = y - selectedElement.Y;
            for(int i = 0; i < selectedElements.Count; i++)
            {
                selectedElements[i].X += offsetX;
                selectedElements[i].Y += offsetY;
            }
        }

        private void TransformSelectedElementsRot(double rot)
        {
            Vector2 p;
            foreach(var shape in selectedElements)
            {
                Debug.WriteLine(shape.Name + " angl offset: " + UtilityStuff.GetAngleFromOrbit(new Vector2(selectedElement.X, selectedElement.Y), new Vector2(shape.X, shape.Y)));
                p = UtilityStuff.GetOrbitPos(rot + UtilityStuff.GetAngleFromOrbit(new Vector2(selectedElement.X, selectedElement.Y), new Vector2(shape.X, shape.Y)), UtilityStuff.Vector2Dist(new Vector2(selectedElement.X, selectedElement.Y), new Vector2(shape.X, shape.Y)));
                shape.X = selectedElement.X + p.X;
                shape.Y = selectedElement.Y + p.Y;
                Debug.WriteLine(shape.X.ToString() + "  /  " + shape.Y.ToString());
            }
            
        }

        private void TransformSelectedElementsScale(double scale)
        {
            for (int i = 0; i < selectedElements.Count; i++)
            {
                selectedElements[i].Scale = scale;
            }
        }
        #endregion

        #region CopyPaset
        private void Paste()
        {
            if(selectedElement == null)
            {
                return;
            }
            var copy = selectedElement.DeepCopy();
            copy.X = mouseUpPoint.X;
            copy.Y = mouseUpPoint.Y;
            vectorShapes.Add(copy);
        }
        #endregion

        #region Layers
        private void CreateLayer(Object element)
        {
            var newLayer = new Layer("New Layer", 0, true, true);
            layers.Add(newLayer);
        }
        #endregion

        #region Create/Edit
        private RectangleVectorShapeModel CreateRect()
        {
            var sizeX = mouseUpPoint.X - mouseDownPoint.X;
            var sizeY = mouseUpPoint.Y - mouseDownPoint.Y;

            var rect1 = new RectangleVectorShapeModel(Math.Min(mouseDownPoint.X, mouseUpPoint.X), Math.Min(mouseDownPoint.Y, mouseUpPoint.Y), 0, -sizeX, -sizeY, color, layers[0]);
            
            Debug.WriteLine("RectSize: " + sizeX + " " + sizeY);
            vectorShapes.Add(rect1);
            return rect1;
        }

        private void ResizeRect(RectangleVectorShapeModel rect)
        {
            var sizeX = mouseUpPoint.X - mouseDownPoint.X;
            var sizeY = mouseUpPoint.Y - mouseDownPoint.Y;

            rect.X = Math.Min(mouseDownPoint.X, mouseUpPoint.X);
            rect.Y = Math.Min(mouseDownPoint.Y, mouseUpPoint.Y);
            rect.Width = Math.Abs(sizeX);
            rect.Height = Math.Abs(sizeY);
        }

        private LineVectorShapeModel CreateLine()
        {
            var line = new LineVectorShapeModel(Math.Min(mouseDownPoint.X, mouseUpPoint.X), Math.Min(mouseDownPoint.Y, mouseUpPoint.Y), Math.Max(mouseDownPoint.X, mouseUpPoint.X), Math.Max(mouseDownPoint.Y, mouseUpPoint.Y), 0, color, layers[0]);
            vectorShapes.Add(line);
            return line;
        }

        private void ResizeLine(LineVectorShapeModel line)
        {
            line.X2 = mouseUpPoint.X - mouseDownPoint.X;
            line.Y2 = mouseUpPoint.Y - mouseDownPoint.Y;
        }

        private CircleVectorShapeModel CreateCircle()
        {
            var circle = new CircleVectorShapeModel(mouseDownPoint.X, mouseDownPoint.Y, 0, color, layers[0]);
            vectorShapes.Add(circle);
            return circle;
        }

        private void ResizeCircle(CircleVectorShapeModel circle)
        {
            circle.X = Math.Min(mouseDownPoint.X, mouseUpPoint.X);
            circle.Y = Math.Min(mouseDownPoint.Y, mouseUpPoint.Y);
            circle.Radius = Math.Max(Math.Abs(mouseUpPoint.X - mouseDownPoint.X), Math.Abs(mouseUpPoint.Y - mouseDownPoint.Y));
        }

        private EllipseVectorShapeModel CreateEllipse()
        {
            var ellipse = new EllipseVectorShapeModel(mouseDownPoint.X, mouseDownPoint.Y, 0, 0, 0, color, layers[0]);
            vectorShapes.Add(ellipse);
            return ellipse;
        }

        private void ResizeEllipse(EllipseVectorShapeModel ellipse)
        {
            var sizeX = mouseUpPoint.X - mouseDownPoint.X;
            var sizeY = mouseUpPoint.Y - mouseDownPoint.Y;

            ellipse.X = Math.Min(mouseDownPoint.X, mouseUpPoint.X);
            ellipse.Y = Math.Min(mouseDownPoint.Y, mouseUpPoint.Y);

            ellipse.Width = Math.Abs(sizeX);
            ellipse.Height = Math.Abs(sizeY);
        }

        private ExamVectorShapeModel CreateExam()
        {
            var exam = new ExamVectorShapeModel(mouseDownPoint.X, mouseDownPoint.Y, 0, color, layers[0]);
            vectorShapes.Add(exam);
            return exam;
        }

        private void ResizeExam(ExamVectorShapeModel exam)
        {
            exam.X = Math.Min(mouseDownPoint.X, mouseUpPoint.X);
            exam.Y = Math.Min(mouseDownPoint.Y, mouseUpPoint.Y);
            exam.Radius = Math.Max(Math.Abs(mouseUpPoint.X - mouseDownPoint.X), Math.Abs(mouseUpPoint.Y - mouseDownPoint.Y));
        }

        private void DeleteShape(Object e)
        {
            if (multipleSelectedElements)
            {
                for(int i = 0; i < selectedElements.Count; i++)
                {
                    vectorShapes.Remove(selectedElements[i]);
                }
                DeselectElement(null);
            }
            else
            {
                vectorShapes.Remove(selectedElement);
                DeselectElement(null);
            }
        }
        #endregion

        #region Open/Save
        private void SaveJson(Object e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".vtf";
            dialog.ShowDialog();
            JsonSerializationManager.SaveShapes(vectorShapes.ToList(), canvasColor, dialog.FileName);
        }

        private void OpenFile(Object e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            dialog.DefaultExt = ".vtf";

            ClearCanvas(null);

            JsonSerializationManager.OpenJson(vectorShapes, layers, (x) => { CanvasColor = x; }, dialog.FileName);
        }

        private void SavePNG(Object canvas)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".png";
            dialog.ShowDialog();
            CanvasToPNG.SaveToPng(dialog.FileName, (Visual)canvas);
        }
        #endregion

        private void ClearCanvas(Object e)
        {
            DeselectElement(null);
            vectorShapes.Clear();
            layers.Clear();
            var defaultLayer = new Layer("default", 0, false, true);
            layers.Add(defaultLayer);
            CanvasColor = Color.FromRgb(255, 255, 255);
        }

        public MainViewModel()
        {
            var defaultLayer = new Layer("default", 0, false, true);
            var topLayer = new Layer("top", 10, true, true);
            layers.Add(defaultLayer);
            layers.Add(topLayer);
            SelectedTool = ToolsEnum.Select;
        }
    }
}
