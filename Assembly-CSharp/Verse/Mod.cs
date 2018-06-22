using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CBD RID: 3261
	public abstract class Mod
	{
		// Token: 0x06004804 RID: 18436 RVA: 0x0025EA76 File Offset: 0x0025CE76
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06004805 RID: 18437 RVA: 0x0025EA88 File Offset: 0x0025CE88
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x0025EAA4 File Offset: 0x0025CEA4
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

		// Token: 0x06004807 RID: 18439 RVA: 0x0025EB79 File Offset: 0x0025CF79
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x0025EB92 File Offset: 0x0025CF92
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0025EB98 File Offset: 0x0025CF98
		public virtual string SettingsCategory()
		{
			return "";
		}

		// Token: 0x040030C9 RID: 12489
		private ModSettings modSettings;

		// Token: 0x040030CA RID: 12490
		private ModContentPack intContent;
	}
}
