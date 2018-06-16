using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7B RID: 3963
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		// Token: 0x06005F7C RID: 24444 RVA: 0x00309309 File Offset: 0x00307709
		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		// Token: 0x06005F7D RID: 24445 RVA: 0x00309313 File Offset: 0x00307713
		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		// Token: 0x06005F7E RID: 24446 RVA: 0x00309334 File Offset: 0x00307734
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

		// Token: 0x06005F7F RID: 24447 RVA: 0x00309370 File Offset: 0x00307770
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

		// Token: 0x04003EC9 RID: 16073
		public Vector3 viewCenter;

		// Token: 0x04003ECA RID: 16074
		public float viewAngle;

		// Token: 0x04003ECB RID: 16075
		public bool invert;
	}
}
