using System;

namespace Verse.Noise
{
	// Token: 0x02000F78 RID: 3960
	public class Cylinders : ModuleBase
	{
		// Token: 0x06005F9B RID: 24475 RVA: 0x0030B37A File Offset: 0x0030977A
		public Cylinders() : base(0)
		{
		}

		// Token: 0x06005F9C RID: 24476 RVA: 0x0030B393 File Offset: 0x00309793
		public Cylinders(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06005F9D RID: 24477 RVA: 0x0030B3B4 File Offset: 0x003097B4
		// (set) Token: 0x06005F9E RID: 24478 RVA: 0x0030B3CF File Offset: 0x003097CF
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

		// Token: 0x06005F9F RID: 24479 RVA: 0x0030B3DC File Offset: 0x003097DC
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

		// Token: 0x04003ED8 RID: 16088
		private double m_frequency = 1.0;
	}
}
