using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9E RID: 3998
	public class Select : ModuleBase
	{
		// Token: 0x04003F39 RID: 16185
		private double m_fallOff = 0.0;

		// Token: 0x04003F3A RID: 16186
		private double m_raw = 0.0;

		// Token: 0x04003F3B RID: 16187
		private double m_min = -1.0;

		// Token: 0x04003F3C RID: 16188
		private double m_max = 1.0;

		// Token: 0x0600607B RID: 24699 RVA: 0x0030F118 File Offset: 0x0030D518
		public Select() : base(3)
		{
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x0030F16C File Offset: 0x0030D56C
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x0030F1D8 File Offset: 0x0030D5D8
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x0600607E RID: 24702 RVA: 0x0030F1FC File Offset: 0x0030D5FC
		// (set) Token: 0x0600607F RID: 24703 RVA: 0x0030F219 File Offset: 0x0030D619
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

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06006080 RID: 24704 RVA: 0x0030F234 File Offset: 0x0030D634
		// (set) Token: 0x06006081 RID: 24705 RVA: 0x0030F250 File Offset: 0x0030D650
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

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06006082 RID: 24706 RVA: 0x0030F29C File Offset: 0x0030D69C
		// (set) Token: 0x06006083 RID: 24707 RVA: 0x0030F2B7 File Offset: 0x0030D6B7
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

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06006084 RID: 24708 RVA: 0x0030F2D0 File Offset: 0x0030D6D0
		// (set) Token: 0x06006085 RID: 24709 RVA: 0x0030F2EB File Offset: 0x0030D6EB
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

		// Token: 0x06006086 RID: 24710 RVA: 0x0030F301 File Offset: 0x0030D701
		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		// Token: 0x06006087 RID: 24711 RVA: 0x0030F328 File Offset: 0x0030D728
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
	}
}
