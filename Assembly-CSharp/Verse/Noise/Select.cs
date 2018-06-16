using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9A RID: 3994
	public class Select : ModuleBase
	{
		// Token: 0x0600604A RID: 24650 RVA: 0x0030C6D4 File Offset: 0x0030AAD4
		public Select() : base(3)
		{
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x0030C728 File Offset: 0x0030AB28
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x0030C794 File Offset: 0x0030AB94
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x0600604D RID: 24653 RVA: 0x0030C7B8 File Offset: 0x0030ABB8
		// (set) Token: 0x0600604E RID: 24654 RVA: 0x0030C7D5 File Offset: 0x0030ABD5
		public ModuleBase Controller
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				Debug.Assert(value != null);
				this.modules[2] = value;
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x0600604F RID: 24655 RVA: 0x0030C7F0 File Offset: 0x0030ABF0
		// (set) Token: 0x06006050 RID: 24656 RVA: 0x0030C80C File Offset: 0x0030AC0C
		public double FallOff
		{
			get
			{
				return this.m_fallOff;
			}
			set
			{
				double num = this.m_max - this.m_min;
				this.m_raw = value;
				this.m_fallOff = ((value <= num / 2.0) ? value : (num / 2.0));
			}
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06006051 RID: 24657 RVA: 0x0030C858 File Offset: 0x0030AC58
		// (set) Token: 0x06006052 RID: 24658 RVA: 0x0030C873 File Offset: 0x0030AC73
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06006053 RID: 24659 RVA: 0x0030C88C File Offset: 0x0030AC8C
		// (set) Token: 0x06006054 RID: 24660 RVA: 0x0030C8A7 File Offset: 0x0030ACA7
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x06006055 RID: 24661 RVA: 0x0030C8BD File Offset: 0x0030ACBD
		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x0030C8E4 File Offset: 0x0030ACE4
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			Debug.Assert(this.modules[2] != null);
			double value = this.modules[2].GetValue(x, y, z);
			double result;
			if (this.m_fallOff > 0.0)
			{
				if (value < this.m_min - this.m_fallOff)
				{
					result = this.modules[0].GetValue(x, y, z);
				}
				else if (value < this.m_min + this.m_fallOff)
				{
					double num = this.m_min - this.m_fallOff;
					double num2 = this.m_min + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num) / (num2 - num));
					result = Utils.InterpolateLinear(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z), position);
				}
				else if (value < this.m_max - this.m_fallOff)
				{
					result = this.modules[1].GetValue(x, y, z);
				}
				else if (value < this.m_max + this.m_fallOff)
				{
					double num3 = this.m_max - this.m_fallOff;
					double num4 = this.m_max + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num3) / (num4 - num3));
					result = Utils.InterpolateLinear(this.modules[1].GetValue(x, y, z), this.modules[0].GetValue(x, y, z), position);
				}
				else
				{
					result = this.modules[0].GetValue(x, y, z);
				}
			}
			else if (value < this.m_min || value > this.m_max)
			{
				result = this.modules[0].GetValue(x, y, z);
			}
			else
			{
				result = this.modules[1].GetValue(x, y, z);
			}
			return result;
		}

		// Token: 0x04003F1D RID: 16157
		private double m_fallOff = 0.0;

		// Token: 0x04003F1E RID: 16158
		private double m_raw = 0.0;

		// Token: 0x04003F1F RID: 16159
		private double m_min = -1.0;

		// Token: 0x04003F20 RID: 16160
		private double m_max = 1.0;
	}
}
