using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D2C RID: 3372
	public class Hediff_Pregnant : HediffWithComps
	{
		// Token: 0x04003241 RID: 12865
		public Pawn father;

		// Token: 0x04003242 RID: 12866
		private const int MiscarryCheckInterval = 1000;

		// Token: 0x04003243 RID: 12867
		private const float MTBMiscarryStarvingDays = 0.5f;

		// Token: 0x04003244 RID: 12868
		private const float MTBMiscarryWoundedDays = 0.5f;

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06004A69 RID: 19049 RVA: 0x0026CF7C File Offset: 0x0026B37C
		// (set) Token: 0x06004A6A RID: 19050 RVA: 0x0026CF97 File Offset: 0x0026B397
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

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06004A6B RID: 19051 RVA: 0x0026CFA4 File Offset: 0x0026B3A4
		private bool IsSeverelyWounded
		{
			get
			{
				float num = 0f;
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i] is Hediff_Injury && !hediffs[i].IsPermanent())
					{
						num += hediffs[i].Severity;
					}
				}
				List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
				{
					if (missingPartsCommonAncestors[j].IsFreshNonSolidExtremity)
					{
						num += missingPartsCommonAncestors[j].Part.def.GetMaxHealth(this.pawn);
					}
				}
				return num > 38f * this.pawn.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x06004A6C RID: 19052 RVA: 0x0026D0A0 File Offset: 0x0026B4A0
		public override void Tick()
		{
			this.ageTicks++;
			if (this.pawn.IsHashIntervalTick(1000))
			{
				if (this.pawn.needs.food != null && this.pawn.needs.food.CurCategory == HungerCategory.Starving && this.pawn.health.hediffSet.HasHediff(HediffDefOf.Malnutrition, false) && this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false).Severity > 0.25f && Rand.MTBEventOccurs(0.5f, 60000f, 1000f))
				{
					if (this.Visible && PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageMiscarriedStarvation".Translate(new object[]
						{
							this.pawn.LabelIndefinite()
						}).CapitalizeFirst(), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					}
					this.Miscarry();
					return;
				}
				if (this.IsSeverelyWounded && Rand.MTBEventOccurs(0.5f, 60000f, 1000f))
				{
					if (this.Visible && PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageMiscarriedPoorHealth".Translate(new object[]
						{
							this.pawn.LabelIndefinite()
						}).CapitalizeFirst(), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					}
					this.Miscarry();
					return;
				}
			}
			this.GestationProgress += 1f / (this.pawn.RaceProps.gestationPeriodDays * 60000f);
			if (this.GestationProgress >= 1f)
			{
				if (this.Visible && PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageGaveBirth".Translate(new object[]
					{
						this.pawn.LabelIndefinite()
					}).CapitalizeFirst(), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
				Hediff_Pregnant.DoBirthSpawn(this.pawn, this.father);
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x06004A6D RID: 19053 RVA: 0x0026D2FB File Offset: 0x0026B6FB
		private void Miscarry()
		{
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x06004A6E RID: 19054 RVA: 0x0026D310 File Offset: 0x0026B710
		public static void DoBirthSpawn(Pawn mother, Pawn father)
		{
			int num = (mother.RaceProps.litterSizeCurve == null) ? 1 : Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve));
			if (num < 1)
			{
				num = 1;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = null;
			for (int i = 0; i < num; i++)
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
				TaleRecorder.RecordTale(TaleDefOf.GaveBirth, new object[]
				{
					mother,
					pawn
				});
			}
			if (mother.Spawned)
			{
				FilthMaker.MakeFilth(mother.Position, mother.Map, ThingDefOf.Filth_AmnioticFluid, mother.LabelIndefinite(), 5);
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

		// Token: 0x06004A6F RID: 19055 RVA: 0x0026D4CB File Offset: 0x0026B8CB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.father, "father", false);
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x0026D4E8 File Offset: 0x0026B8E8
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.DebugString());
			stringBuilder.AppendLine("Gestation progress: " + this.GestationProgress.ToStringPercent());
			stringBuilder.AppendLine("Time left: " + ((int)((1f - this.GestationProgress) * this.pawn.RaceProps.gestationPeriodDays * 60000f)).ToStringTicksToPeriod());
			return stringBuilder.ToString();
		}
	}
}
