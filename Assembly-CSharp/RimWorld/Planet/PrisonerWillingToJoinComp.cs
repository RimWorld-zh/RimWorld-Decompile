using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000622 RID: 1570
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x00113078 File Offset: 0x00111478
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x00113092 File Offset: 0x00111492
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x001130A4 File Offset: 0x001114A4
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.pawn.Any)
			{
				result = "Prisoner".Translate() + ": " + this.pawn[0].LabelCap;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
