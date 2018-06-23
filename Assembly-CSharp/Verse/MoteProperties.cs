using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B1A RID: 2842
	public class MoteProperties
	{
		// Token: 0x04002845 RID: 10309
		public bool realTime = false;

		// Token: 0x04002846 RID: 10310
		public float fadeInTime = 0f;

		// Token: 0x04002847 RID: 10311
		public float solidTime = 1f;

		// Token: 0x04002848 RID: 10312
		public float fadeOutTime = 0f;

		// Token: 0x04002849 RID: 10313
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x0400284A RID: 10314
		public float speedPerTime;

		// Token: 0x0400284B RID: 10315
		public float growthRate = 0f;

		// Token: 0x0400284C RID: 10316
		public bool collide = false;

		// Token: 0x0400284D RID: 10317
		public SoundDef landSound;

		// Token: 0x0400284E RID: 10318
		public Vector3 attachedDrawOffset;

		// Token: 0x0400284F RID: 10319
		public bool needsMaintenance = false;

		// Token: 0x04002850 RID: 10320
		public bool rotateTowardsTarget;

		// Token: 0x04002851 RID: 10321
		public bool rotateTowardsMoveDirection;

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x00210EA4 File Offset: 0x0020F2A4
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}
	}
}
