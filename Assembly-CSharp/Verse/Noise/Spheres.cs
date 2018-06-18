using System;

namespace Verse.Noise
{
	// Token: 0x02000F7D RID: 3965
	public class Spheres : ModuleBase
	{
		// Token: 0x06005F9B RID: 24475 RVA: 0x00309AA2 File Offset: 0x00307EA2
		public Spheres() : base(0)
		{
		}

		// Token: 0x06005F9C RID: 24476 RVA: 0x00309ABB File Offset: 0x00307EBB
		public Spheres(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06005F9D RID: 24477 RVA: 0x00309ADC File Offset: 0x00307EDC
		// (set) Token: 0x06005F9E RID: 24478 RVA: 0x00309AF7 File Offset: 0x00307EF7
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

		// Token: 0x06005F9F RID: 24479 RVA: 0x00309B04 File Offset: 0x00307F04
		public override double GetValue(double x, double y, double z)
		{
			x *= this.m_frequency;
			y *= this.m_frequency;
			z *= this.m_frequency;
			double num = Math.Sqrt(x * x + y * y + z * z);
			double num2 = num - Math.Floor(num);
			double val = 1.0 - num2;
			double num3 = Math.Min(num2, val);
			return 1.0 - num3 * 4.0;
		}

		// Token: 0x04003ED7 RID: 16087
		private double m_frequency = 1.0;
	}
}
