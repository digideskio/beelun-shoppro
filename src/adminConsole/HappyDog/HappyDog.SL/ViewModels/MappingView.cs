using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace HappyDog.SL.ViewModels
{
    /// <summary>
    /// View model object for category/item mapping or tab/category mapping
    /// </summary>
    public class MappingView : INotifyPropertyChanged
    {
        private bool? mappedValue = null;

        public bool? Mapped
        {
            get
            {
                return mappedValue;
            }

            set
            {
                mappedValue = value;
                NotifyPropertyChanged("Mapped");
            }
        }
        /// <summary>
        /// Name of parent object. For example, in Category2Item map, it is the name of Category
        /// </summary>
        public string Name { get; set; } 

        /// <summary>
        /// Id of parent object
        /// </summary>
        public long Id { get; set; } 

        #region INotifyPropertyChanged related
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        public MappingView() { }

        /// <summary>
        /// A copy constructor
        /// </summary>
        /// <param name="oldObject"></param>
        public MappingView(MappingView oldObject)
        {
            this.Mapped = oldObject.Mapped;
            this.Name = oldObject.Name;
            this.Id = oldObject.Id;
        }

        public MappingView(bool? mapped, string name, long id)
        {
            this.Mapped = mapped;
            this.Name = name;
            this.Id = id;
        }
    }
}
