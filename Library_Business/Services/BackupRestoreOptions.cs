using System;

namespace Library_Business.Services
{
    /// <summary>
    /// Options for backup and restore operations
    /// </summary>
    public class BackupRestoreOptions
    {
        /// <summary>
        /// Database name to backup/restore (default: LibrarySystemDB)
        /// </summary>
        public string DatabaseName { get; set; } = "LibrarySystemDB";

        /// <summary>
        /// Whether to compress the backup (default: true)
        /// </summary>
        public bool CompressBackup { get; set; } = true;

        /// <summary>
        /// Whether to verify the backup after creation (default: true)
        /// </summary>
        public bool VerifyBackup { get; set; } = true;

        /// <summary>
        /// Whether to overwrite existing backup files (default: true)
        /// </summary>
        public bool OverwriteExisting { get; set; } = true;

        /// <summary>
        /// Whether to replace the existing database during restore (default: true)
        /// </summary>
        public bool ReplaceExisting { get; set; } = true;

        /// <summary>
        /// Custom data file path for restore operations (optional)
        /// </summary>
        public string CustomDataFilePath { get; set; }

        /// <summary>
        /// Custom log file path for restore operations (optional)
        /// </summary>
        public string CustomLogFilePath { get; set; }

        /// <summary>
        /// Timeout for the operation in seconds (default: 300)
        /// </summary>
        public int TimeoutSeconds { get; set; } = 300;

        /// <summary>
        /// Whether to kill existing connections before restore (default: true)
        /// </summary>
        public bool KillExistingConnections { get; set; } = true;

        /// <summary>
        /// Creates default backup options
        /// </summary>
        /// <returns>Default backup options</returns>
        public static BackupRestoreOptions CreateDefaultBackupOptions()
        {
            return new BackupRestoreOptions
            {
                CompressBackup = true,
                VerifyBackup = true,
                OverwriteExisting = true
            };
        }

        /// <summary>
        /// Creates default restore options
        /// </summary>
        /// <returns>Default restore options</returns>
        public static BackupRestoreOptions CreateDefaultRestoreOptions()
        {
            return new BackupRestoreOptions
            {
                ReplaceExisting = true,
                KillExistingConnections = true
            };
        }
    }
}









