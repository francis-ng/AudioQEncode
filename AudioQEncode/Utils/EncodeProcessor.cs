using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace AudioQEncode.Utils
{
	class EncodeProcessor
	{
		private Queue<string> _files;
		private EncoderPresets.BitRateType _bitrate;
		private double _quality;
		private string _encoder, _command, _progress;
		private int _totalFiles, _filesCompleted;

		public event EventHandler<ProgressEventArgs> ProgressChanged;

		public int TotalFiles
		{
			get { return _totalFiles; }
			set { _totalFiles = value; }
		}

		public int FilesCompleted
		{
			get { return _filesCompleted; }
			set { _filesCompleted = value; }
		}

		public int Progress
		{
			get {
				if (string.IsNullOrEmpty(_progress)) return 0;
				else return Convert.ToInt32(_progress.Substring(0, _progress.Length - 1));
			}
		}

		public EncodeProcessor(List<string> files)
		{
			_files = filterWavFiles(files);
			_bitrate = (EncoderPresets.BitRateType)EncoderSettings.Default.MP3Bitrate;
			_quality = EncoderSettings.Default.MP3Quality;
			_encoder = EncoderSettings.Default.lamePath;
			_command = EncoderPresets.getEncoderCommand(_bitrate, _quality);
			_progress = "0%";
		}

		protected virtual void ProgressUpdated(ProgressEventArgs e)
		{
			ProgressChanged?.Invoke(this, e);
		}

		public void startEncode()
		{
			TotalFiles = _files.Count;
			FilesCompleted = 0;
			encodeFile(_files.Dequeue());
		}

		private void encodeFile(string file)
		{
			Debug.WriteLine("Encoding " + file);
			Process lame = new Process();
			try
			{
				lame.ErrorDataReceived += new DataReceivedEventHandler(Lame_ErrorDataReceived);
				lame.Exited += new EventHandler(Lame_Exited);
				lame.EnableRaisingEvents = true;
				lame.StartInfo.UseShellExecute = false;
				lame.StartInfo.RedirectStandardError = true;
				lame.StartInfo.FileName = _encoder;
				lame.StartInfo.Arguments = string.Format("{0} \"{1}\"", _command, file);
				lame.StartInfo.WorkingDirectory = Path.GetDirectoryName(file);
				lame.StartInfo.CreateNoWindow = true;
				lame.Start();
				lame.BeginErrorReadLine();
			}
			catch (Exception e)
			{
				MessageBoxButton buttons = MessageBoxButton.OK;
				MessageBoxImage icon = MessageBoxImage.Error;
				MessageBoxResult result = MessageBox.Show(e.Message, @"Error", buttons, icon);
			}
		}

		private Queue<string> filterWavFiles(List<string> files)
		{
			Queue<string> result = new Queue<string>(files.Where(s => s.EndsWith("wav")).ToList<string>());
			return result;
		}

		private void Lame_Exited(object sender, EventArgs e)
		{
			((Process)sender).Close();
			FilesCompleted++;
			if (FilesCompleted == TotalFiles) _progress = "100%";
			ProgressEventArgs args = getCurrentProgressArgs();
			ProgressUpdated(args);
			if (FilesCompleted < TotalFiles)
			{
				encodeFile(_files.Dequeue());
			}
		}

		private void Lame_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				string pattern = @"\d{1,3}%";
				_progress = System.Text.RegularExpressions.Regex.Match(e.Data, pattern).Value;
				ProgressEventArgs args = getCurrentProgressArgs();
				ProgressUpdated(args);
			}
		}
		
		private ProgressEventArgs getCurrentProgressArgs()
		{
			ProgressEventArgs args = new ProgressEventArgs();
			args.FilesCompleted = FilesCompleted;
			args.TotalFiles = TotalFiles;
			args.Progress = Progress;
			return args;
		}
	}
}
