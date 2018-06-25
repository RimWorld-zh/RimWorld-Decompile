using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000624 RID: 1572
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x001131C8 File Offset: 0x001115C8
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x001131E2 File Offset: 0x001115E2
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x001131F4 File Offset: 0x001115F4
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
