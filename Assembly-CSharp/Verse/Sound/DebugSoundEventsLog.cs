using System;
using System.Linq;
using System.Text;

namespace Verse.Sound
{
	// Token: 0x02000DAF RID: 3503
	public static class DebugSoundEventsLog
	{
		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06004E1C RID: 19996 RVA: 0x0028D66C File Offset: 0x0028BA6C
		public static string EventsListingDebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LogMessage logMessage in DebugSoundEventsLog.queue.Messages.Reverse<LogMessage>())
				{
					stringBuilder.AppendLine(logMessage.ToString());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x0028D6EC File Offset: 0x0028BAEC
		public static void Notify_SoundEvent(SoundDef def, SoundInfo info)
		{
			if (DebugViewSettings.writeSoundEventsRecord)
			{
				string str;
				if (def == null)
				{
					str = "null: ";
				}
				else if (def.isUndefined)
				{
					str = "Undefined: ";
				}
				else
				{
					str = ((!def.sustain) ? "OneShot: " : "SustainerSpawn: ");
				}
				string str2 = (def == null) ? "null" : def.defName;
				string str3 = str + str2 + " - " + info.ToString();
				DebugSoundEventsLog.CreateRecord(str3);
			}
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x0028D784 File Offset: 0x0028BB84
		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			string str = "SustainerEnd: " + sustainer.def.defName + " - " + info.ToString();
			DebugSoundEventsLog.CreateRecord(str);
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x0028D7C0 File Offset: 0x0028BBC0
		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}

		// Token: 0x04003416 RID: 13334
		private static LogMessageQueue queue = new LogMessageQueue();
	}
}
