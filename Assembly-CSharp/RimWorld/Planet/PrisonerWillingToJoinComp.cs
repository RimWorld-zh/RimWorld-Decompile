using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000624 RID: 1572
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001FF4 RID: 8180 RVA: 0x00113430 File Offset: 0x00111830
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0011344A File Offset: 0x0011184A
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0011345C File Offset: 0x0011185C
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
