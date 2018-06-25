using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000881 RID: 2177
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060031AD RID: 12717 RVA: 0x001AE6E8 File Offset: 0x001ACAE8
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x001AE704 File Offset: 0x001ACB04
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x001AE724 File Offset: 0x001ACB24
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x001AE768 File Offset: 0x001ACB68
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn);
		}
	}
}
