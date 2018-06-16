using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200030F RID: 783
	public class GameCondition_PsychicEmanation : GameCondition
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00072C98 File Offset: 0x00071098
		public override string Label
		{
			get
			{
				return this.def.label + " (" + this.gender.ToString().Translate().ToLower() + ")";
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00072CE2 File Offset: 0x000710E2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x0400087C RID: 2172
		public Gender gender = Gender.Male;
	}
}
