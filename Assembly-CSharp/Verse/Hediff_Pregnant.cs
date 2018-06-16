using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D30 RID: 3376
	public class Hediff_Pregnant : HediffWithComps
	{
		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06004A57 RID: 19031 RVA: 0x0026BA18 File Offset: 0x00269E18
		// (set) Token: 0x06004A58 RID: 19032 RVA: 0x0026BA33 File Offset: 0x00269E33
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

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06004A59 RID: 19033 RVA: 0x0026BA40 File Offset: 0x00269E40
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

		// Token: 0x06004A5A RID: 19034 RVA: 0x0026BB3C File Offset: 0x00269F3C
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

		// Token: 0x06004A5B RID: 19035 RVA: 0x0026BD97 File Offset: 0x0026A197
		private void Miscarry()
		{
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x0026BDAC File Offset: 0x0026A1AC
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

		// Token: 0x06004A5D RID: 19037 RVA: 0x0026BF67 File Offset: 0x0026A367
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.father, "father", false);
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x0026BF84 File Offset: 0x0026A384
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.DebugString());
			stringBuilder.AppendLine("Gestation progress: " + this.GestationProgress.ToStringPercent());
			stringBuilder.AppendLine("Time left: " + ((int)((1f - this.GestationProgress) * this.pawn.RaceProps.gestationPeriodDays * 60000f)).ToStringTicksToPeriod());
			return stringBuilder.ToString();
		}

		// Token: 0x04003238 RID: 12856
		public Pawn father;

		// Token: 0x04003239 RID: 12857
		private const int MiscarryCheckInterval = 1000;

		// Token: 0x0400323A RID: 12858
		private const float MTBMiscarryStarvingDays = 0.5f;

		// Token: 0x0400323B RID: 12859
		private const float MTBMiscarryWoundedDays = 0.5f;
	}
}
