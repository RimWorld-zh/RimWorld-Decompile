using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000883 RID: 2179
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060031AE RID: 12718 RVA: 0x001AE2F4 File Offset: 0x001AC6F4
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x001AE310 File Offset: 0x001AC710
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x001AE330 File Offset: 0x001AC730
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x001AE379 File Offset: 0x001AC779
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn);
		}
	}
}
