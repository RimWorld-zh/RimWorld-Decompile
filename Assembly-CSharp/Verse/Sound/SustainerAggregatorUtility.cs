using System;

namespace Verse.Sound
{
	// Token: 0x02000DC4 RID: 3524
	public static class SustainerAggregatorUtility
	{
		// Token: 0x06004E96 RID: 20118 RVA: 0x00290550 File Offset: 0x0028E950
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

		// Token: 0x04003458 RID: 13400
		private static float AggregateRadius = 12f;
	}
}
