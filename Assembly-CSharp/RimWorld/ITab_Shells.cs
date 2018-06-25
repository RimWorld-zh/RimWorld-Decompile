using System;

namespace RimWorld
{
	// Token: 0x0200085B RID: 2139
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x0600306A RID: 12394 RVA: 0x001A5DEC File Offset: 0x001A41EC
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600306B RID: 12395 RVA: 0x001A5E0C File Offset: 0x001A420C
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

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x0600306C RID: 12396 RVA: 0x001A5E5C File Offset: 0x001A425C
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}
	}
}
