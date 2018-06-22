using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000786 RID: 1926
	internal class AlertBounce
	{
		// Token: 0x06002ABE RID: 10942 RVA: 0x00169D90 File Offset: 0x00168190
		public void DoAlertStartEffect()
		{
			this.position = 300f;
			this.velocity = -200f;
			this.lastTime = Time.time;
			this.idle = false;
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x00169DBC File Offset: 0x001681BC
		public float CalculateHorizontalOffset()
		{
			float result;
			if (this.idle)
			{
				result = this.position;
			}
			else
			{
				float num = Mathf.Min(Time.time - this.lastTime, 0.05f);
				this.lastTime = Time.time;
				this.velocity -= 1200f * num;
				this.position += this.velocity * num;
				if (this.position < 0f)
				{
					this.position = 0f;
					this.velocity = Mathf.Max(-this.velocity / 3f - 1f, 0f);
				}
				if (Mathf.Abs(this.velocity) < 0.0001f && this.position < 1f)
				{
					this.velocity = 0f;
					this.position = 0f;
					this.idle = true;
				}
				result = this.position;
			}
			return result;
		}

		// Token: 0x04001710 RID: 5904
		private float position = 0f;

		// Token: 0x04001711 RID: 5905
		private float velocity = 0f;

		// Token: 0x04001712 RID: 5906
		private float lastTime = Time.time;

		// Token: 0x04001713 RID: 5907
		private bool idle;

		// Token: 0x04001714 RID: 5908
		private const float StartPosition = 300f;

		// Token: 0x04001715 RID: 5909
		private const float StartVelocity = -200f;

		// Token: 0x04001716 RID: 5910
		private const float Acceleration = 1200f;

		// Token: 0x04001717 RID: 5911
		private const float DampingRatio = 3f;

		// Token: 0x04001718 RID: 5912
		private const float DampingConstant = 1f;

		// Token: 0x04001719 RID: 5913
		private const float MaxDelta = 0.05f;
	}
}
