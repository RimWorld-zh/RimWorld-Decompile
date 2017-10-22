using RimWorld;

namespace Verse
{
	public class Root_Entry : Root
	{
		public MusicManagerEntry musicManagerEntry;

		public override void Start()
		{
			base.Start();
			Current.Game = null;
			this.musicManagerEntry = new MusicManagerEntry();
		}

		public override void Update()
		{
			base.Update();
			if (!LongEventHandler.ShouldWaitForEvent && !base.destroyed)
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
		}
	}
}
