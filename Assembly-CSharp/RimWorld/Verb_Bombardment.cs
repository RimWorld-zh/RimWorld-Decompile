using System;
using Verse;

namespace RimWorld
{
	public class Verb_Bombardment : Verb
	{
		private const int DurationTicks = 450;

		public Verb_Bombardment()
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
				Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
				bombardment.duration = 450;
				bombardment.instigator = this.caster;
				bombardment.weaponDef = ((this.ownerEquipment == null) ? null : this.ownerEquipment.def);
				bombardment.StartStrike();
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
			return 23f;
		}
	}
}
