using Verse;

namespace RimWorld
{
	public class Verb_Bombardment : Verb
	{
		private const int DurationTicks = 450;

		protected override bool TryCastShot()
		{
			bool result;
			if (base.currentTarget.HasThing && base.currentTarget.Thing.Map != base.caster.Map)
			{
				result = false;
			}
			else
			{
				Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, base.currentTarget.Cell, base.caster.Map);
				bombardment.duration = 450;
				bombardment.StartStrike();
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
			return 23f;
		}
	}
}
