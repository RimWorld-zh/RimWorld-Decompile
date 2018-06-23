using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BDB RID: 3035
	public class Root_Entry : Root
	{
		// Token: 0x04002D57 RID: 11607
		public MusicManagerEntry musicManagerEntry;

		// Token: 0x0600424F RID: 16975 RVA: 0x0022E4E0 File Offset: 0x0022C8E0
		public override void Start()
		{
			base.Start();
			try
			{
				Current.Game = null;
				this.musicManagerEntry = new MusicManagerEntry();
				FileInfo fileInfo = (!Root.checkedAutostartSaveFile) ? SaveGameFilesUtility.GetAutostartSaveFile() : null;
				Root.checkedAutostartSaveFile = true;
				if (fileInfo != null)
				{
					GameDataSaveLoader.LoadGame(fileInfo);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg, false);
			}
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x0022E560 File Offset: 0x0022C960
		public override void Update()
		{
			base.Update();
			if (!LongEventHandler.ShouldWaitForEvent && !this.destroyed)
			{
				try
				{
					this.musicManagerEntry.MusicManagerEntryUpdate();
					if (Find.World != null)
					{
						Find.World.WorldUpdate();
					}
					if (Current.Game != null)
					{
						Current.Game.UpdateEntry();
					}
				}
				catch (Exception arg)
				{
					Log.Error("Root level exception in Update(): " + arg, false);
				}
			}
		}
	}
}
