namespace Plugin.SavableObject.Shared
{
    public interface ISavableObject
    {
        void Clear();
        void Load();
        void Save();
    }
}