using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Sample.SavableObject.ViewModels
{
    public class SimpleViewModel : Plugin.SavableObject.Shared.SavableObject, INotifyPropertyChanged
    {
        private double _value;
        private string _nameSurname;
        private bool _isAllowed;

        public SimpleViewModel()
        {
            Load();
            SaveCommand = new Command(SaveMethod);
        }

        public double Value { get => _value; set { _value = value; OnPropertyChanged(); } }
        public string NameSurname { get => _nameSurname; set { _nameSurname = value; OnPropertyChanged(); } }
        public bool IsAllowed { get => _isAllowed; set { _isAllowed = value; OnPropertyChanged(); } }
        [IgnoreSave] public Command SaveCommand { get; set; }


        void SaveMethod()
        {
            Save();
        }
        #region Basic MVVM Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "" ) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}

