using System;

namespace DreamHouseSpectra.Networking.Data
{
	[Serializable]
	public class Error
	{
		public string error;
		public string message;

		public string Text
		{
			get
			{
				string text = "Generic Error";
				if (!string.IsNullOrEmpty(message))
					text = message;
				else if (!string.IsNullOrEmpty(error))
					text = error;

				return text;
			}
		}
	}
}