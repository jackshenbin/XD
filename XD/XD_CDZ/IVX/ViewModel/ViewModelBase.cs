using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BOCOM.IVX.Framework;
using DataModel;

namespace BOCOM.IVX.ViewModel
{
    /// <summary>
    /// 所有VM的基类，定义了执行结果事件
    /// 一遍View 收到该事件后在界面上显示结果， 或者关闭界面
    /// </summary>
    public abstract class ViewModelBase : NotifyPropertyChangedModel, IEventAggregatorSubscriber
    {
        public event EventHandler<CommandExecuteResultEventArgs> CommandExecuted;

        public void RaiseCommandExecutedEvent(CommandExecuteResultEventArgs args)
        {
            if (CommandExecuted != null)
            {
                CommandExecuted(this, args);
            }
        }
        
        public virtual void UnSubscribe()
        {
            throw new NotImplementedException();
        }
    }

    public class CommandExecuteResultEventArgs : EventArgs
    {
        public bool Result{get;private set;}

        public string Message{get;private set;}

        public Exception Exception {get;set;}

        public CommandExecuteResultEventArgs()
        {
        }

        public CommandExecuteResultEventArgs(bool result)
        {
            Result = result;
        }

        public CommandExecuteResultEventArgs(bool result, string msg)
            : this(result)
        {
            Message = msg;
        }

        public CommandExecuteResultEventArgs(bool result, string msg, Exception exception)
            : this(result, msg)
        {
            Exception = exception;
        }
    }
}
