using System;

namespace Verse.Noise
{
	// Token: 0x02000F76 RID: 3958
	public class Const : ModuleBase
	{
		// Token: 0x06005F69 RID: 24425 RVA: 0x00309013 File Offset: 0x00307413
		public Const() : base(0)
		{
		}

		// Token: 0x06005F6A RID: 24426 RVA: 0x0030902C File Offset: 0x0030742C
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06005F6B RID: 24427 RVA: 0x0030904C File Offset: 0x0030744C
		// (set) Token: 0x06005F6C RID: 24428 RVA: 0x00309067 File Offset: 0x00307467
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

		// Token: 0x06005F6D RID: 24429 RVA: 0x00309074 File Offset: 0x00307474
		public override double GetValue(double x, double y, double z)
		{
			return this.m_value;
		}

		// Token: 0x04003EC2 RID: 16066
		private double m_value = 0.0;
	}
}
