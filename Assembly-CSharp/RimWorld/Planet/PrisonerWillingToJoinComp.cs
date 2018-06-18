using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000626 RID: 1574
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001FFC RID: 8188 RVA: 0x001130F8 File Offset: 0x001114F8
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x00113112 File Offset: 0x00111512
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x00113124 File Offset: 0x00111524
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
