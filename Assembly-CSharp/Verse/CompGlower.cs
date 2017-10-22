using RimWorld;

namespace Verse
{
	public class CompGlower : ThingComp
	{
		private bool glowOnInt;

		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)base.props;
			}
		}

		private bool ShouldBeLitNow
		{
			get
			{
				if (!base.parent.Spawned)
				{
					return false;
				}
				if (!FlickUtility.WantsToBeOn(base.parent))
				{
					return false;
				}
				CompPowerTrader compPowerTrader = base.parent.TryGetComp<CompPowerTrader>();
				if (compPowerTrader != null && !compPowerTrader.PowerOn)
				{
					return false;
				}
				CompRefuelable compRefuelable = base.parent.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && !compRefuelable.HasFuel)
				{
					return false;
				}
				return true;
			}
		}

		public void UpdateLit(Map map)
		{
			bool shouldBeLitNow = this.ShouldBeLitNow;
			if (this.glowOnInt != shouldBeLitNow)
			{
				this.glowOnInt = shouldBeLitNow;
				if (!this.glowOnInt)
				{
					map.mapDrawer.MapMeshDirty(base.parent.Position, MapMeshFlag.Things);
					map.glowGrid.DeRegisterGlower(this);
				}
				else
				{
					map.mapDrawer.MapMeshDirty(base.parent.Position, MapMeshFlag.Things);
					map.glowGrid.RegisterGlower(this);
				}
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.ShouldBeLitNow)
			{
				this.UpdateLit(base.parent.Map);
				base.parent.Map.glowGrid.RegisterGlower(this);
			}
			else
			{
				this.UpdateLit(base.parent.Map);
			}
		}

		public override void ReceiveCompSignal(string signal)
		{
			if (!(signal == "PowerTurnedOn") && !(signal == "PowerTurnedOff") && !(signal == "FlickedOn") && !(signal == "FlickedOff") && !(signal == "Refueled") && !(signal == "RanOutOfFuel") && !(signal == "ScheduledOn") && !(signal == "ScheduledOff"))
				return;
			this.UpdateLit(base.parent.Map);
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
		}

		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.UpdateLit(map);
		}
	}
}
