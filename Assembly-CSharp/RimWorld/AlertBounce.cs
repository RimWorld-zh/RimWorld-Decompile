using System;
using UnityEngine;

namespace RimWorld
{
	internal class AlertBounce
	{
		private float position = 0f;

		private float velocity = 0f;

		private float lastTime = Time.time;

		private bool idle;

		private const float StartPosition = 300f;

		private const float StartVelocity = -200f;

		private const float Acceleration = 1200f;

		private const float DampingRatio = 3f;

		private const float DampingConstant = 1f;

		private const float MaxDelta = 0.05f;

		public AlertBounce()
		{
		}

		public void DoAlertStartEffect()
		{
			this.position = 300f;
			this.velocity = -200f;
			this.lastTime = Time.time;
			this.idle = false;
		}

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
	}
}
