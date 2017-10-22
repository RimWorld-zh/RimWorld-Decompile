using System.Linq;
using System.Text;

namespace Verse.Sound
{
	public static class DebugSoundEventsLog
	{
		private static LogMessageQueue queue = new LogMessageQueue();

		public static string EventsListingDebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LogMessage item in DebugSoundEventsLog.queue.Messages.Reverse())
				{
					stringBuilder.AppendLine(item.ToString());
				}
				return stringBuilder.ToString();
			}
		}

		public static void Notify_SoundEvent(SoundDef def, SoundInfo info)
		{
			if (DebugViewSettings.writeSoundEventsRecord)
			{
				string str = (def != null) ? ((!def.isUndefined) ? ((!def.sustain) ? "OneShot: " : "SustainerSpawn: ") : "Undefined: ") : "null: ";
				string str2 = (def == null) ? "null" : def.defName;
				string str3 = str + str2 + " - " + info.ToString();
				DebugSoundEventsLog.CreateRecord(str3);
			}
		}

		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			string str = "SustainerEnd: " + sustainer.def.defName + " - " + info.ToString();
			DebugSoundEventsLog.CreateRecord(str);
		}

		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}
	}
}
