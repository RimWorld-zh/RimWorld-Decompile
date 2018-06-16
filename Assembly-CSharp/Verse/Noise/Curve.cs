using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F8A RID: 3978
	public class Curve : ModuleBase
	{
		// Token: 0x06005FF9 RID: 24569 RVA: 0x0030B525 File Offset: 0x00309925
		public Curve() : base(1)
		{
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x0030B53A File Offset: 0x0030993A
		public Curve(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06005FFB RID: 24571 RVA: 0x0030B558 File Offset: 0x00309958
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06005FFC RID: 24572 RVA: 0x0030B578 File Offset: 0x00309978
		public List<KeyValuePair<double, double>> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x0030B594 File Offset: 0x00309994
		public void Add(double input, double output)
		{
			KeyValuePair<double, double> item = new KeyValuePair<double, double>(input, output);
			if (!this.m_data.Contains(item))
			{
				this.m_data.Add(item);
			}
			this.m_data.Sort((KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs) => lhs.Key.CompareTo(rhs.Key));
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x0030B5F2 File Offset: 0x003099F2
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x0030B600 File Offset: 0x00309A00
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

		// Token: 0x04003F04 RID: 16132
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();
	}
}
