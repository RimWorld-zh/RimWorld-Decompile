using Verse;

namespace RimWorld
{
	public class Building_Art : Building
	{
		public override string GetInspectString()
		{
			string inspectString;
			string text = inspectString = base.GetInspectString();
			return inspectString + "\n" + StatDefOf.Beauty.LabelCap + ": " + StatDefOf.Beauty.ValueToString(this.GetStatValue(StatDefOf.Beauty, true), ToStringNumberSense.Absolute);
		}
	}
}
