using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class SitePartWorker
	{
		public SitePartDef def;

		public virtual void SitePartWorkerTick(Site site)
		{
		}

		public virtual void PostMapGenerate(Map map)
		{
		}

		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}
	}
}
