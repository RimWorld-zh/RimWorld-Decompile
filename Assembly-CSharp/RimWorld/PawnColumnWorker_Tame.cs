using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000881 RID: 2177
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060031AC RID: 12716 RVA: 0x001AE950 File Offset: 0x001ACD50
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x001AE96C File Offset: 0x001ACD6C
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x001AE98C File Offset: 0x001ACD8C
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x001AE9D0 File Offset: 0x001ACDD0
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn);
		}
	}
}
