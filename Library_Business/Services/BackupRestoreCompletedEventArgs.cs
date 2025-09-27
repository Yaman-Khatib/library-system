using System;

namespace Library_Business.Services
{
    /// <summary>
    /// Event arguments for backup/restore operation completion
    /// </summary>
    public class BackupRestoreCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Result of the operation
        /// </summary>
        public BackupRestoreResult Result { get; set; }

        /// <summary>
        /// Type of operation that was completed
        /// </summary>
        public OperationType OperationType { get; set; }

        /// <summary>
        /// Creates a new completion event args
        /// </summary>
        /// <param name="result">Operation result</param>
        /// <param name="operationType">Operation type</param>
        public BackupRestoreCompletedEventArgs(BackupRestoreResult result, OperationType operationType)
        {
            Result = result;
            OperationType = operationType;
        }
    }
}









