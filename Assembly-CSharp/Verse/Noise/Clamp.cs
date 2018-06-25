using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8D RID: 3981
	public class Clamp : ModuleBase
	{
		// Token: 0x04003F1E RID: 16158
		private double m_min = -1.0;

		// Token: 0x04003F1F RID: 16159
		private double m_max = 1.0;

		// Token: 0x06006021 RID: 24609 RVA: 0x0030DDCD File Offset: 0x0030C1CD
		public Clamp() : base(1)
		{
		}

		// Token: 0x06006022 RID: 24610 RVA: 0x0030DDF5 File Offset: 0x0030C1F5
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006023 RID: 24611 RVA: 0x0030DE26 File Offset: 0x0030C226
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06006024 RID: 24612 RVA: 0x0030DE68 File Offset: 0x0030C268
		// (set) Token: 0x06006025 RID: 24613 RVA: 0x0030DE83 File Offset: 0x0030C283
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

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06006026 RID: 24614 RVA: 0x0030DE90 File Offset: 0x0030C290
		// (set) Token: 0x06006027 RID: 24615 RVA: 0x0030DEAB File Offset: 0x0030C2AB
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

		// Token: 0x06006028 RID: 24616 RVA: 0x0030DEB5 File Offset: 0x0030C2B5
		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x06006029 RID: 24617 RVA: 0x0030DED0 File Offset: 0x0030C2D0
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			if (this.m_min > this.m_max)
			{
				double min = this.m_min;
				this.m_min = this.m_max;
				this.m_max = min;
			}
			double value = this.modules[0].GetValue(x, y, z);
			double result;
			if (value < this.m_min)
			{
				result = this.m_min;
			}
			else if (value > this.m_max)
			{
				result = this.m_max;
			}
			else
			{
				result = value;
			}
			return result;
		}
	}
}
