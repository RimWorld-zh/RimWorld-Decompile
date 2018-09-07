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

namespace RimWorld
{
	public static class PawnRelationUtility
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

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

		public static void Notify_PawnsSeenByPlayer_Letter_Send(IEnumerable<Pawn> seenPawns, string relationsInfoHeader, LetterDef letterDef, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(seenPawns, ref empty, ref empty2, relationsInfoHeader, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!empty2.NullOrEmpty())
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
				Find.LetterStack.ReceiveLetter(empty, empty2, letterDef, pawn, null, null);
			}
		}

		public static bool TryAppendRelationsWithColonistsInfo(ref string text, Pawn pawn)
		{
			string text2 = null;
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
			if (mostImportantColonyRelative.IsColonist)
			{
				text = text + "\n\n" + "RelationshipAppendedLetterTextColonist".Translate(new object[]
				{
					mostImportantColonyRelative.LabelShort,
					genderSpecificLabel
				}).AdjustedFor(pawn, "PAWN");
			}
			else
			{
				text = text + "\n\n" + "RelationshipAppendedLetterTextPrisoner".Translate(new object[]
				{
					mostImportantColonyRelative.LabelShort,
					genderSpecificLabel
				}).AdjustedFor(pawn, "PAWN");
			}
			return true;
		}

		public static Pawn GetMostImportantColonyRelative(Pawn pawn)
		{
			if (pawn.relations == null || !pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return null;
			}
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
			return pawn2;
		}

		public static float MaxPossibleBioAgeAt(float myBiologicalAge, float myChronologicalAge, float atChronologicalAge)
		{
			float num = Mathf.Min(myBiologicalAge, myChronologicalAge - atChronologicalAge);
			if (num < 0f)
			{
				return -1f;
			}
			return num;
		}

		public static float MinPossibleBioAgeAt(float myBiologicalAge, float atChronologicalAge)
		{
			return Mathf.Max(myBiologicalAge - atChronologicalAge, 0f);
		}

		[CompilerGenerated]
		private static bool <Notify_PawnsSeenByPlayer>m__0(Pawn x)
		{
			return x.relations.everSeenByPlayer;
		}

		[CompilerGenerated]
		private static bool <GetMostImportantColonyRelative>m__1(Pawn x)
		{
			return x.relations.everSeenByPlayer;
		}

		[CompilerGenerated]
		private sealed class <GetRelations>c__Iterator0 : IEnumerable, IEnumerable<PawnRelationDef>, IEnumerator, IDisposable, IEnumerator<PawnRelationDef>
		{
			internal Pawn me;

			internal Pawn other;

			internal bool <anyNonKinFamilyByBloodRelation>__1;

			internal List<PawnRelationDef> <defs>__1;

			internal int <i>__2;

			internal int <count>__2;

			internal PawnRelationDef <def>__3;

			internal PawnRelationDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetRelations>c__Iterator0()
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
					if (me == other)
					{
						return false;
					}
					if (!me.RaceProps.IsFlesh || !other.RaceProps.IsFlesh)
					{
						return false;
					}
					if (!me.relations.RelatedToAnyoneOrAnyoneRelatedToMe || !other.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
					{
						return false;
					}
					num = 4294967293u;
					break;
				case 1u:
				case 2u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_163:
						i++;
						break;
					case 2u:
						goto IL_1DE;
					default:
						anyNonKinFamilyByBloodRelation = false;
						defs = DefDatabase<PawnRelationDef>.AllDefsListForReading;
						i = 0;
						count = defs.Count;
						break;
					}
					if (i >= count)
					{
						if (!anyNonKinFamilyByBloodRelation && PawnRelationDefOf.Kin.Worker.InRelation(me, other))
						{
							this.$current = PawnRelationDefOf.Kin;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
					}
					else
					{
						def = defs[i];
						if (def == PawnRelationDefOf.Kin)
						{
							goto IL_163;
						}
						if (def.Worker.InRelation(me, other))
						{
							if (def.familyByBloodRelation)
							{
								anyNonKinFamilyByBloodRelation = true;
							}
							this.$current = def;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_163;
					}
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				IL_1DE:
				this.$PC = -1;
				return false;
			}

			PawnRelationDef IEnumerator<PawnRelationDef>.Current
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
				case 2u:
					try
					{
					}
					finally
					{
						this.<>__Finally0();
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.PawnRelationDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<PawnRelationDef> IEnumerable<PawnRelationDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PawnRelationUtility.<GetRelations>c__Iterator0 <GetRelations>c__Iterator = new PawnRelationUtility.<GetRelations>c__Iterator0();
				<GetRelations>c__Iterator.me = me;
				<GetRelations>c__Iterator.other = other;
				return <GetRelations>c__Iterator;
			}

			private void <>__Finally0()
			{
			}
		}
	}
}
