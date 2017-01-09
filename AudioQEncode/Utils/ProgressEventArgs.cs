using System;

namespace AudioQEncode.Utils
{
	class ProgressEventArgs : EventArgs
	{
		private int _filesCompleted, _totalFiles, _progress;

		public int FilesCompleted
		{
			get
			{
				return _filesCompleted;
			}
			set
			{
				_filesCompleted = value;
			}
		}

		public int TotalFiles
		{
			get
			{
				return _totalFiles;
			}
			set
			{
				_totalFiles = value;
			}
		}

		public int Progress
		{
			get
			{
				return _progress;
			}
			set
			{
				_progress = value;
			}
		}
	}
}
