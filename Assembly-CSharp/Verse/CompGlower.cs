using RimWorld;

namespace Verse
{
	public class CompGlower : ThingComp
	{
		private bool glowOnInt = false;

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
				bool result;
				if (!base.parent.Spawned)
				{
					result = false;
				}
				else if (!FlickUtility.WantsToBeOn(base.parent))
				{
					result = false;
				}
				else
				{
					CompPowerTrader compPowerTrader = base.parent.TryGetComp<CompPowerTrader>();
					if (compPowerTrader != null && !compPowerTrader.PowerOn)
					{
						result = false;
					}
					else
					{
						CompRefuelable compRefuelable = base.parent.TryGetComp<CompRefuelable>();
						result = ((byte)((compRefuelable == null || compRefuelable.HasFuel) ? 1 : 0) != 0);
					}
				}
				return result;
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
