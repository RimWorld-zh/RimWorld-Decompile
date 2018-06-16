using System;

namespace Verse.Noise
{
	// Token: 0x02000F7E RID: 3966
	public class Spheres : ModuleBase
	{
		// Token: 0x06005F9D RID: 24477 RVA: 0x003099C6 File Offset: 0x00307DC6
		public Spheres() : base(0)
		{
		}

		// Token: 0x06005F9E RID: 24478 RVA: 0x003099DF File Offset: 0x00307DDF
		public Spheres(double frequency) : base(0)
		{
			this.Frequency = frequency;
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06005F9F RID: 24479 RVA: 0x00309A00 File Offset: 0x00307E00
		// (set) Token: 0x06005FA0 RID: 24480 RVA: 0x00309A1B File Offset: 0x00307E1B
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

		// Token: 0x06005FA1 RID: 24481 RVA: 0x00309A28 File Offset: 0x00307E28
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

		// Token: 0x04003ED8 RID: 16088
		private double m_frequency = 1.0;
	}
}
