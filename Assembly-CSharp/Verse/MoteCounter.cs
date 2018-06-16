using System;

namespace Verse
{
	// Token: 0x020006D2 RID: 1746
	public class MoteCounter
	{
		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x00143508 File Offset: 0x00141908
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x00143524 File Offset: 0x00141924
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x00143548 File Offset: 0x00141948
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060025C2 RID: 9666 RVA: 0x0014356C File Offset: 0x0014196C
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x0014358E File Offset: 0x0014198E
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x0014359F File Offset: 0x0014199F
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}

		// Token: 0x0400151A RID: 5402
		private int moteCount;

		// Token: 0x0400151B RID: 5403
		private const int SaturatedCount = 250;
	}
}
