using System;

namespace RimWorld
{
	// Token: 0x0200085D RID: 2141
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x0600306C RID: 12396 RVA: 0x001A578C File Offset: 0x001A3B8C
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600306D RID: 12397 RVA: 0x001A57AC File Offset: 0x001A3BAC
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
		// (get) Token: 0x0600306E RID: 12398 RVA: 0x001A57FC File Offset: 0x001A3BFC
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}
	}
}
