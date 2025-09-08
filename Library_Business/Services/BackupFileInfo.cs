using System;

namespace Library_Business.Services
{
    /// <summary>
    /// Information about a backup file
    /// </summary>
    public class BackupFileInfo
    {
        /// <summary>
        /// Name of the database in the backup
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Date and time when the backup was created
        /// </summary>
        public DateTime BackupDate { get; set; }

        /// <summary>
        /// Size of the backup file in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Backup type (Full, Differential, Log)
        /// </summary>
        public string BackupType { get; set; }

        /// <summary>
        /// SQL Server version that created the backup
        /// </summary>
        public string ServerVersion { get; set; }

        /// <summary>
        /// Whether the backup file is valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Error message if the backup file is invalid
        /// </summary>
        public string ValidationError { get; set; }

        /// <summary>
        /// Logical file names in the backup
        /// </summary>
        public string[] LogicalFileNames { get; set; }

        /// <summary>
        /// Physical file names in the backup
        /// </summary>
        public string[] PhysicalFileNames { get; set; }
    }
}



