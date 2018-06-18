using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B1E RID: 2846
	public class MoteProperties
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06003EC3 RID: 16067 RVA: 0x00210B68 File Offset: 0x0020EF68
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x04002849 RID: 10313
		public bool realTime = false;

		// Token: 0x0400284A RID: 10314
		public float fadeInTime = 0f;

		// Token: 0x0400284B RID: 10315
		public float solidTime = 1f;

		// Token: 0x0400284C RID: 10316
		public float fadeOutTime = 0f;

		// Token: 0x0400284D RID: 10317
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x0400284E RID: 10318
		public float speedPerTime;

		// Token: 0x0400284F RID: 10319
		public float growthRate = 0f;

		// Token: 0x04002850 RID: 10320
		public bool collide = false;

		// Token: 0x04002851 RID: 10321
		public SoundDef landSound;

		// Token: 0x04002852 RID: 10322
		public Vector3 attachedDrawOffset;

		// Token: 0x04002853 RID: 10323
		public bool needsMaintenance = false;

		// Token: 0x04002854 RID: 10324
		public bool rotateTowardsTarget;

		// Token: 0x04002855 RID: 10325
		public bool rotateTowardsMoveDirection;
	}
}
