using Verse;

namespace RimWorld
{
	public class Verb_PowerBeam : Verb
	{
		private const int DurationTicks = 600;

		protected override bool TryCastShot()
		{
			bool result;
			if (base.currentTarget.HasThing && base.currentTarget.Thing.Map != base.caster.Map)
			{
				result = false;
			}
			else
			{
				PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, base.currentTarget.Cell, base.caster.Map);
				powerBeam.duration = 600;
				powerBeam.StartStrike();
				if (base.ownerEquipment != null && !base.ownerEquipment.Destroyed)
				{
					base.ownerEquipment.Destroy(DestroyMode.Vanish);
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
