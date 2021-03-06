using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace BauhofDLL.ListDefinitions
{
    public class ListOfSettings : INotifyPropertyChanged
    {
        public byte[] byteData { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public string wmsAddress { get; set; }
        public bool pEnv { get; set; }
        public string currentVersion { get; set; }
        public bool isSelected
        {
            get { return m_isVisible; }
            set
            {
                m_isVisible = value;
                OnPropertyChanged("isSelected");
            }
        }

        public bool m_isVisible;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

    }
}
