using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000703 RID: 1795
	public class CompBreakdownable : ThingComp
	{
		// Token: 0x040015BB RID: 5563
		private bool brokenDownInt;

		// Token: 0x040015BC RID: 5564
		private CompPowerTrader powerComp;

		// Token: 0x040015BD RID: 5565
		private const int BreakdownMTBTicks = 13679999;

		// Token: 0x040015BE RID: 5566
		public const string BreakdownSignal = "Breakdown";

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x00152248 File Offset: 0x00150648
		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x00152263 File Offset: 0x00150663
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x0015227E File Offset: 0x0015067E
		public override void PostDraw()
		{
			if (this.brokenDownInt)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.BrokenDown);
			}
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x001522A9 File Offset: 0x001506A9
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.parent.Map.GetComponent<BreakdownManager>().Register(this);
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x001522DA File Offset: 0x001506DA
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x001522F0 File Offset: 0x001506F0
		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(13679999f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x00152320 File Offset: 0x00150720
		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x0015235C File Offset: 0x0015075C
		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			this.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(this.parent);
			if (this.parent is Building_PowerSwitch)
			{
				this.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(this.parent.GetComp<CompPower>());
			}
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x001523BC File Offset: 0x001507BC
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

		// Token: 0x06002754 RID: 10068 RVA: 0x00152470 File Offset: 0x00150870
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
