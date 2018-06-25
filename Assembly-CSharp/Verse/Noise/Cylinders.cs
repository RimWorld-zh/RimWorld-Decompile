using System;

namespace Verse.Noise
{
	// Token: 0x02000F7D RID: 3965
	public class Cylinders : ModuleBase
	{
		// Token: 0x04003EE3 RID: 16099
		private double m_frequency = 1.0;

		// Token: 0x06005FA5 RID: 24485 RVA: 0x0030BC3E File Offset: 0x0030A03E
		public Cylinders() : base(0)
		{
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x0030BC57 File Offset: 0x0030A057
		public Cylinders(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06005FA7 RID: 24487 RVA: 0x0030BC78 File Offset: 0x0030A078
		// (set) Token: 0x06005FA8 RID: 24488 RVA: 0x0030BC93 File Offset: 0x0030A093
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

		// Token: 0x06005FA9 RID: 24489 RVA: 0x0030BCA0 File Offset: 0x0030A0A0
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
	}
}
