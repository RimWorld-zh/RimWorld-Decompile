using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000626 RID: 1574
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001FFA RID: 8186 RVA: 0x00113080 File Offset: 0x00111480
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x0011309A File Offset: 0x0011149A
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x001130AC File Offset: 0x001114AC
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
