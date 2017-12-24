using Newtonsoft.Json;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Plugin.SavableObject.Abstractions
{
    public class SavableObject : ISavable
    {
        public SavableObject()
        {

        }

        public virtual void Save()
        {
            foreach (var property in this.GetType().GetRuntimeProperties())
            {
                try
                {
                    if (property.GetCustomAttributes(typeof(IgnoreSave), false).Count() > 0)
                        continue;


                    if (property.GetValue(this) is ICollection)
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


        public virtual void Load()
        {
            foreach (var property in this.GetType().GetRuntimeProperties())
            {
                if (property.GetCustomAttributes(typeof(IgnoreSave), false).Count() > 0)
                    continue;
                try
                {
                    if (property.GetValue(this) is ICollection)
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
            Application.Current.SavePropertiesAsync();
        }


        public class IgnoreSave : Attribute
        {

        }

    }
}
