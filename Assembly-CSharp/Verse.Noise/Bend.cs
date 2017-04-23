using System;

namespace Verse.Noise
{
	public class Bend : ModuleBase
	{
		private float m_angle;

		private float m_radius;

		public Bend() : base(1)
		{
		}

		public Bend(float angle, float radius, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.m_angle = angle;
			this.m_radius = radius;
		}

		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z);
		}
	}
}
