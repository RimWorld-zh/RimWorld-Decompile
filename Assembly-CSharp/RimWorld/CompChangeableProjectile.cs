using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000707 RID: 1799
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		// Token: 0x040015C6 RID: 5574
		private ThingDef loadedShell;

		// Token: 0x040015C7 RID: 5575
		public int loadedCount;

		// Token: 0x040015C8 RID: 5576
		public StorageSettings allowedShellsSettings;

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x001528BC File Offset: 0x00150CBC
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002760 RID: 10080 RVA: 0x001528DC File Offset: 0x00150CDC
		public ThingDef LoadedShell
		{
			get
			{
				return (this.loadedCount <= 0) ? null : this.loadedShell;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x0015290C File Offset: 0x00150D0C
		public ThingDef Projectile
		{
			get
			{
				return (!this.Loaded) ? null : this.LoadedShell.projectileWhenLoaded;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06002762 RID: 10082 RVA: 0x00152940 File Offset: 0x00150D40
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x00152964 File Offset: 0x00150D64
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x0015297A File Offset: 0x00150D7A
		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", new object[0]);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x001529B8 File Offset: 0x00150DB8
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x00152A13 File Offset: 0x00150E13
		public virtual void Notify_ProjectileLaunched()
		{
			if (this.loadedCount > 0)
			{
				this.loadedCount--;
			}
			if (this.loadedCount <= 0)
			{
				this.loadedShell = null;
			}
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x00152A43 File Offset: 0x00150E43
		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count <= 0) ? null : shell);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x00152A68 File Offset: 0x00150E68
		public Thing RemoveShell()
		{
			Thing thing = ThingMaker.MakeThing(this.loadedShell, null);
			thing.stackCount = this.loadedCount;
			this.loadedCount = 0;
			this.loadedShell = null;
			return thing;
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x00152AA8 File Offset: 0x00150EA8
		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x00152AC4 File Offset: 0x00150EC4
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}
	}
}
