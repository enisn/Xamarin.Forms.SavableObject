using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.SavableObject.Shared
{
    public class GlobalSetting
    {
        public GlobalSetting()
        {
            //IgnoredTypes = new List<Type>();
        }
        public IList<Type> IgnoredTypes { get; set; }
        public bool LoadAutomaticly { get; set; }
    }
}
