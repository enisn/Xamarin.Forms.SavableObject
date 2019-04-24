using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using System.Windows.Input;

namespace Plugin.SavableObject.Shared
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
    public class SavableObject : ISavableObject
    {
        public GlobalSetting GlobalSetting { get; set; } = new GlobalSetting
        {
            LoadAutomaticly = false,
            IgnoredTypes = new List<Type>
            {
                typeof(Command),
                typeof(ICommand)
            },
        };

        #region STATIC FIELD
        /// <summary>
        /// Saves an object which does not inherit from savableobject
        /// </summary>
        public static void Save(Object value)
        {
            foreach (var property in value.GetType().GetRuntimeProperties())
            {
                try
                {
                    if (property.GetCustomAttributes(typeof(IgnoreSave), false).Any())
                        continue;


                    if (!IsDirectlyStorageSupported(property.GetValue(value), property.PropertyType))
                    {
                        if (Application.Current.Properties.ContainsKey(property.Name) && property.CanRead)
                            Application.Current.Properties[property.Name] = JsonConvert.SerializeObject(property.GetValue(value));
                        else
                            Application.Current.Properties.Add(property.Name, JsonConvert.SerializeObject(property.GetValue(value)));
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey(property.Name) && property.CanRead)
                            Application.Current.Properties[property.Name] = property.GetValue(value);
                        else
                            Application.Current.Properties.Add(property.Name, property.GetValue(value));
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            Application.Current.SavePropertiesAsync();
        }
        /// <summary>
        /// Loads an object and returns it. If you send an existing object. It'll load into it.
        /// </summary>
        /// <typeparam name="T">Object type to load</typeparam>
        public static T Load<T>(T result = default) where T : new()
        {
            if (result == null)
                result = new T();

            foreach (var property in result.GetType().GetRuntimeProperties())
            {
                if (property.GetCustomAttributes(typeof(IgnoreSave), false).Any())
                    continue;
                try
                {
                    if (!IsDirectlyStorageSupported(property.GetValue(result), property.PropertyType))
                    {
                        if (Application.Current.Properties.ContainsKey(property.Name) && property.CanWrite)
                            property.SetValue(result,
                                       //Convert.ChangeType(
                                       JsonConvert.DeserializeObject(Application.Current.Properties[property.Name].ToString(), property.PropertyType));
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey(property.Name) && property.CanWrite)
                            property.SetValue(result, Application.Current.Properties[property.Name]);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// Clears all properties of an object from local storage
        /// </summary>
        public static void Clear<T>(T value) where T : new()
        {
            if (value == null)
                value = new T();
            foreach (var property in value.GetType().GetRuntimeProperties())
            {
                try
                {
                    if (property.GetCustomAttributes(typeof(IgnoreSave), false).Any())
                        continue;

                    if (Application.Current.Properties.ContainsKey(property.Name) && property.CanRead)
                        Application.Current.Properties.Remove(property.Name);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }
            Application.Current.SavePropertiesAsync();
        }
        #endregion

        public SavableObject()
        {
            if (GlobalSetting.LoadAutomaticly)
                Load();
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
                    if (property.GetCustomAttributes(typeof(IgnoreSave), false).Any() || GlobalSetting.IgnoredTypes.Contains(property.PropertyType))
                        continue;

                    string savedName = this.GetType().Name + property.Name;

                    if (!IsDirectlyStorageSupported(property.GetValue(this), property.PropertyType)  )
                    {
                        if (Application.Current.Properties.ContainsKey(savedName) && property.CanRead)
                            Application.Current.Properties[savedName] = JsonConvert.SerializeObject(property.GetValue(this));
                        else
                            Application.Current.Properties.Add(savedName, JsonConvert.SerializeObject(property.GetValue(this)));
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey(savedName) && property.CanRead)
                            Application.Current.Properties[savedName] = property.GetValue(this);
                        else
                            Application.Current.Properties.Add(savedName, property.GetValue(this));
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// To load all properties to class from device storage
        /// </summary>
        public virtual void Load()
        {
            foreach (var property in this.GetType().GetRuntimeProperties())
            {
                if (property.GetCustomAttributes(typeof(IgnoreSave), false).Any())
                    continue;
                try
                {
                    string savedName = this.GetType().Name + property.Name;
                    //if (property.GetValue(this) is ICollection)
                    if (!IsDirectlyStorageSupported(property.GetValue(this), property.PropertyType))
                    {
                        if (Application.Current.Properties.ContainsKey(savedName) && property.CanWrite)
                            property.SetValue(this,
                                       //Convert.ChangeType(
                                       JsonConvert.DeserializeObject(Application.Current.Properties[savedName].ToString(), property.PropertyType));
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey(savedName) && property.CanWrite)
                            property.SetValue(this, Application.Current.Properties[savedName]);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
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
                    if (property.GetCustomAttributes(typeof(IgnoreSave), false).Any())
                        continue;

                    string savedName = this.GetType().Name + property.Name;

                    if (Application.Current.Properties.ContainsKey(savedName) && property.CanRead)
                        Application.Current.Properties.Remove(savedName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Checks how to save datas
        /// </summary>
        private static bool IsDirectlyStorageSupported(object value, Type type = null)
        {
            return value is ValueType || type == typeof(string);
        }

        /// <summary>
        /// When you use that attribute, that property will not be saved
        /// </summary> 
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class IgnoreSave : Attribute
        {

        }
    }
}
