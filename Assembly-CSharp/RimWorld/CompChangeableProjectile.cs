using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000707 RID: 1799
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		// Token: 0x040015C2 RID: 5570
		private ThingDef loadedShell;

		// Token: 0x040015C3 RID: 5571
		public int loadedCount;

		// Token: 0x040015C4 RID: 5572
		public StorageSettings allowedShellsSettings;

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06002760 RID: 10080 RVA: 0x0015265C File Offset: 0x00150A5C
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x0015267C File Offset: 0x00150A7C
		public ThingDef LoadedShell
		{
			get
			{
				return (this.loadedCount <= 0) ? null : this.loadedShell;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002762 RID: 10082 RVA: 0x001526AC File Offset: 0x00150AAC
		public ThingDef Projectile
		{
			get
			{
				return (!this.Loaded) ? null : this.LoadedShell.projectileWhenLoaded;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x001526E0 File Offset: 0x00150AE0
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06002764 RID: 10084 RVA: 0x00152704 File Offset: 0x00150B04
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x0015271A File Offset: 0x00150B1A
		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", new object[0]);
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x00152758 File Offset: 0x00150B58
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x001527B3 File Offset: 0x00150BB3
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

		// Token: 0x06002768 RID: 10088 RVA: 0x001527E3 File Offset: 0x00150BE3
		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count <= 0) ? null : shell);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x00152808 File Offset: 0x00150C08
		public Thing RemoveShell()
		{
			Thing thing = ThingMaker.MakeThing(this.loadedShell, null);
			thing.stackCount = this.loadedCount;
			this.loadedCount = 0;
			this.loadedShell = null;
			return thing;
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x00152848 File Offset: 0x00150C48
		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x00152864 File Offset: 0x00150C64
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}
	}
}
