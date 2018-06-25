using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7F RID: 3967
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		// Token: 0x04003EE5 RID: 16101
		public Vector3 viewCenter;

		// Token: 0x04003EE6 RID: 16102
		public float viewAngle;

		// Token: 0x04003EE7 RID: 16103
		public bool invert;

		// Token: 0x06005FAD RID: 24493 RVA: 0x0030BD4D File Offset: 0x0030A14D
		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		// Token: 0x06005FAE RID: 24494 RVA: 0x0030BD57 File Offset: 0x0030A157
		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		// Token: 0x06005FAF RID: 24495 RVA: 0x0030BD78 File Offset: 0x0030A178
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

		// Token: 0x06005FB0 RID: 24496 RVA: 0x0030BDB4 File Offset: 0x0030A1B4
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
