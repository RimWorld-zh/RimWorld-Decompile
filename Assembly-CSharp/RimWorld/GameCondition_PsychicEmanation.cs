using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000311 RID: 785
	public class GameCondition_PsychicEmanation : GameCondition
	{
		// Token: 0x04000881 RID: 2177
		public Gender gender = Gender.Male;

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x00072EA4 File Offset: 0x000712A4
		public override string Label
		{
			get
			{
				return this.def.label + " (" + this.gender.ToString().Translate().ToLower() + ")";
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00072EEE File Offset: 0x000712EE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}
	}
}
