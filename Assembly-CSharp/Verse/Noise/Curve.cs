using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F8D RID: 3981
	public class Curve : ModuleBase
	{
		// Token: 0x04003F18 RID: 16152
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();

		// Token: 0x0600602A RID: 24618 RVA: 0x0030DD25 File Offset: 0x0030C125
		public Curve() : base(1)
		{
		}

		// Token: 0x0600602B RID: 24619 RVA: 0x0030DD3A File Offset: 0x0030C13A
		public Curve(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x0600602C RID: 24620 RVA: 0x0030DD58 File Offset: 0x0030C158
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x0600602D RID: 24621 RVA: 0x0030DD78 File Offset: 0x0030C178
		public List<KeyValuePair<double, double>> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x0600602E RID: 24622 RVA: 0x0030DD94 File Offset: 0x0030C194
		public void Add(double input, double output)
		{
			KeyValuePair<double, double> item = new KeyValuePair<double, double>(input, output);
			if (!this.m_data.Contains(item))
			{
				this.m_data.Add(item);
			}
			this.m_data.Sort((KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs) => lhs.Key.CompareTo(rhs.Key));
		}

		// Token: 0x0600602F RID: 24623 RVA: 0x0030DDF2 File Offset: 0x0030C1F2
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06006030 RID: 24624 RVA: 0x0030DE00 File Offset: 0x0030C200
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
	}
}
