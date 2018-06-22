using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E5 RID: 1253
	public class DrugPolicyEntry : IExposable
	{
		// Token: 0x06001663 RID: 5731 RVA: 0x000C6E40 File Offset: 0x000C5240
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<bool>(ref this.allowedForAddiction, "allowedForAddiction", false, false);
			Scribe_Values.Look<bool>(ref this.allowedForJoy, "allowedForJoy", false, false);
			Scribe_Values.Look<bool>(ref this.allowScheduled, "allowScheduled", false, false);
			Scribe_Values.Look<float>(ref this.daysFrequency, "daysFrequency", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfMoodBelow, "onlyIfMoodBelow", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfJoyBelow, "onlyIfJoyBelow", 1f, false);
			Scribe_Values.Look<int>(ref this.takeToInventory, "takeToInventory", 0, false);
		}

		// Token: 0x04000D0D RID: 3341
		public ThingDef drug;

		// Token: 0x04000D0E RID: 3342
		public bool allowedForAddiction = false;

		// Token: 0x04000D0F RID: 3343
		public bool allowedForJoy = false;

		// Token: 0x04000D10 RID: 3344
		public bool allowScheduled = false;

		// Token: 0x04000D11 RID: 3345
		public float daysFrequency = 1f;

		// Token: 0x04000D12 RID: 3346
		public float onlyIfMoodBelow = 1f;

		// Token: 0x04000D13 RID: 3347
		public float onlyIfJoyBelow = 1f;

		// Token: 0x04000D14 RID: 3348
		public int takeToInventory = 0;

		// Token: 0x04000D15 RID: 3349
		public string takeToInventoryTempBuffer;
	}
}
