using System;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A9 RID: 1705
	public class Building_MarriageSpot : Building
	{
		// Token: 0x06002488 RID: 9352 RVA: 0x00138E24 File Offset: 0x00137224
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

		// Token: 0x06002489 RID: 9353 RVA: 0x00138E74 File Offset: 0x00137274
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

		// Token: 0x0600248A RID: 9354 RVA: 0x00138F24 File Offset: 0x00137324
		private bool AnyCoupleForWhichIsValid()
		{
			return base.Map.mapPawns.FreeColonistsSpawned.Any(delegate(Pawn p)
			{
				Pawn firstDirectRelationPawn = p.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => x.Spawned);
				return firstDirectRelationPawn != null && MarriageSpotUtility.IsValidMarriageSpotFor(base.Position, p, firstDirectRelationPawn, null);
			});
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x00138F5C File Offset: 0x0013735C
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
	}
}
