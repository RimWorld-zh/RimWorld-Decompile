using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC0 RID: 3264
	public abstract class Mod
	{
		// Token: 0x040030D0 RID: 12496
		private ModSettings modSettings;

		// Token: 0x040030D1 RID: 12497
		private ModContentPack intContent;

		// Token: 0x06004807 RID: 18439 RVA: 0x0025EE32 File Offset: 0x0025D232
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06004808 RID: 18440 RVA: 0x0025EE44 File Offset: 0x0025D244
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0025EE60 File Offset: 0x0025D260
		public T GetSettings<T>() where T : ModSettings, new()
		{
			T result;
			if (this.modSettings != null && this.modSettings.GetType() != typeof(T))
			{
				Log.Error(string.Format("Mod {0} attempted to read two different settings classes (was {1}, is now {2})", this.Content.Name, this.modSettings.GetType(), typeof(T)), false);
				result = (T)((object)null);
			}
			else if (this.modSettings != null)
			{
				result = (T)((object)this.modSettings);
			}
			else
			{
				this.modSettings = LoadedModManager.ReadModSettings<T>(this.intContent.Identifier, base.GetType().Name);
				this.modSettings.Mod = this;
				result = (this.modSettings as T);
			}
			return result;
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x0025EF35 File Offset: 0x0025D335
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x0025EF4E File Offset: 0x0025D34E
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x0025EF54 File Offset: 0x0025D354
		public virtual string SettingsCategory()
		{
			return "";
		}
	}
}
