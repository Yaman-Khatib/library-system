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
using The_Story_Corner_Project.Readers;
using The_Story_Corner_Project.Users;
using The_Story_Corner_Project.Global_Classes;
using The_Story_Corner_Project.Borrows;
using The_Story_Corner_Project.Payments;
using Library_Business;
using Library_Business.Services;
using The_Story_Corner_Project.Library_settings;
using The_Story_Corner_Project.Courses;
using The_Story_Corner_Project.Books;
using System.Data.SqlClient;
using System.IO;
using static System.Windows.Forms.AxHost;
using System.Diagnostics;
using DVLD.Global_Classes;


namespace The_Story_Corner_Project
{
    public partial class frmMainMenu : KryptonForm
    {

        private System.Windows.Forms.Panel dimmingOverlay;
        private readonly IBackupRestoreService _backupRestoreService;

        // In MainForm's constructor or Load event
        public frmMainMenu()
        {
            InitializeComponent();

            // Initialize backup/restore service
            _backupRestoreService = new BackupRestoreService();
            _backupRestoreService.ProgressChanged += OnBackupRestoreProgressChanged;
            _backupRestoreService.OperationCompleted += OnBackupRestoreCompleted;

            // Initialize and configure the dimming overlay panel
            dimmingOverlay = new Panel
            {

                BackColor = Color.FromArgb(128, Color.Black), // Semi-transparent black
                Dock = DockStyle.Fill,
                Visible = false // Initially hidden
            };
            this.Controls.Add(dimmingOverlay);
            
        }

        public frmMainMenu(bool DataBaseExists)
        {
            InitializeComponent();

            // Initialize backup/restore service
            _backupRestoreService = new BackupRestoreService();
            _backupRestoreService.ProgressChanged += OnBackupRestoreProgressChanged;
            _backupRestoreService.OperationCompleted += OnBackupRestoreCompleted;

            // Initialize and configure the dimming overlay panel
            dimmingOverlay = new Panel
            {

                BackColor = Color.FromArgb(128, Color.Black), // Semi-transparent black
                Dock = DockStyle.Fill,
                Visible = false // Initially hidden
            };
            this.Controls.Add(dimmingOverlay);

            if( !DataBaseExists )
            {
                DisableMainButtons();
            }
        }

        private void DisableMainButtons()
        {
            toolStripMenuItemHome.Enabled = false;
            booksToolStripMenuItem.Enabled = false;
            borrowsToolStripMenuItem.Enabled = false;
            readersToolStripMenuItem.Enabled = false;
            courcesToolStripMenuItem.Enabled = false;
            paymentsToolStripMenuItem.Enabled = false;
            usersToolStripMenuItem.Enabled = false;
            BackUpData.Enabled = false;
            toolStripMenuItemSettings.Enabled = false;
        }

        private void EnableMainButtons()
        {
            toolStripMenuItemHome.Enabled = true;
            booksToolStripMenuItem.Enabled = true;
            borrowsToolStripMenuItem.Enabled = true;
            readersToolStripMenuItem.Enabled = true;
            courcesToolStripMenuItem.Enabled = true;
            paymentsToolStripMenuItem.Enabled = true;
            usersToolStripMenuItem.Enabled = true;
            BackUpData.Enabled = true;
            toolStripMenuItemSettings.Enabled = true;
        }

        public void DimMainForm()
        {
            dimmingOverlay.Visible = true;
        }

        public void UnDimMainForm()
        {
            dimmingOverlay.Visible = false;
        }

        private void ShowFormInPanel(Form formToShow)
        {
            // Clear existing forms in the panel
            contentPanel.Controls.Clear();

            // Set the form to non-top-level
            formToShow.TopLevel = false;
            formToShow.FormBorderStyle = FormBorderStyle.None;
            formToShow.Dock = DockStyle.Fill;

            // Add the form to the panel
            contentPanel.Controls.Add(formToShow);
            formToShow.Show();
        }

        private Control[] originalControls;

