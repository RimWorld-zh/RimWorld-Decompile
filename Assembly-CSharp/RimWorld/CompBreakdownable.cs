using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000701 RID: 1793
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
		// (get) Token: 0x06002747 RID: 10055 RVA: 0x001520F8 File Offset: 0x001504F8
		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x00152113 File Offset: 0x00150513
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x0015212E File Offset: 0x0015052E
		public override void PostDraw()
		{
			if (this.brokenDownInt)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.BrokenDown);
			}
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x00152159 File Offset: 0x00150559
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.parent.Map.GetComponent<BreakdownManager>().Register(this);
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x0015218A File Offset: 0x0015058A
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x001521A0 File Offset: 0x001505A0
		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(13679999f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x001521D0 File Offset: 0x001505D0
		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x0015220C File Offset: 0x0015060C
		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			this.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(this.parent);
			if (this.parent is Building_PowerSwitch)
			{
				this.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(this.parent.GetComp<CompPower>());
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x0015226C File Offset: 0x0015066C
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

		// Token: 0x06002750 RID: 10064 RVA: 0x00152320 File Offset: 0x00150720
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
