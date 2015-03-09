using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace BOCOM.IVX.Framework
{
    public class VVMDataBindings
    {
        #region Fields

        private Dictionary<string, List<DataBindingCtrlInfo>> m_DTDataSource2ControlProperty = new Dictionary<string, List<DataBindingCtrlInfo>>();

        private List<INotifyPropertyChanged> m_RegisteredDataSource = new List<INotifyPropertyChanged>();

        #endregion

        #region Private helper functions

        private string GetKey(INotifyPropertyChanged dataSource, string dataMember)
        {
            return string.Format("{0}#{1}", dataSource.GetHashCode(), dataMember);
        }

        private List<DataBindingCtrlInfo> GetBindingCtrlInfos(object sender, PropertyChangedEventArgs e)
        {
            List<DataBindingCtrlInfo> bindingCtrlInfos = null;
            INotifyPropertyChanged dataSource = sender as INotifyPropertyChanged;
            if (dataSource != null)
            {
                string key = GetKey(dataSource, e.PropertyName);
                if (m_DTDataSource2ControlProperty.ContainsKey(key))
                {
                    bindingCtrlInfos = m_DTDataSource2ControlProperty[key];
                }
            }
            return bindingCtrlInfos;
        }

        private void HandleDataSourceChange(object sender, PropertyChangedEventArgs e)
        {
            List<DataBindingCtrlInfo> bindingCtrlInfos = GetBindingCtrlInfos(sender, e);

            if (bindingCtrlInfos != null && bindingCtrlInfos.Count > 0)
            {
                bindingCtrlInfos.ForEach(binding => HandleDataSourceChange(sender, e, binding));
            }
        }

        private void HandleDataSourceChange(object sender, PropertyChangedEventArgs e, DataBindingCtrlInfo bindingCtrlInfo)
        {
            if (bindingCtrlInfo != null)
            {
                object val = sender.GetType().GetProperty(e.PropertyName).GetValue(sender, null);
                if (string.Compare(bindingCtrlInfo.PropertyName, "Visible", true) == 0)
                {
                    // 修改Visible 属性不生效， 这里改为调用 Show， Hide 方法
                    Control ctrl = bindingCtrlInfo.Ctrl;
                    bool show;
                    if (Boolean.TryParse(val.ToString(), out show))
                    {
                        if (show)
                        {
                            ctrl.Show();
                        }
                        else
                        {
                            ctrl.Hide();
                        }
                    }
                }
                else
                {
                    if (string.Compare(bindingCtrlInfo.PropertyName, "Text", true) == 0 && val != null)
                    {
                        bindingCtrlInfo.Ctrl.GetType().GetProperty(bindingCtrlInfo.PropertyName).SetValue(
                            bindingCtrlInfo.Ctrl, val.ToString(), null);
                    }
                    else
                    {
                        bindingCtrlInfo.Ctrl.GetType().GetProperty(bindingCtrlInfo.PropertyName).SetValue(
                            bindingCtrlInfo.Ctrl, val, null);
                    }
                }
            }
        }

        #endregion

        public void AddBinding(Control control, string propertyName, INotifyPropertyChanged dataSource, string dataMember, DataSourceUpdateMode dataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged)
        {
            Binding binding = new Binding(propertyName, dataSource, dataMember);
            binding.ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
            binding.DataSourceUpdateMode = dataSourceUpdateMode;

            control.DataBindings.Add(binding);
            // 对同一个DataSource，仅需要注册一次事件Handler
            if (!m_RegisteredDataSource.Contains(dataSource))
            {
                dataSource.PropertyChanged += new PropertyChangedEventHandler(dataSource_PropertyChanged);
                m_RegisteredDataSource.Add(dataSource);
            }

            // build mapping from datasource + datamember -> control + propertyname
            string key = GetKey(dataSource, dataMember);

            List<DataBindingCtrlInfo> bindingCtrlInfos;
            DataBindingCtrlInfo bindingCtrlInfo = new DataBindingCtrlInfo(control, propertyName, dataSource);
            if (m_DTDataSource2ControlProperty.ContainsKey(key))
            {
                bindingCtrlInfos = m_DTDataSource2ControlProperty[key];
                bindingCtrlInfos.Add(bindingCtrlInfo);
            }
            else
            {
                bindingCtrlInfos = new List<DataBindingCtrlInfo>();
                bindingCtrlInfos.Add(bindingCtrlInfo);
                m_DTDataSource2ControlProperty.Add(key, bindingCtrlInfos);
            }

            // MyLog4Net.Container.Instance.Log.DebugFormat("Add Binding key {0}, control: {1}, type {2}", key, control.GetHashCode(), control.Name);
        }

        public void RemoveBinding(INotifyPropertyChanged dataSource)
        {
            if (m_RegisteredDataSource.Contains(dataSource))
            {
                dataSource.PropertyChanged -= new PropertyChangedEventHandler(dataSource_PropertyChanged);
                m_RegisteredDataSource.Remove(dataSource);
            }
        }

        public void RemoveBinding(INotifyPropertyChanged dataSource, string dataMember, Control bindingControl)
        {
            if (dataSource != null && bindingControl != null && !string.IsNullOrEmpty(dataMember))
            {
                string key = GetKey(dataSource, dataMember);
                if (m_DTDataSource2ControlProperty.ContainsKey(key))
                {
                    List<DataBindingCtrlInfo> bindingCtrlInfos = m_DTDataSource2ControlProperty[key];
                    if (bindingCtrlInfos != null && bindingCtrlInfos.Count > 0)
                    {
                        List<DataBindingCtrlInfo> dbCtrlInfosObsolete = new List<DataBindingCtrlInfo>();
                        foreach (DataBindingCtrlInfo dbCtrlInfo in bindingCtrlInfos)
                        {
                            if (dbCtrlInfo.Ctrl == bindingControl)
                            {
                                dbCtrlInfosObsolete.Add(dbCtrlInfo);
                            }
                        }

                        // MyLog4Net.Container.Instance.Log.DebugFormat("Remove Binding {0} matched", dbCtrlInfosObsolete.Count);
                        foreach (DataBindingCtrlInfo dbCtrlInfo in dbCtrlInfosObsolete)
                        {
                            // MyLog4Net.Container.Instance.Log.DebugFormat("Remove Binding key {0}, control: {1}, type {2}", key, dbCtrlInfo.Ctrl.GetHashCode(), dbCtrlInfo.Ctrl.Name);
                            bindingCtrlInfos.Remove(dbCtrlInfo);
                            List<Binding> bindings = new List<Binding>();
                            foreach (Binding binding in dbCtrlInfo.Ctrl.DataBindings)
                            {
                                if (binding.DataSource == dataSource && string.Compare(binding.BindingMemberInfo.BindingMember, dataMember, true) == 0)
                                {
                                    bindings.Add(binding);
                                }
                            }

                            if (bindings.Count > 0)
                            {
                                foreach (Binding binding in bindings)
                                {
                                    dbCtrlInfo.Ctrl.DataBindings.Remove(binding);
                                }
                            }

                            dbCtrlInfo.Ctrl = null;                            
                        }

                        if (bindingCtrlInfos.Count == 0)
                        {
                            m_DTDataSource2ControlProperty.Remove(key);
                        }
                    }
                }
            }
        }

        public void RemoveBindings(INotifyPropertyChanged dataSource)
        {
            if (dataSource != null)
            {
                string sKey = dataSource.GetHashCode().ToString();
                List<string> keys = new List<string>();
                foreach (KeyValuePair<string, List<DataBindingCtrlInfo>> kv in m_DTDataSource2ControlProperty)
                {
                    if (kv.Key.StartsWith(sKey))
                    {
                        keys.Add(kv.Key);
                    }
                }

                if (keys.Count > 0)
                {
                    foreach (string key in keys)
                    {
                        m_DTDataSource2ControlProperty.Remove(key);
                    }
                }

                if (m_RegisteredDataSource.Contains(dataSource))
                {
                    dataSource.PropertyChanged -= new PropertyChangedEventHandler(dataSource_PropertyChanged);
                    m_RegisteredDataSource.Remove(dataSource);
                }
            }
        }

        public void RemoveBindings()
        {
            m_RegisteredDataSource.ForEach(item => item.PropertyChanged -= new PropertyChangedEventHandler(dataSource_PropertyChanged));
            m_RegisteredDataSource.Clear();
            m_DTDataSource2ControlProperty.Clear();
        }

        public Control GetControl(INotifyPropertyChanged dataSource, string dataMember)
        {
            Control ctrl = null;
            if (dataMember != null && !string.IsNullOrEmpty(dataMember))
            {
                string key = GetKey(dataSource, dataMember);
                if (m_DTDataSource2ControlProperty.ContainsKey(key))
                {
                    ctrl = m_DTDataSource2ControlProperty[key][0].Ctrl;
                }
            }
            return ctrl;
        }

        void dataSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HandleDataSourceChange(sender, e);
        }
    }

    #region Classes

    public class DataBindingCtrlInfo
    {
        public Control Ctrl { get; set; }

        public string PropertyName { get; set; }

        public INotifyPropertyChanged DataSource { get; set; }

        public DataBindingCtrlInfo(Control ctrl, string propertyName, INotifyPropertyChanged dataSource)
        {
            Ctrl = ctrl;
            PropertyName = propertyName;
            DataSource = dataSource;
        }
    }

    #endregion

}
