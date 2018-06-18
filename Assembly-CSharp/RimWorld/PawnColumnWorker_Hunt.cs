using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000881 RID: 2177
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060031A6 RID: 12710 RVA: 0x001AE274 File Offset: 0x001AC674
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x001AE290 File Offset: 0x001AC690
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x001AE2B0 File Offset: 0x001AC6B0
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == null && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x001AE2F9 File Offset: 0x001AC6F9
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
			HuntUtility.ShowDesignationWarnings(pawn);
		}
	}
}
