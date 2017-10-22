using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		private ThingDef loadedShell;

		public int loadedCount;

		public StorageSettings allowedShellsSettings;

		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)base.props;
			}
		}

		public ThingDef LoadedShell
		{
			get
			{
				return (this.loadedCount <= 0) ? null : this.loadedShell;
			}
		}

		public ThingDef Projectile
		{
			get
			{
				return (!this.Loaded) ? null : this.LoadedShell.projectileWhenLoaded;
			}
		}

		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", new object[0]);
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (base.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(base.parent.def.building.defaultStorageSettings);
			}
		}

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

		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count <= 0) ? null : shell);
		}

		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		public StorageSettings GetParentStoreSettings()
		{
			return base.parent.def.building.fixedStorageSettings;
		}
	}
}
