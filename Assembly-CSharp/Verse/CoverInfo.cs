using System;

namespace Verse
{
	// Token: 0x02000FB1 RID: 4017
	public struct CoverInfo
	{
		// Token: 0x0600610E RID: 24846 RVA: 0x0030F8F4 File Offset: 0x0030DCF4
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x0600610F RID: 24847 RVA: 0x0030F908 File Offset: 0x0030DD08
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06006110 RID: 24848 RVA: 0x0030F924 File Offset: 0x0030DD24
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06006111 RID: 24849 RVA: 0x0030F940 File Offset: 0x0030DD40
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}

		// Token: 0x04003F7A RID: 16250
		private Thing thingInt;

		// Token: 0x04003F7B RID: 16251
		private float blockChanceInt;
	}
}
