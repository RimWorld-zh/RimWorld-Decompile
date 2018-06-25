using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000294 RID: 660
	public class PawnGenOption
	{
		// Token: 0x040005A9 RID: 1449
		public PawnKindDef kind;

		// Token: 0x040005AA RID: 1450
		public float selectionWeight;

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00064DDC File Offset: 0x000631DC
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00064DFC File Offset: 0x000631FC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				(this.kind == null) ? "null" : this.kind.ToString(),
				" w=",
				this.selectionWeight.ToString("F2"),
				" c=",
				(this.kind == null) ? "null" : this.Cost.ToString("F2"),
				")"
			});
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00064E9D File Offset: 0x0006329D
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name);
			this.selectionWeight = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
