using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087D RID: 2173
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x0600319F RID: 12703 RVA: 0x001AE45C File Offset: 0x001AC85C
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x001AE478 File Offset: 0x001AC878
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x001AE498 File Offset: 0x001AC898
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x001AE4E1 File Offset: 0x001AC8E1
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
			HuntUtility.ShowDesignationWarnings(pawn);
		}
	}
}
