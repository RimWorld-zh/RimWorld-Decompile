using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001F7 RID: 503
	public class ThoughtWorker_NotBondedAnimalMaster : ThoughtWorker_BondedAnimalMaster
	{
		// Token: 0x060009BB RID: 2491 RVA: 0x00057AE0 File Offset: 0x00055EE0
		protected override bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster != p && TrainableUtility.MinimumHandlingSkill(animal) <= p.skills.GetSkill(SkillDefOf.Animals).Level;
		}
	}
}
