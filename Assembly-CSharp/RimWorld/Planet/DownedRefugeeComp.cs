using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061B RID: 1563
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x00112328 File Offset: 0x00110728
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x00112344 File Offset: 0x00110744
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			if (this.pawn.Any)
			{
				if (!this.pawn[0].Dead)
				{
					if (this.pawn[0].relations != null)
					{
						this.pawn[0].relations.Notify_FailedRescueQuest();
					}
					HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(this.pawn[0]);
				}
				this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x001123C8 File Offset: 0x001107C8
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.pawn.Any)
			{
				result = "Refugee".Translate() + ": " + this.pawn[0].LabelCap;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
