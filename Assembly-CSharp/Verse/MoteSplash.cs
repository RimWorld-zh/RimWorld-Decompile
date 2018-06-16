using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEE RID: 3566
	public class MoteSplash : Mote
	{
		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004FCD RID: 20429 RVA: 0x00296114 File Offset: 0x00294514
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004FCE RID: 20430 RVA: 0x00296144 File Offset: 0x00294544
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

		// Token: 0x06004FCF RID: 20431 RVA: 0x0029619B File Offset: 0x0029459B
		public void Initialize(Vector3 position, float size, float velocity)
		{
			this.exactPosition = position;
			this.targetSize = size;
			this.velocity = velocity;
			base.Scale = 0f;
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x002961C0 File Offset: 0x002945C0
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

		// Token: 0x06004FD1 RID: 20433 RVA: 0x00296228 File Offset: 0x00294628
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x00296250 File Offset: 0x00294650
		public float CalculatedShockwaveSpan()
		{
			float num = Mathf.Sqrt(this.targetSize) * 0.8f;
			num = Mathf.Min(num, this.exactScale.x);
			return num / this.exactScale.x;
		}

		// Token: 0x040034E2 RID: 13538
		public const float VelocityFootstep = 1.5f;

		// Token: 0x040034E3 RID: 13539
		public const float SizeFootstep = 2f;

		// Token: 0x040034E4 RID: 13540
		public const float VelocityGunfire = 4f;

		// Token: 0x040034E5 RID: 13541
		public const float SizeGunfire = 1f;

		// Token: 0x040034E6 RID: 13542
		public const float VelocityExplosion = 20f;

		// Token: 0x040034E7 RID: 13543
		public const float SizeExplosion = 6f;

		// Token: 0x040034E8 RID: 13544
		private float targetSize;

		// Token: 0x040034E9 RID: 13545
		private float velocity;
	}
}
