using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000311 RID: 785
	public class GameCondition_PsychicEmanation : GameCondition
	{
		// Token: 0x0400087E RID: 2174
		public Gender gender = Gender.Male;

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x00072E9C File Offset: 0x0007129C
		public override string Label
		{
			get
			{
				return this.def.label + " (" + this.gender.ToString().Translate().ToLower() + ")";
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00072EE6 File Offset: 0x000712E6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}
	}
}
