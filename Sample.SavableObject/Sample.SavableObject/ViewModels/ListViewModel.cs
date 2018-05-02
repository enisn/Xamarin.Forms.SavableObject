using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Sample.SavableObject.ViewModels
{
    public class ListViewModel : Plugin.SavableObject.Shared.SavableObject, INotifyPropertyChanged
    {
        private string _text;
        private IList<Note> _items;

        public ListViewModel()
        {
            //This loads the saved data
            Load();

            //This decides if Items null or not. If Items saved before, inits observable collection with loaded data
            Items = Items == null ? new ObservableCollection<Note>() : new ObservableCollection<Note>(Items);


            SaveCommand = new Command(SaveMethod);
            AddToListCommand = new Command(AddToListMethod);
        }
        public string Text { get => _text; set { _text = value; OnPropertyChanged(); } }
        public IList<Note> Items { get => _items; set { _items = value; OnPropertyChanged(); } }

        [IgnoreSave] public Command SaveCommand { get; set; } //Commands shouldn't be saved!!! If you have something to not save use [IgnoreSave] attribute
        [IgnoreSave] public Command AddToListCommand { get; set; }


        void SaveMethod()
        {
            Save(); //this saves properties in that class
        }

        void AddToListMethod()
        {
            Items.Add(new Note(1,this.Text));
        }


        #region Basic MVVM Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }

    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Note(){}
        public Note(int _id, string _content)
        {
            this.Id = _id; this.Content = _content;
        }
    }
}