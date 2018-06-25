using System;

namespace Verse
{
	// Token: 0x02000FB5 RID: 4021
	public struct CoverInfo
	{
		// Token: 0x04003F96 RID: 16278
		private Thing thingInt;

		// Token: 0x04003F97 RID: 16279
		private float blockChanceInt;

		// Token: 0x0600613F RID: 24895 RVA: 0x00312338 File Offset: 0x00310738
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06006140 RID: 24896 RVA: 0x0031234C File Offset: 0x0031074C
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06006141 RID: 24897 RVA: 0x00312368 File Offset: 0x00310768
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06006142 RID: 24898 RVA: 0x00312384 File Offset: 0x00310784
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}
	}
}
