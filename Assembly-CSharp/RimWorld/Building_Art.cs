using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000670 RID: 1648
	public class Building_Art : Building
	{
		// Token: 0x0600228C RID: 8844 RVA: 0x0012A360 File Offset: 0x00128760
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
