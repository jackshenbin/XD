using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using BOCOM.IVX.Controls;

namespace BOCOM.IVX.Common
{
    public class UIUtil
    {
        #region Fields

        private Control m_Container;
        private OneParameterHandler m_FuncNeedAsync;
        private OneParameterHandler m_CompletedCallBack;
        private int m_Timeout;
        private readonly bool m_CanCancel;
        private object m_CallResult;
        private volatile bool m_ThreadNoNeedRun = false;

        private FormStartPosition m_SplashFormStartPosition = FormStartPosition.CenterScreen;
        private Color m_SplashFormBackColor = Color.Empty;

        #endregion

        #region Properties

        public FormStartPosition SplashFormStartPosition
        {
            get { return m_SplashFormStartPosition; }
            set { m_SplashFormStartPosition = value; }
        }

        public Color SplashFormBackColor
        {
            get { return m_SplashFormBackColor; }
            set { m_SplashFormBackColor = value; }
        }

        #endregion

        #region Constructors

        public UIUtil()
        {
        }

        public UIUtil(bool cancelable)
        {
            this.m_CanCancel = cancelable;
        }

        #endregion

        #region Public helper functions

        //public void AsyncCall(Control container, string statusText, int timeout, OneParameterHandler funcNeedAsync, object objPar, OneParameterHandler completedCallback)
        //{
        //    m_Container = container;
        //    m_FuncNeedAsync = funcNeedAsync;
        //    m_CompletedCallBack = completedCallback;
        //    m_Timeout = timeout < 0 ? -1 : timeout;

        //    //Edit By Xkp
        //    //异步调用函数的线程放到SplashForm中进行启动，避免在SplashForm起来之前函数已经返回
        //    //现在新线程只会处理异步函数的调用和返回，不再处理和取消相关的逻辑
        //    var thread = new Thread(AsyncProcessFuncThread);
        //    m_PSplash = CreateSplashForm(m_CanCancel, statusText, thread, objPar);
        //    DialogResult result = m_PSplash.ShowDialog(container);
        //    m_PSplash.Close();
        //    //如果返回结果是OK，则是从新线程返回的，如果是其他，则是从SplashForm返回
        //    if (result != DialogResult.OK)
        //    {
        //        m_ThreadNoNeedRun = true;

        //        var asyncCallResult = new AsyncCallResult();
        //        asyncCallResult.CallException = new AsynCallUserAbortException();
        //        m_CallResult = asyncCallResult;

        //        InvokeCompletedCallback();
        //    }
        //    else
        //    {
        //        InvokeCompletedCallback();
        //    }
        //}

        public static void HorizontalCentralizeControl(Control parentCtrl, ICollection childControls)
        {
            if (parentCtrl != null && childControls != null && childControls.Count > 0)
            {
                foreach (Control childControl in childControls)
                {
                    HorizontalCentralizeControl(parentCtrl, childControl);
                }
            }
        }

        public static void HorizontalCentralizeControl(Control parentCtrl, Control childControl)
        {
            if (parentCtrl != null && childControl != null)
            {
                if (parentCtrl.Width < childControl.Width)
                {
                    childControl.Left = 0;
                }
                else
                {
                    childControl.Left = parentCtrl.Width / 2 - childControl.Width / 2;
                }
            }
        }

        #endregion

        #region Private helper functions

        //private ProgressSplash CreateSplashForm(bool cancelable, string statusText, Thread invokeThread, object threadPara)
        //{
        //    var splashForm = new ProgressSplash(statusText, cancelable, invokeThread, threadPara);

        //    splashForm.StartPosition = this.m_SplashFormStartPosition;
        //    if (m_SplashFormStartPosition == FormStartPosition.Manual)
        //    {
        //        splashForm.SetLocation(Cursor.Position);
        //    }

        //    if (m_SplashFormBackColor != Color.Empty)
        //    {
        //        splashForm.BackColor = m_SplashFormBackColor;
        //    }
        //    return splashForm;
        //}

        private void AsyncProcessFuncThread(object objPar)
        {
            //如果线程起来之前，已经取消，则该线程不需要再执行
            if(m_ThreadNoNeedRun) return;

            var asyncCallResult = new AsyncCallResult();

            IAsyncResult result = m_FuncNeedAsync.BeginInvoke(objPar, null, null);
            if (result.AsyncWaitHandle.WaitOne(m_Timeout))
            {
                try
                {
                    object objRet = m_FuncNeedAsync.EndInvoke(result);
                    var tmpAsyncCallResult = objRet as AsyncCallResult;
                    if (tmpAsyncCallResult != null)
                    {
                        asyncCallResult.Output = tmpAsyncCallResult.Output;
                        if (tmpAsyncCallResult.CallException != null)
                        {
                            asyncCallResult.CallException = tmpAsyncCallResult.CallException;
                        }
                        else
                        {
                            asyncCallResult.Succeed = true;
                        }
                    }
                    else
                    {
                        asyncCallResult.Output = objRet;
                        asyncCallResult.Succeed = true;
                    }
                }
                catch (Exception e)
                {
                    asyncCallResult.CallException = e;
                    asyncCallResult.Succeed = false;
                }
            }
            else
            {
                asyncCallResult.CallException = new TimeoutException();
                asyncCallResult.Succeed = false;
            }

            try
            {
                // m_Container may be closed and cause exception 
                if (m_Container != null && !m_Container.IsDisposed)
                {
                    m_Container.Invoke(new OneParameterHandler(UICallBack), new object[] { asyncCallResult });
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("### FuncInvokeThread error: " + e.Message);
            }
        }

        private object UICallBack(object objPar)
        {
            try
            {
                //if (m_PSplash != null)
                //{
                //    m_CallResult = objPar;
                //    m_PSplash.DialogResult = DialogResult.OK;
                //}
            }
            catch (Exception e)
            {
                Debug.WriteLine("### UICallBack error: " + e.Message);
            }
            return null;
        }

        private void InvokeCompletedCallback()
        {
            try
            {
                if (m_Container != null && !m_Container.IsDisposed)
                {
                    m_CompletedCallBack.Invoke(m_CallResult);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("### m_CallBack.Invoke(m_CallResult) error: " + TextUtil.FormatExceptionMsg(ex));
            }
        }

        #endregion

    }

    public class AsyncCallResult
    {
        public object Output { get; set; }

        public Exception CallException { get; set; }

        public bool Succeed { get; set; }
    }

    public class AsynCallUserAbortException : Exception
    {
        public AsynCallUserAbortException()
            : base("User aborted the operation")
        {

        }
    }

    public delegate void NoParameterHandler();
    public delegate object OneParameterHandler(object para);
}
