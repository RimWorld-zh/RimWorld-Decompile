using System;

namespace Verse.Noise
{
	// Token: 0x02000F75 RID: 3957
	public class Const : ModuleBase
	{
		// Token: 0x06005F67 RID: 24423 RVA: 0x003090EF File Offset: 0x003074EF
		public Const() : base(0)
		{
		}

		// Token: 0x06005F68 RID: 24424 RVA: 0x00309108 File Offset: 0x00307508
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06005F69 RID: 24425 RVA: 0x00309128 File Offset: 0x00307528
		// (set) Token: 0x06005F6A RID: 24426 RVA: 0x00309143 File Offset: 0x00307543
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

		// Token: 0x06005F6B RID: 24427 RVA: 0x00309150 File Offset: 0x00307550
		public override double GetValue(double x, double y, double z)
		{
			return this.m_value;
		}

		// Token: 0x04003EC1 RID: 16065
		private double m_value = 0.0;
	}
}
