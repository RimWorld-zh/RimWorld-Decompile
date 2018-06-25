using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087F RID: 2175
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060031A3 RID: 12707 RVA: 0x001AE5A4 File Offset: 0x001AC9A4
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x001AE5C0 File Offset: 0x001AC9C0
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x001AE5E0 File Offset: 0x001AC9E0
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x001AE624 File Offset: 0x001ACA24
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
			HuntUtility.ShowDesignationWarnings(pawn);
		}
	}
}
