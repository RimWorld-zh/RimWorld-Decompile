using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8D RID: 3981
	public class Exponent : ModuleBase
	{
		// Token: 0x0600600C RID: 24588 RVA: 0x0030B9AE File Offset: 0x00309DAE
		public Exponent() : base(1)
		{
		}

		// Token: 0x0600600D RID: 24589 RVA: 0x0030B9C7 File Offset: 0x00309DC7
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600600E RID: 24590 RVA: 0x0030B9E9 File Offset: 0x00309DE9
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x0600600F RID: 24591 RVA: 0x0030BA14 File Offset: 0x00309E14
		// (set) Token: 0x06006010 RID: 24592 RVA: 0x0030BA2F File Offset: 0x00309E2F
		public double Value
		{
			get
			{
				return this.m_exponent;
			}
			set
			{
				this.m_exponent = value;
			}
		}

		// Token: 0x06006011 RID: 24593 RVA: 0x0030BA3C File Offset: 0x00309E3C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return Math.Pow(Math.Abs((value + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}

		// Token: 0x04003F07 RID: 16135
		private double m_exponent = 1.0;
	}
}
