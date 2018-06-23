using System;

namespace Verse
{
	// Token: 0x02000FB0 RID: 4016
	public struct CoverInfo
	{
		// Token: 0x04003F8B RID: 16267
		private Thing thingInt;

		// Token: 0x04003F8C RID: 16268
		private float blockChanceInt;

		// Token: 0x06006135 RID: 24885 RVA: 0x00311A74 File Offset: 0x0030FE74
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06006136 RID: 24886 RVA: 0x00311A88 File Offset: 0x0030FE88
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06006137 RID: 24887 RVA: 0x00311AA4 File Offset: 0x0030FEA4
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x06006138 RID: 24888 RVA: 0x00311AC0 File Offset: 0x0030FEC0
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}
	}
}
