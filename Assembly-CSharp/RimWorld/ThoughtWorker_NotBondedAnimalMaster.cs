using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001F7 RID: 503
	public class ThoughtWorker_NotBondedAnimalMaster : ThoughtWorker_BondedAnimalMaster
	{
		// Token: 0x060009B9 RID: 2489 RVA: 0x00057B24 File Offset: 0x00055F24
		protected override bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster != p && TrainableUtility.MinimumHandlingSkill(animal) <= p.skills.GetSkill(SkillDefOf.Animals).Level;
		}
	}
}
