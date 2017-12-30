using Plugin.LocalData.Abstractions;
using System;

namespace Plugin.LocalData
{
  /// <summary>
  /// Cross platform LocalData implemenations
  /// </summary>
  public class CrossLocalData
  {
    static Lazy<ILocalData> Implementation = new Lazy<ILocalData>(() => CreateLocalData(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static ILocalData Current
    {
      get
      {
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static ILocalData CreateLocalData()
    {
#if PORTABLE
        return null;
#else
        return new LocalDataImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
