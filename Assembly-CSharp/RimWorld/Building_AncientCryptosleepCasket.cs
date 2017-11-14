using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class Building_AncientCryptosleepCasket : Building_CryptosleepCasket
	{
		public int groupID = -1;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
		}

		public override void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(dinfo, out absorbed);
			if (!absorbed)
			{
				if (!base.contentsKnown && base.innerContainer.Count > 0 && dinfo.Def.harmsHealth && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
				{
					bool flag = false;
					foreach (Thing item in (IEnumerable<Thing>)base.innerContainer)
					{
						Pawn pawn = item as Pawn;
						if (pawn != null)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						this.EjectContents();
					}
				}
				absorbed = false;
			}
		}

		public override void EjectContents()
		{
			List<Thing> list = new List<Thing>();
			if (!base.contentsKnown)
			{
				list.AddRange(base.innerContainer);
				list.AddRange(this.UnopenedCasketsInGroup().SelectMany((Building_AncientCryptosleepCasket c) => c.innerContainer));
			}
			bool contentsKnown = base.contentsKnown;
			base.EjectContents();
			if (!contentsKnown)
			{
				ThingDef filthSlime = ThingDefOf.FilthSlime;
				FilthMaker.MakeFilth(base.Position, base.Map, filthSlime, Rand.Range(8, 12));
				this.SetFaction(null, null);
				foreach (Building_AncientCryptosleepCasket item in this.UnopenedCasketsInGroup())
				{
					item.EjectContents();
				}
				List<Pawn> source = (from t in list
				where t is Pawn
				select t).Cast<Pawn>().ToList();
				IEnumerable<Pawn> enumerable = from p in source
				where p.RaceProps.Humanlike && p.GetLord() == null && p.Faction == Faction.OfSpacerHostile
				select p;
				if (enumerable.Any())
				{
					LordMaker.MakeNewLord(Faction.OfSpacerHostile, new LordJob_AssaultColony(Faction.OfSpacerHostile, false, false, false, false, false), base.Map, enumerable);
				}
			}
		}

		private IEnumerable<Building_AncientCryptosleepCasket> UnopenedCasketsInGroup()
		{
			yield return this;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
