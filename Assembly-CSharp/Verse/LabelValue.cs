using System;

namespace Verse
{
	public struct LabelValue
	{
		private string label;

		private string value;

		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		public string Label
		{
			get
			{
				return this.label;
			}
		}

		public string Value
		{
			get
			{
				return this.value;
			}
		}

		public override string ToString()
		{
			return this.label;
		}
	}
}
