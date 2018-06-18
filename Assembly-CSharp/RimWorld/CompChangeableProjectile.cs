using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000709 RID: 1801
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06002764 RID: 10084 RVA: 0x00152368 File Offset: 0x00150768
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x00152388 File Offset: 0x00150788
		public ThingDef LoadedShell
		{
			get
			{
				return (this.loadedCount <= 0) ? null : this.loadedShell;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002766 RID: 10086 RVA: 0x001523B8 File Offset: 0x001507B8
		public ThingDef Projectile
		{
			get
			{
				return (!this.Loaded) ? null : this.LoadedShell.projectileWhenLoaded;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x001523EC File Offset: 0x001507EC
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06002768 RID: 10088 RVA: 0x00152410 File Offset: 0x00150810
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x00152426 File Offset: 0x00150826
		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", new object[0]);
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x00152464 File Offset: 0x00150864
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x001524BF File Offset: 0x001508BF
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

		// Token: 0x0600276C RID: 10092 RVA: 0x001524EF File Offset: 0x001508EF
		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count <= 0) ? null : shell);
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x00152514 File Offset: 0x00150914
		public Thing RemoveShell()
		{
			Thing thing = ThingMaker.MakeThing(this.loadedShell, null);
			thing.stackCount = this.loadedCount;
			this.loadedCount = 0;
			this.loadedShell = null;
			return thing;
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x00152554 File Offset: 0x00150954
		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x00152570 File Offset: 0x00150970
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		// Token: 0x040015C4 RID: 5572
		private ThingDef loadedShell;

		// Token: 0x040015C5 RID: 5573
		public int loadedCount;

		// Token: 0x040015C6 RID: 5574
		public StorageSettings allowedShellsSettings;
	}
}
