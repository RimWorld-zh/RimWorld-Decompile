using System;

namespace Verse.Noise
{
	// Token: 0x02000F78 RID: 3960
	public class Cylinders : ModuleBase
	{
		// Token: 0x06005F72 RID: 24434 RVA: 0x003092D6 File Offset: 0x003076D6
		public Cylinders() : base(0)
		{
		}

		// Token: 0x06005F73 RID: 24435 RVA: 0x003092EF File Offset: 0x003076EF
		public Cylinders(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06005F74 RID: 24436 RVA: 0x00309310 File Offset: 0x00307710
		// (set) Token: 0x06005F75 RID: 24437 RVA: 0x0030932B File Offset: 0x0030772B
		public double Frequency
		{
			get
			{
				return this.m_frequency;
			}
			set
			{
				this.m_frequency = value;
			}
		}

		// Token: 0x06005F76 RID: 24438 RVA: 0x00309338 File Offset: 0x00307738
		public override double GetValue(double x, double y, double z)
		{
			x *= this.m_frequency;
			z *= this.m_frequency;
			double num = Math.Sqrt(x * x + z * z);
			double num2 = num - Math.Floor(num);
			double val = 1.0 - num2;
			double num3 = Math.Min(num2, val);
			return 1.0 - num3 * 4.0;
		}

		// Token: 0x04003EC6 RID: 16070
		private double m_frequency = 1.0;
	}
}
