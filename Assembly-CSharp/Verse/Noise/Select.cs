using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9D RID: 3997
	public class Select : ModuleBase
	{
		// Token: 0x04003F31 RID: 16177
		private double m_fallOff = 0.0;

		// Token: 0x04003F32 RID: 16178
		private double m_raw = 0.0;

		// Token: 0x04003F33 RID: 16179
		private double m_min = -1.0;

		// Token: 0x04003F34 RID: 16180
		private double m_max = 1.0;

		// Token: 0x0600607B RID: 24699 RVA: 0x0030EED4 File Offset: 0x0030D2D4
		public Select() : base(3)
		{
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x0030EF28 File Offset: 0x0030D328
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x0030EF94 File Offset: 0x0030D394
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x0600607E RID: 24702 RVA: 0x0030EFB8 File Offset: 0x0030D3B8
		// (set) Token: 0x0600607F RID: 24703 RVA: 0x0030EFD5 File Offset: 0x0030D3D5
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
		// (get) Token: 0x06006080 RID: 24704 RVA: 0x0030EFF0 File Offset: 0x0030D3F0
		// (set) Token: 0x06006081 RID: 24705 RVA: 0x0030F00C File Offset: 0x0030D40C
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
		// (get) Token: 0x06006082 RID: 24706 RVA: 0x0030F058 File Offset: 0x0030D458
		// (set) Token: 0x06006083 RID: 24707 RVA: 0x0030F073 File Offset: 0x0030D473
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
		// (get) Token: 0x06006084 RID: 24708 RVA: 0x0030F08C File Offset: 0x0030D48C
		// (set) Token: 0x06006085 RID: 24709 RVA: 0x0030F0A7 File Offset: 0x0030D4A7
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

		// Token: 0x06006086 RID: 24710 RVA: 0x0030F0BD File Offset: 0x0030D4BD
		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		// Token: 0x06006087 RID: 24711 RVA: 0x0030F0E4 File Offset: 0x0030D4E4
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
