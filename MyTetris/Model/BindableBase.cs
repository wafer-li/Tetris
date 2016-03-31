using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyTetris.Model
{
    public abstract class BindableBase:INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChanded Event delegation
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public BindableBase()
        { }

        /// <summary>
        /// Delegate method
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Setter method 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">Ref to instance variable</param>
        /// <param name="value">Value</param>
        /// <param name="propertyName">Name of caller "Property"</param>
        public void SetPropertyAndNotifyChanged<T>(ref T item, T value, [CallerMemberName] string propertyName=null)
        {
            if(!EqualityComparer<T>.Default.Equals(item,value))
            {
                item = value;
                OnPropertyChanged(propertyName);
            }
        }
        /// <summary>
        /// Setter method without PropertyChanged Notification
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="value"></param>
        public void SetProperty<T>(ref T item, T value)
        {
            if (value != null)
                item = value;

        }
    }
}
