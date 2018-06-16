using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000882 RID: 2178
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060031A9 RID: 12713 RVA: 0x001AE258 File Offset: 0x001AC658
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x001AE274 File Offset: 0x001AC674
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x001AE294 File Offset: 0x001AC694
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x001AE2E2 File Offset: 0x001AC6E2
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}
	}
}
