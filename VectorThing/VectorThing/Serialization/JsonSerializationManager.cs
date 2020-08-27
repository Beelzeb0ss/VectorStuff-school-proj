using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VectorThing.Models;
using VectorThing.Utility.ViewModelStuff;
using System.Windows.Media;

namespace VectorThing.Serialization
{
    static class JsonSerializationManager
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public static void SaveShapes(List<VectorShapeModel> shapes, Color canvasColor, string fileName)
        {
            List<Object> shapesCopy = new List<Object>();
            foreach(var shape in shapes)
            {
                shapesCopy.Add(shape);
            }
            shapesCopy.Add(canvasColor);

            string jsonString = JsonConvert.SerializeObject(shapesCopy, Formatting.Indented, settings);
            try
            {
                File.WriteAllText(fileName, jsonString);
            }
            catch (Exception)
            {
            }
        }

        public static void OpenJson(ObservableCollection<VectorShapeModel> shapes, ObservableCollection<Layer> layers, Action<Color> ChangeCavasColor, string fileName)
        {
            string jsonString;
            List<Object> list;
            List<VectorShapeModel> shapeList = new List<VectorShapeModel>();
            try
            {
                jsonString = File.ReadAllText(fileName);
                list = JsonConvert.DeserializeObject<List<Object>>(jsonString, settings);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            for (int i = 0; i < list.Count - 1; i++)
            {
                shapeList.Add((VectorShapeModel)list[i]);
            }
            ChangeCavasColor((Color)ColorConverter.ConvertFromString(list[list.Count-1].ToString()));

            bool layerExists = false;
            foreach(var shape in shapeList)
            {
                shapes.Add(shape);
                if(layers.Count >= 1)
                {
                    layerExists = false;
                    foreach(var l in layers)
                    {
                        if (l.Equals(shape.Layer))
                        {
                            layerExists = true;
                            break;
                        }
                    }
                    if (!layerExists)
                    {
                        layers.Add(shape.Layer);
                    }
                }
                else
                {
                    layers.Add(shape.Layer);
                }
            }
        }
    }
}
