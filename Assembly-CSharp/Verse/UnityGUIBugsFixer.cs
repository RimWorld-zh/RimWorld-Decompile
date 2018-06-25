using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EAC RID: 3756
	public static class UnityGUIBugsFixer
	{
		// Token: 0x04003AFE RID: 15102
		private static List<Resolution> resolutions = new List<Resolution>();

		// Token: 0x04003AFF RID: 15103
		private const float ScrollFactor = -6f;

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06005895 RID: 22677 RVA: 0x002D6CC0 File Offset: 0x002D50C0
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

		// Token: 0x06005896 RID: 22678 RVA: 0x002D6D8F File Offset: 0x002D518F
		public static void OnGUI()
		{
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x002D6D9C File Offset: 0x002D519C
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * -6f);
			}
		}

		// Token: 0x06005898 RID: 22680 RVA: 0x002D6E04 File Offset: 0x002D5204
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
