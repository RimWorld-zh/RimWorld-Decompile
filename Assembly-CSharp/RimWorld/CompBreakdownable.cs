using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000705 RID: 1797
	public class CompBreakdownable : ThingComp
	{
		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600274F RID: 10063 RVA: 0x00151F54 File Offset: 0x00150354
		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x00151F6F File Offset: 0x0015036F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x00151F8A File Offset: 0x0015038A
		public override void PostDraw()
		{
			if (this.brokenDownInt)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.BrokenDown);
			}
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x00151FB5 File Offset: 0x001503B5
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.parent.Map.GetComponent<BreakdownManager>().Register(this);
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x00151FE6 File Offset: 0x001503E6
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x00151FFC File Offset: 0x001503FC
		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(13679999f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x0015202C File Offset: 0x0015042C
		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x00152068 File Offset: 0x00150468
		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			this.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(this.parent);
			if (this.parent is Building_PowerSwitch)
			{
				this.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(this.parent.GetComp<CompPower>());
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x001520C8 File Offset: 0x001504C8
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

		// Token: 0x06002758 RID: 10072 RVA: 0x0015217C File Offset: 0x0015057C
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

		// Token: 0x040015BD RID: 5565
		private bool brokenDownInt;

		// Token: 0x040015BE RID: 5566
		private CompPowerTrader powerComp;

		// Token: 0x040015BF RID: 5567
		private const int BreakdownMTBTicks = 13679999;

		// Token: 0x040015C0 RID: 5568
		public const string BreakdownSignal = "Breakdown";
	}
}
