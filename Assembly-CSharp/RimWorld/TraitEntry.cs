using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020004DB RID: 1243
	public class TraitEntry
	{
		// Token: 0x04000CCE RID: 3278
		public TraitDef def;

		// Token: 0x04000CCF RID: 3279
		public int degree;

		// Token: 0x06001612 RID: 5650 RVA: 0x000C3FA2 File Offset: 0x000C23A2
		public TraitEntry()
		{
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x000C3FAB File Offset: 0x000C23AB
		public TraitEntry(TraitDef def, int degree)
		{
			this.def = def;
			this.degree = degree;
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x000C3FC4 File Offset: 0x000C23C4
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
