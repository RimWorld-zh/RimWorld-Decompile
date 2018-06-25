using System;

namespace Verse.Noise
{
	// Token: 0x02000F7C RID: 3964
	public class Cylinders : ModuleBase
	{
		// Token: 0x04003EDB RID: 16091
		private double m_frequency = 1.0;

		// Token: 0x06005FA5 RID: 24485 RVA: 0x0030B9FA File Offset: 0x00309DFA
		public Cylinders() : base(0)
		{
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x0030BA13 File Offset: 0x00309E13
		public Cylinders(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06005FA7 RID: 24487 RVA: 0x0030BA34 File Offset: 0x00309E34
		// (set) Token: 0x06005FA8 RID: 24488 RVA: 0x0030BA4F File Offset: 0x00309E4F
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

		// Token: 0x06005FA9 RID: 24489 RVA: 0x0030BA5C File Offset: 0x00309E5C
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
