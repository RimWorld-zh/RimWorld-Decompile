using Verse;

namespace RimWorld
{
	public class Verb_Bombardment : Verb
	{
		private const int DurationTicks = 450;

		protected override bool TryCastShot()
		{
			if (base.currentTarget.HasThing && base.currentTarget.Thing.Map != base.caster.Map)
			{
				return false;
			}
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, base.currentTarget.Cell, base.caster.Map);
			bombardment.duration = 450;
			bombardment.instigator = base.caster;
			bombardment.weaponDef = ((base.ownerEquipment == null) ? null : base.ownerEquipment.def);
			bombardment.StartStrike();
			if (base.ownerEquipment != null && !base.ownerEquipment.Destroyed)
			{
				base.ownerEquipment.Destroy(DestroyMode.Vanish);
			}
			return true;
		}

		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}
	}
}
