using System;

namespace Verse.Sound
{
	// Token: 0x02000B88 RID: 2952
	public class SoundParamSource_External : SoundParamSource
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x0600402A RID: 16426 RVA: 0x0021C4A4 File Offset: 0x0021A8A4
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

		// Token: 0x0600402B RID: 16427 RVA: 0x0021C4E0 File Offset: 0x0021A8E0
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

		// Token: 0x04002B0F RID: 11023
		[Description("The name of the independent parameter that the game will change to drive this relationship.\n\nThis must exactly match a string that the code will use to modify this sound. If the code doesn't reference this, it will have no effect.\n\nOn the graph, this is the X axis.")]
		public string inParamName = "";

		// Token: 0x04002B10 RID: 11024
		[Description("If the code has never set this parameter on a sustainer, it will use this value.")]
		private float defaultValue = 1f;
	}
}
