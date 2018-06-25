using System;

namespace Verse.Noise
{
	// Token: 0x02000F7A RID: 3962
	public class Const : ModuleBase
	{
		// Token: 0x04003EDE RID: 16094
		private double m_value = 0.0;

		// Token: 0x06005F9A RID: 24474 RVA: 0x0030BA57 File Offset: 0x00309E57
		public Const() : base(0)
		{
		}

		// Token: 0x06005F9B RID: 24475 RVA: 0x0030BA70 File Offset: 0x00309E70
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06005F9C RID: 24476 RVA: 0x0030BA90 File Offset: 0x00309E90
		// (set) Token: 0x06005F9D RID: 24477 RVA: 0x0030BAAB File Offset: 0x00309EAB
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

		// Token: 0x06005F9E RID: 24478 RVA: 0x0030BAB8 File Offset: 0x00309EB8
		public override double GetValue(double x, double y, double z)
		{
			return this.m_value;
		}
	}
}
