using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AC RID: 428
	public class TriggerData_FractionColonyDamageTaken : TriggerData
	{
		// Token: 0x060008DA RID: 2266 RVA: 0x000537CE File Offset: 0x00051BCE
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.startColonyDamage, "startColonyDamage", 0f, false);
		}

		// Token: 0x040003BC RID: 956
		public float startColonyDamage;
	}
}
