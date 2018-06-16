using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000672 RID: 1650
	public class Building_Art : Building
	{
		// Token: 0x0600228E RID: 8846 RVA: 0x0012A050 File Offset: 0x00128450
		public override string GetInspectString()
		{
			string inspectString = base.GetInspectString();
			string text = inspectString;
			return string.Concat(new string[]
			{
				text,
				"\n",
				StatDefOf.Beauty.LabelCap,
				": ",
				StatDefOf.Beauty.ValueToString(this.GetStatValue(StatDefOf.Beauty, true), ToStringNumberSense.Absolute)
			});
		}
	}
}
