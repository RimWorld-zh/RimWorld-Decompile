using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_BondedAnimalMaster : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			int num = 0;
			ThoughtState result;
			while (true)
			{
				if (num < directRelations.Count)
				{
					DirectPawnRelation directPawnRelation = directRelations[num];
					Pawn otherPawn = directPawnRelation.otherPawn;
					if (directPawnRelation.def == PawnRelationDefOf.Bond && !otherPawn.Dead && otherPawn.Spawned && otherPawn.Faction == Faction.OfPlayer && otherPawn.training.IsCompleted(TrainableDefOf.Obedience) && p.skills.GetSkill(SkillDefOf.Animals).Level >= TrainableUtility.MinimumHandlingSkill(otherPawn) && this.AnimalMasterCheck(p, otherPawn))
					{
						result = ThoughtState.ActiveWithReason(otherPawn.LabelShort);
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		protected virtual bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster == p;
		}
	}
}
