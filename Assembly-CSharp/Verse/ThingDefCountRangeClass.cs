using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F04 RID: 3844
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x06005C29 RID: 23593 RVA: 0x002EC814 File Offset: 0x002EAC14
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x002EC81D File Offset: 0x002EAC1D
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x06005C2B RID: 23595 RVA: 0x002EC82E File Offset: 0x002EAC2E
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06005C2C RID: 23596 RVA: 0x002EC848 File Offset: 0x002EAC48
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06005C2D RID: 23597 RVA: 0x002EC868 File Offset: 0x002EAC68
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06005C2E RID: 23598 RVA: 0x002EC888 File Offset: 0x002EAC88
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06005C2F RID: 23599 RVA: 0x002EC8A8 File Offset: 0x002EACA8
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C30 RID: 23600 RVA: 0x002EC8C8 File Offset: 0x002EACC8
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x002EC900 File Offset: 0x002EAD00
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountRangeClass: " + xmlRoot.OuterXml, false);
			}
			else
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name);
				this.countRange = (IntRange)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(IntRange));
			}
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x002EC974 File Offset: 0x002EAD74
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.countRange,
				"x ",
				(this.thingDef == null) ? "null" : this.thingDef.defName,
				")"
			});
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x002EC9E0 File Offset: 0x002EADE0
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x002ECA08 File Offset: 0x002EAE08
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C35 RID: 23605 RVA: 0x002ECA38 File Offset: 0x002EAE38
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}

		// Token: 0x04003CD7 RID: 15575
		public ThingDef thingDef;

		// Token: 0x04003CD8 RID: 15576
		public IntRange countRange;
	}
}
