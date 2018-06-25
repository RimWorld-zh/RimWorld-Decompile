using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000FA1 RID: 4001
	public class Terrace : ModuleBase
	{
		// Token: 0x04003F37 RID: 16183
		private List<double> m_data = new List<double>();

		// Token: 0x04003F38 RID: 16184
		private bool m_inverted = false;

		// Token: 0x0600608F RID: 24719 RVA: 0x0030F3F8 File Offset: 0x0030D7F8
		public Terrace() : base(1)
		{
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x0030F414 File Offset: 0x0030D814
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x0030F439 File Offset: 0x0030D839
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06006092 RID: 24722 RVA: 0x0030F468 File Offset: 0x0030D868
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06006093 RID: 24723 RVA: 0x0030F488 File Offset: 0x0030D888
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06006094 RID: 24724 RVA: 0x0030F4A4 File Offset: 0x0030D8A4
		// (set) Token: 0x06006095 RID: 24725 RVA: 0x0030F4BF File Offset: 0x0030D8BF
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

		// Token: 0x06006096 RID: 24726 RVA: 0x0030F4CC File Offset: 0x0030D8CC
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x06006097 RID: 24727 RVA: 0x0030F521 File Offset: 0x0030D921
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x0030F530 File Offset: 0x0030D930
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

		// Token: 0x06006099 RID: 24729 RVA: 0x0030F598 File Offset: 0x0030D998
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
