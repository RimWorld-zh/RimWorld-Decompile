using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000703 RID: 1795
	public class CompBreakdownable : ThingComp
	{
		// Token: 0x040015BF RID: 5567
		private bool brokenDownInt;

		// Token: 0x040015C0 RID: 5568
		private CompPowerTrader powerComp;

		// Token: 0x040015C1 RID: 5569
		private const int BreakdownMTBTicks = 13679999;

		// Token: 0x040015C2 RID: 5570
		public const string BreakdownSignal = "Breakdown";

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600274A RID: 10058 RVA: 0x001524A8 File Offset: 0x001508A8
		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x001524C3 File Offset: 0x001508C3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x001524DE File Offset: 0x001508DE
		public override void PostDraw()
		{
			if (this.brokenDownInt)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.BrokenDown);
			}
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x00152509 File Offset: 0x00150909
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.parent.Map.GetComponent<BreakdownManager>().Register(this);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x0015253A File Offset: 0x0015093A
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x00152550 File Offset: 0x00150950
		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(13679999f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x00152580 File Offset: 0x00150980
		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x001525BC File Offset: 0x001509BC
		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			this.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(this.parent);
			if (this.parent is Building_PowerSwitch)
			{
				this.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(this.parent.GetComp<CompPower>());
			}
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x0015261C File Offset: 0x00150A1C
		public void DoBreakdown()
		{
			this.brokenDownInt = true;
			this.parent.BroadcastCompSignal("Breakdown");
			this.parent.Map.GetComponent<BreakdownManager>().Notify_BrokenDown(this.parent);
			if (this.parent.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelBuildingBrokenDown".Translate(new object[]
				{
					this.parent.LabelShort.CapitalizeFirst()
				}), "LetterBuildingBrokenDown".Translate(new object[]
				{
					this.parent.LabelShort
				}), LetterDefOf.NegativeEvent, this.parent, null, null);
			}
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x001526D0 File Offset: 0x00150AD0
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.BrokenDown)
			{
				result = "BrokenDown".Translate();
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
