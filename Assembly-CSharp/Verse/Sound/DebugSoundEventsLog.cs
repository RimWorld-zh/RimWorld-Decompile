using System;
using System.Linq;
using System.Text;

namespace Verse.Sound
{
	// Token: 0x02000DAE RID: 3502
	public static class DebugSoundEventsLog
	{
		// Token: 0x04003426 RID: 13350
		private static LogMessageQueue queue = new LogMessageQueue();

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06004E33 RID: 20019 RVA: 0x0028F008 File Offset: 0x0028D408
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

		// Token: 0x06004E34 RID: 20020 RVA: 0x0028F088 File Offset: 0x0028D488
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

		// Token: 0x06004E35 RID: 20021 RVA: 0x0028F120 File Offset: 0x0028D520
		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			string str = "SustainerEnd: " + sustainer.def.defName + " - " + info.ToString();
			DebugSoundEventsLog.CreateRecord(str);
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x0028F15C File Offset: 0x0028D55C
		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}
	}
}
