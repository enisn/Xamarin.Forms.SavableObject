using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.LocalData.Abstractions
{
    interface ISavable
    {
        void Save();
        void Load();
        void Clear();
    }
}
