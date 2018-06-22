using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F88 RID: 3976
	public class Clamp : ModuleBase
	{
		// Token: 0x06006017 RID: 24599 RVA: 0x0030D509 File Offset: 0x0030B909
		public Clamp() : base(1)
		{
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x0030D531 File Offset: 0x0030B931
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006019 RID: 24601 RVA: 0x0030D562 File Offset: 0x0030B962
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x0600601A RID: 24602 RVA: 0x0030D5A4 File Offset: 0x0030B9A4
		// (set) Token: 0x0600601B RID: 24603 RVA: 0x0030D5BF File Offset: 0x0030B9BF
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
			}
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x0600601C RID: 24604 RVA: 0x0030D5CC File Offset: 0x0030B9CC
		// (set) Token: 0x0600601D RID: 24605 RVA: 0x0030D5E7 File Offset: 0x0030B9E7
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
			}
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x0030D5F1 File Offset: 0x0030B9F1
		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x0600601F RID: 24607 RVA: 0x0030D60C File Offset: 0x0030BA0C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			if (this.m_min > this.m_max)
			{
				double min = this.m_min;
				this.m_min = this.m_max;
				this.m_max = min;
			}
			double value = this.modules[0].GetValue(x, y, z);
			double result;
			if (value < this.m_min)
			{
				result = this.m_min;
			}
			else if (value > this.m_max)
			{
				result = this.m_max;
			}
			else
			{
				result = value;
			}
			return result;
		}

		// Token: 0x04003F13 RID: 16147
		private double m_min = -1.0;

		// Token: 0x04003F14 RID: 16148
		private double m_max = 1.0;
	}
}
