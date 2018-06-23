using System;

namespace Verse.Sound
{
	// Token: 0x02000B84 RID: 2948
	public class SoundParamSource_External : SoundParamSource
	{
		// Token: 0x04002B14 RID: 11028
		[Description("The name of the independent parameter that the game will change to drive this relationship.\n\nThis must exactly match a string that the code will use to modify this sound. If the code doesn't reference this, it will have no effect.\n\nOn the graph, this is the X axis.")]
		public string inParamName = "";

		// Token: 0x04002B15 RID: 11029
		[Description("If the code has never set this parameter on a sustainer, it will use this value.")]
		private float defaultValue = 1f;

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600402C RID: 16428 RVA: 0x0021CB40 File Offset: 0x0021AF40
		public override string Label
		{
			get
			{
				string result;
				if (this.inParamName == "")
				{
					result = "Undefined external";
				}
				else
				{
					result = this.inParamName;
				}
				return result;
			}
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x0021CB7C File Offset: 0x0021AF7C
		public override float ValueFor(Sample samp)
		{
			float num;
			float result;
			if (samp.ExternalParams.TryGetValue(this.inParamName, out num))
			{
				result = num;
			}
			else
			{
				result = this.defaultValue;
			}
			return result;
		}
	}
}
