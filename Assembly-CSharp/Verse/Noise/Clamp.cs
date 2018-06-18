using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F88 RID: 3976
	public class Clamp : ModuleBase
	{
		// Token: 0x06005FEE RID: 24558 RVA: 0x0030B465 File Offset: 0x00309865
		public Clamp() : base(1)
		{
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x0030B48D File Offset: 0x0030988D
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x0030B4BE File Offset: 0x003098BE
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06005FF1 RID: 24561 RVA: 0x0030B500 File Offset: 0x00309900
		// (set) Token: 0x06005FF2 RID: 24562 RVA: 0x0030B51B File Offset: 0x0030991B
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

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06005FF3 RID: 24563 RVA: 0x0030B528 File Offset: 0x00309928
		// (set) Token: 0x06005FF4 RID: 24564 RVA: 0x0030B543 File Offset: 0x00309943
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

		// Token: 0x06005FF5 RID: 24565 RVA: 0x0030B54D File Offset: 0x0030994D
		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x06005FF6 RID: 24566 RVA: 0x0030B568 File Offset: 0x00309968
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

		// Token: 0x04003F01 RID: 16129
		private double m_min = -1.0;

		// Token: 0x04003F02 RID: 16130
		private double m_max = 1.0;
	}
}
