using System;

namespace Verse.Noise
{
	// Token: 0x02000F7D RID: 3965
	public class Spheres : ModuleBase
	{
		// Token: 0x06005FC4 RID: 24516 RVA: 0x0030BB46 File Offset: 0x00309F46
		public Spheres() : base(0)
		{
		}

		// Token: 0x06005FC5 RID: 24517 RVA: 0x0030BB5F File Offset: 0x00309F5F
		public Spheres(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06005FC6 RID: 24518 RVA: 0x0030BB80 File Offset: 0x00309F80
		// (set) Token: 0x06005FC7 RID: 24519 RVA: 0x0030BB9B File Offset: 0x00309F9B
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

		// Token: 0x06005FC8 RID: 24520 RVA: 0x0030BBA8 File Offset: 0x00309FA8
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

		// Token: 0x04003EE9 RID: 16105
		private double m_frequency = 1.0;
	}
}
