using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000880 RID: 2176
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060031A7 RID: 12711 RVA: 0x001AE8B4 File Offset: 0x001ACCB4
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x001AE8D0 File Offset: 0x001ACCD0
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x001AE8F0 File Offset: 0x001ACCF0
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x001AE93E File Offset: 0x001ACD3E
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}
	}
}
