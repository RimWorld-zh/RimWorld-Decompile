using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200069B RID: 1691
	public class Building_AncientCryptosleepCasket : Building_CryptosleepCasket
	{
		// Token: 0x040013FE RID: 5118
		public int groupID = -1;

		// Token: 0x060023D1 RID: 9169 RVA: 0x00133D42 File Offset: 0x00132142
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x00133D60 File Offset: 0x00132160
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed)
			{
				if (!this.contentsKnown && this.innerContainer.Count > 0 && dinfo.Def.harmsHealth && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
				{
					bool flag = false;
					foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
					{
						Pawn pawn = thing as Pawn;
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

		// Token: 0x060023D3 RID: 9171 RVA: 0x00133E3C File Offset: 0x0013223C
		public override void EjectContents()
		{
			List<Thing> list = new List<Thing>();
			if (!this.contentsKnown)
			{
				list.AddRange(this.innerContainer);
				list.AddRange(this.UnopenedCasketsInGroup().SelectMany((Building_AncientCryptosleepCasket c) => c.innerContainer));
			}
			bool contentsKnown = this.contentsKnown;
			base.EjectContents();
			if (!contentsKnown)
			{
				ThingDef filth_Slime = ThingDefOf.Filth_Slime;
				FilthMaker.MakeFilth(base.Position, base.Map, filth_Slime, Rand.Range(8, 12));
				this.SetFaction(null, null);
				foreach (Building_AncientCryptosleepCasket building_AncientCryptosleepCasket in this.UnopenedCasketsInGroup())
				{
					building_AncientCryptosleepCasket.EjectContents();
				}
				List<Pawn> source = list.OfType<Pawn>().ToList<Pawn>();
				IEnumerable<Pawn> enumerable = from p in source
				where p.RaceProps.Humanlike && p.GetLord() == null && p.Faction == Faction.OfAncientsHostile
				select p;
				if (enumerable.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_AssaultColony(Faction.OfAncientsHostile, false, false, false, false, false), base.Map, enumerable);
				}
			}
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x00133F8C File Offset: 0x0013238C
		private IEnumerable<Building_AncientCryptosleepCasket> UnopenedCasketsInGroup()
		{
			yield return this;
			if (this.groupID != -1)
			{
				foreach (Thing t in base.Map.listerThings.ThingsOfDef(ThingDefOf.AncientCryptosleepCasket))
				{
					Building_AncientCryptosleepCasket casket = t as Building_AncientCryptosleepCasket;
					if (casket.groupID == this.groupID && !casket.contentsKnown)
					{
						yield return casket;
					}
				}
			}
			yield break;
		}
	}
}
