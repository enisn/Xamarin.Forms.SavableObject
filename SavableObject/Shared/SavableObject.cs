using Newtonsoft.Json;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Plugin.SavableObject.Shared
{
    public class SavableObject : ISavableObject
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
                    if (property.GetCustomAttributes(typeof(IgnoreSaveAttribute), false).Count() > 0)
                        continue;


                    if (property.GetValue(this) is ICollection)
                    {
                        if (Xamarin.Forms.Application.Current.Properties.ContainsKey(this.GetType().Name + property.Name) && property.CanRead)
                            Xamarin.Forms.Application.Current.Properties[this.GetType().Name + property.Name] = JsonConvert.SerializeObject(property.GetValue(this));
                        else
                            Xamarin.Forms.Application.Current.Properties.Add(this.GetType().Name + property.Name, JsonConvert.SerializeObject(property.GetValue(this)));
                    }
                    else
                    {
                        if (Xamarin.Forms.Application.Current.Properties.ContainsKey(this.GetType().Name + property.Name) && property.CanRead)
                            Xamarin.Forms.Application.Current.Properties[this.GetType().Name + property.Name] = property.GetValue(this);
                        else
                            Xamarin.Forms.Application.Current.Properties.Add(this.GetType().Name + property.Name, property.GetValue(this));
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
                if (property.GetCustomAttributes(typeof(IgnoreSaveAttribute), false).Count() > 0)
                    continue;
                try
                {
                    if (property.GetValue(this) is ICollection)
                    {
                        if (Xamarin.Forms.Application.Current.Properties.ContainsKey(this.GetType().Name + property.Name) && property.CanWrite)
                            property.SetValue(this,
                                       //Convert.ChangeType(
                                       JsonConvert.DeserializeObject(Xamarin.Forms.Application.Current.Properties[this.GetType().Name + property.Name].ToString(), property.PropertyType));
                    }
                    else
                    {
                        if (Xamarin.Forms.Application.Current.Properties.ContainsKey(this.GetType().Name + property.Name) && property.CanWrite)
                            property.SetValue(this, Xamarin.Forms.Application.Current.Properties[this.GetType().Name + property.Name]);
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
                    if (property.GetCustomAttributes(typeof(IgnoreSaveAttribute), false).Count() > 0)
                        continue;

                    if (Xamarin.Forms.Application.Current.Properties.ContainsKey(this.GetType().Name + property.Name) && property.CanRead)
                        Xamarin.Forms.Application.Current.Properties.Remove(this.GetType().Name + property.Name);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }
            Application.Current.SavePropertiesAsync();
        }

        [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        sealed class IgnoreSaveAttribute : Attribute
        {
            public IgnoreSaveAttribute()
            {
            }
        }
    }
}
