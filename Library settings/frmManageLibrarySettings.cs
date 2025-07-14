using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Library_Business;

namespace The_Story_Corner_Project.Library_settings
{
    public partial class frmManageLibrarySettings : KryptonForm
    {
        public frmManageLibrarySettings()
        {
            InitializeComponent();
        }

        private void frmManageLibrarySettings_Load(object sender, EventArgs e)
        {
            
            ToolTip toolTip = new ToolTip();
            toolTip.ShowAlways = true;
            toolTip.AutoPopDelay = 5000; // Keeps tooltip visible for 5 seconds
            toolTip.InitialDelay = 500; // Brief delay before showing tooltip
            toolTip.ReshowDelay = 500; // Delay before showing tooltip again
            toolTip.ToolTipIcon = ToolTipIcon.Info;

            // Set enhanced tooltip text for each PictureBox (information icon).
            toolTip.SetToolTip(pbMonthlySubscriptionInfo, "The fee for a 1-month library membership, allowing access to library resources.");
            toolTip.SetToolTip(pbThreeMonthSubscriptionInfo, "The fee for a 3-month library membership, providing extended access to library resources.");
            toolTip.SetToolTip(pbOneYearSubscriptionInfo, "The fee for a 1-year library membership, offering long-term access to all library resources.");

            toolTip.SetToolTip(pbLateReturnFinesInfo, "A fine applied for each week that a borrowed item is overdue after the due date.");
            toolTip.SetToolTip(pbDefaultBorrowDays, "The standard number of days allowed for borrowing an item.");
            toolTip.SetToolTip(pbExtendDays, "The number of additional days that a borrower can request to extend the due date.");
            toolTip.SetToolTip(pbExtendTimes, "The maximum number of times a borrower can request an extension for an item.");

            //Fill subscription types fees 
            MonthlySubscriptionFees.Value = (int)clsSubscriptionType.Find(1).SubscriptionTypeFees.Value;
            ThreeMonthFees.Value = (int)clsSubscriptionType.Find(2).SubscriptionTypeFees.Value;
            OneYearFees.Value = (int)clsSubscriptionType.Find(3).SubscriptionTypeFees.Value;


            //Fill borrow settings info
            LateReturnFees.Value = clsLibrarySettings.GetLateReturnFineAmount();
            DefaultBorrowDays.Value = clsLibrarySettings.GetDefaultBorrowingDays();
            ExtendTimesPerBorrow.Value = clsLibrarySettings.GetDefaultExtendTimesPerBorrow();
            ExtendBorrowDays.Value = clsLibrarySettings.GetDefaultExtendDays();
        }


        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool result = true;
            result &= clsSubscriptionType.UpdateFees(1, Convert.ToDouble(MonthlySubscriptionFees.Value));
            result &= clsSubscriptionType.UpdateFees(2, Convert.ToDouble(ThreeMonthFees.Value));
            result &= clsSubscriptionType.UpdateFees(3, Convert.ToDouble(OneYearFees.Value));

            result &= clsLibrarySettings.UpdateSetting(clsLibrarySettings.enSettingType.ExtendBorrowDaysID,Convert.ToInt16(ExtendBorrowDays.Value));
            result &= clsLibrarySettings.UpdateSetting(clsLibrarySettings.enSettingType.DefaultBorrowDays, Convert.ToInt16(DefaultBorrowDays.Value));
            result &= clsLibrarySettings.UpdateSetting(clsLibrarySettings.enSettingType.LateReturnFeesAmountID, Convert.ToInt16(LateReturnFees.Value));
            result &= clsLibrarySettings.UpdateSetting(clsLibrarySettings.enSettingType.ExtendPerBorrowCountID, Convert.ToInt16(ExtendTimesPerBorrow.Value));
            if (result == true)
                MessageBox.Show("Settings was saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                MessageBox.Show("An error occurred!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnSave.Enabled = false;
        }
    }
}
