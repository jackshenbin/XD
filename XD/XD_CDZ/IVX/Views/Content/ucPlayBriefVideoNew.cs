using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BOCOM.IVX.Views.Content;
using BOCOM.IVX.ViewModel;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX
{
    public partial class ucPlayBriefVideoNew : ucContentBase
	{
        private PlayBriefVideoViewModel m_viewModel;

        public ucPlayBriefVideoNew()
		{
            InitializeComponent();
            m_viewModel = new PlayBriefVideoViewModel(xtraSinglePlayer1);
            m_viewModel.PlayPosChange += new Action<XtraSinglePlayer,uint>(m_viewModel_PlayPosChange);
            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxPlay, "Enabled", m_viewModel, "PlayBtnEnable");
            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxStop, "Enabled", m_viewModel, "StopBtnEnable");
            //Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxPrivFrame, "Enabled", m_viewModel, "PrivFrameBtnEnable");
            //Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxNextFrame, "Enabled", m_viewModel, "NextFrameBtnEnable");
            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxSlow, "Enabled", m_viewModel, "SlowBtnEnable");
            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxFast, "Enabled", m_viewModel, "FastBtnEnable");

            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxPlay, "Image", m_viewModel, "PlayBtnImage");
            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxPlay, "CheckedImage", m_viewModel, "PlayBtnCheckedImage");
            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxPlay, "MouseOverImage", m_viewModel, "PlayBtnMouseOverImage");
            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxPlay, "DisableImage", m_viewModel, "PlayBtnDisableImage");
            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxPlay, "OrignalImage", m_viewModel, "PlayBtnOrigianlImage");

            xtraSinglePlayer1.ProgressChanged += new  Action<XtraSinglePlayer,uint,uint>(singlePlayer_ProgressChanged);
            xtraSinglePlayer1.CloseClicked += xtraSinglePlayer1_CloseClicked;
            xtraSinglePlayer1.IsDBClickFullScreen = false;
            Framework.Container.Instance.VVMDataBindings.AddBinding(TimeOverlayer,"Checked",m_viewModel,"TimeOverlayer");
            Framework.Container.Instance.VVMDataBindings.AddBinding(MoveObjOverlayer,"Checked",m_viewModel,"MoveObjOverlayer");
            Framework.Container.Instance.VVMDataBindings.AddBinding(AvtionOverlayer,"Checked",m_viewModel,"AvtionOverlayer");
            Framework.Container.Instance.VVMDataBindings.AddBinding(AreaOverlayer,"Checked",m_viewModel,"AreaOverlayer");

            checkEditAll.Tag = E_VDA_MOVEOBJ_TYPE.E_VDA_MOVEOBJ_TYPE_ALL;
            checkEditCar.Tag = E_VDA_MOVEOBJ_TYPE.E_VDA_MOVEOBJ_TYPE_CAR;
            checkEditPeople.Tag = E_VDA_MOVEOBJ_TYPE.E_VDA_MOVEOBJ_TYPE_PEOPLE;

            Framework.Container.Instance.VVMDataBindings.AddBinding(btnSetBrief, "Enabled", m_viewModel, "SetBriefBtnEnable");
            
            
            //Framework.Container.Instance.VVMDataBindings.AddBinding(btnExportVideo, "Enabled", m_viewModel, "ExportVideoBtnEnable");
            //Framework.Container.Instance.VVMDataBindings.AddBinding(btnMark, "Enabled", m_viewModel, "MarkBtnEnable");
            btnPlayBack.Enabled = false;
            btnExportVideo.Enabled = false;

            Framework.Container.Instance.VVMDataBindings.AddBinding(btnPassline, "Checked", m_viewModel, "PasslineDrawFilter");
            Framework.Container.Instance.VVMDataBindings.AddBinding(btnBreakArea, "Checked", m_viewModel, "BreakAreaDrawFilter");
            Framework.Container.Instance.VVMDataBindings.AddBinding(btnSheild, "Checked", m_viewModel, "SheildDrawFilter");
            Framework.Container.Instance.VVMDataBindings.AddBinding(btnInterest, "Checked", m_viewModel, "InterestDrawFilter");

            Framework.Container.Instance.VVMDataBindings.AddBinding(labelControlTime, "Text", m_viewModel, "TimeInfo");

            ColorName[] colors = Framework.Container.Instance.ColorService.GetMoveObjColors();
            colorComboBoxEx1.SetColors(colors);
            colorComboBoxEx1.SelectedColor = Color.FromArgb( m_viewModel.BriefdwMoveObjColorFilter);

            //colorComboBoxEx1.Enabled  = false;
            //Framework.Container.Instance.VVMDataBindings.AddBinding(zoomTrackBarDensity, "EditValue", m_viewModel, "BriefDensityFilter");
            zoomTrackBarDensity.Value = m_viewModel.NBriefDensityFilter;

            Framework.Container.Instance.VVMDataBindings.AddBinding(timeEditStart, "Time", m_viewModel, "StartTime");
            Framework.Container.Instance.VVMDataBindings.AddBinding(timeEditEnd, "Time", m_viewModel, "EndTime");

            Framework.Container.Instance.VVMDataBindings.AddBinding(btnSaveObjectPic, "Enabled", m_viewModel, "HasSelectedBriefObject");
            Framework.Container.Instance.VVMDataBindings.AddBinding(btnPlayBack, "Enabled", m_viewModel, "HasSelectedBriefObject");
            Framework.Container.Instance.VVMDataBindings.AddBinding(pictureBoxGotoCompare, "Enabled", m_viewModel, "HasSelectedBriefObject");
            Framework.Container.Instance.VVMDataBindings.AddBinding(splitContainerControl1, "Collapsed", m_viewModel, "HidePlayBackWnd");
            //Framework.Container.Instance.VVMDataBindings.AddBinding(btnSaveObjectPic, "Enabled", m_viewModel, "SaveObjectPicBtnEnable");
            //Framework.Container.Instance.VVMDataBindings.AddBinding(btnPlayBack, "Enabled", m_viewModel, "PlayBackBtnEnable");
            m_viewModel.PropertyChanged += new PropertyChangedEventHandler(m_viewModel_PropertyChanged);
            
		}

        #region Event handlers


        void xtraSinglePlayer1_CloseClicked(XtraSinglePlayer obj)
        {
            m_viewModel.CloseBriefVideo();
        }

        void m_viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NBriefDensityFilter")
            {
                zoomTrackBarDensity.Value = m_viewModel.NBriefDensityFilter;
            }
            else if (e.PropertyName == "BriefdwMoveObjColorFilter")
            { 
                colorComboBoxEx1.SelectedColor = Color.FromArgb(m_viewModel.BriefdwMoveObjColorFilter); 
            }
            else if (e.PropertyName == "BriefMoveObjTypeFilter")
            {
                if (m_viewModel.BriefMoveObjTypeFilter == E_VDA_MOVEOBJ_TYPE.E_VDA_MOVEOBJ_TYPE_ALL)
                    checkEditAll.Checked = true;
                else if (m_viewModel.BriefMoveObjTypeFilter == E_VDA_MOVEOBJ_TYPE.E_VDA_MOVEOBJ_TYPE_CAR)
                    checkEditCar.Checked = true;
                else if (m_viewModel.BriefMoveObjTypeFilter == E_VDA_MOVEOBJ_TYPE.E_VDA_MOVEOBJ_TYPE_PEOPLE)
                    checkEditPeople.Checked = true;
                else
                    checkEditAll.Checked = true;

            }
        }
        
        void singlePlayer_ProgressChanged(XtraSinglePlayer sender, uint curr,uint max)
        {
            m_viewModel.PosBriefVideo(curr);
        }

        void m_viewModel_PlayPosChange(XtraSinglePlayer player,uint playpercent)
        {
            //VideoStatusInfo info = (VideoStatusInfo)sender;

            player.SetProgress((int)playpercent, 1000);
        }
        
        private void btnSetBrief_CheckedChanged(object sender, EventArgs e)
        {
            if (btnSetBrief.Checked)
            {
                panelBriefSetting.Size = tableLayoutPanel1.Size;
                panelBriefSetting.Location = tableLayoutPanel1.Location;
                panelBriefSetting.Visible = btnSetBrief.Checked;
                xtraSinglePlayer1.EditModel = true;
                m_viewModel.BeginBriefEdit();
            }
        }

        private void btnSetBriefOK_Click(object sender, EventArgs e)
        {
            panelBriefSetting.Visible = btnSetBrief.Checked = false;
            xtraSinglePlayer1.EditModel = false;

            m_viewModel.FinishBriefEdit();

        }
        
        private void singlePlayer_DragEnter(object sender, DragEventArgs e)
        {
            //if ((e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link) e.Effect = DragDropEffects.Link;
            Type dataType = typeof(List<object>);

            if (e.Data.GetDataPresent(dataType))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent("BOCOM.ICAS.Protocol.TSEARCH_RESULT"))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent("BOCOM.ICAS.Protocol.TVEHICLE_INFO"))
            {
                e.Effect = DragDropEffects.Move;
            }

        }

        private void singlePlayer_DragDrop(object sender, DragEventArgs e)
        {
            XtraSinglePlayer p = sender as XtraSinglePlayer;

            Type dataType = typeof(List<object>);

            if (e.Data.GetDataPresent(dataType))
            {
                List<object> selectedNodes = (List<object>)e.Data.GetData(dataType);
                if (panelBriefSetting.Visible)
                {
                    panelBriefSetting.Visible = btnSetBrief.Checked = false;
                    xtraSinglePlayer1.EditModel = false;
                    m_viewModel.CancelBriefEdit();
                }

                m_viewModel.PlayBriefVideo(selectedNodes);
            }
            else if (e.Data.GetDataPresent("BOCOM.ICAS.Protocol.TSEARCH_RESULT"))
            {
            }
            else if (e.Data.GetDataPresent("BOCOM.ICAS.Protocol.TVEHICLE_INFO"))
            {
            }
        }
        
        private void pictureBoxPlay_Click(object sender, EventArgs e)
        {
            m_viewModel.PlayOrPauseVideo();
        }

        private void pictureBoxStop_Click(object sender, EventArgs e)
        {
            m_viewModel.StopBriefVideo();
        }
        
        private void pictureBoxSlow_Click(object sender, EventArgs e)
        {
            m_viewModel.SlowBriefVideo();
        }

        private void pictureBoxFast_Click(object sender, EventArgs e)
        {
            m_viewModel.FastBriefVideo();
        }

        private void btnSetBriefCancel_Click(object sender, EventArgs e)
        {
            panelBriefSetting.Visible = btnSetBrief.Checked = false;
            xtraSinglePlayer1.EditModel = false;
            m_viewModel.CancelBriefEdit();

        }

        private void checkEditMoveObjType_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit c = (CheckEdit)sender;
            m_viewModel.BriefMoveObjTypeFilter = (E_VDA_MOVEOBJ_TYPE)c.Tag;
        }

        private void btnPassline_CheckedChanged(object sender, EventArgs e)
        {
            if(btnPassline.Checked)
                m_viewModel.SetBriefDrawFilter(E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_PASSLINE);
        }

        private void btnBreakArea_CheckedChanged(object sender, EventArgs e)
        {
            if (btnBreakArea.Checked)
                m_viewModel.SetBriefDrawFilter(E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_BREAK_AREA);

        }

        private void checkedPictureBox5_Click(object sender, EventArgs e)
        {
            m_viewModel.ClearBriefDrawFilter(E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_PASSLINE);
        }

        private void btnSheild_CheckedChanged(object sender, EventArgs e)
        {
            if (btnSheild.Checked)
                m_viewModel.SetBriefDrawFilter(E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_SHEILD);

        }

        private void btnInterest_CheckedChanged(object sender, EventArgs e)
        {
            if (btnInterest.Checked)
                m_viewModel.SetBriefDrawFilter(E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_INTEREST);

        }

        private void checkedPictureBox8_Click(object sender, EventArgs e)
        {
            m_viewModel.ClearBriefDrawFilter(E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_SHEILD);

        }

        private void btnPlayBack_Click(object sender, EventArgs e)
        {
            m_viewModel.ObjectPlayBack();
        }

        private void zoomTrackBarDensity_EditValueChanged(object sender, EventArgs e)
        {
            m_viewModel.NBriefDensityFilter =  zoomTrackBarDensity.Value;
        }

        private void colorComboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_viewModel.BriefdwMoveObjColorFilter = colorComboBoxEx1.SelectedColor.ToArgb();

        }

        private void btnSaveObjectPic_Click(object sender, EventArgs e)
        {
            m_viewModel.SaveObjectPic();

        }

        private void pictureBoxGotoCompare_Click(object sender, EventArgs e)
        {
            m_viewModel.GotoCompareSearch();

        }

        #endregion

        private void splitContainerControl1_SizeChanged(object sender, EventArgs e)
        {
            if (!splitContainerControl1.Collapsed)
                splitContainerControl1.SplitterPosition = splitContainerControl1.Width / 2;
        }

        private void splitContainerControl1_SplitGroupPanelCollapsed(object sender, SplitGroupPanelCollapsedEventArgs e)
        {
            if (!splitContainerControl1.Collapsed)
                splitContainerControl1.SplitterPosition = splitContainerControl1.Width / 2;


            if (m_viewModel != null && m_viewModel.HidePlayBackWnd != splitContainerControl1.Collapsed)
                m_viewModel.HidePlayBackWnd = splitContainerControl1.Collapsed;
        }


    }
}
