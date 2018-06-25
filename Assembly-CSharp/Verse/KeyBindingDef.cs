using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B4D RID: 2893
	public class KeyBindingDef : Def
	{
		// Token: 0x040029DB RID: 10715
		public KeyBindingCategoryDef category;

		// Token: 0x040029DC RID: 10716
		public KeyCode defaultKeyCodeA;

		// Token: 0x040029DD RID: 10717
		public KeyCode defaultKeyCodeB;

		// Token: 0x040029DE RID: 10718
		public bool devModeOnly = false;

		// Token: 0x040029DF RID: 10719
		[NoTranslate]
		public List<string> extraConflictTags;

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06003F5A RID: 16218 RVA: 0x00216858 File Offset: 0x00214C58
		public KeyCode MainKey
		{
			get
			{
				KeyBindingData keyBindingData;
				if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					if (keyBindingData.keyBindingA != KeyCode.None)
					{
						return keyBindingData.keyBindingA;
					}
					if (keyBindingData.keyBindingB != KeyCode.None)
					{
						return keyBindingData.keyBindingB;
					}
				}
				return KeyCode.None;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06003F5B RID: 16219 RVA: 0x002168B8 File Offset: 0x00214CB8
		public string MainKeyLabel
		{
			get
			{
				return this.MainKey.ToStringReadable();
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06003F5C RID: 16220 RVA: 0x002168D8 File Offset: 0x00214CD8
		public bool KeyDownEvent
		{
			get
			{
				if (Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None)
				{
					KeyBindingData keyBindingData;
					if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
					{
						return (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand || !Event.current.command) && (Event.current.keyCode == keyBindingData.keyBindingA || Event.current.keyCode == keyBindingData.keyBindingB);
					}
				}
				return false;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06003F5D RID: 16221 RVA: 0x002169B0 File Offset: 0x00214DB0
		public bool IsDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current != null && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (this.KeyDownEvent || (Event.current.shift && (keyBindingData.keyBindingA == KeyCode.LeftShift || keyBindingData.keyBindingA == KeyCode.RightShift || keyBindingData.keyBindingB == KeyCode.LeftShift || keyBindingData.keyBindingB == KeyCode.RightShift)) || (Event.current.control && (keyBindingData.keyBindingA == KeyCode.LeftControl || keyBindingData.keyBindingA == KeyCode.RightControl || keyBindingData.keyBindingB == KeyCode.LeftControl || keyBindingData.keyBindingB == KeyCode.RightControl)) || (Event.current.alt && (keyBindingData.keyBindingA == KeyCode.LeftAlt || keyBindingData.keyBindingA == KeyCode.RightAlt || keyBindingData.keyBindingB == KeyCode.LeftAlt || keyBindingData.keyBindingB == KeyCode.RightAlt)) || (Event.current.command && (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand)) || this.IsDown);
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06003F5E RID: 16222 RVA: 0x00216B68 File Offset: 0x00214F68
		public bool JustPressed
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKeyDown(keyBindingData.keyBindingA) || Input.GetKeyDown(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06003F5F RID: 16223 RVA: 0x00216BBC File Offset: 0x00214FBC
		public bool IsDown
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKey(keyBindingData.keyBindingA) || Input.GetKey(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x00216C10 File Offset: 0x00215010
		public KeyCode GetDefaultKeyCode(KeyPrefs.BindingSlot slot)
		{
			KeyCode result;
			if (slot == KeyPrefs.BindingSlot.A)
			{
				result = this.defaultKeyCodeA;
			}
			else
			{
				if (slot != KeyPrefs.BindingSlot.B)
				{
					throw new InvalidOperationException();
				}
				result = this.defaultKeyCodeB;
			}
			return result;
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x00216C4C File Offset: 0x0021504C
		public static KeyBindingDef Named(string name)
		{
			return DefDatabase<KeyBindingDef>.GetNamedSilentFail(name);
		}
	}
}
