using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087F RID: 2175
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060031A9 RID: 12713 RVA: 0x001AE5A4 File Offset: 0x001AC9A4
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x001AE5C0 File Offset: 0x001AC9C0
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x001AE5E0 File Offset: 0x001AC9E0
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x001AE629 File Offset: 0x001ACA29
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn);
		}
	}
}
