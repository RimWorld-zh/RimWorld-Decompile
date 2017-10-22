using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class PawnRelationUtility
	{
		public static IEnumerable<PawnRelationDef> GetRelations(this Pawn me, Pawn other)
		{
			if (me != other && me.RaceProps.IsFlesh && other.RaceProps.IsFlesh && me.relations.RelatedToAnyoneOrAnyoneRelatedToMe && other.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				ProfilerThreadCheck.BeginSample("GetRelations()");
				try
				{
					bool isKin = false;
					bool anyNonKinFamilyByBloodRelation = false;
					List<PawnRelationDef> defs = DefDatabase<PawnRelationDef>.AllDefsListForReading;
					int i = 0;
					int count = defs.Count;
					while (i < count)
					{
						PawnRelationDef def = defs[i];
						if (def.Worker.InRelation(me, other))
						{
							if (def == PawnRelationDefOf.Kin)
							{
								isKin = true;
							}
							else
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
					if (isKin && !anyNonKinFamilyByBloodRelation)
					{
						yield return PawnRelationDefOf.Kin;
					}
				}
				finally
				{
					((_003CGetRelations_003Ec__IteratorDA)/*Error near IL_01ca: stateMachine*/)._003C_003E__Finally0();
				}
			}
		}

		public static PawnRelationDef GetMostImportantRelation(this Pawn me, Pawn other)
		{
			PawnRelationDef pawnRelationDef = null;
			foreach (PawnRelationDef relation in me.GetRelations(other))
			{
				if (pawnRelationDef == null || relation.importance > pawnRelationDef.importance)
				{
					pawnRelationDef = relation;
				}
			}
			return pawnRelationDef;
		}

		public static void Notify_PawnsSeenByPlayer(IEnumerable<Pawn> seenPawns, out string pawnRelationsInfo, bool informEvenIfSeenBefore = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Pawn> enumerable = from x in seenPawns
			where x.RaceProps.IsFlesh
			select x;
			IEnumerable<Pawn> enumerable2 = from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods
			where x.RaceProps.Humanlike && (x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer) && x.relations.everSeenByPlayer
			select x;
			if (!informEvenIfSeenBefore)
			{
				enumerable = from x in enumerable
				where !x.relations.everSeenByPlayer
				select x;
			}
			bool flag = false;
			foreach (Pawn item in enumerable)
			{
				bool flag2 = false;
				foreach (Pawn item2 in enumerable2)
				{
					if (item != item2)
					{
						PawnRelationDef mostImportantRelation = item.GetMostImportantRelation(item2);
						if (mostImportantRelation != null)
						{
							if (!flag2)
							{
								flag2 = true;
								if (flag)
								{
									stringBuilder.AppendLine();
								}
								stringBuilder.AppendLine(item.KindLabel.CapitalizeFirst() + " " + item.LabelShort + ":");
							}
							flag = true;
							stringBuilder.AppendLine("  " + mostImportantRelation.GetGenderSpecificLabelCap(item2) + " - " + item2.KindLabel + " " + item2.LabelShort);
							item.relations.everSeenByPlayer = true;
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
				pawnRelationsInfo = (string)null;
			}
		}

		public static void Notify_PawnsSeenByPlayer(IEnumerable<Pawn> seenPawns, ref string letterLabel, ref string letterText, string relationsInfoHeader, bool informEvenIfSeenBefore = false)
		{
			string text = default(string);
			PawnRelationUtility.Notify_PawnsSeenByPlayer(seenPawns, out text, informEvenIfSeenBefore);
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

		public static bool TryAppendRelationsWithColonistsInfo(ref string text, Pawn pawn)
		{
			string text2 = (string)null;
			return PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref text2, pawn);
		}

		public static bool TryAppendRelationsWithColonistsInfo(ref string text, ref string title, Pawn pawn)
		{
			Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
			if (mostImportantColonyRelative == null)
			{
				return false;
			}
			if (title != null)
			{
				title = title + " " + "RelationshipAppendedLetterSuffix".Translate();
			}
			string genderSpecificLabel = mostImportantColonyRelative.GetMostImportantRelation(pawn).GetGenderSpecificLabel(pawn);
			string str = "\n\n";
			str = ((!mostImportantColonyRelative.IsColonist) ? (str + "RelationshipAppendedLetterTextPrisoner".Translate(mostImportantColonyRelative.LabelShort, genderSpecificLabel)) : (str + "RelationshipAppendedLetterTextColonist".Translate(mostImportantColonyRelative.LabelShort, genderSpecificLabel)));
			text += str.AdjustedFor(pawn);
			return true;
		}

		public static Pawn GetMostImportantColonyRelative(Pawn pawn)
		{
			IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMaps_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x;
			float num = 0f;
			Pawn pawn2 = null;
			foreach (Pawn item in enumerable)
			{
				PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(item);
				if (mostImportantRelation != null && (pawn2 == null || mostImportantRelation.importance > num))
				{
					num = mostImportantRelation.importance;
					pawn2 = item;
				}
			}
			return pawn2;
		}

		public static float MaxPossibleBioAgeAt(float myBiologicalAge, float myChronologicalAge, float atChronologicalAge)
		{
			float num = Mathf.Min(myBiologicalAge, myChronologicalAge - atChronologicalAge);
			if (num < 0.0)
			{
				return -1f;
			}
			return num;
		}

		public static float MinPossibleBioAgeAt(float myBiologicalAge, float atChronologicalAge)
		{
			return Mathf.Max(myBiologicalAge - atChronologicalAge, 0f);
		}
	}
}
