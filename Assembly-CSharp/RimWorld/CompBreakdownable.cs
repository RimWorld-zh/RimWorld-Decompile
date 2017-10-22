using Verse;

namespace RimWorld
{
	public class CompBreakdownable : ThingComp
	{
		private bool brokenDownInt;

		private CompPowerTrader powerComp;

		private const int BreakdownMTBTicks = 13679999;

		public const string BreakdownSignal = "Breakdown";

		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		public override void PostDraw()
		{
			if (this.brokenDownInt)
			{
				base.parent.Map.overlayDrawer.DrawOverlay(base.parent, OverlayTypes.BrokenDown);
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = base.parent.GetComp<CompPowerTrader>();
			base.parent.Map.GetComponent<BreakdownManager>().Register(this);
		}

		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(13679999f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			base.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(base.parent);
			if (base.parent is Building_PowerSwitch)
			{
				base.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(base.parent.GetComp<CompPower>());
			}
		}

		public void DoBreakdown()
		{
			this.brokenDownInt = true;
			base.parent.BroadcastCompSignal("Breakdown");
			base.parent.Map.GetComponent<BreakdownManager>().Notify_BrokenDown(base.parent);
			if (base.parent.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelBuildingBrokenDown".Translate(base.parent.LabelShort), "LetterBuildingBrokenDown".Translate(base.parent.LabelShort), LetterDefOf.NegativeEvent, (Thing)base.parent, (string)null);
			}
		}

		public override string CompInspectStringExtra()
		{
			return (!this.BrokenDown) ? null : "BrokenDown".Translate();
		}
	}
}
