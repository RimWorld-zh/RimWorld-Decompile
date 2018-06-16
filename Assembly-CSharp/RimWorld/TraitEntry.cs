using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020004DD RID: 1245
	public class TraitEntry
	{
		// Token: 0x06001618 RID: 5656 RVA: 0x000C3C46 File Offset: 0x000C2046
		public TraitEntry()
		{
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x000C3C4F File Offset: 0x000C204F
		public TraitEntry(TraitDef def, int degree)
		{
			this.def = def;
			this.degree = degree;
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x000C3C68 File Offset: 0x000C2068
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

		// Token: 0x04000CCE RID: 3278
		public TraitDef def;

		// Token: 0x04000CCF RID: 3279
		public int degree;
	}
}
