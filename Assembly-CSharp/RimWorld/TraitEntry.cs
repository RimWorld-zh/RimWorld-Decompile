using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020004DB RID: 1243
	public class TraitEntry
	{
		// Token: 0x04000CCB RID: 3275
		public TraitDef def;

		// Token: 0x04000CCC RID: 3276
		public int degree;

		// Token: 0x06001613 RID: 5651 RVA: 0x000C3DA2 File Offset: 0x000C21A2
		public TraitEntry()
		{
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x000C3DAB File Offset: 0x000C21AB
		public TraitEntry(TraitDef def, int degree)
		{
			this.def = def;
			this.degree = degree;
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x000C3DC4 File Offset: 0x000C21C4
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
	}
}
