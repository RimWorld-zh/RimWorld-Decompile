using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000292 RID: 658
	public class PawnGenOption
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00064C28 File Offset: 0x00063028
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00064C48 File Offset: 0x00063048
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

		// Token: 0x06000B1D RID: 2845 RVA: 0x00064CE9 File Offset: 0x000630E9
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name);
			this.selectionWeight = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x040005A8 RID: 1448
		public PawnKindDef kind;

		// Token: 0x040005A9 RID: 1449
		public float selectionWeight;
	}
}