        private void frmMainMenu_Load(object sender, EventArgs e)
        {
            originalControls = contentPanel.Controls.Cast<Control>().ToArray();

        }


        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void manageBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.BooksManagement))
            {
                MessageBox.Show("You don't have permissions to manage books , contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ShowFormInPanel(new frmManageBooks());
        }


        private void toolStripMenuItemHome_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.AddRange(originalControls);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.ManagementSettings))
            {
                MessageBox.Show("You don't have permissions to edit library management settings!", "No permission", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            frmManageLibrarySettings frm = new frmManageLibrarySettings();
            frm.ShowDialog();

        }


        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.UsersManagement))
            {
                MessageBox.Show("You don't have permissions to manage users please contact your admin", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ShowFormInPanel(new frmManageUsers());
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUserInfo frm = new frmShowUserInfo(clsGlobal.CurrentUser.UserID.Value);
            frm.ShowDialog();
        }

        private void bookSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.BooksManagement))
            {
                MessageBox.Show("You don't have permissions to sell books , contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ShowFormInPanel(new frmBookSales());
        }

        private void borrowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.BooksManagement))
            {
                MessageBox.Show("You don't have permissions to manage borrows , contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ShowFormInPanel(new frmManageBorrows());
        }

        private void toolStripMenuItemEnrollmentManagment_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.CoursesManagement))
            {
                MessageBox.Show("You don't have permissions to manage courses , contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            return;
        }

        private void toolStripMenuItemCourseManagment_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.CoursesManagement))
            {
                MessageBox.Show("You don't have permissions to manage courses , contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ShowFormInPanel(new frmManageCourses());
            return;
        }

        private void managePaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.PaymentsManagement))
            {
                MessageBox.Show("You don't have permissions to view payments , contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            frmManagePayments frm = new frmManagePayments();
            ShowFormInPanel(frm);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.PaymentsManagement))
            {
                MessageBox.Show("You don't have permissions to access payments for a reader, contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            frmReaderPayments frm = new frmReaderPayments();
            frm.ShowDialog();
        }

        private void toolStripMenuItemReaderPayments_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.PaymentsManagement))
            {
                MessageBox.Show("You don't have permissions to access payments management section, contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void manageSubscriptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageReaderSubscriptions frm = new frmManageReaderSubscriptions();
            frm.ShowDialog();
        }

        private void courcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.CoursesManagement))
            {
                MessageBox.Show("You don't have permissions to manage courses \n contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ShowFormInPanel(new frmManageCourses());
            return;
        }

        private async void BackUpData_Click(object sender, EventArgs e)
        {
            DialogResult backupResult = MessageBox.Show(
    "Do you want to backup the current data to a backup file?",
    "Confirm Backup",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);

            if (backupResult != DialogResult.Yes)
            {
                return;
            }

            // Open the Save File Dialog for selecting the backup location
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                saveFileDialog.Title = "Select Backup File Location";
                saveFileDialog.FileName = $"LibrarySystemDB_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string backupPath = saveFileDialog.FileName;

                    try
                    {
                        // Show progress dialog
                        ShowProgressDialog("Creating backup...", OperationType.Backup);

                        // Perform the backup using the service
                        var result = await _backupRestoreService.CreateBackupAsync(backupPath);

                        HideProgressDialog();

                    // Notify the user about the result
                        if (result.IsSuccess)
                        {
                            string message = $"Successfully backed up the data.\n\n" +
                                           $"File: {result.BackupFilePath}\n" +
                                           $"Size: {FormatFileSize(result.BackupFileSize)}\n" +
                                           $"Duration: {result.Duration.TotalSeconds:F1} seconds";
                            
                            MessageBox.Show(message, "Backup Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Data backup failed: {result.ErrorMessage}", "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        HideProgressDialog();
                        MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // User canceled the operation
                    MessageBox.Show("Backup operation was canceled.", "Backup Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        #region Progress Dialog Methods

        private Form _progressDialog;
        private Label _progressLabel;
        private ProgressBar _progressBar;

        private void ShowProgressDialog(string initialMessage, OperationType operationType)
        {
            _progressDialog = new Form
            {
                Text = operationType == OperationType.Backup ? "Creating Backup" : "Restoring Database",
                Size = new Size(400, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false
            };

            _progressLabel = new Label
            {
                Text = initialMessage,
                Location = new Point(20, 20),
                Size = new Size(350, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _progressBar = new ProgressBar
            {
                Location = new Point(20, 50),
                Size = new Size(350, 23),
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };

            _progressDialog.Controls.Add(_progressLabel);
            _progressDialog.Controls.Add(_progressBar);
            _progressDialog.Show(this);
        }

        private void HideProgressDialog()
        {
            _progressDialog?.Close();
            _progressDialog?.Dispose();
            _progressDialog = null;
        }

        private void UpdateProgressDialog(int percentage, string message)
        {
            if (_progressDialog != null && !_progressDialog.IsDisposed)
            {
                _progressLabel.Text = message;
                _progressBar.Style = ProgressBarStyle.Continuous;
                _progressBar.Value = Math.Min(100, Math.Max(0, percentage));
            }
        }

        #endregion

        #region Event Handlers

        private void OnBackupRestoreProgressChanged(object sender, BackupRestoreProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnBackupRestoreProgressChanged(sender, e)));
                return;
            }

            UpdateProgressDialog(e.ProgressPercentage, e.StatusMessage);
        }

        private void OnBackupRestoreCompleted(object sender, BackupRestoreCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnBackupRestoreCompleted(sender, e)));
                return;
            }

            // Progress dialog will be hidden by the calling method
        }

        #endregion

        #region Helper Methods

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void ShowAccessDeniedHelp()
        {
            string helpMessage = "ACCESS DENIED ERROR - SOLUTIONS:\n\n" +
                               "1. MOVE THE BACKUP FILE:\n" +
                               "   • Copy the backup file from OneDrive to a local folder\n" +
                               "   • Suggested locations:\n" +
                               "     - C:\\Temp\\\n" +
                               "     - C:\\Backups\\\n" +
                               "     - C:\\Users\\" + Environment.UserName + "\\Documents\\\n\n" +
                               "2. CREATE A SAFE FOLDER:\n" +
                               "   • Create C:\\Backups\\ folder\n" +
                               "   • Copy your backup file there\n" +
                               "   • Try restore again\n\n" +
                               "3. WHY THIS HAPPENS:\n" +
                               "   • OneDrive folders have restricted permissions\n" +
                               "   • SQL Server cannot access cloud-synced folders\n" +
                               "   • Local folders work best for database operations";

            MessageBox.Show(helpMessage, "Access Denied - Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion




        private async void RestoreData_Click(object sender, EventArgs e)
        {
            // Check permissions
            if (clsGlobal.CurrentUser != null)
            {
                if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.FullAccess))
                {
                    MessageBox.Show("You don't have permissions to restore data backup.\nPlease contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            DialogResult restoreResult = MessageBox.Show(
                "Are you sure you want to restore data from the backup file?\n\n" +
                "WARNING: This operation will overwrite the current database and cannot be undone!",
    "Confirm Restore",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Warning);

            if (restoreResult != DialogResult.Yes)
            {
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                openFileDialog.Title = "Select a backup file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string backupFilePath = openFileDialog.FileName;

                    try
                    {
                        // Show progress dialog
                        ShowProgressDialog("Validating backup file...", OperationType.Restore);

                        // First validate the backup file
                        var validationResult = await _backupRestoreService.ValidateBackupFileAsync(backupFilePath);
                        
                        if (!validationResult.IsSuccess)
                        {
                            HideProgressDialog();
                            
                            // Check if it's an access denied error
                            if (validationResult.ErrorMessage.Contains("Access is denied") || 
                                validationResult.ErrorMessage.Contains("restricted location"))
                            {
                                MessageBox.Show($"Access Error: {validationResult.ErrorMessage}", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ShowAccessDeniedHelp();
                            }
                            else
                            {
                                MessageBox.Show($"Invalid backup file: {validationResult.ErrorMessage}", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            return;
                        }

                        // Get backup file info
                        var backupInfo = await _backupRestoreService.GetBackupFileInfoAsync(backupFilePath);
                        
                        if (!backupInfo.IsValid)
                        {
                            HideProgressDialog();
                            MessageBox.Show($"Backup file is corrupted: {backupInfo.ValidationError}", "Invalid Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Show backup info and confirm
                        string infoMessage = $"Backup File Information:\n\n" +
                                           $"Database: {backupInfo.DatabaseName}\n" +
                                           $"Backup Date: {backupInfo.BackupDate:yyyy-MM-dd HH:mm:ss}\n" +
                                           $"File Size: {FormatFileSize(backupInfo.FileSize)}\n" +
                                           $"Backup Type: {backupInfo.BackupType}\n\n" +
                                           $"Do you want to proceed with the restore?";

                        DialogResult finalConfirm = MessageBox.Show(infoMessage, "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        
                        if (finalConfirm != DialogResult.Yes)
                        {
                            HideProgressDialog();
                            return;
                        }

                        // Perform the restore
                        var result = await _backupRestoreService.RestoreDatabaseAsync(backupFilePath);

                        HideProgressDialog();

                        if (result.IsSuccess)
                        {
                            MessageBox.Show("Data restoration has been successfully completed.\n\nThe program will now close. Please reopen it to continue.", 
                                "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Data restoration failed: {result.ErrorMessage}", "Restore Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        HideProgressDialog();
                        MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manageReadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.ReadersManagement))
            {
                MessageBox.Show("You don't have permissions to manage readers , contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ShowFormInPanel(new frmManageReaders());
        }

        private void subscriptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.ReadersManagement))
            {
                MessageBox.Show("You don't have permissions to manage subscriptions, contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ShowFormInPanel(new frmManageSubscriptions());
        }
    }
}
    