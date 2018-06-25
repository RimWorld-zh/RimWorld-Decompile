using System;

namespace Verse.Noise
{
	// Token: 0x02000F79 RID: 3961
	public class Const : ModuleBase
	{
		// Token: 0x04003ED6 RID: 16086
		private double m_value = 0.0;

		// Token: 0x06005F9A RID: 24474 RVA: 0x0030B813 File Offset: 0x00309C13
		public Const() : base(0)
		{
		}

		// Token: 0x06005F9B RID: 24475 RVA: 0x0030B82C File Offset: 0x00309C2C
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06005F9C RID: 24476 RVA: 0x0030B84C File Offset: 0x00309C4C
		// (set) Token: 0x06005F9D RID: 24477 RVA: 0x0030B867 File Offset: 0x00309C67
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

		// Token: 0x06005F9E RID: 24478 RVA: 0x0030B874 File Offset: 0x00309C74
		public override double GetValue(double x, double y, double z)
		{
			return this.m_value;
		}
	}
}
