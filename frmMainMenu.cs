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

        // In MainForm's constructor or Load event
        public frmMainMenu()
        {
            InitializeComponent();

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

        private void readersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.ReadersManagement))
            {
                MessageBox.Show("You don't have permissions to manage readers , contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ShowFormInPanel(new frmManageReaders());
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
                saveFileDialog.Filter = "Backup Files All Files (*.*)|*.*";

                saveFileDialog.Title = "Select Backup File Location";
                saveFileDialog.FileName = "DatabaseBackup.bak";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string backupPath = saveFileDialog.FileName;

                    // Notify the user that the backup has started
                    MessageBox.Show("Data backup has started.", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Perform the backup asynchronously
                    bool success = await BackupDatabaseAsync(backupPath);

                    // Notify the user about the result
                    if (success)
                    {
                        MessageBox.Show("Successfully backed up the data.", "Backup Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Data backup failed. Please try again.", "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // User canceled the operation
                    MessageBox.Show("Backup operation was canceled.", "Backup Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

            private async Task<bool> BackupDatabaseAsync(string backupPath)
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        // Define your database connection string
                        string connectionString = clsConnectionString.ConnectionString;

                        // SQL command for backing up the database
                        string backupCommand = $"BACKUP DATABASE [LibrarySystemDB] TO DISK = '{backupPath}' WITH FORMAT";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(backupCommand, connection);
                            connection.Open();
                            command.ExecuteNonQuery();
                        }

                        return true; // Backup succeeded
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (optional)
                        MessageBox.Show(ex.Message);
                        clsLogEvent.Log(ex);
                        return false; // Backup failed
                    }
                });

            }

    //    private bool DatabaseExists(SqlConnection connection, string databaseName)
    //    {
    //        string query = $"SELECT database_id FROM sys.databases WHERE Name = @DatabaseName";
    //        using (SqlCommand command = new SqlCommand(query, connection))
    //        {
    //            command.Parameters.AddWithValue("@DatabaseName", databaseName);
    //            return command.ExecuteScalar() != null;
    //        }
    //    }

    //    private void KillDatabaseConnections(SqlConnection connection, string databaseName)
    //    {
    //        string killConnectionsQuery = @"
    //    DECLARE @kill VARCHAR(MAX) = '';
    //    SELECT @kill = @kill + 'KILL ' + CONVERT(VARCHAR(5), session_id) + ';'
    //    FROM sys.dm_exec_sessions
    //    WHERE database_id = DB_ID(@DatabaseName);

    //    IF @kill <> ''
    //        EXEC(@kill);
    //";

    //        // Temporarily change the database context to "master"
    //        if (connection.State == ConnectionState.Closed)
    //            connection.Open();

    //        connection.ChangeDatabase("master");

    //        using (SqlCommand command = new SqlCommand(killConnectionsQuery, connection))
    //        {
    //            command.Parameters.AddWithValue("@DatabaseName", databaseName);
    //            command.ExecuteNonQuery();
    //        }
    //    }


    //    private void DropDatabase(SqlConnection connection, string databaseName)
    //    {
    //        string dropQuery = $"DROP DATABASE [{databaseName}]";
    //        using (SqlCommand command = new SqlCommand(dropQuery, connection))
    //        {
    //            command.ExecuteNonQuery();
    //        }
    //    }

    //    private void RestoreDatabase(SqlConnection connection, string backupFilePath, string databaseName)
    //    {
    //        string restoreQuery = $@"
    //    RESTORE DATABASE [{databaseName}]
    //    FROM DISK = @BackupFilePath
    //    WITH REPLACE, MOVE '{databaseName}_Data' TO 'C:\\SQLData\\{databaseName}.mdf',
    //         MOVE '{databaseName}_Log' TO 'C:\\SQLData\\{databaseName}.ldf';
    //";
    //        using (SqlCommand command = new SqlCommand(restoreQuery, connection))
    //        {
    //            command.Parameters.AddWithValue("@BackupFilePath", backupFilePath);
    //            command.ExecuteNonQuery();
    //        }
    //    }


        //private async void RestoreData_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog openFileDialog = new OpenFileDialog())
        //    {
        //        openFileDialog.Filter = "Backup Files (*.bak)|*.bak";
        //        openFileDialog.Title = "Select Backup File";

        //        if (openFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            string backupFilePath = openFileDialog.FileName;
        //            string targetDatabaseName = "LibrarySystemDB";

        //            try
        //            {
        //                // Display a message while restoring
        //                MessageBox.Show("Restoring database... Please wait.", "Restore Data", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                using (SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString))
        //                {
        //                    await connection.OpenAsync();

        //                    // Step 1: Check if the target database exists
        //                    if (DatabaseExists(connection, targetDatabaseName))
        //                    {
        //                        // Step 2: Cut off connections and drop the existing database
        //                        KillDatabaseConnections(connection, targetDatabaseName);
        //                        DropDatabase(connection, targetDatabaseName);
        //                    }

        //                    // Step 3: Restore the backup file as the target database
        //                    RestoreDatabase(connection, backupFilePath, targetDatabaseName);

        //                    MessageBox.Show("Database restored successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"Error: {ex.Message}", "Restore Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //    }
        //}

        private void RestoreData_Click(object sender, EventArgs e)
        {
            //bool AvailableDataBase
            //using (SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString))
            //{
            //    connection.Open();
            //    clsGlobal.DatabaseExists(connection, "LibrarySystemDB");

            //}
            if (clsGlobal.CurrentUser != null)
            {
                if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.FullAccess))
                {
                    MessageBox.Show("You don't have permissions to retore data backup \n Please contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            DialogResult restoreResult = MessageBox.Show(
    "Are you sure you want to restore data from the backup file? This operation will overwrite the current database.",
    "Confirm Restore",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Warning);

            if (restoreResult != DialogResult.Yes)
            {
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Backup Files All Files (*.*)|*.*",
                Title = "Select a backup file"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string backupFilePath = openFileDialog.FileName;
                RestoreDatabase(backupFilePath);
            }
        }

        private void RestoreDatabase(string backupFilePath)
        {
            if (clsGlobal.CurrentUser != null)
            {
                if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.FullAccess))
                {
                    MessageBox.Show("You don't have permissions to retore data backup \n Please contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            string connectionString = clsConnectionString.MasterConnectionString; // Replace with your actual connection string

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the database exists
                if (DatabaseExists(connection, "LibrarySystemDB"))
                {
                    // Drop the existing database
                    DropDatabase(connection, "LibrarySystemDB");
                }

                // Restore the database with MOVE options
                string restoreQuery = $@"
        RESTORE DATABASE LibrarySystemDB 
        FROM DISK = '{backupFilePath}' 
        WITH REPLACE,
        MOVE 'LibraryManagement' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LibrarySystemDB.mdf',
        MOVE 'LibraryManagement_Log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LibrarySystemDB_Log.ldf'";

                using (SqlCommand command = new SqlCommand(restoreQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                EnableMainButtons();
                MessageBox.Show("Data restoration has been successfully completed. The program will now close. Please reopen it to continue.", "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private bool DatabaseExists(SqlConnection connection, string databaseName)
        {
            string query = $@"
        IF EXISTS (SELECT name 
                   FROM master.dbo.sysdatabases 
                   WHERE name = '{databaseName}')
        SELECT 1
        ELSE
        SELECT 0";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                return (int)command.ExecuteScalar() == 1;
            }
        }

        private void DropDatabase(SqlConnection connection, string databaseName)
        {
            // Switch to master database to execute the DROP DATABASE command
            using (SqlCommand useMaster = new SqlCommand("USE master;", connection))
            {
                useMaster.ExecuteNonQuery();
            }

            string dropQuery = $@"
        ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        DROP DATABASE [{databaseName}]";

            using (SqlCommand command = new SqlCommand(dropQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        //private bool DatabaseExists(string databaseName)
        //{
        //    string connectionString = clsConnectionString.ConnectionString;

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string query = $"SELECT database_id FROM sys.databases WHERE name = '{databaseName}'";
        //        SqlCommand command = new SqlCommand(query, connection);
        //        return command.ExecuteScalar() != null;
        //    }
        //}
        //private void DropDatabase(string databaseName)
        //{
        //    string connectionString = clsConnectionString.ConnectionString;

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        SqlCommand dropCommand = new SqlCommand($"DROP DATABASE IF EXISTS [{databaseName}]", connection);
        //        dropCommand.ExecuteNonQuery();
        //    }
        //}

        //private void ReplaceOriginalDatabase(string tempDatabaseName, string originalDatabaseName)
        //{
        //    string connectionString = clsConnectionString.ConnectionString;

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        // First, switch to master database to avoid being connected to the database you're trying to drop
        //        SqlCommand switchDatabaseCommand = new SqlCommand("USE master;", connection);
        //        switchDatabaseCommand.ExecuteNonQuery();

        //        // Kill all active connections to the original database
        //        SqlCommand killConnectionsCommand = new SqlCommand($@"
        //    DECLARE @spid INT;
        //    DECLARE kill_cursor CURSOR FOR
        //    SELECT spid
        //    FROM sys.sysprocesses
        //    WHERE dbid = DB_ID('{originalDatabaseName}');
        //    OPEN kill_cursor;
        //    FETCH NEXT FROM kill_cursor INTO @spid;
        //    WHILE @@FETCH_STATUS = 0
        //    BEGIN
        //        EXEC('KILL ' + @spid);
        //        FETCH NEXT FROM kill_cursor INTO @spid;
        //    END
        //    CLOSE kill_cursor;
        //    DEALLOCATE kill_cursor;", connection);

        //        killConnectionsCommand.ExecuteNonQuery();

        //        // Set the original database to SINGLE_USER mode to disconnect active users, if necessary
        //        SqlCommand setSingleUserCommand = new SqlCommand($"ALTER DATABASE [{originalDatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", connection);
        //        setSingleUserCommand.ExecuteNonQuery();

        //        // Drop the original database
        //        SqlCommand dropCommand = new SqlCommand($"DROP DATABASE IF EXISTS [{originalDatabaseName}]", connection);
        //        dropCommand.ExecuteNonQuery();

        //        // Rename the temporary database to the original name
        //        SqlCommand renameCommand = new SqlCommand($"ALTER DATABASE [{tempDatabaseName}] MODIFY NAME = [{originalDatabaseName}]", connection);
        //        renameCommand.ExecuteNonQuery();
        //    }
        //}



        //private bool ValidateDatabaseSchema(string databaseName)
        //{
        //    try
        //    {
        //        string connectionString = $"{clsConnectionString.ConnectionString};Initial Catalog={databaseName}";

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();

        //            // Example: Validate that required tables exist
        //            string[] requiredTables = { "People", "Users", "Readers","Subscriptions","Books","Payments" };
        //            foreach (string table in requiredTables)
        //            {
        //                string query = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{table}'";
        //                SqlCommand command = new SqlCommand(query, connection);
        //                if (command.ExecuteScalar() == null)
        //                {
        //                    return false; // Table missing
        //                }
        //            }

        //            // Example: Validate column existence (extend as needed)
        //            // Additional checks for column names, data types, etc., can be implemented here

        //            return true; // Validation passed
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsLogEvent.Log(ex);
        //        MessageBox.Show($"Error: {ex.Message}", "Restore Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false; // Validation failed

        //    }
        //}

        //private async Task<bool> RestoreDatabaseWithNewNameAsync(string backupFilePath, string newDatabaseName)
        //{
        //    return await Task.Run(() =>
        //    {
        //        try
        //        {
        //            string connectionString = clsConnectionString.ConnectionString;

        //            // Logical file names of the database in the backup
        //            string logicalDataFileName = "LibraryManagement";       // Replace with your actual logical name
        //            string logicalLogFileName = "LibraryManagement_log";   // Replace with your actual logical name

        //            // New file paths for the restored database
        //            string newMdfPath = $@"C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\{newDatabaseName}.mdf";
        //            string newLdfPath = $@"C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\{newDatabaseName}_log.ldf";

        //            // SQL commands
        //            string setSingleUserQuery = $@"
        //        ALTER DATABASE {newDatabaseName} 
        //        SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

        //            string restoreQuery = $@"
        //        RESTORE DATABASE {newDatabaseName}
        //        FROM DISK = '{backupFilePath}'
        //        WITH MOVE '{logicalDataFileName}' TO '{newMdfPath}',
        //             MOVE '{logicalLogFileName}' TO '{newLdfPath}',
        //             REPLACE,
        //             RECOVERY;";

        //            string setMultiUserQuery = $@"
        //        ALTER DATABASE {newDatabaseName} 
        //        SET MULTI_USER;";

        //            using (SqlConnection connection = new SqlConnection(connectionString))
        //            {
        //                connection.Open();

        //                // Set to single-user mode to terminate active connections
        //                using (SqlCommand command = new SqlCommand(setSingleUserQuery, connection))
        //                {
        //                    command.ExecuteNonQuery();
        //                }

        //                // Restore the database
        //                using (SqlCommand command = new SqlCommand(restoreQuery, connection))
        //                {
        //                    command.ExecuteNonQuery();
        //                }

        //                // Set back to multi-user mode
        //                using (SqlCommand command = new SqlCommand(setMultiUserQuery, connection))
        //                {
        //                    command.ExecuteNonQuery();
        //                }
        //            }

        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log or show the exception message for debugging
        //            clsLogEvent.Log(ex);
        //            return false;
        //        }
        //    });
        //}

        //private async void RestoreData_Click(object sender, EventArgs e)
        //{

        //    using (OpenFileDialog openFileDialog = new OpenFileDialog())
        //    {
        //        openFileDialog.Filter = "Backup Files (*.bak)|*.bak";
        //        openFileDialog.Title = "Select Backup File";

        //        if (openFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            string backupFilePath = openFileDialog.FileName;

        //            try
        //            {
        //                MessageBox.Show("Restoring data... Please wait.", "Restore Data", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                // Step 1: Restore to a temporary database
        //                string tempDatabaseName = "TempRestoreDB";
        //                bool restoreSuccess = await RestoreDatabaseWithNewNameAsync(backupFilePath, tempDatabaseName);
        //                if (restoreSuccess)
        //                {
        //                    // Check if the original database exists
        //                    bool originalDatabaseExists = DatabaseExists("LibraryManagement");

        //                    if (originalDatabaseExists)
        //                    {
        //                        // Validate against the existing database schema
        //                        if (ValidateDatabaseSchema(tempDatabaseName))
        //                        {
        //                            // Replace the original database
        //                            ReplaceOriginalDatabase(tempDatabaseName, "LibraryManagement");
        //                            MessageBox.Show("Database restored successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("The restored database is incompatible with the application schema.", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                            DropDatabase(tempDatabaseName); // Cleanup temporary database
        //                        }
        //                    }
        //                    else
        //                    {

        //                        // Rename the temporary database to the target name
        //                        RenameDatabase(tempDatabaseName, "LibraryManagement");
        //                        MessageBox.Show("Database restored successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Failed to restore the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }




        //            }

        //            catch (Exception ex)
        //            {
        //                clsLogEvent.Log(ex);
        //                MessageBox.Show($"Error: {ex.Message}", "Restore Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        } 
        //    }

        //    }


        //private void RenameDatabase(string currentName, string newName)
        //{
        //    string connectionString = clsConnectionString.ConnectionString;

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string renameQuery = $@"
        //    ALTER DATABASE [{currentName}]
        //    MODIFY NAME = [{newName}]";

        //        SqlCommand command = new SqlCommand(renameQuery, connection);
        //        command.ExecuteNonQuery();
        //    }
        //}


    }
}
    