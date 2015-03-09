using System;
using System.Collections.Generic;
using BOCOM.IVX.ViewModel;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;

namespace BOCOM.IVX.Controls
{
    public interface ISortableListView
    {
        event EventHandler<VAResultRecordSelectedEventArgs> SelectedIndexChanged;
        event EventHandler<VAResultRecordDoubleClickedEventArgs> DoubleClicked;

        bool SupportMixAndSingleMode { get;}

        //SearchResultSummaryOld ResultSummary { get; set; }

        IList<FieldInfo> Fields { get;}

        FieldInfo SortField { get; set; }

        List<IVAResultRecord> VAResultRecords { get; }
        
        bool IsMixedMode { get; set; }

        void SwitchDisplayMode();

        void ShowLoadingStatus();

        void DisplaySummary(/*SearchResultSummaryOld summary*/);

        void DisplayResult(/*SearchResultSummaryOld summary*/);

        // void RefreshContent(SearchResultSummary summary, PageInfo pagesInfo, IEnumerable<IVAResultRecord> snapshots);

        void SetSize(int width, int height);

        void Clear();
    }

    public class VAResultRecordSelectedEventArgs : EventArgs
    {
        IVAResultRecord m_vaResultRecord;

        public IVAResultRecord VAResultRecord
        {
            get
            {
                return m_vaResultRecord;
            }
        }

        public VAResultRecordSelectedEventArgs(IVAResultRecord record)
        {
            m_vaResultRecord = record;
        }
    }

    public class VAResultRecordDoubleClickedEventArgs : EventArgs
    {
        IVAResultRecord m_vaResultRecord;
        private List<IVAResultRecord> m_vaResultRecords;

        public IVAResultRecord VAResultRecord
        {
            get
            {
                return m_vaResultRecord;
            }
        }

        public List<IVAResultRecord> VAResultRecords
        {
            get
            {
                return m_vaResultRecords;
            }
        }

        public VAResultRecordDoubleClickedEventArgs(IVAResultRecord record, List<IVAResultRecord> records)
        {
            m_vaResultRecord = record;
            m_vaResultRecords = records;
        }
    }

    public class SortFieldChangedEventArgs : EventArgs
    {
        private FieldInfo m_fieldInfo;

        public FieldInfo SortField
        {
            get
            {
                return m_fieldInfo;
            }
        }
                
        public SortFieldChangedEventArgs(FieldInfo fieldInfo)
        {
            m_fieldInfo = fieldInfo;
        }
    }

    public class FieldInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsAscendant { get; set; }

        public FieldInfo()
        {

        }

        public FieldInfo(string displayName, string name, bool isASC)
        {
            Name = name;
            DisplayName = displayName;
            IsAscendant = isASC;
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            FieldInfo fi2 = obj as FieldInfo;
            if (fi2 != null)
            {
                result = this.IsAscendant == fi2.IsAscendant &&
                    string.Compare(Name, fi2.Name, true) == 0;
            }

            return result;
        }
    }
}
