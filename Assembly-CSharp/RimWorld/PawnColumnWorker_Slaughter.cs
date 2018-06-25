using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000880 RID: 2176
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x001AE64C File Offset: 0x001ACA4C
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x001AE668 File Offset: 0x001ACA68
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x001AE688 File Offset: 0x001ACA88
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x001AE6D6 File Offset: 0x001ACAD6
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}
	}
}
