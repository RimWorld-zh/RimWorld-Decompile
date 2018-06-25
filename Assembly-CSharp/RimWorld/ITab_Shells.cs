using System;

namespace RimWorld
{
	public class ITab_Shells : ITab_Storage
	{
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}

		protected override IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				IStoreSettingsParent selStoreSettingsParent = base.SelStoreSettingsParent;
				IStoreSettingsParent result;
				if (selStoreSettingsParent != null)
				{
					result = selStoreSettingsParent;
				}
				else
				{
					Building_TurretGun building_TurretGun = base.SelObject as Building_TurretGun;
					if (building_TurretGun != null)
					{
						result = base.GetThingOrThingCompStoreSettingsParent(building_TurretGun.gun);
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}
	}
}
