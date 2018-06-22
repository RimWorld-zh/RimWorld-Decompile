using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200066E RID: 1646
	public class Building_Art : Building
	{
		// Token: 0x06002288 RID: 8840 RVA: 0x0012A210 File Offset: 0x00128610
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
