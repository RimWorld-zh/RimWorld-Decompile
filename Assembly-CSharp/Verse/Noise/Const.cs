using System;

namespace Verse.Noise
{
	// Token: 0x02000F75 RID: 3957
	public class Const : ModuleBase
	{
		// Token: 0x06005F90 RID: 24464 RVA: 0x0030B193 File Offset: 0x00309593
		public Const() : base(0)
		{
		}

		// Token: 0x06005F91 RID: 24465 RVA: 0x0030B1AC File Offset: 0x003095AC
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06005F92 RID: 24466 RVA: 0x0030B1CC File Offset: 0x003095CC
		// (set) Token: 0x06005F93 RID: 24467 RVA: 0x0030B1E7 File Offset: 0x003095E7
		public double Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		// Token: 0x06005F94 RID: 24468 RVA: 0x0030B1F4 File Offset: 0x003095F4
		public override double GetValue(double x, double y, double z)
		{
			return this.m_value;
		}

		// Token: 0x04003ED3 RID: 16083
		private double m_value = 0.0;
	}
}
