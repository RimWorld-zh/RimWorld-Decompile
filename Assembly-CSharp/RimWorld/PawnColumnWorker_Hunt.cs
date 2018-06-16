using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000881 RID: 2177
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x001AE1AC File Offset: 0x001AC5AC
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x001AE1C8 File Offset: 0x001AC5C8
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x001AE1E8 File Offset: 0x001AC5E8
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x001AE231 File Offset: 0x001AC631
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
			HuntUtility.ShowDesignationWarnings(pawn);
		}
	}
}
