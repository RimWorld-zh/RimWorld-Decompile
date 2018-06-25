using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7B RID: 3963
	public class ConvertToIsland : ModuleBase
	{
		// Token: 0x04003EDF RID: 16095
		public Vector3 viewCenter;

		// Token: 0x04003EE0 RID: 16096
		public float viewAngle;

		// Token: 0x04003EE1 RID: 16097
		private const float WaterLevel = -0.12f;

		// Token: 0x06005F9F RID: 24479 RVA: 0x0030BAD3 File Offset: 0x00309ED3
		public ConvertToIsland() : base(1)
		{
		}

		// Token: 0x06005FA0 RID: 24480 RVA: 0x0030BADD File Offset: 0x00309EDD
		public ConvertToIsland(Vector3 viewCenter, float viewAngle, ModuleBase input) : base(1)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.modules[0] = input;
		}

		// Token: 0x06005FA1 RID: 24481 RVA: 0x0030BB00 File Offset: 0x00309F00
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
	}
}
