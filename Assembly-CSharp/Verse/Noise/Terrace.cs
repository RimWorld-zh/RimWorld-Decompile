using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F9D RID: 3997
	public class Terrace : ModuleBase
	{
		// Token: 0x04003F34 RID: 16180
		private List<double> m_data = new List<double>();

		// Token: 0x04003F35 RID: 16181
		private bool m_inverted = false;

		// Token: 0x06006085 RID: 24709 RVA: 0x0030ED78 File Offset: 0x0030D178
		public Terrace() : base(1)
		{
		}

		// Token: 0x06006086 RID: 24710 RVA: 0x0030ED94 File Offset: 0x0030D194
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006087 RID: 24711 RVA: 0x0030EDB9 File Offset: 0x0030D1B9
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06006088 RID: 24712 RVA: 0x0030EDE8 File Offset: 0x0030D1E8
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06006089 RID: 24713 RVA: 0x0030EE08 File Offset: 0x0030D208
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x0600608A RID: 24714 RVA: 0x0030EE24 File Offset: 0x0030D224
		// (set) Token: 0x0600608B RID: 24715 RVA: 0x0030EE3F File Offset: 0x0030D23F
		public bool IsInverted
		{
			get
			{
				return this.m_inverted;
			}
			set
			{
				this.m_inverted = value;
			}
		}

		// Token: 0x0600608C RID: 24716 RVA: 0x0030EE4C File Offset: 0x0030D24C
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x0030EEA1 File Offset: 0x0030D2A1
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x0030EEB0 File Offset: 0x0030D2B0
		public void Generate(int steps)
		{
			if (steps < 2)
			{
				throw new ArgumentException("Need at least two steps");
			}
			this.Clear();
			double num = 2.0 / ((double)steps - 1.0);
			double num2 = -1.0;
			for (int i = 0; i < steps; i++)
			{
				this.Add(num2);
				num2 += num;
			}
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x0030EF18 File Offset: 0x0030D318
		public override double GetValue(double x, double y, double z)
		{
			System.Diagnostics.Debug.Assert(this.modules[0] != null);
			System.Diagnostics.Debug.Assert(this.ControlPointCount >= 2);
			double value = this.modules[0].GetValue(x, y, z);
			int i;
			for (i = 0; i < this.m_data.Count; i++)
			{
				if (value < this.m_data[i])
				{
					break;
				}
			}
			int num = Mathf.Clamp(i - 1, 0, this.m_data.Count - 1);
			int num2 = Mathf.Clamp(i, 0, this.m_data.Count - 1);
			double result;
			if (num == num2)
			{
				result = this.m_data[num2];
			}
			else
			{
				double num3 = this.m_data[num];
				double num4 = this.m_data[num2];
				double num5 = (value - num3) / (num4 - num3);
				if (this.m_inverted)
				{
					num5 = 1.0 - num5;
					double num6 = num3;
					num3 = num4;
					num4 = num6;
				}
				num5 *= num5;
				result = Utils.InterpolateLinear(num3, num4, num5);
			}
			return result;
		}
	}
}
