using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC0 RID: 3264
	public abstract class Mod
	{
		// Token: 0x060047F3 RID: 18419 RVA: 0x0025D65E File Offset: 0x0025BA5E
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x060047F4 RID: 18420 RVA: 0x0025D670 File Offset: 0x0025BA70
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x0025D68C File Offset: 0x0025BA8C
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

		// Token: 0x060047F6 RID: 18422 RVA: 0x0025D761 File Offset: 0x0025BB61
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x0025D77A File Offset: 0x0025BB7A
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x0025D780 File Offset: 0x0025BB80
		public virtual string SettingsCategory()
		{
			return "";
		}

		// Token: 0x040030BE RID: 12478
		private ModSettings modSettings;

		// Token: 0x040030BF RID: 12479
		private ModContentPack intContent;
	}
}
