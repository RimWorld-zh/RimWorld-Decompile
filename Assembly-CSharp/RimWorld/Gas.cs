using Verse;

namespace RimWorld
{
	public class Gas : Thing
	{
		public int destroyTick;

		public float graphicRotation = 0f;

		public float graphicRotationSpeed = 0f;

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			while (true)
			{
				Thing gas = base.Position.GetGas(map);
				if (gas != null)
				{
					gas.Destroy(DestroyMode.Vanish);
					continue;
				}
				break;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.destroyTick = Find.TickManager.TicksGame + base.def.gas.expireSeconds.RandomInRange.SecondsToTicks();
			this.graphicRotationSpeed = (float)(Rand.Range((float)(0.0 - base.def.gas.rotationSpeed), base.def.gas.rotationSpeed) / 60.0);
		}

		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}
	}
}
