using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7A RID: 3962
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		// Token: 0x04003EDA RID: 16090
		public Vector3 viewCenter;

		// Token: 0x04003EDB RID: 16091
		public float viewAngle;

		// Token: 0x04003EDC RID: 16092
		public bool invert;

		// Token: 0x06005FA3 RID: 24483 RVA: 0x0030B489 File Offset: 0x00309889
		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x0030B493 File Offset: 0x00309893
		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		// Token: 0x06005FA5 RID: 24485 RVA: 0x0030B4B4 File Offset: 0x003098B4
		public override double GetValue(double x, double y, double z)
		{
			float valueInt = this.GetValueInt(x, y, z);
			double result;
			if (this.invert)
			{
				result = (double)(1f - valueInt);
			}
			else
			{
				result = (double)valueInt;
			}
			return result;
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x0030B4F0 File Offset: 0x003098F0
		private float GetValueInt(double x, double y, double z)
		{
			float result;
			if (this.viewAngle >= 180f)
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
