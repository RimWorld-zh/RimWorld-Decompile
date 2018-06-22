using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F99 RID: 3993
	public class Select : ModuleBase
	{
		// Token: 0x06006071 RID: 24689 RVA: 0x0030E854 File Offset: 0x0030CC54
		public Select() : base(3)
		{
		}

		// Token: 0x06006072 RID: 24690 RVA: 0x0030E8A8 File Offset: 0x0030CCA8
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		// Token: 0x06006073 RID: 24691 RVA: 0x0030E914 File Offset: 0x0030CD14
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06006074 RID: 24692 RVA: 0x0030E938 File Offset: 0x0030CD38
		// (set) Token: 0x06006075 RID: 24693 RVA: 0x0030E955 File Offset: 0x0030CD55
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

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06006076 RID: 24694 RVA: 0x0030E970 File Offset: 0x0030CD70
		// (set) Token: 0x06006077 RID: 24695 RVA: 0x0030E98C File Offset: 0x0030CD8C
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

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x0030E9D8 File Offset: 0x0030CDD8
		// (set) Token: 0x06006079 RID: 24697 RVA: 0x0030E9F3 File Offset: 0x0030CDF3
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

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x0600607A RID: 24698 RVA: 0x0030EA0C File Offset: 0x0030CE0C
		// (set) Token: 0x0600607B RID: 24699 RVA: 0x0030EA27 File Offset: 0x0030CE27
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

		// Token: 0x0600607C RID: 24700 RVA: 0x0030EA3D File Offset: 0x0030CE3D
		public void SetBounds(double min, double max)
		{
			Debug.Assert(min < max);
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x0030EA64 File Offset: 0x0030CE64
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

		// Token: 0x04003F2E RID: 16174
		private double m_fallOff = 0.0;

		// Token: 0x04003F2F RID: 16175
		private double m_raw = 0.0;

		// Token: 0x04003F30 RID: 16176
		private double m_min = -1.0;

		// Token: 0x04003F31 RID: 16177
		private double m_max = 1.0;
	}
}
