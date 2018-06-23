using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000276 RID: 630
	public class StatModifier
	{
		// Token: 0x04000556 RID: 1366
		public StatDef stat;

		// Token: 0x04000557 RID: 1367
		public float value;

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0006224C File Offset: 0x0006064C
		public string ValueToStringAsOffset
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Offset);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0006227C File Offset: 0x0006067C
		public string ToStringAsFactor
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Factor);
			}
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x000622A9 File Offset: 0x000606A9
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "stat", xmlRoot.Name);
			this.value = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000622E4 File Offset: 0x000606E4
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
