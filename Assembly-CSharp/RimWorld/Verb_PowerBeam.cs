using System;
using Verse;

namespace RimWorld
{
	public class Verb_PowerBeam : Verb
	{
		private const int DurationTicks = 600;

		public Verb_PowerBeam()
		{
		}

		protected override bool TryCastShot()
		{
			bool result;
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				result = false;
			}
			else
			{
				PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
				powerBeam.duration = 600;
				powerBeam.instigator = this.caster;
				powerBeam.weaponDef = ((this.ownerEquipment == null) ? null : this.ownerEquipment.def);
				powerBeam.StartStrike();
				if (this.ownerEquipment != null && !this.ownerEquipment.Destroyed)
				{
					this.ownerEquipment.Destroy(DestroyMode.Vanish);
				}
				result = true;
			}
			return result;
		}

		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}
	}
}
