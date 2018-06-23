using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B7 RID: 1207
	public class InteractionWorker_RecruitAttempt : InteractionWorker
	{
		// Token: 0x04000CB5 RID: 3253
		private const float MinRecruitChance = 0.005f;

		// Token: 0x04000CB6 RID: 3254
		private const float BondRelationChanceFactor = 4f;

		// Token: 0x04000CB7 RID: 3255
		private static readonly SimpleCurve RecruitChanceFactorCurve_Wildness = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(0f, 2f),
				true
			}
		};

		// Token: 0x04000CB8 RID: 3256
		private static readonly SimpleCurve RecruitChanceFactorCurve_Opinion = new SimpleCurve
		{
			{
				new CurvePoint(-50f, 0f),
				true
			},
			{
				new CurvePoint(50f, 1f),
				true
			},
			{
				new CurvePoint(100f, 2f),
				true
			}
		};

		// Token: 0x04000CB9 RID: 3257
		private static readonly SimpleCurve RecruitChanceFactorCurve_Mood = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.25f),
				true
			},
			{
				new CurvePoint(0.1f, 0.25f),
				true
			},
			{
				new CurvePoint(0.25f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.5f),
				true
			}
		};

		// Token: 0x04000CBA RID: 3258
		private const int MenagerieThreshold = 10;

		// Token: 0x04000CBB RID: 3259
		private const float WildManTameChanceFactor = 2f;

		// Token: 0x06001586 RID: 5510 RVA: 0x000BF380 File Offset: 0x000BD780
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			if (!recipient.mindState.CheckStartMentalStateBecauseRecruitAttempted(initiator))
			{
				bool flag = recipient.NonHumanlikeOrWildMan() && !recipient.IsPrisoner;
				bool flag2 = initiator.InspirationDef == InspirationDefOf.Inspired_Recruitment && recipient.RaceProps.Humanlike && !flag;
				float num = 1f;
				if (flag2 || DebugSettings.instantRecruit)
				{
					num = 1f;
				}
				else
				{
					num *= ((!flag) ? initiator.GetStatValue(StatDefOf.RecruitPrisonerChance, true) : initiator.GetStatValue(StatDefOf.TameAnimalChance, true));
					if (recipient.IsWildMan() && flag)
					{
						num *= 2f;
					}
					else if (recipient.RaceProps.Humanlike)
					{
						num *= Mathf.Clamp01(1f - recipient.RecruitDifficulty(initiator.Faction, true));
					}
					else
					{
						num *= InteractionWorker_RecruitAttempt.RecruitChanceFactorCurve_Wildness.Evaluate(recipient.RaceProps.wildness);
					}
					if (!recipient.NonHumanlikeOrWildMan())
					{
						float x = (float)recipient.relations.OpinionOf(initiator);
						num *= InteractionWorker_RecruitAttempt.RecruitChanceFactorCurve_Opinion.Evaluate(x);
						if (recipient.needs.mood != null)
						{
							float curLevel = recipient.needs.mood.CurLevel;
							num *= InteractionWorker_RecruitAttempt.RecruitChanceFactorCurve_Mood.Evaluate(curLevel);
						}
					}
					if (initiator.relations.DirectRelationExists(PawnRelationDefOf.Bond, recipient))
					{
						num *= 4f;
					}
					num = Mathf.Clamp(num, 0.005f, 1f);
				}
				if (Rand.Chance(num))
				{
					InteractionWorker_RecruitAttempt.DoRecruit(initiator, recipient, num, true);
					extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptAccepted);
					if (flag2)
					{
						initiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Recruitment);
					}
				}
				else
				{
					string text;
					if (flag)
					{
						text = "TextMote_TameFail".Translate(new object[]
						{
							num.ToStringPercent()
						});
					}
					else
					{
						text = "TextMote_RecruitFail".Translate(new object[]
						{
							num.ToStringPercent()
						});
					}
					MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, text, 8f);
					extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptRejected);
				}
			}
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x000BF5E8 File Offset: 0x000BD9E8
		public static void DoRecruit(Pawn recruiter, Pawn recruitee, float recruitChance, bool useAudiovisualEffects = true)
		{
			string text = recruitee.LabelIndefinite();
			if (recruitee.guest != null)
			{
				recruitee.guest.SetGuestStatus(null, false);
			}
			bool flag = recruitee.Name != null;
			if (recruitee.Faction != recruiter.Faction)
			{
				recruitee.SetFaction(recruiter.Faction, recruiter);
			}
			if (recruitee.RaceProps.Humanlike)
			{
				if (useAudiovisualEffects)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelMessageRecruitSuccess".Translate(), "MessageRecruitSuccess".Translate(new object[]
					{
						recruiter,
						recruitee,
						recruitChance.ToStringPercent()
					}), LetterDefOf.PositiveEvent, recruitee, null, null);
				}
				TaleRecorder.RecordTale(TaleDefOf.Recruited, new object[]
				{
					recruiter,
					recruitee
				});
				recruiter.records.Increment(RecordDefOf.PrisonersRecruited);
				recruitee.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RecruitedMe, recruiter);
			}
			else
			{
				if (useAudiovisualEffects)
				{
					if (!flag)
					{
						Messages.Message("MessageTameAndNameSuccess".Translate(new object[]
						{
							recruiter.LabelShort,
							text,
							recruitChance.ToStringPercent(),
							recruitee.Name.ToStringFull
						}).AdjustedFor(recruitee, "PAWN"), recruitee, MessageTypeDefOf.PositiveEvent, true);
					}
					else
					{
						Messages.Message("MessageTameSuccess".Translate(new object[]
						{
							recruiter.LabelShort,
							text,
							recruitChance.ToStringPercent()
						}), recruitee, MessageTypeDefOf.PositiveEvent, true);
					}
					if (recruiter.Spawned && recruitee.Spawned)
					{
						MoteMaker.ThrowText((recruiter.DrawPos + recruitee.DrawPos) / 2f, recruiter.Map, "TextMote_TameSuccess".Translate(new object[]
						{
							recruitChance.ToStringPercent()
						}), 8f);
					}
				}
				recruiter.records.Increment(RecordDefOf.AnimalsTamed);
				RelationsUtility.TryDevelopBondRelation(recruiter, recruitee, 0.01f);
				float chance = Mathf.Lerp(0.02f, 1f, recruitee.RaceProps.wildness);
				if (Rand.Chance(chance) || recruitee.IsWildMan())
				{
					TaleRecorder.RecordTale(TaleDefOf.TamedAnimal, new object[]
					{
						recruiter,
						recruitee
					});
				}
				if (PawnsFinder.AllMapsWorldAndTemporary_Alive.Count((Pawn p) => p.playerSettings != null && p.playerSettings.Master == recruiter) >= 10)
				{
					TaleRecorder.RecordTale(TaleDefOf.IncreasedMenagerie, new object[]
					{
						recruiter,
						recruitee
					});
				}
			}
			if (recruitee.caller != null)
			{
				recruitee.caller.DoCall();
			}
		}
	}
}
