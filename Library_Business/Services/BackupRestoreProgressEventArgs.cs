using System;

namespace Library_Business.Services
{
    /// <summary>
    /// Event arguments for backup/restore progress updates
    /// </summary>
    public class BackupRestoreProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Current progress percentage (0-100)
        /// </summary>
        public int ProgressPercentage { get; set; }

        /// <summary>
        /// Current status message
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Type of operation being performed
        /// </summary>
        public OperationType OperationType { get; set; }

        /// <summary>
        /// Current stage of the operation
        /// </summary>
        public OperationStage CurrentStage { get; set; }

        /// <summary>
        /// Estimated time remaining
        /// </summary>
        public TimeSpan? EstimatedTimeRemaining { get; set; }

        /// <summary>
        /// Creates a new progress event args
        /// </summary>
        /// <param name="progressPercentage">Progress percentage</param>
        /// <param name="statusMessage">Status message</param>
        /// <param name="operationType">Operation type</param>
        /// <param name="currentStage">Current stage</param>
        public BackupRestoreProgressEventArgs(int progressPercentage, string statusMessage, OperationType operationType, OperationStage currentStage)
        {
            ProgressPercentage = progressPercentage;
            StatusMessage = statusMessage;
            OperationType = operationType;
            CurrentStage = currentStage;
        }
    }

    /// <summary>
    /// Type of backup/restore operation
    /// </summary>
    public enum OperationType
    {
        Backup,
        Restore,
        Validation
    }

    /// <summary>
    /// Current stage of the operation
    /// </summary>
    public enum OperationStage
    {
        Initializing,
        Validating,
        CreatingBackup,
        RestoringDatabase,
        Verifying,
        Completing,
        Completed,
        Failed
    }
}









