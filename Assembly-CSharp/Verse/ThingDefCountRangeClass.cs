using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F05 RID: 3845
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x06005C2B RID: 23595 RVA: 0x002EC738 File Offset: 0x002EAB38
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x06005C2C RID: 23596 RVA: 0x002EC741 File Offset: 0x002EAB41
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x06005C2D RID: 23597 RVA: 0x002EC752 File Offset: 0x002EAB52
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06005C2E RID: 23598 RVA: 0x002EC76C File Offset: 0x002EAB6C
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06005C2F RID: 23599 RVA: 0x002EC78C File Offset: 0x002EAB8C
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06005C30 RID: 23600 RVA: 0x002EC7AC File Offset: 0x002EABAC
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06005C31 RID: 23601 RVA: 0x002EC7CC File Offset: 0x002EABCC
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x002EC7EC File Offset: 0x002EABEC
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x002EC824 File Offset: 0x002EAC24
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

		// Token: 0x06005C34 RID: 23604 RVA: 0x002EC898 File Offset: 0x002EAC98
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

		// Token: 0x06005C35 RID: 23605 RVA: 0x002EC904 File Offset: 0x002EAD04
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x002EC92C File Offset: 0x002EAD2C
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x002EC95C File Offset: 0x002EAD5C
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}

		// Token: 0x04003CD8 RID: 15576
		public ThingDef thingDef;

		// Token: 0x04003CD9 RID: 15577
		public IntRange countRange;
	}
}
