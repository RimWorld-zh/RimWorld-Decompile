using System;

namespace Verse.Noise
{
	// Token: 0x02000F81 RID: 3969
	public class Spheres : ModuleBase
	{
		// Token: 0x04003EEC RID: 16108
		private double m_frequency = 1.0;

		// Token: 0x06005FCE RID: 24526 RVA: 0x0030C1C6 File Offset: 0x0030A5C6
		public Spheres() : base(0)
		{
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x0030C1DF File Offset: 0x0030A5DF
		public Spheres(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06005FD0 RID: 24528 RVA: 0x0030C200 File Offset: 0x0030A600
		// (set) Token: 0x06005FD1 RID: 24529 RVA: 0x0030C21B File Offset: 0x0030A61B
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

		// Token: 0x06005FD2 RID: 24530 RVA: 0x0030C228 File Offset: 0x0030A628
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
	}
}
