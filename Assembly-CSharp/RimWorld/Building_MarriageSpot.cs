using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Building_MarriageSpot : Building
	{
		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache1;

		public Building_MarriageSpot()
		{
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append(this.UsableNowStatus());
			return stringBuilder.ToString();
		}

		private string UsableNowStatus()
		{
			if (!this.AnyCoupleForWhichIsValid())
			{
				StringBuilder stringBuilder = new StringBuilder();
				Pair<Pawn, Pawn> pair;
				if (this.TryFindAnyFiancesCouple(out pair))
				{
					if (!MarriageSpotUtility.IsValidMarriageSpotFor(base.Position, pair.First, pair.Second, stringBuilder))
					{
						return "MarriageSpotNotUsable".Translate(new object[]
						{
							stringBuilder
						});
					}
				}
				else if (!MarriageSpotUtility.IsValidMarriageSpot(base.Position, base.Map, stringBuilder))
				{
					return "MarriageSpotNotUsable".Translate(new object[]
					{
						stringBuilder
					});
				}
			}
			return "MarriageSpotUsable".Translate();
		}

		private bool AnyCoupleForWhichIsValid()
		{
			return base.Map.mapPawns.FreeColonistsSpawned.Any(delegate(Pawn p)
			{
				Pawn firstDirectRelationPawn = p.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => x.Spawned);
				return firstDirectRelationPawn != null && MarriageSpotUtility.IsValidMarriageSpotFor(base.Position, p, firstDirectRelationPawn, null);
			});
		}

		private bool TryFindAnyFiancesCouple(out Pair<Pawn, Pawn> fiances)
		{
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => x.Spawned);
				if (firstDirectRelationPawn != null)
				{
					fiances = new Pair<Pawn, Pawn>(pawn, firstDirectRelationPawn);
					return true;
				}
			}
			fiances = default(Pair<Pawn, Pawn>);
			return false;
		}

		[CompilerGenerated]
		private bool <AnyCoupleForWhichIsValid>m__0(Pawn p)
		{
			Pawn firstDirectRelationPawn = p.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => x.Spawned);
			return firstDirectRelationPawn != null && MarriageSpotUtility.IsValidMarriageSpotFor(base.Position, p, firstDirectRelationPawn, null);
		}

		[CompilerGenerated]
		private static bool <TryFindAnyFiancesCouple>m__1(Pawn x)
		{
			return x.Spawned;
		}

		[CompilerGenerated]
		private static bool <AnyCoupleForWhichIsValid>m__2(Pawn x)
		{
			return x.Spawned;
		}
	}
}
