using UnityEngine;

namespace Verse
{
	public abstract class Mod
	{
		private ModSettings modSettings;

		private ModContentPack intContent;

		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		public T GetSettings<T>() where T : ModSettings, new()
		{
			T result;
			if (this.modSettings != null && this.modSettings.GetType() != typeof(T))
			{
				Log.Error(string.Format("Mod {0} attempted to read two different settings classes (was {1}, is now {2})", this.Content.Name, this.modSettings.GetType(), typeof(T)));
				result = (T)null;
			}
			else if (this.modSettings != null)
			{
				result = (T)this.modSettings;
			}
			else
			{
				this.modSettings = (ModSettings)(object)LoadedModManager.ReadModSettings<T>(this.intContent.Identifier, base.GetType().Name);
				this.modSettings.Mod = this;
				result = (T)(this.modSettings as T);
			}
			return result;
		}

		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		public virtual string SettingsCategory()
		{
			return "";
		}
	}
}
