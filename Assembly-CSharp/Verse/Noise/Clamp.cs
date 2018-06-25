using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8C RID: 3980
	public class Clamp : ModuleBase
	{
		// Token: 0x04003F16 RID: 16150
		private double m_min = -1.0;

		// Token: 0x04003F17 RID: 16151
		private double m_max = 1.0;

		// Token: 0x06006021 RID: 24609 RVA: 0x0030DB89 File Offset: 0x0030BF89
		public Clamp() : base(1)
		{
		}

		// Token: 0x06006022 RID: 24610 RVA: 0x0030DBB1 File Offset: 0x0030BFB1
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006023 RID: 24611 RVA: 0x0030DBE2 File Offset: 0x0030BFE2
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06006024 RID: 24612 RVA: 0x0030DC24 File Offset: 0x0030C024
		// (set) Token: 0x06006025 RID: 24613 RVA: 0x0030DC3F File Offset: 0x0030C03F
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
		// (get) Token: 0x06006026 RID: 24614 RVA: 0x0030DC4C File Offset: 0x0030C04C
		// (set) Token: 0x06006027 RID: 24615 RVA: 0x0030DC67 File Offset: 0x0030C067
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

		// Token: 0x06006028 RID: 24616 RVA: 0x0030DC71 File Offset: 0x0030C071
		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x06006029 RID: 24617 RVA: 0x0030DC8C File Offset: 0x0030C08C
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
