using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7A RID: 3962
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		// Token: 0x06005F7A RID: 24442 RVA: 0x003093E5 File Offset: 0x003077E5
		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		// Token: 0x06005F7B RID: 24443 RVA: 0x003093EF File Offset: 0x003077EF
		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		// Token: 0x06005F7C RID: 24444 RVA: 0x00309410 File Offset: 0x00307810
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

		// Token: 0x06005F7D RID: 24445 RVA: 0x0030944C File Offset: 0x0030784C
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

		// Token: 0x04003EC8 RID: 16072
		public Vector3 viewCenter;

		// Token: 0x04003EC9 RID: 16073
		public float viewAngle;

		// Token: 0x04003ECA RID: 16074
		public bool invert;
	}
}
