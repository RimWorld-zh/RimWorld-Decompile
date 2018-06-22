using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F89 RID: 3977
	public class Curve : ModuleBase
	{
		// Token: 0x06006020 RID: 24608 RVA: 0x0030D6A5 File Offset: 0x0030BAA5
		public Curve() : base(1)
		{
		}

		// Token: 0x06006021 RID: 24609 RVA: 0x0030D6BA File Offset: 0x0030BABA
		public Curve(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06006022 RID: 24610 RVA: 0x0030D6D8 File Offset: 0x0030BAD8
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06006023 RID: 24611 RVA: 0x0030D6F8 File Offset: 0x0030BAF8
		public List<KeyValuePair<double, double>> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x06006024 RID: 24612 RVA: 0x0030D714 File Offset: 0x0030BB14
		public void Add(double input, double output)
		{
			KeyValuePair<double, double> item = new KeyValuePair<double, double>(input, output);
			if (!this.m_data.Contains(item))
			{
				this.m_data.Add(item);
			}
			this.m_data.Sort((KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs) => lhs.Key.CompareTo(rhs.Key));
		}

		// Token: 0x06006025 RID: 24613 RVA: 0x0030D772 File Offset: 0x0030BB72
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06006026 RID: 24614 RVA: 0x0030D780 File Offset: 0x0030BB80
		public override double GetValue(double x, double y, double z)
		{
			System.Diagnostics.Debug.Assert(this.modules[0] != null);
			System.Diagnostics.Debug.Assert(this.ControlPointCount >= 4);
			double value = this.modules[0].GetValue(x, y, z);
			int i;
			for (i = 0; i < this.m_data.Count; i++)
			{
				if (value < this.m_data[i].Key)
				{
					break;
				}
			}
			int index = Mathf.Clamp(i - 2, 0, this.m_data.Count - 1);
			int num = Mathf.Clamp(i - 1, 0, this.m_data.Count - 1);
			int num2 = Mathf.Clamp(i, 0, this.m_data.Count - 1);
			int index2 = Mathf.Clamp(i + 1, 0, this.m_data.Count - 1);
			double result;
			if (num == num2)
			{
				result = this.m_data[num].Value;
			}
			else
			{
				double key = this.m_data[num].Key;
				double key2 = this.m_data[num2].Key;
				double position = (value - key) / (key2 - key);
				result = Utils.InterpolateCubic(this.m_data[index].Value, this.m_data[num].Value, this.m_data[num2].Value, this.m_data[index2].Value, position);
			}
			return result;
		}

		// Token: 0x04003F15 RID: 16149
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();
	}
}
