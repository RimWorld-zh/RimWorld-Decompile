using System;
using System.Diagnostics;

namespace Verse.Noise
{
	public class Cache : ModuleBase
	{
		private double m_value = 0.0;

		private bool m_cached = false;

		private double m_x = 0.0;

		private double m_y = 0.0;

		private double m_z = 0.0;

		public Cache() : base(1)
		{
		}

		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		public override ModuleBase this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				base[index] = value;
				this.m_cached = false;
			}
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			if (!this.m_cached || this.m_x != x || this.m_y != y || this.m_z != z)
			{
				this.m_value = this.modules[0].GetValue(x, y, z);
				this.m_x = x;
				this.m_y = y;
				this.m_z = z;
			}
			this.m_cached = true;
			return this.m_value;
		}
	}
}
