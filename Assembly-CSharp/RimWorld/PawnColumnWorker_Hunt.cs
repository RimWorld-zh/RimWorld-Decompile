using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087F RID: 2175
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060031A2 RID: 12706 RVA: 0x001AE80C File Offset: 0x001ACC0C
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x001AE828 File Offset: 0x001ACC28
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x001AE848 File Offset: 0x001ACC48
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x001AE88C File Offset: 0x001ACC8C
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
			HuntUtility.ShowDesignationWarnings(pawn);
		}
	}
}
