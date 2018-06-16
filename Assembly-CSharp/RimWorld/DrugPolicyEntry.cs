using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E9 RID: 1257
	public class DrugPolicyEntry : IExposable
	{
		// Token: 0x0600166B RID: 5739 RVA: 0x000C6DF8 File Offset: 0x000C51F8
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

		// Token: 0x04000D10 RID: 3344
		public ThingDef drug;

		// Token: 0x04000D11 RID: 3345
		public bool allowedForAddiction = false;

		// Token: 0x04000D12 RID: 3346
		public bool allowedForJoy = false;

		// Token: 0x04000D13 RID: 3347
		public bool allowScheduled = false;

		// Token: 0x04000D14 RID: 3348
		public float daysFrequency = 1f;

		// Token: 0x04000D15 RID: 3349
		public float onlyIfMoodBelow = 1f;

		// Token: 0x04000D16 RID: 3350
		public float onlyIfJoyBelow = 1f;

		// Token: 0x04000D17 RID: 3351
		public int takeToInventory = 0;

		// Token: 0x04000D18 RID: 3352
		public string takeToInventoryTempBuffer;
	}
}
