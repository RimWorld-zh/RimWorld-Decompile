using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E64 RID: 3684
	internal static class GUIEventFilterForOSX
	{
		// Token: 0x060056A8 RID: 22184 RVA: 0x002C9EA8 File Offset: 0x002C82A8
		public static void CheckRejectGUIEvent()
		{
			if (UnityData.platform == RuntimePlatform.OSXPlayer)
			{
				if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp)
				{
					if (Time.frameCount != GUIEventFilterForOSX.lastRecordedFrame)
					{
						GUIEventFilterForOSX.eventsThisFrame.Clear();
						GUIEventFilterForOSX.lastRecordedFrame = Time.frameCount;
					}
					for (int i = 0; i < GUIEventFilterForOSX.eventsThisFrame.Count; i++)
					{
						if (GUIEventFilterForOSX.EventsAreEquivalent(GUIEventFilterForOSX.eventsThisFrame[i], Event.current))
						{
							GUIEventFilterForOSX.RejectEvent();
						}
					}
					GUIEventFilterForOSX.eventsThisFrame.Add(Event.current);
				}
			}
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x002C9F5C File Offset: 0x002C835C
		private static bool EventsAreEquivalent(Event A, Event B)
		{
			return A.button == B.button && A.keyCode == B.keyCode && A.type == B.type;
		}

		// Token: 0x060056AA RID: 22186 RVA: 0x002C9FA4 File Offset: 0x002C83A4
		private static void RejectEvent()
		{
			if (DebugViewSettings.logInput)
			{
				Log.Message(string.Concat(new object[]
				{
					"Frame ",
					Time.frameCount,
					": REJECTED ",
					Event.current.ToStringFull()
				}), false);
			}
			Event.current.Use();
		}

		// Token: 0x0400396C RID: 14700
		private static List<Event> eventsThisFrame = new List<Event>();

		// Token: 0x0400396D RID: 14701
		private static int lastRecordedFrame = -1;
	}
}
