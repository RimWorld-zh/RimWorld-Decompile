using System;
using Verse;

namespace RimWorld
{
	public class Verb_ShootOneUse : Verb_Shoot
	{
		public Verb_ShootOneUse()
		{
		}

		protected override bool TryCastShot()
		{
			bool result;
			if (base.TryCastShot())
			{
				if (this.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				result = true;
			}
			else
			{
				if (this.burstShotsLeft < this.verbProps.burstShotCount)
				{
					this.SelfConsume();
				}
				result = false;
			}
			return result;
		}

		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < this.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
		}

		private void SelfConsume()
		{
			if (this.ownerEquipment != null && !this.ownerEquipment.Destroyed)
			{
				this.ownerEquipment.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
