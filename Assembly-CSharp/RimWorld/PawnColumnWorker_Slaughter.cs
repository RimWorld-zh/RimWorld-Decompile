using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087E RID: 2174
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x001AE508 File Offset: 0x001AC908
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x001AE524 File Offset: 0x001AC924
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x001AE544 File Offset: 0x001AC944
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x001AE592 File Offset: 0x001AC992
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}
	}
}
