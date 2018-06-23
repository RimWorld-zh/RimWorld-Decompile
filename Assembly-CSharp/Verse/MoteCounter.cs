using System;

namespace Verse
{
	// Token: 0x020006CE RID: 1742
	public class MoteCounter
	{
		// Token: 0x04001518 RID: 5400
		private int moteCount;

		// Token: 0x04001519 RID: 5401
		private const int SaturatedCount = 250;

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x001436CC File Offset: 0x00141ACC
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x001436E8 File Offset: 0x00141AE8
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x0014370C File Offset: 0x00141B0C
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060025BC RID: 9660 RVA: 0x00143730 File Offset: 0x00141B30
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x00143752 File Offset: 0x00141B52
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x00143763 File Offset: 0x00141B63
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}
	}
}
