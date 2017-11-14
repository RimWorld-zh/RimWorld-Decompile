using UnityEngine;

namespace Verse
{
	public class MoteSplash : Mote
	{
		public const float VelocityFootstep = 1.5f;

		public const float SizeFootstep = 2f;

		public const float VelocityGunfire = 4f;

		public const float SizeGunfire = 1f;

		public const float VelocityExplosion = 20f;

		public const float SizeExplosion = 6f;

		private float targetSize;

		private float velocity;

		protected override float LifespanSecs
		{
			get
			{
				return this.targetSize / this.velocity;
			}
		}

		public void Initialize(Vector3 position, float size, float velocity)
		{
			base.exactPosition = position;
			this.targetSize = size;
			this.velocity = velocity;
			base.Scale = 0f;
		}

		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (!base.Destroyed)
			{
				float num2 = base.Scale = base.AgeSecs * this.velocity;
				base.exactPosition += base.Map.waterInfo.GetWaterMovement(base.exactPosition) * deltaTime;
			}
		}

		public float CalculatedIntensity()
		{
			return (float)(Mathf.Sqrt(this.targetSize) / 10.0);
		}

		public float CalculatedAlpha()
		{
			float num = Mathf.Clamp01((float)(base.AgeSecs * 10.0));
			num = 1f;
			float num2 = Mathf.Clamp01((float)(1.0 - base.AgeSecs / this.LifespanSecs));
			return num * num2 * this.CalculatedIntensity();
		}

		public float CalculatedShockwaveSpan()
		{
			float a = (float)(Mathf.Sqrt(this.targetSize) * 0.800000011920929);
			a = Mathf.Min(a, base.exactScale.x);
			return a / base.exactScale.x;
		}
	}
}
