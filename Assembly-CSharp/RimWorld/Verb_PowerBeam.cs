using Verse;

namespace RimWorld
{
	public class Verb_PowerBeam : Verb
	{
		private const int DurationTicks = 600;

		protected override bool TryCastShot()
		{
			if (base.currentTarget.HasThing && base.currentTarget.Thing.Map != base.caster.Map)
			{
				return false;
			}
			PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, base.currentTarget.Cell, base.caster.Map);
			powerBeam.duration = 600;
			powerBeam.instigator = base.caster;
			powerBeam.weaponDef = ((base.ownerEquipment == null) ? null : base.ownerEquipment.def);
			powerBeam.StartStrike();
			if (base.ownerEquipment != null && !base.ownerEquipment.Destroyed)
			{
				base.ownerEquipment.Destroy(DestroyMode.Vanish);
			}
			return true;
		}

		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}
	}
}
