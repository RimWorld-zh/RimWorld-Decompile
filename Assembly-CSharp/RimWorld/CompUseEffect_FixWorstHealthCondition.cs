using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000761 RID: 1889
	public class CompUseEffect_FixWorstHealthCondition : CompUseEffect
	{
		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060029BE RID: 10686 RVA: 0x001624A0 File Offset: 0x001608A0
		private float HandCoverageAbsWithChildren
		{
			get
			{
				return ThingDefOf.Human.race.body.GetPartsWithDef(BodyPartDefOf.Hand).First<BodyPartRecord>().coverageAbsWithChildren;
			}
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x001624D8 File Offset: 0x001608D8
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			Hediff hediff = this.FindLifeThreateningHediff(usedBy);
			if (hediff != null)
			{
				this.Cure(hediff);
			}
			else
			{
				if (HealthUtility.TicksUntilDeathDueToBloodLoss(usedBy) < 2500)
				{
					Hediff hediff2 = this.FindMostBleedingHediff(usedBy);
					if (hediff2 != null)
					{
						this.Cure(hediff2);
						return;
					}
				}
				if (usedBy.health.hediffSet.GetBrain() != null)
				{
					Hediff_Injury hediff_Injury = this.FindPermanentInjury(usedBy, Gen.YieldSingle<BodyPartRecord>(usedBy.health.hediffSet.GetBrain()));
					if (hediff_Injury != null)
					{
						this.Cure(hediff_Injury);
						return;
					}
				}
				BodyPartRecord bodyPartRecord = this.FindBiggestMissingBodyPart(usedBy, this.HandCoverageAbsWithChildren);
				if (bodyPartRecord != null)
				{
					this.Cure(bodyPartRecord, usedBy);
				}
				else
				{
					Hediff_Injury hediff_Injury2 = this.FindPermanentInjury(usedBy, from x in usedBy.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
					where x.def == BodyPartDefOf.Eye
					select x);
					if (hediff_Injury2 != null)
					{
						this.Cure(hediff_Injury2);
					}
					else
					{
						Hediff hediff3 = this.FindImmunizableHediffWhichCanKill(usedBy);
						if (hediff3 != null)
						{
							this.Cure(hediff3);
						}
						else
						{
							Hediff hediff4 = this.FindNonInjuryMiscBadHediff(usedBy, true);
							if (hediff4 != null)
							{
								this.Cure(hediff4);
							}
							else
							{
								Hediff hediff5 = this.FindNonInjuryMiscBadHediff(usedBy, false);
								if (hediff5 != null)
								{
									this.Cure(hediff5);
								}
								else
								{
									if (usedBy.health.hediffSet.GetBrain() != null)
									{
										Hediff_Injury hediff_Injury3 = this.FindInjury(usedBy, Gen.YieldSingle<BodyPartRecord>(usedBy.health.hediffSet.GetBrain()));
										if (hediff_Injury3 != null)
										{
											this.Cure(hediff_Injury3);
											return;
										}
									}
									BodyPartRecord bodyPartRecord2 = this.FindBiggestMissingBodyPart(usedBy, 0f);
									if (bodyPartRecord2 != null)
									{
										this.Cure(bodyPartRecord2, usedBy);
									}
									else
									{
										Hediff_Addiction hediff_Addiction = this.FindAddiction(usedBy);
										if (hediff_Addiction != null)
										{
											this.Cure(hediff_Addiction);
										}
										else
										{
											Hediff_Injury hediff_Injury4 = this.FindPermanentInjury(usedBy, null);
											if (hediff_Injury4 != null)
											{
												this.Cure(hediff_Injury4);
											}
											else
											{
												Hediff_Injury hediff_Injury5 = this.FindInjury(usedBy, null);
												if (hediff_Injury5 != null)
												{
													this.Cure(hediff_Injury5);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x00162718 File Offset: 0x00160B18
		private Hediff FindLifeThreateningHediff(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem)
				{
					if (!hediffs[i].FullyImmune())
					{
						HediffStage curStage = hediffs[i].CurStage;
						bool flag = curStage != null && curStage.lifeThreatening;
						bool flag2 = hediffs[i].def.lethalSeverity >= 0f && hediffs[i].Severity / hediffs[i].def.lethalSeverity >= 0.8f;
						if (flag || flag2)
						{
							float num2 = (hediffs[i].Part == null) ? 999f : hediffs[i].Part.coverageAbsWithChildren;
							if (hediff == null || num2 > num)
							{
								hediff = hediffs[i];
								num = num2;
							}
						}
					}
				}
			}
			return hediff;
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x0016286C File Offset: 0x00160C6C
		private Hediff FindMostBleedingHediff(Pawn pawn)
		{
			float num = 0f;
			Hediff hediff = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem)
				{
					float bleedRate = hediffs[i].BleedRate;
					if (bleedRate > 0f && (bleedRate > num || hediff == null))
					{
						num = bleedRate;
						hediff = hediffs[i];
					}
				}
			}
			return hediff;
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x00162918 File Offset: 0x00160D18
		private Hediff FindImmunizableHediffWhichCanKill(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem)
				{
					if (hediffs[i].TryGetComp<HediffComp_Immunizable>() != null)
					{
						if (!hediffs[i].FullyImmune())
						{
							if (this.CanEverKill(hediffs[i]))
							{
								float severity = hediffs[i].Severity;
								if (hediff == null || severity > num)
								{
									hediff = hediffs[i];
									num = severity;
								}
							}
						}
					}
				}
			}
			return hediff;
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x001629F8 File Offset: 0x00160DF8
		private Hediff FindNonInjuryMiscBadHediff(Pawn pawn, bool onlyIfCanKill)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.isBad && hediffs[i].def.everCurableByItem)
				{
					if (!(hediffs[i] is Hediff_Injury) && !(hediffs[i] is Hediff_MissingPart) && !(hediffs[i] is Hediff_Addiction) && !(hediffs[i] is Hediff_AddedPart))
					{
						if (!onlyIfCanKill || this.CanEverKill(hediffs[i]))
						{
							float num2 = (hediffs[i].Part == null) ? 999f : hediffs[i].Part.coverageAbsWithChildren;
							if (hediff == null || num2 > num)
							{
								hediff = hediffs[i];
								num = num2;
							}
						}
					}
				}
			}
			return hediff;
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x00162B34 File Offset: 0x00160F34
		private BodyPartRecord FindBiggestMissingBodyPart(Pawn pawn, float minCoverage = 0f)
		{
			BodyPartRecord bodyPartRecord = null;
			foreach (Hediff_MissingPart hediff_MissingPart in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
			{
				if (hediff_MissingPart.Part.coverageAbsWithChildren >= minCoverage)
				{
					if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff_MissingPart.Part))
					{
						if (bodyPartRecord == null || hediff_MissingPart.Part.coverageAbsWithChildren > bodyPartRecord.coverageAbsWithChildren)
						{
							bodyPartRecord = hediff_MissingPart.Part;
						}
					}
				}
			}
			return bodyPartRecord;
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x00162BFC File Offset: 0x00160FFC
		private Hediff_Addiction FindAddiction(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && hediff_Addiction.Visible && hediff_Addiction.def.everCurableByItem)
				{
					return hediff_Addiction;
				}
			}
			return null;
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x00162C78 File Offset: 0x00161078
		private Hediff_Injury FindPermanentInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.IsPermanent() && hediff_Injury2.def.everCurableByItem)
				{
					if (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part))
					{
						if (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity)
						{
							hediff_Injury = hediff_Injury2;
						}
					}
				}
			}
			return hediff_Injury;
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x00162D30 File Offset: 0x00161130
		private Hediff_Injury FindInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.def.everCurableByItem)
				{
					if (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part))
					{
						if (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity)
						{
							hediff_Injury = hediff_Injury2;
						}
					}
				}
			}
			return hediff_Injury;
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x00162DDC File Offset: 0x001611DC
		private void Cure(Hediff hediff)
		{
			Pawn pawn = hediff.pawn;
			pawn.health.RemoveHediff(hediff);
			if (hediff.def.cureAllAtOnceIfCuredByItem)
			{
				int num = 0;
				for (;;)
				{
					num++;
					if (num > 10000)
					{
						break;
					}
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediff.def, false);
					if (firstHediffOfDef == null)
					{
						goto Block_3;
					}
					pawn.health.RemoveHediff(firstHediffOfDef);
				}
				Log.Error("Too many iterations.", false);
				Block_3:;
			}
			Messages.Message("MessageHediffCuredByItem".Translate(new object[]
			{
				hediff.LabelBase.CapitalizeFirst()
			}), pawn, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x00162E97 File Offset: 0x00161297
		private void Cure(BodyPartRecord part, Pawn pawn)
		{
			pawn.health.RestorePart(part, null, true);
			Messages.Message("MessageBodyPartCuredByItem".Translate(new object[]
			{
				part.LabelCap
			}), pawn, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x00162ED4 File Offset: 0x001612D4
		private bool CanEverKill(Hediff hediff)
		{
			if (hediff.def.stages != null)
			{
				for (int i = 0; i < hediff.def.stages.Count; i++)
				{
					if (hediff.def.stages[i].lifeThreatening)
					{
						return true;
					}
				}
			}
			return hediff.def.lethalSeverity >= 0f;
		}
	}
}
