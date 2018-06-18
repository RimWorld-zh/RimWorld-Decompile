using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000672 RID: 1650
	public class Building_Art : Building
	{
		// Token: 0x06002290 RID: 8848 RVA: 0x0012A0C8 File Offset: 0x001284C8
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
