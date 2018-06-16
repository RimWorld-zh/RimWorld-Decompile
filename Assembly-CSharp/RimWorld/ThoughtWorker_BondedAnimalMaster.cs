using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020001F6 RID: 502
	public class ThoughtWorker_BondedAnimalMaster : ThoughtWorker
	{
		// Token: 0x060009B8 RID: 2488 RVA: 0x000579D8 File Offset: 0x00055DD8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				Pawn otherPawn = directPawnRelation.otherPawn;
				if (directPawnRelation.def == PawnRelationDefOf.Bond && !otherPawn.Dead && otherPawn.Spawned && otherPawn.Faction == Faction.OfPlayer && otherPawn.training.HasLearned(TrainableDefOf.Obedience) && p.skills.GetSkill(SkillDefOf.Animals).Level >= TrainableUtility.MinimumHandlingSkill(otherPawn) && this.AnimalMasterCheck(p, otherPawn))
				{
					return ThoughtState.ActiveWithReason(otherPawn.LabelShort);
				}
			}
			return false;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x00057AB4 File Offset: 0x00055EB4
		protected virtual bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster == p;
		}
	}
}
