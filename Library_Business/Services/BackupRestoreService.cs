using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Global_Classes;

namespace Library_Business.Services
{
    /// <summary>
    /// Service for database backup and restore operations
    /// </summary>
    public class BackupRestoreService : IBackupRestoreService
    {
        private readonly string _connectionString;
        private readonly string _masterConnectionString;

        public event EventHandler<BackupRestoreProgressEventArgs> ProgressChanged;
        public event EventHandler<BackupRestoreCompletedEventArgs> OperationCompleted;

        public BackupRestoreService()
        {
            _connectionString = clsDataAccessSettings.connectionString;
            _masterConnectionString = clsDataAccessSettings.masterConnectionString;
        }

        /// <summary>
        /// Creates a backup of the database
        /// </summary>
        public async Task<BackupRestoreResult> CreateBackupAsync(string backupPath, BackupRestoreOptions options = null)
        {
            var startTime = DateTime.Now;
            options = options ?? BackupRestoreOptions.CreateDefaultBackupOptions();

            try
            {
                LogOperationStart("Backup", backupPath, options);
                OnProgressChanged(0, "Initializing backup...", OperationType.Backup, OperationStage.Initializing);

                // Validate inputs
                var validationResult = await ValidateBackupInputsAsync(backupPath, options);
                if (!validationResult.IsSuccess)
                {
                    LogOperationFailure("Backup", "Input validation failed", validationResult.ErrorMessage);
                    return validationResult;
                }

                OnProgressChanged(10, "Preparing backup command...", OperationType.Backup, OperationStage.CreatingBackup);

                // Build backup command
                var backupCommand = BuildBackupCommand(backupPath, options);
                LogOperationDetail("Backup", $"Backup command: {backupCommand}");

                OnProgressChanged(20, "Executing backup...", OperationType.Backup, OperationStage.CreatingBackup);

                // Execute backup
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(backupCommand, connection))
                    {
                        command.CommandTimeout = options.TimeoutSeconds;
                        await command.ExecuteNonQueryAsync();
                    }
                }

                OnProgressChanged(80, "Verifying backup...", OperationType.Backup, OperationStage.Verifying);

                // Verify backup if requested
                if (options.VerifyBackup)
                {
                    var verifyResult = await VerifyBackupAsync(backupPath);
                    if (!verifyResult.IsSuccess)
                    {
                        LogOperationFailure("Backup", "Backup verification failed", verifyResult.ErrorMessage);
                        return BackupRestoreResult.Failure($"Backup verification failed: {verifyResult.ErrorMessage}", 
                            verifyResult.Exception, DateTime.Now - startTime);
                    }
                }

                OnProgressChanged(100, "Backup completed successfully", OperationType.Backup, OperationStage.Completed);

                var fileInfo = new FileInfo(backupPath);
                var result = BackupRestoreResult.Success(backupPath, fileInfo.Length, DateTime.Now - startTime);
                
