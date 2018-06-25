using System;

namespace Verse
{
	// Token: 0x02000FB4 RID: 4020
	public struct CoverInfo
	{
		// Token: 0x04003F8E RID: 16270
		private Thing thingInt;

		// Token: 0x04003F8F RID: 16271
		private float blockChanceInt;

		// Token: 0x0600613F RID: 24895 RVA: 0x003120F4 File Offset: 0x003104F4
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06006140 RID: 24896 RVA: 0x00312108 File Offset: 0x00310508
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06006141 RID: 24897 RVA: 0x00312124 File Offset: 0x00310524
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06006142 RID: 24898 RVA: 0x00312140 File Offset: 0x00310540
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}
	}
}
