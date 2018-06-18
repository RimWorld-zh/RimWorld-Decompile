using System;

namespace Verse
{
	// Token: 0x02000FB0 RID: 4016
	public struct CoverInfo
	{
		// Token: 0x0600610C RID: 24844 RVA: 0x0030F9D0 File Offset: 0x0030DDD0
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x0600610D RID: 24845 RVA: 0x0030F9E4 File Offset: 0x0030DDE4
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x0600610E RID: 24846 RVA: 0x0030FA00 File Offset: 0x0030DE00
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x0600610F RID: 24847 RVA: 0x0030FA1C File Offset: 0x0030DE1C
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}

		// Token: 0x04003F79 RID: 16249
		private Thing thingInt;

		// Token: 0x04003F7A RID: 16250
		private float blockChanceInt;
	}
}
