using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BDE RID: 3038
	public class Root_Entry : Root
	{
		// Token: 0x04002D5E RID: 11614
		public MusicManagerEntry musicManagerEntry;

		// Token: 0x06004252 RID: 16978 RVA: 0x0022E89C File Offset: 0x0022CC9C
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

		// Token: 0x06004253 RID: 16979 RVA: 0x0022E91C File Offset: 0x0022CD1C
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
