using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000883 RID: 2179
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060031B0 RID: 12720 RVA: 0x001AE3BC File Offset: 0x001AC7BC
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x001AE3D8 File Offset: 0x001AC7D8
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x001AE3F8 File Offset: 0x001AC7F8
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x001AE441 File Offset: 0x001AC841
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn);
		}
	}
}
