using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	public class Hediff_Pregnant : HediffWithComps
	{
		public Pawn father;

		private const int MiscarryCheckInterval = 1000;

		private const float MTBMiscarryStarvingDays = 0.5f;

		private const float MTBMiscarryWoundedDays = 0.5f;

		public float GestationProgress
		{
			get
			{
				return this.Severity;
			}
			private set
			{
				this.Severity = value;
			}
		}

		private bool IsSeverelyWounded
		{
			get
			{
				float num = 0f;
				List<Hediff> hediffs = base.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i] is Hediff_Injury && !hediffs[i].IsOld())
					{
						num += hediffs[i].Severity;
					}
				}
				List<Hediff_MissingPart> missingPartsCommonAncestors = base.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
				{
					if (missingPartsCommonAncestors[j].IsFreshNonSolidExtremity)
					{
						num += missingPartsCommonAncestors[j].Part.def.GetMaxHealth(base.pawn);
					}
				}
				return num > 38.0 * base.pawn.RaceProps.baseHealthScale;
			}
		}

		public override void Tick()
		{
			base.ageTicks++;
			if (base.pawn.IsHashIntervalTick(1000))
			{
				if (base.pawn.needs.food != null && base.pawn.needs.food.CurCategory == HungerCategory.Starving && Rand.MTBEventOccurs(0.5f, 60000f, 1000f))
				{
					if (this.Visible && PawnUtility.ShouldSendNotificationAbout(base.pawn))
					{
						Messages.Message("MessageMiscarriedStarvation".Translate(base.pawn.LabelIndefinite()).CapitalizeFirst(), (Thing)base.pawn, MessageTypeDefOf.NegativeHealthEvent);
					}
					this.Miscarry();
					return;
				}
				if (this.IsSeverelyWounded && Rand.MTBEventOccurs(0.5f, 60000f, 1000f))
				{
					if (this.Visible && PawnUtility.ShouldSendNotificationAbout(base.pawn))
					{
						Messages.Message("MessageMiscarriedPoorHealth".Translate(base.pawn.LabelIndefinite()).CapitalizeFirst(), (Thing)base.pawn, MessageTypeDefOf.NegativeHealthEvent);
					}
					this.Miscarry();
					return;
				}
			}
			this.GestationProgress += (float)(1.0 / (base.pawn.RaceProps.gestationPeriodDays * 60000.0));
			if (this.GestationProgress >= 1.0)
			{
				if (this.Visible && PawnUtility.ShouldSendNotificationAbout(base.pawn))
				{
					Messages.Message("MessageGaveBirth".Translate(base.pawn.LabelIndefinite()).CapitalizeFirst(), (Thing)base.pawn, MessageTypeDefOf.PositiveEvent);
				}
				Hediff_Pregnant.DoBirthSpawn(base.pawn, this.father);
				base.pawn.health.RemoveHediff(this);
			}
		}

		private void Miscarry()
		{
			base.pawn.health.RemoveHediff(this);
		}

		public static void DoBirthSpawn(Pawn mother, Pawn father)
		{
			int num = (mother.RaceProps.litterSizeCurve == null) ? 1 : Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve, 300));
			if (num < 1)
			{
				num = 1;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, false, false, false, false, null, default(float?), default(float?), default(float?), default(Gender?), default(float?), (string)null);
			Pawn pawn = null;
			for (int num2 = 0; num2 < num; num2++)
			{
				pawn = PawnGenerator.GeneratePawn(request);
				if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, mother))
				{
					if (pawn.playerSettings != null && mother.playerSettings != null)
					{
						pawn.playerSettings.AreaRestriction = mother.playerSettings.AreaRestriction;
					}
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
						if (father != null)
						{
							pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
						}
					}
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				TaleRecorder.RecordTale(TaleDefOf.GaveBirth, mother, pawn);
			}
			if (mother.Spawned)
			{
				FilthMaker.MakeFilth(mother.Position, mother.Map, ThingDefOf.FilthAmnioticFluid, mother.LabelIndefinite(), 5);
				if (mother.caller != null)
				{
					mother.caller.DoCall();
				}
				if (pawn.caller != null)
				{
					pawn.caller.DoCall();
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.father, "father", false);
		}

		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.DebugString());
			stringBuilder.AppendLine("Gestation progress: " + this.GestationProgress.ToStringPercent());
			stringBuilder.AppendLine("Time left: " + ((int)((1.0 - this.GestationProgress) * base.pawn.RaceProps.gestationPeriodDays * 60000.0)).ToStringTicksToPeriod(true, false, true));
			return stringBuilder.ToString();
		}
	}
}
