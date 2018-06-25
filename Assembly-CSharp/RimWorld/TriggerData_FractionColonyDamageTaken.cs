using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AC RID: 428
	public class TriggerData_FractionColonyDamageTaken : TriggerData
	{
		// Token: 0x040003BB RID: 955
		public float startColonyDamage;

		// Token: 0x060008D7 RID: 2263 RVA: 0x000537DE File Offset: 0x00051BDE
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.startColonyDamage, "startColonyDamage", 0f, false);
		}
	}
}
