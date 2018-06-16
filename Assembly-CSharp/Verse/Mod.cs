using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC1 RID: 3265
	public abstract class Mod
	{
		// Token: 0x060047F5 RID: 18421 RVA: 0x0025D686 File Offset: 0x0025BA86
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x060047F6 RID: 18422 RVA: 0x0025D698 File Offset: 0x0025BA98
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x0025D6B4 File Offset: 0x0025BAB4
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

		// Token: 0x060047F8 RID: 18424 RVA: 0x0025D789 File Offset: 0x0025BB89
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x0025D7A2 File Offset: 0x0025BBA2
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x0025D7A8 File Offset: 0x0025BBA8
		public virtual string SettingsCategory()
		{
			return "";
		}

		// Token: 0x040030C0 RID: 12480
		private ModSettings modSettings;

		// Token: 0x040030C1 RID: 12481
		private ModContentPack intContent;
	}
}
