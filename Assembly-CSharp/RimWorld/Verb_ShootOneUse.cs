using Verse;

namespace RimWorld
{
	public class Verb_ShootOneUse : Verb_Shoot
	{
		protected override bool TryCastShot()
		{
			bool result;
			if (base.TryCastShot())
			{
				if (base.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				result = true;
			}
			else
			{
				if (base.burstShotsLeft < base.verbProps.burstShotCount)
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
			if (base.state == VerbState.Bursting && base.burstShotsLeft < base.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
		}

		private void SelfConsume()
		{
			if (base.ownerEquipment != null && !base.ownerEquipment.Destroyed)
			{
				base.ownerEquipment.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
