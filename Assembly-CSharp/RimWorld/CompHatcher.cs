using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompHatcher : ThingComp
	{
		private float gestateProgress = 0f;

		public Pawn hatcheeParent = null;

		public Pawn otherParent = null;

		public Faction hatcheeFaction = null;

		public CompProperties_Hatcher Props
		{
			get
			{
				return (CompProperties_Hatcher)base.props;
			}
		}

		private CompTemperatureRuinable FreezerComp
		{
			get
			{
				return base.parent.GetComp<CompTemperatureRuinable>();
			}
		}

		public bool TemperatureDamaged
		{
			get
			{
				CompTemperatureRuinable freezerComp = this.FreezerComp;
				return freezerComp != null && this.FreezerComp.Ruined;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.gestateProgress, "gestateProgress", 0f, false);
			Scribe_References.Look<Pawn>(ref this.hatcheeParent, "hatcheeParent", false);
			Scribe_References.Look<Pawn>(ref this.otherParent, "otherParent", false);
			Scribe_References.Look<Faction>(ref this.hatcheeFaction, "hatcheeFaction", false);
		}

		public override void CompTick()
		{
			if (!this.TemperatureDamaged)
			{
				float num = (float)(1.0 / (this.Props.hatcherDaystoHatch * 60000.0));
				this.gestateProgress += num;
				if (this.gestateProgress > 1.0)
				{
					this.Hatch();
				}
			}
		}

		public void Hatch()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(this.Props.hatcherPawn, this.hatcheeFaction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, false, false, false, false, null, default(float?), default(float?), default(float?), default(Gender?), default(float?), (string)null);
			for (int i = 0; i < base.parent.stackCount; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, base.parent))
				{
					if (pawn != null)
					{
						if (this.hatcheeParent != null)
						{
							if (pawn.playerSettings != null && this.hatcheeParent.playerSettings != null && this.hatcheeParent.Faction == this.hatcheeFaction)
							{
								pawn.playerSettings.AreaRestriction = this.hatcheeParent.playerSettings.AreaRestriction;
							}
							if (pawn.RaceProps.IsFlesh)
							{
								pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.hatcheeParent);
							}
						}
						if (this.otherParent != null && (this.hatcheeParent == null || this.hatcheeParent.gender != this.otherParent.gender) && pawn.RaceProps.IsFlesh)
						{
							pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.otherParent);
						}
					}
					if (base.parent.Spawned)
					{
						FilthMaker.MakeFilth(base.parent.Position, base.parent.Map, ThingDefOf.FilthAmnioticFluid, 1);
					}
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
			}
			base.parent.Destroy(DestroyMode.Vanish);
		}

		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(base.parent.stackCount + count);
			CompHatcher comp = ((ThingWithComps)otherStack).GetComp<CompHatcher>();
			float b = comp.gestateProgress;
			this.gestateProgress = Mathf.Lerp(this.gestateProgress, b, t);
		}

		public override void PostSplitOff(Thing piece)
		{
			CompHatcher comp = ((ThingWithComps)piece).GetComp<CompHatcher>();
			comp.gestateProgress = this.gestateProgress;
			comp.hatcheeParent = this.hatcheeParent;
			comp.otherParent = this.otherParent;
			comp.hatcheeFaction = this.hatcheeFaction;
		}

		public override void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PrePreTraded(action, playerNegotiator, trader);
			switch (action)
			{
			case TradeAction.PlayerBuys:
			{
				this.hatcheeFaction = Faction.OfPlayer;
				break;
			}
			case TradeAction.PlayerSells:
			{
				this.hatcheeFaction = trader.Faction;
				break;
			}
			}
		}

		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			base.PostPostGeneratedForTrader(trader, forTile, forFaction);
			this.hatcheeFaction = forFaction;
		}

		public override string CompInspectStringExtra()
		{
			return this.TemperatureDamaged ? null : ("EggProgress".Translate() + ": " + this.gestateProgress.ToStringPercent());
		}
	}
}
