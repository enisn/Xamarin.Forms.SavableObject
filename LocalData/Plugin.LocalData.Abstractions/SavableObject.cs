using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plugin.LocalData.Abstractions
{
    /// <summary>
    /// An object which saves own properties of class
    /// 
    /// Be Careful. If you have any ObservableCollection as property, it'll not be saved,
    /// Other all the Collections is supported, List, Array, Stack, IColection, INumarable, IList etc.
    /// 
    /// If you need to use ObservableCollection, define your property and variable as IList, and then set it in constuctor as ObservableCollection.
    /// You can see how it works in sample project
    /// 
    /// </summary>
    public class SavableObject : ISavable
    {
        /// <summary>
        /// An object which saves own properties of class
        /// </summary>
        public SavableObject()
        {

        }

        /// <summary>
        /// To save all properties in this class.
        /// </summary>
        public virtual void Save()
        {
            foreach (var property in this.GetType().GetRuntimeProperties())
            {
                try
                {
                    if (property.GetCustomAttributes(typeof(IgnoreSave), false).Count() > 0)
                        continue;

                    
                    if (!IsDirectlyStorageSupported(property.GetValue(this),property.PropertyType))
                    {
                        if (Xamarin.Forms.Application.Current.Properties.ContainsKey(property.Name) && property.CanRead)
                            Xamarin.Forms.Application.Current.Properties[property.Name] = JsonConvert.SerializeObject(property.GetValue(this));
                        else
                            Xamarin.Forms.Application.Current.Properties.Add(property.Name, JsonConvert.SerializeObject(property.GetValue(this)));
                    }
                    else
                    {
                        if (Xamarin.Forms.Application.Current.Properties.ContainsKey(property.Name) && property.CanRead)
                            Xamarin.Forms.Application.Current.Properties[property.Name] = property.GetValue(this);
                        else
                            Xamarin.Forms.Application.Current.Properties.Add(property.Name, property.GetValue(this));
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    continue;
                }
            }
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// To load all properties to class from device storage
        /// </summary>
        public virtual void Load()
        {
            foreach (var property in this.GetType().GetRuntimeProperties())
            {
                if (property.GetCustomAttributes(typeof(IgnoreSave), false).Count() > 0)
                    continue;
                try
                {

                    //if (property.GetValue(this) is ICollection)
                    if (!IsDirectlyStorageSupported(property.GetValue(this),property.PropertyType))
                    {
                        if (Xamarin.Forms.Application.Current.Properties.ContainsKey(property.Name) && property.CanWrite)
                            property.SetValue(this,
                                       //Convert.ChangeType(
                                       JsonConvert.DeserializeObject(Xamarin.Forms.Application.Current.Properties[property.Name].ToString(), property.PropertyType));
                    }
                    else
                    {
                        if (Xamarin.Forms.Application.Current.Properties.ContainsKey(property.Name) && property.CanWrite)
                            property.SetValue(this, Xamarin.Forms.Application.Current.Properties[property.Name]);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    continue;
                }
            }
        }

        /// <summary>
        /// Clears all proeprties from device storage for these properties
        /// </summary>
        public virtual void Clear()
        {
            foreach (var property in this.GetType().GetRuntimeProperties())
            {
                try
                {
                    if (property.GetCustomAttributes(typeof(IgnoreSave), false).Count() > 0)
                        continue;

                    if (Xamarin.Forms.Application.Current.Properties.ContainsKey(property.Name) && property.CanRead)
                        Xamarin.Forms.Application.Current.Properties.Remove(property.Name);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Checks how to save datas
        /// </summary>
        private bool IsDirectlyStorageSupported(object value, Type type = null)
        {
            return value is ValueType || type == typeof(string);
        }

        /// <summary>
        /// When you use that attribute, that property will not be saved
        /// </summary>
        public class IgnoreSave : Attribute
        {

        }
    }
}
