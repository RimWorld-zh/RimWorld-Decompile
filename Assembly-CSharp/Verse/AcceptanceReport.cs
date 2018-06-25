using System;

namespace Verse
{
	public struct AcceptanceReport
	{
		private string reasonTextInt;

		private bool acceptedInt;

		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		public static AcceptanceReport WasAccepted
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = true
				};
			}
		}

		public static AcceptanceReport WasRejected
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = false
				};
			}
		}

		public static implicit operator AcceptanceReport(bool value)
		{
			AcceptanceReport result;
			if (value)
			{
				result = AcceptanceReport.WasAccepted;
			}
			else
			{
				result = AcceptanceReport.WasRejected;
			}
			return result;
		}

		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}
	}
}
