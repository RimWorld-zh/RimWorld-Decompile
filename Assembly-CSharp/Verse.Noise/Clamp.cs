#define DEBUG
using System.Diagnostics;

namespace Verse.Noise
{
	public class Clamp : ModuleBase
	{
		private double m_min = -1.0;

		private double m_max = 1.0;

		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
			}
		}

		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
			}
		}

		public Clamp() : base(1)
		{
		}

		public Clamp(ModuleBase input) : base(1)
		{
			base.modules[0] = input;
		}

		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			base.modules[0] = input;
		}

		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(base.modules[0] != null);
			if (this.m_min > this.m_max)
			{
				double min = this.m_min;
				this.m_min = this.m_max;
				this.m_max = min;
			}
			double value = base.modules[0].GetValue(x, y, z);
			return (!(value < this.m_min)) ? ((!(value > this.m_max)) ? value : this.m_max) : this.m_min;
		}
	}
}
