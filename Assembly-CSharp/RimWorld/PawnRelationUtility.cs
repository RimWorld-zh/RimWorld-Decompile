using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BF RID: 1215
	public static class PawnRelationUtility
	{
		// Token: 0x060015BA RID: 5562 RVA: 0x000C17FC File Offset: 0x000BFBFC
		public static IEnumerable<PawnRelationDef> GetRelations(this Pawn me, Pawn other)
		{
			if (me == other)
			{
				yield break;
			}
			if (!me.RaceProps.IsFlesh || !other.RaceProps.IsFlesh)
			{
				yield break;
			}
			if (!me.relations.RelatedToAnyoneOrAnyoneRelatedToMe || !other.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				yield break;
			}
			try
			{
				bool anyNonKinFamilyByBloodRelation = false;
				List<PawnRelationDef> defs = DefDatabase<PawnRelationDef>.AllDefsListForReading;
				int i = 0;
				int count = defs.Count;
				while (i < count)
				{
					PawnRelationDef def = defs[i];
					if (def != PawnRelationDefOf.Kin)
					{
						if (def.Worker.InRelation(me, other))
						{
							if (def.familyByBloodRelation)
							{
								anyNonKinFamilyByBloodRelation = true;
							}
							yield return def;
						}
					}
					i++;
				}
				if (!anyNonKinFamilyByBloodRelation && PawnRelationDefOf.Kin.Worker.InRelation(me, other))
				{
					yield return PawnRelationDefOf.Kin;
				}
			}
			finally
			{
			}
			yield break;
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x000C1830 File Offset: 0x000BFC30
		public static PawnRelationDef GetMostImportantRelation(this Pawn me, Pawn other)
		{
			PawnRelationDef pawnRelationDef = null;
			foreach (PawnRelationDef pawnRelationDef2 in me.GetRelations(other))
			{
				if (pawnRelationDef == null || pawnRelationDef2.importance > pawnRelationDef.importance)
				{
					pawnRelationDef = pawnRelationDef2;
				}
			}
			return pawnRelationDef;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x000C18AC File Offset: 0x000BFCAC
		public static void Notify_PawnsSeenByPlayer(IEnumerable<Pawn> seenPawns, out string pawnRelationsInfo, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x;
			bool flag = false;
			foreach (Pawn pawn in seenPawns)
			{
				if (pawn.RaceProps.IsFlesh)
				{
					if (informEvenIfSeenBefore || !pawn.relations.everSeenByPlayer)
					{
						pawn.relations.everSeenByPlayer = true;
						bool flag2 = false;
						foreach (Pawn pawn2 in enumerable)
						{
							if (pawn != pawn2)
							{
								PawnRelationDef mostImportantRelation = pawn2.GetMostImportantRelation(pawn);
								if (mostImportantRelation != null)
								{
									if (!flag2)
									{
										flag2 = true;
										if (flag)
										{
											stringBuilder.AppendLine();
										}
										if (writeSeenPawnsNames)
										{
											stringBuilder.AppendLine(pawn.KindLabel.CapitalizeFirst() + " " + pawn.LabelShort + ":");
										}
									}
									flag = true;
									stringBuilder.AppendLine("  " + "Relationship".Translate(new object[]
									{
										mostImportantRelation.GetGenderSpecificLabelCap(pawn),
										pawn2.KindLabel + " " + pawn2.LabelShort
									}));
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				pawnRelationsInfo = stringBuilder.ToString().TrimEndNewlines();
			}
			else
			{
				pawnRelationsInfo = null;
			}
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x000C1A9C File Offset: 0x000BFE9C
		public static void Notify_PawnsSeenByPlayer_Letter(IEnumerable<Pawn> seenPawns, ref string letterLabel, ref string letterText, string relationsInfoHeader, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			string text;
			PawnRelationUtility.Notify_PawnsSeenByPlayer(seenPawns, out text, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				if (letterLabel.NullOrEmpty())
				{
					letterLabel = "LetterLabelNoticedRelatedPawns".Translate();
				}
				else
				{
					letterLabel = letterLabel + " " + "RelationshipAppendedLetterSuffix".Translate();
				}
				if (!letterText.NullOrEmpty())
				{
					letterText += "\n\n";
				}
				letterText = letterText + relationsInfoHeader + "\n\n" + text;
			}
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x000C1B24 File Offset: 0x000BFF24
		public static void Notify_PawnsSeenByPlayer_Letter_Send(IEnumerable<Pawn> seenPawns, string relationsInfoHeader, LetterDef letterDef, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			string label = "";
			string text = "";
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(seenPawns, ref label, ref text, relationsInfoHeader, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				Pawn pawn = null;
				foreach (Pawn pawn2 in seenPawns)
				{
					if (PawnRelationUtility.GetMostImportantColonyRelative(pawn2) != null)
					{
						pawn = pawn2;
						break;
					}
				}
				if (pawn == null)
				{
					pawn = seenPawns.FirstOrDefault<Pawn>();
				}
				Find.LetterStack.ReceiveLetter(label, text, letterDef, pawn, null, null);
			}
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x000C1BD8 File Offset: 0x000BFFD8
		public static bool TryAppendRelationsWithColonistsInfo(ref string text, Pawn pawn)
		{
			string text2 = null;
			return PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref text2, pawn);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x000C1BF8 File Offset: 0x000BFFF8
		public static bool TryAppendRelationsWithColonistsInfo(ref string text, ref string title, Pawn pawn)
		{
			Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
			bool result;
			if (mostImportantColonyRelative == null)
			{
				result = false;
			}
			else
			{
				if (title != null)
				{
					title = title + " " + "RelationshipAppendedLetterSuffix".Translate();
				}
				string genderSpecificLabel = mostImportantColonyRelative.GetMostImportantRelation(pawn).GetGenderSpecificLabel(pawn);
				string text2 = "\n\n";
				if (mostImportantColonyRelative.IsColonist)
				{
					text2 += "RelationshipAppendedLetterTextColonist".Translate(new object[]
					{
						mostImportantColonyRelative.LabelShort,
						genderSpecificLabel
					});
				}
				else
				{
					text2 += "RelationshipAppendedLetterTextPrisoner".Translate(new object[]
					{
						mostImportantColonyRelative.LabelShort,
						genderSpecificLabel
					});
				}
				text += text2.AdjustedFor(pawn, "PAWN");
				result = true;
			}
			return result;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x000C1CC4 File Offset: 0x000C00C4
		public static Pawn GetMostImportantColonyRelative(Pawn pawn)
		{
			Pawn result;
			if (pawn.relations == null || !pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				result = null;
			}
			else
			{
				IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
				where x.relations.everSeenByPlayer
				select x;
				float num = 0f;
				Pawn pawn2 = null;
				foreach (Pawn pawn3 in enumerable)
				{
					PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(pawn3);
					if (mostImportantRelation != null)
					{
						if (pawn2 == null || mostImportantRelation.importance > num)
						{
							num = mostImportantRelation.importance;
							pawn2 = pawn3;
						}
					}
				}
				result = pawn2;
			}
			return result;
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x000C1DAC File Offset: 0x000C01AC
		public static float MaxPossibleBioAgeAt(float myBiologicalAge, float myChronologicalAge, float atChronologicalAge)
		{
			float num = Mathf.Min(myBiologicalAge, myChronologicalAge - atChronologicalAge);
			float result;
			if (num < 0f)
			{
				result = -1f;
			}
			else
			{
				result = num;
			}
			return result;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x000C1DE4 File Offset: 0x000C01E4
		public static float MinPossibleBioAgeAt(float myBiologicalAge, float atChronologicalAge)
		{
			return Mathf.Max(myBiologicalAge - atChronologicalAge, 0f);
		}
	}
}
