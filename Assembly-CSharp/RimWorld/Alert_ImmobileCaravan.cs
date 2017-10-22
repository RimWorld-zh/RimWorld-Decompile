using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_ImmobileCaravan : Alert
	{
		private Caravan FirstImmobileCaravan
		{
			get
			{
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < caravans.Count; i++)
				{
					if (caravans[i].IsPlayerControlled && caravans[i].ImmobilizedByMass)
					{
						return caravans[i];
					}
				}
				return null;
			}
		}

		public Alert_ImmobileCaravan()
		{
			base.defaultLabel = "ImmobileCaravan".Translate();
			base.defaultExplanation = "ImmobileCaravanDesc".Translate();
			base.defaultPriority = AlertPriority.High;
		}

		public override AlertReport GetReport()
		{
			return (WorldObject)this.FirstImmobileCaravan;
		}
	}
}
