using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOCOM.IVX.Views.Content
{
    public partial class formDevSetting : Form
    {
        public formDevSetting(string devID)
        {
            InitializeComponent();
            TextBoxDevidSearch.Value = devID;
            Framework.Container.Instance.DevStateService.OnSetParamRet += DevStateService_OnSetParamRet;
            Framework.Container.Instance.DevStateService.OnGetParamRet += DevStateService_OnGetParamRet;
            Framework.Container.Instance.DevStateService.OnGetChargePriceRet += DevStateService_OnGetChargePriceRet;
            Framework.Container.Instance.DevStateService.OnSetChargePriceRet += DevStateService_OnSetChargePriceRet;

        }
        void DevStateService_OnSetChargePriceRet(object sender, EventArgs e)
        {
            XDTCPProtocol.SetChargePriceRet ret = (XDTCPProtocol.SetChargePriceRet)sender;
            string str = "";
            if (ret.RetFlag > 0) str += "设置失败。";

            labelRet2.Text = str == "" ? "设置成功" : str;
        }

        void DevStateService_OnGetChargePriceRet(object sender, EventArgs e)
        {
            XDTCPProtocol.GetChargePriceRet ret = (XDTCPProtocol.GetChargePriceRet)sender;
            doubleInputFlatPrice.Value = ret.FlatPrice / 100000f;
            doubleInputPeakPrice.Value = ret.PeakPrice / 100000f;
            doubleInputTaperPrice.Value = ret.TaperPrice / 100000f;
            doubleInputValleyPrice.Value = ret.ValleyPrice / 100000f;
        }

        void DevStateService_OnGetParamRet(object sender, EventArgs e)
        {
            XDTCPProtocol.GetDevParamRet ret = (XDTCPProtocol.GetDevParamRet)sender;
            integerInputDevAddr.Value = ret.DevAddr;
            integerInputStation.Value = ret.Station;
            switchButtonControl.Value = ret.Control == 1;
            comboBoxExElock.SelectedIndex = ret.ELock;
            doubleInputRatio.Value = ret.Ratio / 100f;
            textBoxXPassword.Text = new string(ret.Password);
            switchButtonModel.Value = ret.Model == 1;
            integerInputContrast.Value = ret.Contrast;
            integerInputBackLight.Value = ret.BackLight;
        }

        void DevStateService_OnSetParamRet(object sender, EventArgs e)
        {
            XDTCPProtocol.SetDevParamRet ret = (XDTCPProtocol.SetDevParamRet)sender;
            string str = "";
            if (ret.BackLightEnable == 1) str += "背光时间设置失败。";
            if (ret.ContrastEnable == 1) str += "对比度设置失败。";
            if (ret.ControlEnable == 1) str += "控制引导设置失败。";
            if (ret.DevAddrEnable == 1) str += "设备地址设置失败。";
            if (ret.ELockEnable == 1) str += "电子锁设置失败。";
            if (ret.ModelEnable == 1) str += "账户模式设置失败。";
            if (ret.PasswordEnable == 1) str += "维护密码设置失败。";
            if (ret.RatioEnable == 1) str += "占空比设置失败。";
            if (ret.StationEnable == 1) str += "站级地址设置失败。";

            labelRet.Text = str == "" ? "全部设置成功" : str;

        }
        private void buttonSetAllParam_Click(object sender, EventArgs e)
        {
            Framework.Container.Instance.DevStateService.SetDevParam(TextBoxDevidSearch.Value,
                (Int16)integerInputDevAddr.Value, checkBoxDevAddrEnable.Checked,
                (Int16)integerInputStation.Value, checkBoxStationEnable.Checked,
                switchButtonControl.Value, checkBoxControlEnable.Checked,
                (byte)comboBoxExElock.SelectedIndex, checkBoxELockEnable.Checked,
                (float)doubleInputRatio.Value, checkBoxRatioEnable.Checked,
                textBoxXPassword.Text, checkBoxPasswordEnable.Checked,
                switchButtonModel.Value, checkBoxModelEnable.Checked,
                (byte)integerInputContrast.Value, checkBoxContrastEnable.Checked,
                (byte)integerInputBackLight.Value, checkBoxBackLightEnable.Checked);
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxBackLightEnable.Checked = checkBoxAll.Checked;
            checkBoxContrastEnable.Checked = checkBoxAll.Checked;
            checkBoxControlEnable.Checked = checkBoxAll.Checked;
            checkBoxDevAddrEnable.Checked = checkBoxAll.Checked;
            checkBoxELockEnable.Checked = checkBoxAll.Checked;
            checkBoxModelEnable.Checked = checkBoxAll.Checked;
            checkBoxPasswordEnable.Checked = checkBoxAll.Checked;
            checkBoxRatioEnable.Checked = checkBoxAll.Checked;
            checkBoxStationEnable.Checked = checkBoxAll.Checked;
        }

        private void checkBoxDevAddrEnable_CheckedChanged(object sender, EventArgs e)
        {
            integerInputDevAddr.Enabled = checkBoxDevAddrEnable.Checked;
        }

        private void checkBoxStationEnable_CheckedChanged(object sender, EventArgs e)
        {
            integerInputStation.Enabled = checkBoxStationEnable.Checked;
        }

        private void checkBoxControlEnable_CheckedChanged(object sender, EventArgs e)
        {
            switchButtonControl.Enabled = checkBoxControlEnable.Checked;
        }

        private void checkBoxELockEnable_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxExElock.Enabled = checkBoxELockEnable.Checked;
        }

        private void checkBoxRatioEnable_CheckedChanged(object sender, EventArgs e)
        {
            doubleInputRatio.Enabled = checkBoxRatioEnable.Checked;
        }

        private void checkBoxPasswordEnable_CheckedChanged(object sender, EventArgs e)
        {
            textBoxXPassword.Enabled = checkBoxPasswordEnable.Checked;
        }

        private void checkBoxModelEnable_CheckedChanged(object sender, EventArgs e)
        {
            switchButtonModel.Enabled = checkBoxModelEnable.Checked;
        }

        private void checkBoxContrastEnable_CheckedChanged(object sender, EventArgs e)
        {
            integerInputContrast.Enabled = checkBoxContrastEnable.Checked;
        }

        private void checkBoxBackLightEnable_CheckedChanged(object sender, EventArgs e)
        {
            integerInputBackLight.Enabled = checkBoxBackLightEnable.Checked;
        }
        private void btnGetParam_Click(object sender, EventArgs e)
        {
            Framework.Container.Instance.DevStateService.GetDevParam(TextBoxDevidSearch.Value);
        }

        private void buttonSetChargeValue_Click(object sender, EventArgs e)
        {
            Framework.Container.Instance.DevStateService.SetChargePrice(
                TextBoxDevidSearch.Value,
                (float)doubleInputTaperPrice.Value,
                (float)doubleInputPeakPrice.Value,
                (float)doubleInputFlatPrice.Value,
                (float)doubleInputValleyPrice.Value
                );

        }

        private void buttonGetPrice_Click(object sender, EventArgs e)
        {
            Framework.Container.Instance.DevStateService.GetChargePrice(TextBoxDevidSearch.Value);

        }

        private void formDevSetting_Load(object sender, EventArgs e)
        {
            integerInputDevAddr.Value = 0;
            integerInputStation.Value = 0;
            switchButtonControl.Value = false;
            comboBoxExElock.SelectedIndex = 0;
            doubleInputRatio.Value = 0;
            textBoxXPassword.Text = "";
            switchButtonModel.Value = false;
            integerInputContrast.Value = 0;
            integerInputBackLight.Value = 0;
            comboBoxServiceState.SelectedIndex 
                = Framework.Container.Instance.DevStateService.GetDevByID(TextBoxDevidSearch.Value).ServiceStat;
            TextBoxNewDevID.Value = TextBoxDevidSearch.Value;
        }

        private void buttonSetServiceState_Click(object sender, EventArgs e)
        {
            Framework.Container.Instance.DevStateService.SetServiceState(
                TextBoxDevidSearch.Value,
                comboBoxServiceState.SelectedIndex    
                );

        }

        private void buttonSetDevID_Click(object sender, EventArgs e)
        {
            
            Framework.Container.Instance.DevStateService.SetDevID(
                TextBoxDevidSearch.Value,
                TextBoxNewDevID.Value    
                );
        }


    }
}
