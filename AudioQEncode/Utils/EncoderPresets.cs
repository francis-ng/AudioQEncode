namespace AudioQEncode.Utils
{
	class EncoderPresets
	{
		public const int CBRPresetCount = 10;
		public const int VBRPresetCount = 7;

		public enum BitRateType
		{
			Constant,
			Variable
		}

		public static string getEncoderCommand(BitRateType type, double presetNum)
		{
			string command = "--nohist ";
			int preset = (int)presetNum;
			if (type == BitRateType.Constant)
			{
				switch (preset)
				{
					case 0:
						command += "-b 64";
						break;
					case 1:
						command += "-b 80";
						break;
					case 2:
						command += "-b 96";
						break;
					case 3:
						command += "-b 112";
						break;
					case 4:
						command += "-b 128";
						break;
					case 5:
						command += "-b 160";
						break;
					case 6:
						command += "-b 192";
						break;
					case 7:
						command += "-b 224";
						break;
					case 8:
						command += "-b 256";
						break;
					case 9:
						command += "-b 320";
						break;
				}
			}
			else if (type == BitRateType.Variable)
			{
				switch (preset)
				{
					case 0:
						command += "-V6";
						break;
					case 1:
						command += "-V5";
						break;
					case 2:
						command += "-V4";
						break;
					case 3:
						command += "-V3";
						break;
					case 4:
						command += "-V2";
						break;
					case 5:
						command += "-V1";
						break;
					case 6:
						command += "-V0";
						break;
				}
			}
			return command;
		}

		public static string getPresetDescription(BitRateType type, double presetNum)
		{
			string desc = "";
			int preset = (int)presetNum;
			if (type == BitRateType.Constant)
			{
				switch (preset)
				{
					case 0:
						desc = "64kbps";
						break;
					case 1:
						desc = "80kbps";
						break;
					case 2:
						desc = "96kbps";
						break;
					case 3:
						desc = "112kbps";
						break;
					case 4:
						desc = "128kbps";
						break;
					case 5:
						desc = "160kbps";
						break;
					case 6:
						desc = "192kbps";
						break;
					case 7:
						desc = "224kbps";
						break;
					case 8:
						desc = "256kbps";
						break;
					case 9:
						desc = "320kbps";
						break;
				}
			}
			else if (type == BitRateType.Variable)
			{
				switch (preset)
				{
					case 0:
						desc = "~120kbps";
						break;
					case 1:
						desc = "~130kbps";
						break;
					case 2:
						desc = "~160kbps";
						break;
					case 3:
						desc = "~170kbps";
						break;
					case 4:
						desc = "~190kbps";
						break;
					case 5:
						desc = "~220kbps";
						break;
					case 6:
						desc = "~240kbps";
						break;
				}
			}
			return desc;
		}
	}
}
