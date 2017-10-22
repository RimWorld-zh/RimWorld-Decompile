using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompUseEffect_FixWorstHealthCondition : CompUseEffect
	{
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
				if (HealthUtility.TicksUntilDeathDueToBloodLoss(usedBy) < 5000)
				{
					Hediff hediff2 = this.FindMostBleedingHediff(usedBy);
					if (hediff2 != null)
					{
						this.Cure(hediff2);
						return;
					}
				}
				Hediff hediff3 = this.FindImmunizableHediffWhichCanKill(usedBy);
				if (hediff3 != null)
				{
					this.Cure(hediff3);
				}
				else
				{
					Hediff hediff4 = this.FindCarcinoma(usedBy);
					if (hediff4 != null)
					{
						this.Cure(hediff4);
					}
					else
					{
						Hediff hediff5 = this.FindNonInjuryMiscBadHediff(usedBy, true);
						if (hediff5 != null)
						{
							this.Cure(hediff5);
						}
						else
						{
							Hediff hediff6 = this.FindNonInjuryMiscBadHediff(usedBy, false);
							if (hediff6 != null)
							{
								this.Cure(hediff6);
							}
							else
							{
								BodyPartRecord bodyPartRecord = this.FindBiggestMissingBodyPart(usedBy);
								if (bodyPartRecord != null)
								{
									this.Cure(bodyPartRecord, usedBy);
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
										Hediff_Injury hediff_Injury = this.FindOldInjury(usedBy);
										if (hediff_Injury != null)
										{
											this.Cure(hediff_Injury);
										}
										else
										{
											Hediff_Injury hediff_Injury2 = this.FindInjury(usedBy);
											if (hediff_Injury2 != null)
											{
												this.Cure(hediff_Injury2);
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

		private Hediff FindLifeThreateningHediff(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible)
				{
					HediffStage curStage = hediffs[i].CurStage;
					if (curStage != null && curStage.lifeThreatening)
					{
						float num2 = (float)((hediffs[i].Part == null) ? 999.0 : hediffs[i].Part.coverageAbsWithChildren);
						if (hediff == null || num2 > num)
						{
							hediff = hediffs[i];
							num = num2;
						}
					}
				}
			}
			return hediff;
		}

		private Hediff FindMostBleedingHediff(Pawn pawn)
		{
			float num = 0f;
			Hediff hediff = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible)
				{
					float bleedRate = hediffs[i].BleedRate;
					if (bleedRate > 0.0 && (bleedRate > num || hediff == null))
					{
						num = bleedRate;
						hediff = hediffs[i];
					}
				}
			}
			return hediff;
		}

		private Hediff FindImmunizableHediffWhichCanKill(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].TryGetComp<HediffComp_Immunizable>() != null && this.CanKill(hediffs[i]))
				{
					float severity = hediffs[i].Severity;
					if (hediff == null || severity > num)
					{
						hediff = hediffs[i];
						num = severity;
					}
				}
			}
			return hediff;
		}

		private Hediff FindCarcinoma(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def == HediffDefOf.Carcinoma)
				{
					float num2 = (float)((hediffs[i].Part == null) ? 999.0 : hediffs[i].Part.coverageAbsWithChildren);
					if (hediff == null || num2 > num)
					{
						hediff = hediffs[i];
						num = num2;
					}
				}
			}
			return hediff;
		}

		private Hediff FindNonInjuryMiscBadHediff(Pawn pawn, bool onlyIfCanKill)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.isBad && !(hediffs[i] is Hediff_Injury) && !(hediffs[i] is Hediff_MissingPart) && !(hediffs[i] is Hediff_Addiction) && !(hediffs[i] is Hediff_AddedPart) && (!onlyIfCanKill || this.CanKill(hediffs[i])))
				{
					float num2 = (float)((hediffs[i].Part == null) ? 999.0 : hediffs[i].Part.coverageAbsWithChildren);
					if (hediff == null || num2 > num)
					{
						hediff = hediffs[i];
						num = num2;
					}
				}
			}
			return hediff;
		}

		private BodyPartRecord FindBiggestMissingBodyPart(Pawn pawn)
		{
			BodyPartRecord bodyPartRecord = null;
			foreach (Hediff_MissingPart missingPartsCommonAncestor in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
			{
				if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(missingPartsCommonAncestor.Part) && (bodyPartRecord == null || missingPartsCommonAncestor.Part.coverageAbsWithChildren > bodyPartRecord.coverageAbsWithChildren))
				{
					bodyPartRecord = missingPartsCommonAncestor.Part;
				}
			}
			return bodyPartRecord;
		}

		private Hediff_Addiction FindAddiction(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			int num = 0;
			Hediff_Addiction result;
			while (true)
			{
				if (num < hediffs.Count)
				{
					Hediff_Addiction hediff_Addiction = hediffs[num] as Hediff_Addiction;
					if (hediff_Addiction != null && hediff_Addiction.Visible)
					{
						result = hediff_Addiction;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		private Hediff_Injury FindOldInjury(Pawn pawn)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.IsOld() && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		private Hediff_Injury FindInjury(Pawn pawn)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		private void Cure(Hediff hediff)
		{
			Pawn pawn = hediff.pawn;
			pawn.health.RemoveHediff(hediff);
			if (hediff.def.cureAllAtOnceIfCuredByItem)
			{
				while (true)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediff.def, false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
						continue;
					}
					break;
				}
			}
			Messages.Message("MessageHediffCuredByItem".Translate(hediff.LabelBase), (Thing)pawn, MessageTypeDefOf.PositiveEvent);
		}

		private void Cure(BodyPartRecord part, Pawn pawn)
		{
			pawn.health.RestorePart(part, null, true);
			Messages.Message("MessageBodyPartCuredByItem".Translate(part.def.label), (Thing)pawn, MessageTypeDefOf.PositiveEvent);
		}

		private bool CanKill(Hediff hediff)
		{
			if (hediff.def.stages != null)
			{
				for (int i = 0; i < hediff.def.stages.Count; i++)
				{
					if (hediff.def.stages[i].lifeThreatening)
						goto IL_0035;
				}
			}
			bool result = hediff.def.lethalSeverity >= 0.0;
			goto IL_0073;
			IL_0035:
			result = true;
			goto IL_0073;
			IL_0073:
			return result;
		}
	}
}
