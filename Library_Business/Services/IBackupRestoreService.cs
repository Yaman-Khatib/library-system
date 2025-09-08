using System;
using System.Threading.Tasks;

namespace Library_Business.Services
{
    /// <summary>
    /// Interface for database backup and restore operations
    /// </summary>
    public interface IBackupRestoreService
    {
        /// <summary>
        /// Event fired when backup/restore progress changes
        /// </summary>
        event EventHandler<BackupRestoreProgressEventArgs> ProgressChanged;

        /// <summary>
        /// Event fired when backup/restore operation completes
        /// </summary>
        event EventHandler<BackupRestoreCompletedEventArgs> OperationCompleted;

        /// <summary>
        /// Creates a backup of the database
        /// </summary>
        /// <param name="backupPath">Full path where the backup file should be created</param>
        /// <param name="options">Backup options</param>
        /// <returns>Result of the backup operation</returns>
        Task<BackupRestoreResult> CreateBackupAsync(string backupPath, BackupRestoreOptions options = null);

        /// <summary>
        /// Restores the database from a backup file
        /// </summary>
        /// <param name="backupPath">Full path to the backup file</param>
        /// <param name="options">Restore options</param>
        /// <returns>Result of the restore operation</returns>
        Task<BackupRestoreResult> RestoreDatabaseAsync(string backupPath, BackupRestoreOptions options = null);

        /// <summary>
        /// Validates if a backup file is valid and can be restored
        /// </summary>
        /// <param name="backupPath">Full path to the backup file</param>
        /// <returns>Validation result</returns>
        Task<BackupRestoreResult> ValidateBackupFileAsync(string backupPath);

        /// <summary>
        /// Checks if the database exists
        /// </summary>
        /// <param name="databaseName">Name of the database to check</param>
        /// <returns>True if database exists, false otherwise</returns>
        Task<bool> DatabaseExistsAsync(string databaseName);

        /// <summary>
        /// Gets information about a backup file
        /// </summary>
        /// <param name="backupPath">Full path to the backup file</param>
        /// <returns>Backup file information</returns>
        Task<BackupFileInfo> GetBackupFileInfoAsync(string backupPath);
    }
}



