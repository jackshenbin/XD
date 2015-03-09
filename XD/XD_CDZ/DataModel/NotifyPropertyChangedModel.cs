using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace DataModel
{
    public abstract class NotifyPropertyChangedModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<NotifyPropertyChangedModel, string> InputValidateFailed;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RaiseValidateFailEvent(string propertyName)
        {
            if (InputValidateFailed != null)
            {
                InputValidateFailed(this, propertyName);
            }
        }

    }
}
