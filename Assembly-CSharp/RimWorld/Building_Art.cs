using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000670 RID: 1648
	public class Building_Art : Building
	{
		// Token: 0x0600228B RID: 8843 RVA: 0x0012A5C8 File Offset: 0x001289C8
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
