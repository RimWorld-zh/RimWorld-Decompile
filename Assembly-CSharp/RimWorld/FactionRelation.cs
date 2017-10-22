using Verse;

namespace RimWorld
{
	public class FactionRelation : IExposable
	{
		public Faction other = null;

		public float goodwill = 100f;

		public bool hostile = false;

		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.other, "other", false);
			Scribe_Values.Look<float>(ref this.goodwill, "goodwill", 0f, false);
			Scribe_Values.Look<bool>(ref this.hostile, "hostile", false, false);
		}

		public override string ToString()
		{
			return "(" + this.other + ", goodwill=" + this.goodwill.ToString("F1") + ((!this.hostile) ? "" : " hostile") + ")";
		}
	}
}
