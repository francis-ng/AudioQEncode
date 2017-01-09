using System;
using System.IO;
using System.Windows;

using AudioQEncode.Utils;

namespace AudioQEncode
{
	/// <summary>
	/// Interaction logic for EncoderSettingsWindow.xaml
	/// </summary>
	public partial class EncoderSettingsWindow : Window
	{
		private bool optionsClicked = false;
		private EncoderPresets.BitRateType bitrateType = EncoderPresets.BitRateType.Constant;

		public EncoderSettingsWindow()
		{
			InitializeComponent();
			Closing += EncoderSettingsWindow_Closing;
			loadSettings();
			setSliderText(sldQuality.Value);
		}

		private void EncoderSettingsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool pathValidated = false;
			if (File.Exists(txtLamePath.Text))
			{
				pathValidated = true;
			}
			else
			{
				string message = @"lame.exe could not be found. Please check your executable path.";
				string caption = @"lame.exe missing";
				MessageBoxButton buttons = MessageBoxButton.OK;
				MessageBoxImage icon = MessageBoxImage.Warning;
				MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon);
				txtLamePath.Focus();
				e.Cancel = true;
			}
			if (pathValidated && !optionsClicked)
			{
				string warnText = @"Do you want to save your settings?";
				string caption = @"Save settings?";
				MessageBoxButton buttons = MessageBoxButton.YesNo;
				MessageBoxImage icon = MessageBoxImage.Warning;
				MessageBoxResult result = MessageBox.Show(warnText, caption, buttons, icon);
				switch (result)
				{
					case MessageBoxResult.Yes:
						saveSettings();
						break;
					case MessageBoxResult.No:
						break;
				}
			}
		}

		#region Utility Functions
		private void loadSettings()
		{
			setBitrateType((EncoderPresets.BitRateType)EncoderSettings.Default.MP3Bitrate);
			sldQuality.Value = (double)EncoderSettings.Default.MP3Quality;
			setSliderText(sldQuality.Value);
			txtLamePath.Text = EncoderSettings.Default.lamePath;
		}

		private void saveSettings()
		{
			EncoderSettings.Default.MP3Bitrate = (byte)bitrateType;
			EncoderSettings.Default.MP3Quality = (byte)sldQuality.Value;
			EncoderSettings.Default.lamePath = txtLamePath.Text;
			EncoderSettings.Default.Save();
		}

		private void setBitrateType(EncoderPresets.BitRateType type)
		{
			bitrateType = type;
			switch (type)
			{
				case EncoderPresets.BitRateType.Constant:
					radioCBR.IsChecked = true;
					radioVBR.IsChecked = false;
					break;
				case EncoderPresets.BitRateType.Variable:
					radioCBR.IsChecked = false;
					radioVBR.IsChecked = true;
					break;
			}
		}

		private void setSliderText(double value)
		{
			string desc = EncoderPresets.getPresetDescription(bitrateType, value);
			lblQuality.Content = desc;
		}
		#endregion

		#region Event Handlers
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			optionsClicked = true;
			Close();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			optionsClicked = true;
			saveSettings();
			Close();
		}

		private void btnBrowse_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.InitialDirectory = EncoderSettings.Default.lamePath;
			dlg.DefaultExt = "lame.exe";
			dlg.Filter = "lame executable (lame.exe)|lame.exe";
			Nullable<bool> result = dlg.ShowDialog();
			if (result == true)
			{
				txtLamePath.Text = dlg.FileName;
			}
		}

		private void sldQuality_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			setSliderText(e.NewValue);
		}

		private void bitrateRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			if (sldQuality != null)
			{
				if ((bool)radioCBR.IsChecked)
				{
					if (sldQuality.Value > sldQuality.Maximum) sldQuality.Value = sldQuality.Maximum;
					sldQuality.Maximum = EncoderPresets.CBRPresetCount - 1;
					bitrateType = EncoderPresets.BitRateType.Constant;
				}
				else if ((bool)radioVBR.IsChecked)
				{
					if (sldQuality.Value > sldQuality.Maximum) sldQuality.Value = sldQuality.Maximum;
					sldQuality.Maximum = EncoderPresets.VBRPresetCount - 1;
					bitrateType = EncoderPresets.BitRateType.Variable;
				}
				setSliderText(sldQuality.Value);
			}
		}
		#endregion
	}
}
