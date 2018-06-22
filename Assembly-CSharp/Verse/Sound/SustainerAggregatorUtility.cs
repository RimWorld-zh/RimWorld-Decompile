using System;

namespace Verse.Sound
{
	// Token: 0x02000DC1 RID: 3521
	public static class SustainerAggregatorUtility
	{
		// Token: 0x06004EAB RID: 20139 RVA: 0x00291B00 File Offset: 0x0028FF00
		public static Sustainer AggregateOrSpawnSustainerFor(ISizeReporter reporter, SoundDef def, SoundInfo info)
		{
			Sustainer sustainer = null;
			foreach (Sustainer sustainer2 in Find.SoundRoot.sustainerManager.AllSustainers)
			{
				if (sustainer2.def == def && sustainer2.info.Maker.Map == info.Maker.Map && sustainer2.info.Maker.Cell.InHorDistOf(info.Maker.Cell, SustainerAggregatorUtility.AggregateRadius))
				{
					sustainer = sustainer2;
					break;
				}
			}
			if (sustainer == null)
			{
				sustainer = def.TrySpawnSustainer(info);
			}
			else
			{
				sustainer.Maintain();
			}
			if (sustainer.externalParams.sizeAggregator == null)
			{
				sustainer.externalParams.sizeAggregator = new SoundSizeAggregator();
			}
			sustainer.externalParams.sizeAggregator.RegisterReporter(reporter);
			return sustainer;
		}

		// Token: 0x04003463 RID: 13411
		private static float AggregateRadius = 12f;
	}
}
