using UnityEngine;

namespace Verse.Noise
{
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		public Vector3 viewCenter;

		public float viewAngle;

		public bool invert;

		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		public override double GetValue(double x, double y, double z)
		{
			float valueInt = this.GetValueInt(x, y, z);
			return (!this.invert) ? ((double)valueInt) : (1.0 - valueInt);
		}

		private float GetValueInt(double x, double y, double z)
		{
			float result;
			if (this.viewAngle >= 180.0)
			{
				result = 0f;
			}
			else
			{
				float num = Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z));
				result = Mathf.Min(num / this.viewAngle, 1f);
			}
			return result;
		}
	}
}
