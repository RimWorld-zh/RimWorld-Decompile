using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	// Token: 0x02000600 RID: 1536
	[HasDebugOutput]
	public class PeaceTalks : WorldObject
	{
		// Token: 0x0400121A RID: 4634
		private Material cachedMat;

		// Token: 0x0400121B RID: 4635
		private static readonly SimpleCurve BadOutcomeFactorAtDiplomacyPower = new SimpleCurve
		{
			{
				new CurvePoint(0f, 4f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.4f),
				true
			}
		};

		// Token: 0x0400121C RID: 4636
		private const float BaseWeight_Disaster = 0.05f;

		// Token: 0x0400121D RID: 4637
		private const float BaseWeight_Backfire = 0.1f;

		// Token: 0x0400121E RID: 4638
		private const float BaseWeight_TalksFlounder = 0.2f;

		// Token: 0x0400121F RID: 4639
		private const float BaseWeight_Success = 0.55f;

		// Token: 0x04001220 RID: 4640
		private const float BaseWeight_Triumph = 0.1f;

		// Token: 0x04001221 RID: 4641
		private static List<Pair<Action, float>> tmpPossibleOutcomes = new List<Pair<Action, float>>();

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001EAC RID: 7852 RVA: 0x0010B928 File Offset: 0x00109D28
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0010B99C File Offset: 0x00109D9C
		public void Notify_CaravanArrived(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn == null)
			{
				Messages.Message("MessagePeaceTalksNoDiplomat".Translate(), caravan, MessageTypeDefOf.NegativeEvent, false);
			}
			else
			{
				float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(pawn);
				float num = 1f / badOutcomeWeightFactor;
				PeaceTalks.tmpPossibleOutcomes.Clear();
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
				{
					this.Outcome_Disaster(caravan);
				}, 0.05f * badOutcomeWeightFactor));
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
				{
					this.Outcome_Backfire(caravan);
				}, 0.1f * badOutcomeWeightFactor));
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
				{
					this.Outcome_TalksFlounder(caravan);
				}, 0.2f));
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
				{
					this.Outcome_Success(caravan);
				}, 0.55f * num));
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
				{
					this.Outcome_Triumph(caravan);
				}, 0.1f * num));
				Action first = PeaceTalks.tmpPossibleOutcomes.RandomElementByWeight((Pair<Action, float> x) => x.Second).First;
				first();
				pawn.skills.Learn(SkillDefOf.Social, 6000f, true);
				Find.WorldObjects.Remove(this);
			}
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0010BB14 File Offset: 0x00109F14
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption o in this.<GetFloatMenuOptions>__BaseCallProxy0(caravan))
			{
				yield return o;
			}
			foreach (FloatMenuOption f in CaravanArrivalAction_VisitPeaceTalks.GetFloatMenuOptions(caravan, this))
			{
				yield return f;
			}
			yield break;
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0010BB48 File Offset: 0x00109F48
		private void Outcome_Disaster(Caravan caravan)
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				FactionRelationKind playerRelationKind = this.Faction.PlayerRelationKind;
				int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksDisasterRange.RandomInRange;
				this.Faction.TryAffectGoodwillWith(Faction.OfPlayer, randomInRange, false, false, null, null);
				this.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, caravan);
				incidentParms.faction = this.Faction;
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, true);
				defaultPawnGroupMakerParms.generateFightersOnly = true;
				List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(caravan, list, false);
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(incidentParms.faction, new LordJob_AssaultColony(this.Faction, true, true, false, false, true), map, list);
				}
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				GlobalTargetInfo target = (!list.Any<Pawn>()) ? GlobalTargetInfo.Invalid : new GlobalTargetInfo(list[0].Position, map, false);
				string label = "LetterLabelPeaceTalks_Disaster".Translate();
				string letterText = this.GetLetterText("LetterPeaceTalks_Disaster".Translate(new object[]
				{
					this.Faction.def.pawnsPlural.CapitalizeFirst(),
					this.Faction.Name,
					Mathf.RoundToInt((float)randomInRange)
				}), caravan, playerRelationKind);
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref letterText, "LetterRelatedPawnsGroupGeneric".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), true, true);
				Find.LetterStack.ReceiveLetter(label, letterText, LetterDefOf.ThreatBig, target, this.Faction, null);
			}, "GeneratingMapForNewEncounter", false, null);
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0010BB84 File Offset: 0x00109F84
		private void Outcome_Backfire(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksBackfireRange.RandomInRange;
			base.Faction.TryAffectGoodwillWith(Faction.OfPlayer, randomInRange, false, false, null, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Backfire".Translate(), this.GetLetterText("LetterPeaceTalks_Backfire".Translate(new object[]
			{
				base.Faction.Name,
				randomInRange
			}), caravan, playerRelationKind), LetterDefOf.NegativeEvent, caravan, base.Faction, null);
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x0010BC20 File Offset: 0x0010A020
		private void Outcome_TalksFlounder(Caravan caravan)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_TalksFlounder".Translate(), this.GetLetterText("LetterPeaceTalks_TalksFlounder".Translate(new object[]
			{
				base.Faction.Name
			}), caravan, base.Faction.PlayerRelationKind), LetterDefOf.NeutralEvent, caravan, base.Faction, null);
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x0010BC84 File Offset: 0x0010A084
		private void Outcome_Success(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksSuccessRange.RandomInRange;
			base.Faction.TryAffectGoodwillWith(Faction.OfPlayer, randomInRange, false, false, null, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Success".Translate(), this.GetLetterText("LetterPeaceTalks_Success".Translate(new object[]
			{
				base.Faction.Name,
				randomInRange
			}), caravan, playerRelationKind), LetterDefOf.PositiveEvent, caravan, base.Faction, null);
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x0010BD20 File Offset: 0x0010A120
		private void Outcome_Triumph(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksTriumphRange.RandomInRange;
			base.Faction.TryAffectGoodwillWith(Faction.OfPlayer, randomInRange, false, false, null, null);
			List<Thing> list = ThingSetMakerDefOf.Reward_PeaceTalksGift.root.Generate();
			for (int i = 0; i < list.Count; i++)
			{
				caravan.AddPawnOrItem(list[i], true);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Triumph".Translate(), this.GetLetterText("LetterPeaceTalks_Triumph".Translate(new object[]
			{
				base.Faction.Name,
				randomInRange,
				list[0].Label
			}), caravan, playerRelationKind), LetterDefOf.PositiveEvent, caravan, base.Faction, null);
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x0010BE0C File Offset: 0x0010A20C
		private string GetLetterText(string baseText, Caravan caravan, FactionRelationKind previousRelationKind)
		{
			string text = baseText;
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn != null)
			{
				text = text + "\n\n" + "PeaceTalksSocialXPGain".Translate(new object[]
				{
					pawn.LabelShort,
					6000f.ToString("F0")
				});
			}
			base.Faction.TryAppendRelationKindChangedInfo(ref text, previousRelationKind, base.Faction.PlayerRelationKind, null);
			return text;
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x0010BE88 File Offset: 0x0010A288
		private static float GetBadOutcomeWeightFactor(Pawn diplomat)
		{
			float statValue = diplomat.GetStatValue(StatDefOf.DiplomacyPower, true);
			return PeaceTalks.GetBadOutcomeWeightFactor(statValue);
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x0010BEB0 File Offset: 0x0010A2B0
		private static float GetBadOutcomeWeightFactor(float diplomacyPower)
		{
			return PeaceTalks.BadOutcomeFactorAtDiplomacyPower.Evaluate(diplomacyPower);
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0010BED0 File Offset: 0x0010A2D0
		[DebugOutput]
		[Category("Incidents")]
		private static void PeaceTalksChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			PeaceTalks.AppendDebugChances(stringBuilder, 0f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1.5f);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x0010BF14 File Offset: 0x0010A314
		private static void AppendDebugChances(StringBuilder sb, float diplomacyPower)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			sb.AppendLine("--- DiplomacyPower = " + diplomacyPower.ToStringPercent() + " ---");
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(diplomacyPower);
			float num = 1f / badOutcomeWeightFactor;
			sb.AppendLine("Bad outcome weight factor: " + badOutcomeWeightFactor.ToString("0.##"));
			float num2 = 0.05f * badOutcomeWeightFactor;
			float num3 = 0.1f * badOutcomeWeightFactor;
			float num4 = 0.2f;
			float num5 = 0.55f * num;
			float num6 = 0.1f * num;
			float num7 = num2 + num3 + num4 + num5 + num6;
			sb.AppendLine("Disaster: " + (num2 / num7).ToStringPercent());
			sb.AppendLine("Backfire: " + (num3 / num7).ToStringPercent());
			sb.AppendLine("Talks flounder: " + (num4 / num7).ToStringPercent());
			sb.AppendLine("Success: " + (num5 / num7).ToStringPercent());
			sb.AppendLine("Triumph: " + (num6 / num7).ToStringPercent());
		}
	}
}
