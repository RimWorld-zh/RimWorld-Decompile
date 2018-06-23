using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EAA RID: 3754
	public static class UnityGUIBugsFixer
	{
		// Token: 0x04003AFE RID: 15102
		private static List<Resolution> resolutions = new List<Resolution>();

		// Token: 0x04003AFF RID: 15103
		private const float ScrollFactor = -6f;

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06005891 RID: 22673 RVA: 0x002D6B94 File Offset: 0x002D4F94
		public static List<Resolution> ScreenResolutionsWithoutDuplicates
		{
			get
			{
				UnityGUIBugsFixer.resolutions.Clear();
				Resolution[] array = Screen.resolutions;
				for (int i = 0; i < array.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < UnityGUIBugsFixer.resolutions.Count; j++)
					{
						if (UnityGUIBugsFixer.resolutions[j].width == array[i].width && UnityGUIBugsFixer.resolutions[j].height == array[i].height)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						UnityGUIBugsFixer.resolutions.Add(array[i]);
					}
				}
				return UnityGUIBugsFixer.resolutions;
			}
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x002D6C63 File Offset: 0x002D5063
		public static void OnGUI()
		{
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x002D6C70 File Offset: 0x002D5070
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * -6f);
			}
		}

		// Token: 0x06005894 RID: 22676 RVA: 0x002D6CD8 File Offset: 0x002D50D8
		private static void FixShift()
		{
			if (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer)
			{
				if (!Event.current.shift)
				{
					Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
				}
			}
		}
	}
}
