using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	[HasDebugOutput]
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

		private static List<Pair<Action, float>> tmpPossibleOutcomes = new List<Pair<Action, float>>();

		[CompilerGenerated]
		private static Func<Pair<Action, float>, float> <>f__am$cache0;

		public PeaceTalks()
		{
		}

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

		private void Outcome_TalksFlounder(Caravan caravan)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_TalksFlounder".Translate(), this.GetLetterText("LetterPeaceTalks_TalksFlounder".Translate(new object[]
			{
				base.Faction.Name
			}), caravan, base.Faction.PlayerRelationKind), LetterDefOf.NeutralEvent, caravan, base.Faction, null);
		}

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
				GenLabel.ThingsLabel(list)
			}), caravan, playerRelationKind), LetterDefOf.PositiveEvent, caravan, base.Faction, null);
		}

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

		private static float GetBadOutcomeWeightFactor(Pawn diplomat)
		{
			float statValue = diplomat.GetStatValue(StatDefOf.DiplomacyPower, true);
			return PeaceTalks.GetBadOutcomeWeightFactor(statValue);
		}

		private static float GetBadOutcomeWeightFactor(float diplomacyPower)
		{
			return PeaceTalks.BadOutcomeFactorAtDiplomacyPower.Evaluate(diplomacyPower);
		}

		[Category("Incidents")]
		[DebugOutput]
		private static void PeaceTalksChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			PeaceTalks.AppendDebugChances(stringBuilder, 0f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1.5f);
			Log.Message(stringBuilder.ToString(), false);
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static PeaceTalks()
		{
		}

		[CompilerGenerated]
		private static float <Notify_CaravanArrived>m__0(Pair<Action, float> x)
		{
			return x.Second;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <GetFloatMenuOptions>__BaseCallProxy0(Caravan caravan)
		{
			return base.GetFloatMenuOptions(caravan);
		}

		[CompilerGenerated]
		private sealed class <Notify_CaravanArrived>c__AnonStorey1
		{
			internal Caravan caravan;

			internal PeaceTalks $this;

			public <Notify_CaravanArrived>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.$this.Outcome_Disaster(this.caravan);
			}

			internal void <>m__1()
			{
				this.$this.Outcome_Backfire(this.caravan);
			}

			internal void <>m__2()
			{
				this.$this.Outcome_TalksFlounder(this.caravan);
			}

			internal void <>m__3()
			{
				this.$this.Outcome_Success(this.caravan);
			}

			internal void <>m__4()
			{
				this.$this.Outcome_Triumph(this.caravan);
			}
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator0 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal Caravan caravan;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__1;

			internal IEnumerator<FloatMenuOption> $locvar1;

			internal FloatMenuOption <f>__2;

			internal PeaceTalks $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<GetFloatMenuOptions>__BaseCallProxy0(caravan).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_DE;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						o = enumerator.Current;
						this.$current = o;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				enumerator2 = CaravanArrivalAction_VisitPeaceTalks.GetFloatMenuOptions(caravan, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_DE:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						f = enumerator2.Current;
						this.$current = f;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PeaceTalks.<GetFloatMenuOptions>c__Iterator0 <GetFloatMenuOptions>c__Iterator = new PeaceTalks.<GetFloatMenuOptions>c__Iterator0();
				<GetFloatMenuOptions>c__Iterator.$this = this;
				<GetFloatMenuOptions>c__Iterator.caravan = caravan;
				return <GetFloatMenuOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <Outcome_Disaster>c__AnonStorey2
		{
			internal Caravan caravan;

			internal PeaceTalks $this;

			public <Outcome_Disaster>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				FactionRelationKind playerRelationKind = this.$this.Faction.PlayerRelationKind;
				int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksDisasterRange.RandomInRange;
				this.$this.Faction.TryAffectGoodwillWith(Faction.OfPlayer, randomInRange, false, false, null, null);
				this.$this.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, this.caravan);
				incidentParms.faction = this.$this.Faction;
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, true);
				defaultPawnGroupMakerParms.generateFightersOnly = true;
				List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(this.caravan, list, false);
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(incidentParms.faction, new LordJob_AssaultColony(this.$this.Faction, true, true, false, false, true), map, list);
				}
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				GlobalTargetInfo target = (!list.Any<Pawn>()) ? GlobalTargetInfo.Invalid : new GlobalTargetInfo(list[0].Position, map, false);
				string label = "LetterLabelPeaceTalks_Disaster".Translate();
				string letterText = this.$this.GetLetterText("LetterPeaceTalks_Disaster".Translate(new object[]
				{
					this.$this.Faction.def.pawnsPlural.CapitalizeFirst(),
					this.$this.Faction.Name,
					Mathf.RoundToInt((float)randomInRange)
				}), this.caravan, playerRelationKind);
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref letterText, "LetterRelatedPawnsGroupGeneric".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), true, true);
				Find.LetterStack.ReceiveLetter(label, letterText, LetterDefOf.ThreatBig, target, this.$this.Faction, null);
			}
		}
	}
}
