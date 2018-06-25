using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B1D RID: 2845
	public class MoteProperties
	{
		// Token: 0x0400284D RID: 10317
		public bool realTime = false;

		// Token: 0x0400284E RID: 10318
		public float fadeInTime = 0f;

		// Token: 0x0400284F RID: 10319
		public float solidTime = 1f;

		// Token: 0x04002850 RID: 10320
		public float fadeOutTime = 0f;

		// Token: 0x04002851 RID: 10321
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x04002852 RID: 10322
		public float speedPerTime;

		// Token: 0x04002853 RID: 10323
		public float growthRate = 0f;

		// Token: 0x04002854 RID: 10324
		public bool collide = false;

		// Token: 0x04002855 RID: 10325
		public SoundDef landSound;

		// Token: 0x04002856 RID: 10326
		public Vector3 attachedDrawOffset;

		// Token: 0x04002857 RID: 10327
		public bool needsMaintenance = false;

		// Token: 0x04002858 RID: 10328
		public bool rotateTowardsTarget;

		// Token: 0x04002859 RID: 10329
		public bool rotateTowardsMoveDirection;

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06003EC3 RID: 16067 RVA: 0x002112B0 File Offset: 0x0020F6B0
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}
	}
}
