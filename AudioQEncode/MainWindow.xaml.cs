using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using AudioQEncode.Utils;

namespace AudioQEncode
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		delegate void progressUpdater(ProgressBar target, int max, int value);

		public MainWindow()
		{
			InitializeComponent();
		}

		private void window_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
				e.Effects = DragDropEffects.All;
			else
				e.Effects = DragDropEffects.None;
		}

		private void window_Drop(object sender, DragEventArgs e)
		{
			List<string> filepaths = new List<string>();
			foreach (var s in (string[])e.Data.GetData(DataFormats.FileDrop, false))
			{
				if (Directory.Exists(s))
				{
					//Add files from folder
					filepaths.AddRange(Directory.GetFiles(s));
				}
				else
				{
					//Add filepath
					filepaths.Add(s);
				}
			}
			Debug.WriteLine(string.Join("\n", filepaths));
			
			EncodeProcessor ep = new EncodeProcessor(filepaths);
			ep.ProgressChanged += Encode_ProgressChanged;
			ep.startEncode();
		}

		private void Encode_ProgressChanged(object sender, ProgressEventArgs e)
		{
			updateProgress(prgTotal, e.TotalFiles, e.FilesCompleted);
			updateProgress(prgFile, 100, e.Progress);
		}

		private void updateProgress(ProgressBar target, int max, int value)
		{
			if (target.Dispatcher.CheckAccess())
			{
				target.Maximum = max;
				target.Value = value;
			}
			else
			{
				target.Dispatcher.BeginInvoke(new progressUpdater(updateProgress), new object[] { target, max, value });
			}
		}

		private void OpenSettings(object sender, RoutedEventArgs e)
		{
			EncoderSettingsWindow es = new EncoderSettingsWindow();
			es.Closed += EncoderSettings_Closed;
			Topmost = false;
			es.ShowDialog();
		}

		private void openContextMenu(object sender, MouseButtonEventArgs e)
		{
			ContextMenu cm = this.FindResource("rClickMenu") as ContextMenu;
			cm.IsOpen = true;
		}

		private void EncoderSettings_Closed(object sender, EventArgs e)
		{
			Topmost = true;
		}

		private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
