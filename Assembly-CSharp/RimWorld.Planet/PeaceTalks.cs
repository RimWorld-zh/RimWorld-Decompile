using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	public class PeaceTalks : WorldObject
	{
		private Material cachedMat;

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

		private const float BaseWeight_Disaster = 0.05f;

		private const float BaseWeight_Backfire = 0.1f;

		private const float BaseWeight_TalksFlounder = 0.2f;

		private const float BaseWeight_Success = 0.55f;

		private const float BaseWeight_Triumph = 0.1f;

		private static readonly FloatRange DisasterFactionRelationOffset = new FloatRange(-75f, -25f);

		private static readonly FloatRange BackfireFactionRelationOffset = new FloatRange(-50f, -10f);

		private static readonly FloatRange SuccessFactionRelationOffset = new FloatRange(25f, 75f);

		private static readonly FloatRange TriumphFactionRelationOffset = new FloatRange(50f, 75f);

		private const float SocialXPGainAmount = 6000f;

		private static List<Pair<Action, float>> tmpPossibleOutcomes = new List<Pair<Action, float>>();

		public override Material Material
		{
			get
			{
				if ((UnityEngine.Object)this.cachedMat == (UnityEngine.Object)null)
				{
					Color color = (base.Faction == null) ? Color.white : base.Faction.Color;
					this.cachedMat = MaterialPool.MatFrom(base.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		public void Notify_CaravanArrived(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn == null)
			{
				Messages.Message("MessagePeaceTalksNoDiplomat".Translate(), (WorldObject)caravan, MessageTypeDefOf.NegativeEvent);
			}
			else
			{
				float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(pawn);
				float num = (float)(1.0 / badOutcomeWeightFactor);
				PeaceTalks.tmpPossibleOutcomes.Clear();
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>((Action)delegate()
				{
					this.Outcome_Disaster(caravan);
				}, (float)(0.05000000074505806 * badOutcomeWeightFactor)));
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>((Action)delegate()
				{
					this.Outcome_Backfire(caravan);
				}, (float)(0.10000000149011612 * badOutcomeWeightFactor)));
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>((Action)delegate()
				{
					this.Outcome_TalksFlounder(caravan);
				}, 0.2f));
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>((Action)delegate()
				{
					this.Outcome_Success(caravan);
				}, (float)(0.550000011920929 * num)));
				PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>((Action)delegate()
				{
					this.Outcome_Triumph(caravan);
				}, (float)(0.10000000149011612 * num)));
				Action first = PeaceTalks.tmpPossibleOutcomes.RandomElementByWeight((Func<Pair<Action, float>, float>)((Pair<Action, float> x) => x.Second)).First;
				first();
				pawn.skills.Learn(SkillDefOf.Social, 6000f, true);
				Find.WorldObjects.Remove(this);
			}
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			_003CGetFloatMenuOptions_003Ec__Iterator0 _003CGetFloatMenuOptions_003Ec__Iterator = (_003CGetFloatMenuOptions_003Ec__Iterator0)/*Error near IL_003c: stateMachine*/;
			using (IEnumerator<FloatMenuOption> enumerator = this._003CGetFloatMenuOptions_003E__BaseCallProxy0(caravan).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					FloatMenuOption o = enumerator.Current;
					yield return o;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield return new FloatMenuOption("VisitPeaceTalks".Translate(this.Label), (Action)delegate()
			{
				caravan.pather.StartPath(_003CGetFloatMenuOptions_003Ec__Iterator._0024this.Tile, new CaravanArrivalAction_VisitPeaceTalks(_003CGetFloatMenuOptions_003Ec__Iterator._0024this), true);
			}, MenuOptionPriority.Default, null, null, 0f, null, this);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_01c8:
			/*Error near IL_01c9: Unexpected return in MoveNext()*/;
		}

		private void Outcome_Disaster(Caravan caravan)
		{
			LongEventHandler.QueueLongEvent((Action)delegate()
			{
				float randomInRange = PeaceTalks.DisasterFactionRelationOffset.RandomInRange;
				base.Faction.AffectGoodwillWith(Faction.OfPlayer, randomInRange);
				if (!base.Faction.HostileTo(Faction.OfPlayer))
				{
					base.Faction.SetHostileTo(Faction.OfPlayer, true);
				}
				IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, IncidentCategory.ThreatBig, caravan);
				incidentParms.faction = base.Faction;
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(incidentParms, true);
				defaultPawnGroupMakerParms.generateFightersOnly = true;
				List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(PawnGroupKindDefOf.Normal, defaultPawnGroupMakerParms, true).ToList();
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(caravan, list);
				if (list.Any())
				{
					LordMaker.MakeNewLord(incidentParms.faction, new LordJob_AssaultColony(base.Faction, true, true, false, false, true), map, list);
				}
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				GlobalTargetInfo lookTarget = (!list.Any()) ? GlobalTargetInfo.Invalid : new GlobalTargetInfo(list[0].Position, map, false);
				Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Disaster".Translate(), this.GetLetterText("LetterPeaceTalks_Disaster".Translate(base.Faction.def.pawnsPlural.CapitalizeFirst(), base.Faction.Name, Mathf.RoundToInt(randomInRange)), caravan), LetterDefOf.ThreatBig, lookTarget, (string)null);
			}, "GeneratingMapForNewEncounter", false, null);
		}

		private void Outcome_Backfire(Caravan caravan)
		{
			float randomInRange = PeaceTalks.BackfireFactionRelationOffset.RandomInRange;
			base.Faction.AffectGoodwillWith(Faction.OfPlayer, randomInRange);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Backfire".Translate(), this.GetLetterText("LetterPeaceTalks_Backfire".Translate(base.Faction.Name, Mathf.RoundToInt(randomInRange)), caravan), LetterDefOf.NegativeEvent, (WorldObject)caravan, (string)null);
		}

		private void Outcome_TalksFlounder(Caravan caravan)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_TalksFlounder".Translate(), this.GetLetterText("LetterPeaceTalks_TalksFlounder".Translate(base.Faction.Name), caravan), LetterDefOf.NeutralEvent, (WorldObject)caravan, (string)null);
		}

		private void Outcome_Success(Caravan caravan)
		{
			float randomInRange = PeaceTalks.SuccessFactionRelationOffset.RandomInRange;
			base.Faction.AffectGoodwillWith(Faction.OfPlayer, randomInRange);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Success".Translate(), this.GetLetterText("LetterPeaceTalks_Success".Translate(base.Faction.Name, Mathf.RoundToInt(randomInRange)), caravan), LetterDefOf.PositiveEvent, (WorldObject)caravan, (string)null);
		}

		private void Outcome_Triumph(Caravan caravan)
		{
			float randomInRange = PeaceTalks.TriumphFactionRelationOffset.RandomInRange;
			base.Faction.AffectGoodwillWith(Faction.OfPlayer, randomInRange);
			List<Thing> list = ItemCollectionGeneratorDefOf.PeaceTalksGift.Worker.Generate(default(ItemCollectionGeneratorParams));
			for (int i = 0; i < list.Count; i++)
			{
				caravan.AddPawnOrItem(list[0], true);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Triumph".Translate(), this.GetLetterText("LetterPeaceTalks_Triumph".Translate(base.Faction.Name, Mathf.RoundToInt(randomInRange), list[0].Label), caravan), LetterDefOf.PositiveEvent, (WorldObject)caravan, (string)null);
		}

		private string GetLetterText(string baseText, Caravan caravan)
		{
			string text = baseText;
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn != null)
			{
				text = text + "\n\n" + "PeaceTalksSocialXPGain".Translate(pawn.LabelShort, 6000f);
			}
			return text;
		}

		private static float GetBadOutcomeWeightFactor(Pawn diplomat)
		{
			float statValue = diplomat.GetStatValue(StatDefOf.DiplomacyPower, true);
			return PeaceTalks.GetBadOutcomeWeightFactor(statValue);
		}

		private static float GetBadOutcomeWeightFactor(float diplomacyPower)
		{
			return PeaceTalks.BadOutcomeFactorAtDiplomacyPower.Evaluate(diplomacyPower);
		}

		public static void LogChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			PeaceTalks.AppendDebugChances(stringBuilder, 0f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1.5f);
			Log.Message(stringBuilder.ToString());
		}

		private static void AppendDebugChances(StringBuilder sb, float diplomacyPower)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			sb.AppendLine("--- DiplomacyPower = " + diplomacyPower.ToStringPercent() + " ---");
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(diplomacyPower);
			float num = (float)(1.0 / badOutcomeWeightFactor);
			sb.AppendLine("Bad outcome weight factor: " + badOutcomeWeightFactor.ToString("0.##"));
			float num2 = (float)(0.05000000074505806 * badOutcomeWeightFactor);
			float num3 = (float)(0.10000000149011612 * badOutcomeWeightFactor);
			float num4 = 0.2f;
			float num5 = (float)(0.550000011920929 * num);
			float num6 = (float)(0.10000000149011612 * num);
			float num7 = num2 + num3 + num4 + num5 + num6;
			sb.AppendLine("Disaster: " + (num2 / num7).ToStringPercent());
			sb.AppendLine("Backfire: " + (num3 / num7).ToStringPercent());
			sb.AppendLine("Talks flounder: " + (num4 / num7).ToStringPercent());
			sb.AppendLine("Success: " + (num5 / num7).ToStringPercent());
			sb.AppendLine("Triumph: " + (num6 / num7).ToStringPercent());
		}
	}
}
