using System;

namespace Verse.Sound
{
	// Token: 0x02000B87 RID: 2951
	public class SoundParamSource_External : SoundParamSource
	{
		// Token: 0x04002B1B RID: 11035
		[Description("The name of the independent parameter that the game will change to drive this relationship.\n\nThis must exactly match a string that the code will use to modify this sound. If the code doesn't reference this, it will have no effect.\n\nOn the graph, this is the X axis.")]
		public string inParamName = "";

		// Token: 0x04002B1C RID: 11036
		[Description("If the code has never set this parameter on a sustainer, it will use this value.")]
		private float defaultValue = 1f;

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600402F RID: 16431 RVA: 0x0021CEFC File Offset: 0x0021B2FC
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

		// Token: 0x06004030 RID: 16432 RVA: 0x0021CF38 File Offset: 0x0021B338
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
