using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FB RID: 763
	public class HistoryAutoRecorder : IExposable
	{
		// Token: 0x0400084F RID: 2127
		public HistoryAutoRecorderDef def = null;

		// Token: 0x04000850 RID: 2128
		public List<float> records;

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0006FF30 File Offset: 0x0006E330
		public HistoryAutoRecorder()
		{
			this.records = new List<float>();
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0006FF4C File Offset: 0x0006E34C
		public void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame % this.def.recordTicksFrequency == 0 || !this.records.Any<float>())
			{
				float item = this.def.Worker.PullRecord();
				this.records.Add(item);
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0006FFA8 File Offset: 0x0006E3A8
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderDef>(ref this.def, "def");
			byte[] array = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				array = new byte[this.records.Count * 4];
				for (int i = 0; i < this.records.Count; i++)
				{
					byte[] bytes = BitConverter.GetBytes(this.records[i]);
					for (int j = 0; j < 4; j++)
					{
						array[i * 4 + j] = bytes[j];
					}
				}
			}
			DataExposeUtility.ByteArray(ref array, "records");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int num = array.Length / 4;
				this.records.Clear();
				for (int k = 0; k < num; k++)
				{
					float item = BitConverter.ToSingle(array, k * 4);
					this.records.Add(item);
				}
			}
		}
	}
}
