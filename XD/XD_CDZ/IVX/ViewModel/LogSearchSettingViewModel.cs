using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using System.Data;

namespace BOCOM.IVX.ViewModel
{
    public class LogSearchSettingViewModel : ViewModelBase,IEventAggregatorSubscriber
    {

        public LogSearchSettingViewModel()
        {
            //Framework.Container.Instance.EvtAggregator.GetEvent<LogSearchReceivedEvent>().Subscribe(OnLogSearchReceived, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        public void UnSubscribe()
        {
            //Framework.Container.Instance.EvtAggregator.GetEvent<LogSearchReceivedEvent>().Unsubscribe(OnLogSearchReceived);
        }
    }
}
