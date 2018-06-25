using System;
using System.IO;
using RimWorld;

namespace Verse
{
	public class Root_Entry : Root
	{
		public MusicManagerEntry musicManagerEntry;

		public Root_Entry()
		{
		}

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
