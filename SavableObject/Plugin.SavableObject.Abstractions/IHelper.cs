namespace Plugin.SavableObject.Abstractions
{
    interface ISavable
    {
        void Save();
        void Load();
        void Clear();
    }
}
