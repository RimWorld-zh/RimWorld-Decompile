using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000882 RID: 2178
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x001AE320 File Offset: 0x001AC720
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x001AE33C File Offset: 0x001AC73C
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x001AE35C File Offset: 0x001AC75C
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x001AE3AA File Offset: 0x001AC7AA
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}
	}
}
