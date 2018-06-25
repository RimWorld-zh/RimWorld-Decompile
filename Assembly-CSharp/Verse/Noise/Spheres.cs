using System;

namespace Verse.Noise
{
	// Token: 0x02000F82 RID: 3970
	public class Spheres : ModuleBase
	{
		// Token: 0x04003EF4 RID: 16116
		private double m_frequency = 1.0;

		// Token: 0x06005FCE RID: 24526 RVA: 0x0030C40A File Offset: 0x0030A80A
		public Spheres() : base(0)
		{
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x0030C423 File Offset: 0x0030A823
		public Spheres(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06005FD0 RID: 24528 RVA: 0x0030C444 File Offset: 0x0030A844
		// (set) Token: 0x06005FD1 RID: 24529 RVA: 0x0030C45F File Offset: 0x0030A85F
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

		// Token: 0x06005FD2 RID: 24530 RVA: 0x0030C46C File Offset: 0x0030A86C
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
