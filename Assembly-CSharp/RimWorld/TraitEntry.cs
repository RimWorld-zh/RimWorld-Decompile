using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D9 RID: 1241
	public class TraitEntry
	{
		// Token: 0x0600160F RID: 5647 RVA: 0x000C3C52 File Offset: 0x000C2052
		public TraitEntry()
		{
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x000C3C5B File Offset: 0x000C205B
		public TraitEntry(TraitDef def, int degree)
		{
			this.def = def;
			this.degree = degree;
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x000C3C74 File Offset: 0x000C2074
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.def = DefDatabase<TraitDef>.GetNamed(xmlRoot.Name, true);
			if (xmlRoot.HasChildNodes)
			{
				this.degree = (int)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(int));
			}
			else
			{
				this.degree = 0;
			}
		}

		// Token: 0x04000CCB RID: 3275
		public TraitDef def;

		// Token: 0x04000CCC RID: 3276
		public int degree;
	}
}
