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
				int num = 0;
				Caravan result;
				while (true)
				{
					if (num < caravans.Count)
					{
						if (caravans[num].IsPlayerControlled && caravans[num].ImmobilizedByMass)
						{
							result = caravans[num];
							break;
						}
						num++;
						continue;
					}
					result = null;
					break;
				}
				return result;
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
