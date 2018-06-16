using System;

namespace Verse.Noise
{
	// Token: 0x02000F79 RID: 3961
	public class Cylinders : ModuleBase
	{
		// Token: 0x06005F74 RID: 24436 RVA: 0x003091FA File Offset: 0x003075FA
		public Cylinders() : base(0)
		{
		}

		// Token: 0x06005F75 RID: 24437 RVA: 0x00309213 File Offset: 0x00307613
		public Cylinders(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06005F76 RID: 24438 RVA: 0x00309234 File Offset: 0x00307634
		// (set) Token: 0x06005F77 RID: 24439 RVA: 0x0030924F File Offset: 0x0030764F
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

		// Token: 0x06005F78 RID: 24440 RVA: 0x0030925C File Offset: 0x0030765C
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

		// Token: 0x04003EC7 RID: 16071
		private double m_frequency = 1.0;
	}
}
