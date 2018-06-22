using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F76 RID: 3958
	public class ConvertToIsland : ModuleBase
	{
		// Token: 0x06005F95 RID: 24469 RVA: 0x0030B20F File Offset: 0x0030960F
		public ConvertToIsland() : base(1)
		{
		}

		// Token: 0x06005F96 RID: 24470 RVA: 0x0030B219 File Offset: 0x00309619
		public ConvertToIsland(Vector3 viewCenter, float viewAngle, ModuleBase input) : base(1)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.modules[0] = input;
		}

		// Token: 0x06005F97 RID: 24471 RVA: 0x0030B23C File Offset: 0x0030963C
		public override double GetValue(double x, double y, double z)
		{
			float num = Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z));
			double value = this.modules[0].GetValue(x, y, z);
			float num2 = Mathf.Max(2.5f, this.viewAngle * 0.25f);
			float num3 = Mathf.Max(0.8f, this.viewAngle * 0.1f);
			double result;
			if (num < this.viewAngle - num2)
			{
				result = value;
			}
			else
			{
				float num4 = GenMath.LerpDouble(this.viewAngle - num2, this.viewAngle - num3, 0f, 0.62f, num);
				if (value > -0.11999999731779099)
				{
					result = (value - -0.11999999731779099) * (double)(1f - num4 * 0.7f) - (double)(num4 * 0.3f) + -0.11999999731779099;
				}
				else
				{
					result = value - (double)(num4 * 0.3f);
				}
			}
			return result;
		}

		// Token: 0x04003ED4 RID: 16084
		public Vector3 viewCenter;

		// Token: 0x04003ED5 RID: 16085
		public float viewAngle;

		// Token: 0x04003ED6 RID: 16086
		private const float WaterLevel = -0.12f;
	}
}
