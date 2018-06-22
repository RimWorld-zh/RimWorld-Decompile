using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000292 RID: 658
	public class PawnGenOption
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00064C90 File Offset: 0x00063090
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00064CB0 File Offset: 0x000630B0
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

		// Token: 0x06000B1B RID: 2843 RVA: 0x00064D51 File Offset: 0x00063151
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name);
			this.selectionWeight = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x040005A7 RID: 1447
		public PawnKindDef kind;

		// Token: 0x040005A8 RID: 1448
		public float selectionWeight;
	}
}
