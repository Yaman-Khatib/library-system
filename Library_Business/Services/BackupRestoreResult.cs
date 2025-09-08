using System;
using System.Collections.Generic;

namespace Library_Business.Services
{
    /// <summary>
    /// Result of a backup or restore operation
    /// </summary>
    public class BackupRestoreResult
    {
        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error message if the operation failed
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Exception that occurred during the operation (if any)
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Additional details about the operation
        /// </summary>
        public Dictionary<string, object> Details { get; set; }

        /// <summary>
        /// Path to the backup file (for backup operations)
        /// </summary>
        public string BackupFilePath { get; set; }

        /// <summary>
        /// Size of the backup file in bytes
        /// </summary>
        public long BackupFileSize { get; set; }

        /// <summary>
        /// Duration of the operation
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Creates a successful result
        /// </summary>
        /// <param name="backupFilePath">Path to the backup file</param>
        /// <param name="backupFileSize">Size of the backup file</param>
        /// <param name="duration">Duration of the operation</param>
        /// <returns>Successful result</returns>
        public static BackupRestoreResult Success(string backupFilePath = null, long backupFileSize = 0, TimeSpan duration = default)
        {
            return new BackupRestoreResult
            {
                IsSuccess = true,
                BackupFilePath = backupFilePath,
                BackupFileSize = backupFileSize,
                Duration = duration,
                Details = new Dictionary<string, object>()
            };
        }

        /// <summary>
        /// Creates a failed result
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="exception">Exception that occurred</param>
        /// <param name="duration">Duration of the operation</param>
        /// <returns>Failed result</returns>
        public static BackupRestoreResult Failure(string errorMessage, Exception exception = null, TimeSpan duration = default)
        {
            return new BackupRestoreResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Exception = exception,
                Duration = duration,
                Details = new Dictionary<string, object>()
            };
        }
    }
}



