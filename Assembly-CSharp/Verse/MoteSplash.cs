using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DED RID: 3565
	public class MoteSplash : Mote
	{
		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06004FCB RID: 20427 RVA: 0x002960F4 File Offset: 0x002944F4
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004FCC RID: 20428 RVA: 0x00296124 File Offset: 0x00294524
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

		// Token: 0x06004FCD RID: 20429 RVA: 0x0029617B File Offset: 0x0029457B
		public void Initialize(Vector3 position, float size, float velocity)
		{
			this.exactPosition = position;
			this.targetSize = size;
			this.velocity = velocity;
			base.Scale = 0f;
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x002961A0 File Offset: 0x002945A0
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

		// Token: 0x06004FCF RID: 20431 RVA: 0x00296208 File Offset: 0x00294608
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x00296230 File Offset: 0x00294630
		public float CalculatedShockwaveSpan()
		{
			float num = Mathf.Sqrt(this.targetSize) * 0.8f;
			num = Mathf.Min(num, this.exactScale.x);
			return num / this.exactScale.x;
		}

		// Token: 0x040034E0 RID: 13536
		public const float VelocityFootstep = 1.5f;

		// Token: 0x040034E1 RID: 13537
		public const float SizeFootstep = 2f;

		// Token: 0x040034E2 RID: 13538
		public const float VelocityGunfire = 4f;

		// Token: 0x040034E3 RID: 13539
		public const float SizeGunfire = 1f;

		// Token: 0x040034E4 RID: 13540
		public const float VelocityExplosion = 20f;

		// Token: 0x040034E5 RID: 13541
		public const float SizeExplosion = 6f;

		// Token: 0x040034E6 RID: 13542
		private float targetSize;

		// Token: 0x040034E7 RID: 13543
		private float velocity;
	}
}
