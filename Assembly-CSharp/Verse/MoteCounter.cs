using System;

namespace Verse
{
	// Token: 0x020006D0 RID: 1744
	public class MoteCounter
	{
		// Token: 0x04001518 RID: 5400
		private int moteCount;

		// Token: 0x04001519 RID: 5401
		private const int SaturatedCount = 250;

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x0014381C File Offset: 0x00141C1C
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x00143838 File Offset: 0x00141C38
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x0014385C File Offset: 0x00141C5C
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x00143880 File Offset: 0x00141C80
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x001438A2 File Offset: 0x00141CA2
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x001438B3 File Offset: 0x00141CB3
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}
	}
}