                LogOperationSuccess("Backup", backupPath, fileInfo.Length, DateTime.Now - startTime);
                OnOperationCompleted(result, OperationType.Backup);
                return result;
            }
            catch (Exception ex)
            {
                LogOperationFailure("Backup", "Unexpected error", ex.Message, ex);
                var result = BackupRestoreResult.Failure($"Backup failed: {ex.Message}", ex, DateTime.Now - startTime);
                OnOperationCompleted(result, OperationType.Backup);
                return result;
            }
        }

        /// <summary>
        /// Restores the database from a backup file
        /// </summary>
        public async Task<BackupRestoreResult> RestoreDatabaseAsync(string backupPath, BackupRestoreOptions options = null)
        {
            var startTime = DateTime.Now;
            options = options ?? BackupRestoreOptions.CreateDefaultRestoreOptions();

            try
            {
                LogOperationStart("Restore", backupPath, options);
                OnProgressChanged(0, "Initializing restore...", OperationType.Restore, OperationStage.Initializing);

                // Validate inputs
                var validationResult = await ValidateRestoreInputsAsync(backupPath, options);
                if (!validationResult.IsSuccess)
                {
                    LogOperationFailure("Restore", "Input validation failed", validationResult.ErrorMessage);
                    return validationResult;
                }

                OnProgressChanged(10, "Validating backup file...", OperationType.Restore, OperationStage.Validating);

                // Validate backup file
                var backupValidation = await ValidateBackupFileAsync(backupPath);
                if (!backupValidation.IsSuccess)
                {
                    LogOperationFailure("Restore", "Backup file validation failed", backupValidation.ErrorMessage);
                    return backupValidation;
                }

                OnProgressChanged(20, "Preparing for restore...", OperationType.Restore, OperationStage.RestoringDatabase);

                using (var connection = new SqlConnection(_masterConnectionString))
                {
                    connection.Open();

                    // Kill existing connections if requested
                    if (options.KillExistingConnections)
                    {
                        OnProgressChanged(30, "Closing existing connections...", OperationType.Restore, OperationStage.RestoringDatabase);
                        await KillDatabaseConnectionsAsync(connection, options.DatabaseName);
                        LogOperationDetail("Restore", "Killed existing database connections");
                    }

                    // Drop existing database if it exists
                    if (await DatabaseExistsAsync(options.DatabaseName))
                    {
                        OnProgressChanged(40, "Removing existing database...", OperationType.Restore, OperationStage.RestoringDatabase);
                        await DropDatabaseAsync(connection, options.DatabaseName);
                        LogOperationDetail("Restore", "Dropped existing database");
                    }

                    OnProgressChanged(50, "Restoring database...", OperationType.Restore, OperationStage.RestoringDatabase);

                    // Get backup file information for proper restore
                    var backupInfo = await GetBackupFileInfoAsync(backupPath);
                    
                    // Log the logical file names found
                    if (backupInfo?.LogicalFileNames != null && backupInfo.LogicalFileNames.Length > 0)
                    {
                        LogOperationDetail("Restore", $"Found logical file names: {string.Join(", ", backupInfo.LogicalFileNames)}");
                    }
                    else
                    {
                        LogOperationDetail("Restore", "No logical file names found in backup info, will query backup file directly");
                    }
                    
                    var restoreCommand = BuildRestoreCommand(backupPath, options, backupInfo);
                    LogOperationDetail("Restore", $"Restore command: {restoreCommand}");

                    using (var command = new SqlCommand(restoreCommand, connection))
                    {
                        command.CommandTimeout = options.TimeoutSeconds;
                        await command.ExecuteNonQueryAsync();
                    }
                }

                OnProgressChanged(90, "Verifying restore...", OperationType.Restore, OperationStage.Verifying);

                // Verify the restore was successful
                if (!await DatabaseExistsAsync(options.DatabaseName))
                {
                    LogOperationFailure("Restore", "Database verification failed", "Database was not created during restore operation");
                    return BackupRestoreResult.Failure("Database was not created during restore operation", 
                        null, DateTime.Now - startTime);
                }

                OnProgressChanged(100, "Restore completed successfully", OperationType.Restore, OperationStage.Completed);

                var result = BackupRestoreResult.Success(null, 0, DateTime.Now - startTime);
                LogOperationSuccess("Restore", backupPath, 0, DateTime.Now - startTime);
                OnOperationCompleted(result, OperationType.Restore);
                return result;
            }
            catch (Exception ex)
            {
                LogOperationFailure("Restore", "Unexpected error", ex.Message, ex);
                var result = BackupRestoreResult.Failure($"Restore failed: {ex.Message}", ex, DateTime.Now - startTime);
                OnOperationCompleted(result, OperationType.Restore);
                return result;
            }
        }

        /// <summary>
        /// Validates if a backup file is valid and can be restored
        /// </summary>
        public async Task<BackupRestoreResult> ValidateBackupFileAsync(string backupPath)
        {
            try
            {
                if (!File.Exists(backupPath))
                {
                    return BackupRestoreResult.Failure("Backup file does not exist");
                }

                using (var connection = new SqlConnection(_masterConnectionString))
                {
                    connection.Open();
                    var command = $"RESTORE VERIFYONLY FROM DISK = '{backupPath}'";
                    
                    using (var sqlCommand = new SqlCommand(command, connection))
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                }

                return BackupRestoreResult.Success();
            }
            catch (Exception ex)
            {
                return BackupRestoreResult.Failure($"Backup file validation failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if the database exists
        /// </summary>
        public async Task<bool> DatabaseExistsAsync(string databaseName)
        {
            try
            {
                using (var connection = new SqlConnection(_masterConnectionString))
                {
                    connection.Open();
                    var query = "SELECT database_id FROM sys.databases WHERE Name = @DatabaseName";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DatabaseName", databaseName);
                        var result = await command.ExecuteScalarAsync();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to check if database exists: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets information about a backup file
        /// </summary>
        public async Task<BackupFileInfo> GetBackupFileInfoAsync(string backupPath)
        {
            try
            {
                var fileInfo = new FileInfo(backupPath);
                var backupInfo = new BackupFileInfo
                {
                    FileSize = fileInfo.Length,
                    IsValid = true
                };

                using (var connection = new SqlConnection(_masterConnectionString))
                {
                    connection.Open();
                    
                    // Get backup header information
                    var headerQuery = $"RESTORE HEADERONLY FROM DISK = '{backupPath}'";
                    using (var command = new SqlCommand(headerQuery, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                backupInfo.DatabaseName = reader["DatabaseName"]?.ToString();
                                backupInfo.BackupDate = reader.GetDateTime(reader.GetOrdinal("BackupStartDate"));
                                backupInfo.BackupType = reader["BackupType"]?.ToString();
                                backupInfo.ServerVersion = reader["SoftwareVersionMajor"]?.ToString();
                            }
                        }
                    }

                    // Get file list information
                    var fileListQuery = $"RESTORE FILELISTONLY FROM DISK = '{backupPath}'";
                    using (var command = new SqlCommand(fileListQuery, connection))
                    {
                        var logicalNames = new List<string>();
                        var physicalNames = new List<string>();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var logicalName = reader["LogicalName"]?.ToString();
                                var physicalName = reader["PhysicalName"]?.ToString();
                                
                                if (!string.IsNullOrEmpty(logicalName))
                                {
                                    logicalNames.Add(logicalName);
                                }
                                
                                if (!string.IsNullOrEmpty(physicalName))
                                {
                                    physicalNames.Add(physicalName);
                                }
                            }
                        }

                        backupInfo.LogicalFileNames = logicalNames.ToArray();
                        backupInfo.PhysicalFileNames = physicalNames.ToArray();
                    }
                }

                return backupInfo;
            }
            catch (Exception ex)
            {
                return new BackupFileInfo
                {
                    IsValid = false,
                    ValidationError = ex.Message
                };
            }
        }

        #region Logging Methods

        private void LogOperationStart(string operation, string path, BackupRestoreOptions options)
        {
            var logMessage = $"Starting {operation} operation:\n" +
                           $"Path: {path}\n" +
                           $"Database: {options.DatabaseName}\n" +
                           $"Compress: {options.CompressBackup}\n" +
                           $"Verify: {options.VerifyBackup}\n" +
                           $"Timeout: {options.TimeoutSeconds}s";
            
            LogInformation(logMessage);
        }

        private void LogOperationSuccess(string operation, string path, long fileSize, TimeSpan duration)
        {
            var logMessage = $"{operation} operation completed successfully:\n" +
                           $"Path: {path}\n" +
                           $"File Size: {FormatFileSize(fileSize)}\n" +
                           $"Duration: {duration.TotalSeconds:F1} seconds";
            
            LogInformation(logMessage);
        }

        private void LogOperationFailure(string operation, string stage, string errorMessage, Exception exception = null)
        {
            var logMessage = $"{operation} operation failed at {stage}:\n" +
                           $"Error: {errorMessage}";
            
            if (exception != null)
            {
                LogError(logMessage, exception);
            }
            else
            {
                LogWarning(logMessage);
            }
        }

        private void LogOperationDetail(string operation, string detail)
        {
            LogInformation($"{operation} - {detail}");
        }

        private void LogInformation(string message)
        {
            try
            {
                var logEntry = $"BACKUP_RESTORE_INFO: {message}";
                EventLog.WriteEntry("Library_Management_System", logEntry, EventLogEntryType.Information);
            }
            catch
            {
                // Ignore logging errors to prevent cascading failures
            }
        }

        private void LogWarning(string message)
        {
            try
            {
                var logEntry = $"BACKUP_RESTORE_WARNING: {message}";
                EventLog.WriteEntry("Library_Management_System", logEntry, EventLogEntryType.Warning);
            }
            catch
            {
                // Ignore logging errors to prevent cascading failures
            }
        }

        private void LogError(string message, Exception exception)
        {
            try
            {
                var logEntry = $"BACKUP_RESTORE_ERROR: {message}\nException: {exception?.Message}";
                EventLog.WriteEntry("Library_Management_System", logEntry, EventLogEntryType.Error);
                
                // Also use the existing logging system
                clsLogEvent.Log(exception);
            }
            catch
            {
                // Ignore logging errors to prevent cascading failures
            }
        }

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

        #endregion

        #region Private Methods

        private async Task<BackupRestoreResult> ValidateBackupInputsAsync(string backupPath, BackupRestoreOptions options)
        {
            if (string.IsNullOrWhiteSpace(backupPath))
            {
                return BackupRestoreResult.Failure("Backup path cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(options.DatabaseName))
            {
                return BackupRestoreResult.Failure("Database name cannot be empty");
            }

            // Check if database exists
            if (!await DatabaseExistsAsync(options.DatabaseName))
            {
                return BackupRestoreResult.Failure($"Database '{options.DatabaseName}' does not exist");
            }

            // Validate backup path
            try
            {
                var directory = Path.GetDirectoryName(backupPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                return BackupRestoreResult.Failure($"Invalid backup path: {ex.Message}", ex);
            }

            return BackupRestoreResult.Success();
        }

        private async Task<BackupRestoreResult> ValidateRestoreInputsAsync(string backupPath, BackupRestoreOptions options)
        {
            if (string.IsNullOrWhiteSpace(backupPath))
            {
                return BackupRestoreResult.Failure("Backup path cannot be empty");
            }

            if (!File.Exists(backupPath))
            {
                return BackupRestoreResult.Failure("Backup file does not exist");
            }

            if (string.IsNullOrWhiteSpace(options.DatabaseName))
            {
                return BackupRestoreResult.Failure("Database name cannot be empty");
            }

            // Check if the file is in a restricted location (like OneDrive)
            if (IsRestrictedLocation(backupPath))
            {
                return BackupRestoreResult.Failure(
                    "Backup file is in a restricted location (like OneDrive). " +
                    "Please copy the backup file to a local folder (e.g., C:\\Temp\\ or C:\\Backups\\) and try again.");
            }

            // Validate that we can read the logical file names from the backup
            try
            {
                var logicalNames = GetLogicalFileNamesFromBackup(backupPath);
                if (!logicalNames.Any())
                {
                    return BackupRestoreResult.Failure("Could not read logical file names from backup file. The backup may be corrupted or incompatible.");
                }
                
                LogOperationDetail("Restore", $"Validated logical file names: {string.Join(", ", logicalNames)}");
            }
            catch (Exception ex)
            {
                return BackupRestoreResult.Failure($"Failed to validate backup file structure: {ex.Message}", ex);
            }

            return BackupRestoreResult.Success();
        }

        private bool IsRestrictedLocation(string filePath)
        {
            var restrictedPaths = new[]
            {
                "OneDrive",
                "Google Drive",
                "Dropbox",
                "iCloud"
            };

            return restrictedPaths.Any(restricted => 
                filePath.IndexOf(restricted, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private string BuildBackupCommand(string backupPath, BackupRestoreOptions options)
        {
            var command = $"BACKUP DATABASE [{options.DatabaseName}] TO DISK = '{backupPath}'";
            
            var optionsList = new List<string>();
            
            if (options.CompressBackup)
            {
                optionsList.Add("COMPRESSION");
            }
            
            if (options.OverwriteExisting)
            {
                optionsList.Add("INIT");
            }
            else
            {
                optionsList.Add("NOINIT");
            }

            if (options.VerifyBackup)
            {
                optionsList.Add("CHECKSUM");
            }

            if (optionsList.Any())
            {
                command += $" WITH {string.Join(", ", optionsList)}";
            }

            return command;
        }

        private string BuildRestoreCommand(string backupPath, BackupRestoreOptions options, BackupFileInfo backupInfo)
        {
            var command = $"RESTORE DATABASE [{options.DatabaseName}] FROM DISK = '{backupPath}'";
            
            var optionsList = new List<string>();
            
            if (options.ReplaceExisting)
            {
                optionsList.Add("REPLACE");
            }

            // Add MOVE options if we have backup file info
            if (backupInfo?.LogicalFileNames != null && backupInfo.LogicalFileNames.Length > 0)
            {
                for (int i = 0; i < backupInfo.LogicalFileNames.Length; i++)
                {
                    var logicalName = backupInfo.LogicalFileNames[i];
                    var physicalName = GetDefaultPhysicalPath(options.DatabaseName, logicalName);
                    optionsList.Add($"MOVE '{logicalName}' TO '{physicalName}'");
                }
            }
            else
            {
                // Fallback: try to get logical names from the backup file directly
                var logicalNames = GetLogicalFileNamesFromBackup(backupPath);
                if (logicalNames.Any())
                {
                    foreach (var logicalName in logicalNames)
                    {
                        var physicalName = GetDefaultPhysicalPath(options.DatabaseName, logicalName);
                        optionsList.Add($"MOVE '{logicalName}' TO '{physicalName}'");
                    }
                }
            }

            if (optionsList.Any())
            {
                command += $" WITH {string.Join(", ", optionsList)}";
            }

            return command;
        }

        private string GetDefaultPhysicalPath(string databaseName, string logicalName)
        {
            // Get SQL Server default data directory
            var defaultDataPath = GetSqlServerDefaultDataPath();
            
            if (logicalName.EndsWith("_Log") || logicalName.EndsWith("_log"))
            {
                return Path.Combine(defaultDataPath, $"{databaseName}_Log.ldf");
            }
            else
            {
                return Path.Combine(defaultDataPath, $"{databaseName}.mdf");
            }
        }

        private List<string> GetLogicalFileNamesFromBackup(string backupPath)
        {
            var logicalNames = new List<string>();
            
            try
            {
                using (var connection = new SqlConnection(_masterConnectionString))
                {
                    connection.Open();
                    var fileListQuery = $"RESTORE FILELISTONLY FROM DISK = '{backupPath}'";
                    
                    using (var command = new SqlCommand(fileListQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var logicalName = reader["LogicalName"]?.ToString();
                                if (!string.IsNullOrEmpty(logicalName))
                                {
                                    logicalNames.Add(logicalName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogOperationFailure("Restore", "Failed to get logical file names", ex.Message, ex);
            }
            
            return logicalNames;
        }

        private string GetSqlServerDefaultDataPath()
        {
            try
            {
                using (var connection = new SqlConnection(_masterConnectionString))
                {
                    connection.Open();
                    var query = "SELECT SERVERPROPERTY('InstanceDefaultDataPath') as DefaultDataPath";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        var result = command.ExecuteScalar()?.ToString();
                        return result ?? @"C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA";
                    }
                }
            }
            catch
            {
                // Fallback to default path
                return @"C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA";
            }
        }

        private async Task KillDatabaseConnectionsAsync(SqlConnection connection, string databaseName)
        {
            var killQuery = @"
                DECLARE @kill VARCHAR(MAX) = '';
                SELECT @kill = @kill + 'KILL ' + CONVERT(VARCHAR(5), session_id) + ';'
                FROM sys.dm_exec_sessions
                WHERE database_id = DB_ID(@DatabaseName);

                IF @kill <> ''
                    EXEC(@kill);";

            using (var command = new SqlCommand(killQuery, connection))
            {
                command.Parameters.AddWithValue("@DatabaseName", databaseName);
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task DropDatabaseAsync(SqlConnection connection, string databaseName)
        {
            var dropQuery = $@"
                ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                DROP DATABASE [{databaseName}]";

            using (var command = new SqlCommand(dropQuery, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task<BackupRestoreResult> VerifyBackupAsync(string backupPath)
        {
            try
            {
                using (var connection = new SqlConnection(_masterConnectionString))
                {
                    connection.Open();
                    var command = $"RESTORE VERIFYONLY FROM DISK = '{backupPath}'";
                    
                    using (var sqlCommand = new SqlCommand(command, connection))
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                }

                return BackupRestoreResult.Success();
            }
            catch (Exception ex)
            {
                return BackupRestoreResult.Failure($"Backup verification failed: {ex.Message}", ex);
            }
        }

        private void OnProgressChanged(int percentage, string message, OperationType operationType, OperationStage stage)
        {
            ProgressChanged?.Invoke(this, new BackupRestoreProgressEventArgs(percentage, message, operationType, stage));
        }

        private void OnOperationCompleted(BackupRestoreResult result, OperationType operationType)
        {
            OperationCompleted?.Invoke(this, new BackupRestoreCompletedEventArgs(result, operationType));
        }

        #endregion
    }
}
