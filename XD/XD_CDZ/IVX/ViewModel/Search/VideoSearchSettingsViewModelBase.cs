using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Protocol;
using System.Diagnostics;
using BOCOM.IVX.Framework;
using Microsoft.Practices.Prism.Events;

namespace BOCOM.IVX.ViewModel.Search
{
    public class VideoSearchSettingsViewModelBase : ViewModelBase
    {
        #region Fields
        
        protected Dictionary<string, object> m_dtParaKey2Val;

        protected SearchPara m_SearchPara;

        protected DateTime m_DTTempStart;
        protected DateTime m_DTTempEnd;

        private string m_SearchStatusText;

        private bool m_SearchCancelSearchEnabled = true;

        private System.Windows.Forms.Timer m_Timer;

        private static readonly int MIN_SEARCH_INTERVAL = 2000;

        #endregion

        #region Properties
        
        public DateTime StartTime
        {
            get { return m_SearchPara.StartTime; }
            set
            {
                if (value >= BOCOM.DataModel.Common.ZEROTIME && m_SearchPara.StartTime != value)
                {
                    m_SearchPara.StartTime = value;
                    Debug.WriteLine("StartTime: " + value.ToString(DataModel.Constant.DATETIME_FORMAT));
                }
            }
        }
        
        public DateTime EndTime
        {
            get { return m_SearchPara.EndTime; }
            set
            {
                if (value >= BOCOM.DataModel.Common.ZEROTIME && m_SearchPara.EndTime != value)
                {
                    m_SearchPara.EndTime = value;
                    Debug.WriteLine("StartTime: " + value.ToString(DataModel.Constant.DATETIME_FORMAT));
                }
            }
        }

        public bool IsMinStartTime
        {
            get
            {
                return StartTime == BOCOM.DataModel.Common.ZEROTIME;
            }
            set
            {
                if (value)
                {
                    if (StartTime != BOCOM.DataModel.Common.ZEROTIME)
                    {
                        m_DTTempStart = StartTime;
                    }
                    m_SearchPara.StartTime = BOCOM.DataModel.Common.ZEROTIME;
                }
                else
                {
                    StartTime = m_DTTempStart;
                }
            }
        }

        public bool IsMaxEndTime
        {
            get
            {
                return EndTime == BOCOM.DataModel.Common.MAXTIME;
            }
            set
            {
                if (value)
                {
                    if (EndTime != BOCOM.DataModel.Common.MAXTIME)
                    {
                        m_DTTempEnd = EndTime;
                    }
                    m_SearchPara.EndTime = BOCOM.DataModel.Common.MAXTIME;
                }
                else
                {
                    EndTime = m_DTTempEnd;
                }
            }
        }

        protected virtual SearchType SearchType
        {
            get
            {
                return DataModel.SearchType.Normal;
            }
        }

        public string SearchStatusText
        {
            get
            {
                return m_SearchStatusText;
            }
            set
            {
                m_SearchStatusText = value;
                RaisePropertyChangedEvent("SearchStatusText");
            }
        }

        public bool CanSearchOrCancelSearch
        {
            get
            {
                return m_SearchCancelSearchEnabled;
            }
            set
            {
                m_SearchCancelSearchEnabled = value;
                RaisePropertyChangedEvent("CanSearchOrCancelSearch");
            }
        }


        #endregion

        #region Constructors
        
        public VideoSearchSettingsViewModelBase()
        {
            m_SearchPara = new SearchPara();
            
            m_DTTempEnd = DateTime.Now;
            m_DTTempStart = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            m_Timer = new System.Windows.Forms.Timer();
            m_Timer.Interval = MIN_SEARCH_INTERVAL;
            m_Timer.Tick += new EventHandler(Timer_Tick);

            // 注册检索事件, TODO: 实现取消检索功能
            // Framework.Container.Instance.EvtAggregator.GetEvent<SearchBeginEvent>().Subscribe(OnSearchBegin, ThreadOption.WinFormUIThread);
        }

        #endregion

        public void Commit()
        {
            TaskUnitInfo[] taskUnits = Framework.Container.Instance.SelectedTaskUnitsForSearch;
           if(Validate(taskUnits))
            {
                // ComposeSearchPara();
                bool multiItem = taskUnits.Length > 1;
                SearchResultDisplayMode displayMode = Framework.Environment.GetDisplayMode(SearchType);
                m_SearchPara.PageInfo.CountPerPage = Framework.Environment.GetDefaultCountPerPage(multiItem, displayMode);
                SearchItem[] searchItems = GetSearchItems(taskUnits);
                if(searchItems != null)
                {
                    m_SearchPara.CurrentSearchItemIndex = 0;
                    m_SearchPara.SearchItems = searchItems.ToList();
                }
                CommitEx();
            }
        }

        protected virtual bool Validate(TaskUnitInfo[] taskUnits)
        {
            bool result = true;
            if (taskUnits == null || taskUnits.Length == 0)
            {
                result = false;
                Framework.Container.Instance.InteractionService.ShowMessageBox("请选中视频资源后再进行检索！", Framework.Environment.PROGRAM_NAME,
                  System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return result;
            }

            if (taskUnits.Length > Common.Constant.TASKUNIT_MAXIMUM_SEARCH)
            {
                result = false;
                Framework.Container.Instance.InteractionService.ShowMessageBox(string.Format("一次检索最多不能超过 {0} 路, 当前已经选中 {1} 路", Common.Constant.TASKUNIT_MAXIMUM_SEARCH, taskUnits.Length),
                    Framework.Environment.PROGRAM_NAME, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return result;
            }

            if(StartTime>=EndTime)
            {
                result = false;
                Framework.Container.Instance.InteractionService.ShowMessageBox("检索高级参数中开始时间必须小于结束时间，请重新设置时间后再进行检索！", Framework.Environment.PROGRAM_NAME,
                  System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return result;

            }
            return result;
        }

        private SearchItem[] GetSearchItems(TaskUnitInfo[] taskUnits)
        {
            SearchItem[] searchItems = new SearchItem[taskUnits.Length];

            PageInfoBase pageInfo = new PageInfoBase()
            {
                Index = Framework.Environment.DefaultPageIndex,
                CountPerPage = m_SearchPara.PageInfo.CountPerPage
            };

            int index;
            for (index = 0; index < taskUnits.Length; index++)
            {
                searchItems[index] = taskUnits[index].ToSearchItem(pageInfo);
            }

                return searchItems;
        }

        protected virtual void ComposeSearchPara()
        {
            m_SearchPara = new SearchPara();
        }

        protected virtual void CommitEx()
        {
            // 需要clone 一份SearchPara， 避免与界面上的重用冲突
            SearchPara searchPara = m_SearchPara.Clone() as SearchPara;
            try
            {
                Framework.Container.Instance.VideoSearchService.StartSearch(m_SearchPara);
                CanSearchOrCancelSearch = false;
                m_Timer.Start();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "检索");
            }
        }

        private void OnSearchBegin(SearchSession session)
        {
            if (session.SearchPara.SearchType == SearchType)
            {
                // 显示取消， 检索完毕后再恢复
                SearchStatusText = "取消";
                CanSearchOrCancelSearch = true;
            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            m_Timer.Stop();
            CanSearchOrCancelSearch = true;
        }

    }
}
