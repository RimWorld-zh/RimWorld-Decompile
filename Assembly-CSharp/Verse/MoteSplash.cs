using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DED RID: 3565
	public class MoteSplash : Mote
	{
		// Token: 0x040034F2 RID: 13554
		public const float VelocityFootstep = 1.5f;

		// Token: 0x040034F3 RID: 13555
		public const float SizeFootstep = 2f;

		// Token: 0x040034F4 RID: 13556
		public const float VelocityGunfire = 4f;

		// Token: 0x040034F5 RID: 13557
		public const float SizeGunfire = 1f;

		// Token: 0x040034F6 RID: 13558
		public const float VelocityExplosion = 20f;

		// Token: 0x040034F7 RID: 13559
		public const float SizeExplosion = 6f;

		// Token: 0x040034F8 RID: 13560
		private float targetSize;

		// Token: 0x040034F9 RID: 13561
		private float velocity;

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004FE4 RID: 20452 RVA: 0x00297ADC File Offset: 0x00295EDC
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004FE5 RID: 20453 RVA: 0x00297B0C File Offset: 0x00295F0C
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

		// Token: 0x06004FE6 RID: 20454 RVA: 0x00297B63 File Offset: 0x00295F63
		public void Initialize(Vector3 position, float size, float velocity)
		{
			this.exactPosition = position;
			this.targetSize = size;
			this.velocity = velocity;
			base.Scale = 0f;
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x00297B88 File Offset: 0x00295F88
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

		// Token: 0x06004FE8 RID: 20456 RVA: 0x00297BF0 File Offset: 0x00295FF0
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x00297C18 File Offset: 0x00296018
		public float CalculatedShockwaveSpan()
		{
			float num = Mathf.Sqrt(this.targetSize) * 0.8f;
			num = Mathf.Min(num, this.exactScale.x);
			return num / this.exactScale.x;
		}
	}
}
