using UnityEngine;

namespace Verse.Noise
{
	public class ConvertToIsland : ModuleBase
	{
		private const float WaterLevel = -0.12f;

		public Vector3 viewCenter;

		public float viewAngle;

		public ConvertToIsland() : base(1)
		{
		}

		public ConvertToIsland(Vector3 viewCenter, float viewAngle, ModuleBase input) : base(1)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			base.modules[0] = input;
		}

		public override double GetValue(double x, double y, double z)
		{
			float num = Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z));
			double value = base.modules[0].GetValue(x, y, z);
			float num2 = Mathf.Max(2.5f, (float)(this.viewAngle * 0.25));
			float num3 = Mathf.Max(0.8f, (float)(this.viewAngle * 0.10000000149011612));
			if (num < this.viewAngle - num2)
			{
				return value;
			}
			float num4 = GenMath.LerpDouble(this.viewAngle - num2, this.viewAngle - num3, 0f, 0.62f, num);
			if (value > -0.11999999731779099)
			{
				return (value - -0.11999999731779099) * (1.0 - num4 * 0.699999988079071) - num4 * 0.30000001192092896 + -0.11999999731779099;
			}
			return value - num4 * 0.30000001192092896;
		}
	}
}
