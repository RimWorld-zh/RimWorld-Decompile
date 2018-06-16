using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F89 RID: 3977
	public class Clamp : ModuleBase
	{
		// Token: 0x06005FF0 RID: 24560 RVA: 0x0030B389 File Offset: 0x00309789
		public Clamp() : base(1)
		{
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x0030B3B1 File Offset: 0x003097B1
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x0030B3E2 File Offset: 0x003097E2
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06005FF3 RID: 24563 RVA: 0x0030B424 File Offset: 0x00309824
		// (set) Token: 0x06005FF4 RID: 24564 RVA: 0x0030B43F File Offset: 0x0030983F
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

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06005FF5 RID: 24565 RVA: 0x0030B44C File Offset: 0x0030984C
		// (set) Token: 0x06005FF6 RID: 24566 RVA: 0x0030B467 File Offset: 0x00309867
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

		// Token: 0x06005FF7 RID: 24567 RVA: 0x0030B471 File Offset: 0x00309871
		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x0030B48C File Offset: 0x0030988C
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

		// Token: 0x04003F02 RID: 16130
		private double m_min = -1.0;

		// Token: 0x04003F03 RID: 16131
		private double m_max = 1.0;
	}
}
