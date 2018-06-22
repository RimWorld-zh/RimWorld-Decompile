using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AC RID: 428
	public class TriggerData_FractionColonyDamageTaken : TriggerData
	{
		// Token: 0x060008D8 RID: 2264 RVA: 0x000537E2 File Offset: 0x00051BE2
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.startColonyDamage, "startColonyDamage", 0f, false);
		}

		// Token: 0x040003BA RID: 954
		public float startColonyDamage;
	}
}
