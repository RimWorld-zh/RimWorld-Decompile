using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000278 RID: 632
	public class StatModifier
	{
		// Token: 0x04000558 RID: 1368
		public StatDef stat;

		// Token: 0x04000559 RID: 1369
		public float value;

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00062398 File Offset: 0x00060798
		public string ValueToStringAsOffset
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Offset);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x000623C8 File Offset: 0x000607C8
		public string ToStringAsFactor
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Factor);
			}
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x000623F5 File Offset: 0x000607F5
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "stat", xmlRoot.Name);
			this.value = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00062430 File Offset: 0x00060830
		public override string ToString()
		{
			string result;
			if (this.stat == null)
			{
				result = "(null stat)";
			}
			else
			{
				result = this.stat.defName + "-" + this.value.ToString();
			}
			return result;
		}
	}
}
