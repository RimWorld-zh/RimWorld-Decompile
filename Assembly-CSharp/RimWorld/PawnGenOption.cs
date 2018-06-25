using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000294 RID: 660
	public class PawnGenOption
	{
		// Token: 0x040005A7 RID: 1447
		public PawnKindDef kind;

		// Token: 0x040005A8 RID: 1448
		public float selectionWeight;

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00064DE0 File Offset: 0x000631E0
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00064E00 File Offset: 0x00063200
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

		// Token: 0x06000B1F RID: 2847 RVA: 0x00064EA1 File Offset: 0x000632A1
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name);
			this.selectionWeight = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
