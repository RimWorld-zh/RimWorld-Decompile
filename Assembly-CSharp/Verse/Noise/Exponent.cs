using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8C RID: 3980
	public class Exponent : ModuleBase
	{
		// Token: 0x0600600A RID: 24586 RVA: 0x0030BA8A File Offset: 0x00309E8A
		public Exponent() : base(1)
		{
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x0030BAA3 File Offset: 0x00309EA3
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x0030BAC5 File Offset: 0x00309EC5
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x0600600D RID: 24589 RVA: 0x0030BAF0 File Offset: 0x00309EF0
		// (set) Token: 0x0600600E RID: 24590 RVA: 0x0030BB0B File Offset: 0x00309F0B
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

		// Token: 0x0600600F RID: 24591 RVA: 0x0030BB18 File Offset: 0x00309F18
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return Math.Pow(Math.Abs((value + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}

		// Token: 0x04003F06 RID: 16134
		private double m_exponent = 1.0;
	}
}
