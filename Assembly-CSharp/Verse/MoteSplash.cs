using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEC RID: 3564
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

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004FE4 RID: 20452 RVA: 0x002977FC File Offset: 0x00295BFC
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004FE5 RID: 20453 RVA: 0x0029782C File Offset: 0x00295C2C
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

		// Token: 0x06004FE6 RID: 20454 RVA: 0x00297883 File Offset: 0x00295C83
		public void Initialize(Vector3 position, float size, float velocity)
		{
			this.exactPosition = position;
			this.targetSize = size;
			this.velocity = velocity;
			base.Scale = 0f;
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x002978A8 File Offset: 0x00295CA8
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

		// Token: 0x06004FE8 RID: 20456 RVA: 0x00297910 File Offset: 0x00295D10
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x00297938 File Offset: 0x00295D38
		public float CalculatedShockwaveSpan()
		{
			float num = Mathf.Sqrt(this.targetSize) * 0.8f;
			num = Mathf.Min(num, this.exactScale.x);
			return num / this.exactScale.x;
		}
	}
}
