using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEA RID: 3562
	public class MoteSplash : Mote
	{
		// Token: 0x040034EB RID: 13547
		public const float VelocityFootstep = 1.5f;

		// Token: 0x040034EC RID: 13548
		public const float SizeFootstep = 2f;

		// Token: 0x040034ED RID: 13549
		public const float VelocityGunfire = 4f;

		// Token: 0x040034EE RID: 13550
		public const float SizeGunfire = 1f;

		// Token: 0x040034EF RID: 13551
		public const float VelocityExplosion = 20f;

		// Token: 0x040034F0 RID: 13552
		public const float SizeExplosion = 6f;

		// Token: 0x040034F1 RID: 13553
		private float targetSize;

		// Token: 0x040034F2 RID: 13554
		private float velocity;

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004FE0 RID: 20448 RVA: 0x002976D0 File Offset: 0x00295AD0
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004FE1 RID: 20449 RVA: 0x00297700 File Offset: 0x00295B00
		public override float Alpha
		{
			get
			{
				float num = Mathf.Clamp01(base.AgeSecs * 10f);
				num = 1f;
				float num2 = Mathf.Clamp01(1f - base.AgeSecs / (this.targetSize / this.velocity));
				return num * num2 * this.CalculatedIntensity();
			}
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x00297757 File Offset: 0x00295B57
		public void Initialize(Vector3 position, float size, float velocity)
		{
			this.exactPosition = position;
			this.targetSize = size;
			this.velocity = velocity;
			base.Scale = 0f;
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x0029777C File Offset: 0x00295B7C
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (!base.Destroyed)
			{
				float scale = base.AgeSecs * this.velocity;
				base.Scale = scale;
				this.exactPosition += base.Map.waterInfo.GetWaterMovement(this.exactPosition) * deltaTime;
			}
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x002977E4 File Offset: 0x00295BE4
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x0029780C File Offset: 0x00295C0C
		public float CalculatedShockwaveSpan()
		{
			float num = Mathf.Sqrt(this.targetSize) * 0.8f;
			num = Mathf.Min(num, this.exactScale.x);
			return num / this.exactScale.x;
		}
	}
}
