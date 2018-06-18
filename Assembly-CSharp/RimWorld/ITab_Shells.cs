using System;

namespace RimWorld
{
	// Token: 0x0200085D RID: 2141
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x0600306E RID: 12398 RVA: 0x001A5854 File Offset: 0x001A3C54
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600306F RID: 12399 RVA: 0x001A5874 File Offset: 0x001A3C74
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

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06003070 RID: 12400 RVA: 0x001A58C4 File Offset: 0x001A3CC4
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}
	}
}
