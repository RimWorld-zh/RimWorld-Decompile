using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F61 RID: 3937
	public class KeyPrefsData
	{
		// Token: 0x04003E78 RID: 15992
		public Dictionary<KeyBindingDef, KeyBindingData> keyPrefs = new Dictionary<KeyBindingDef, KeyBindingData>();

		// Token: 0x06005F44 RID: 24388 RVA: 0x003095BF File Offset: 0x003079BF
		public void ResetToDefaults()
		{
			this.keyPrefs.Clear();
			this.AddMissingDefaultBindings();
		}

		// Token: 0x06005F45 RID: 24389 RVA: 0x003095D4 File Offset: 0x003079D4
		public void AddMissingDefaultBindings()
		{
			foreach (KeyBindingDef keyBindingDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				if (!this.keyPrefs.ContainsKey(keyBindingDef))
				{
					this.keyPrefs.Add(keyBindingDef, new KeyBindingData(keyBindingDef.defaultKeyCodeA, keyBindingDef.defaultKeyCodeB));
				}
			}
		}

		// Token: 0x06005F46 RID: 24390 RVA: 0x00309658 File Offset: 0x00307A58
		public bool SetBinding(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot, KeyCode keyCode)
		{
			KeyBindingData keyBindingData;
			bool result;
			if (this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				if (slot != KeyPrefs.BindingSlot.A)
				{
					if (slot != KeyPrefs.BindingSlot.B)
					{
						Log.Error("Tried to set a key binding for \"" + keyDef.LabelCap + "\" on a nonexistent slot: " + slot.ToString(), false);
						return false;
					}
					keyBindingData.keyBindingB = keyCode;
				}
				else
				{
					keyBindingData.keyBindingA = keyCode;
				}
				result = true;
			}
			else
			{
				Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"", false);
				result = false;
			}
			return result;
		}

		// Token: 0x06005F47 RID: 24391 RVA: 0x00309700 File Offset: 0x00307B00
		public KeyCode GetBoundKeyCode(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyBindingData keyBindingData;
			KeyCode result;
			if (!this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"", false);
				result = KeyCode.None;
			}
			else if (slot != KeyPrefs.BindingSlot.A)
			{
				if (slot != KeyPrefs.BindingSlot.B)
				{
					throw new InvalidOperationException();
				}
				result = keyBindingData.keyBindingB;
			}
			else
			{
				result = keyBindingData.keyBindingA;
			}
			return result;
		}

		// Token: 0x06005F48 RID: 24392 RVA: 0x00309778 File Offset: 0x00307B78
		private IEnumerable<KeyBindingDef> ConflictingBindings(KeyBindingDef keyDef, KeyCode code)
		{
			using (IEnumerator<KeyBindingDef> enumerator = DefDatabase<KeyBindingDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyBindingDef def = enumerator.Current;
					KeyBindingData prefData;
					if (def != keyDef && ((def.category == keyDef.category && def.category.selfConflicting) || keyDef.category.checkForConflicts.Contains(def.category) || (keyDef.extraConflictTags != null && def.extraConflictTags != null && keyDef.extraConflictTags.Any((string tag) => def.extraConflictTags.Contains(tag)))) && this.keyPrefs.TryGetValue(def, out prefData))
					{
						if (prefData.keyBindingA == code || prefData.keyBindingB == code)
						{
							yield return def;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06005F49 RID: 24393 RVA: 0x003097B0 File Offset: 0x00307BB0
		public void EraseConflictingBindingsForKeyCode(KeyBindingDef keyDef, KeyCode keyCode, Action<KeyBindingDef> callBackOnErase = null)
		{
			foreach (KeyBindingDef keyBindingDef in this.ConflictingBindings(keyDef, keyCode))
			{
				KeyBindingData keyBindingData = this.keyPrefs[keyBindingDef];
				if (keyBindingData.keyBindingA == keyCode)
				{
					keyBindingData.keyBindingA = KeyCode.None;
				}
				if (keyBindingData.keyBindingB == keyCode)
				{
					keyBindingData.keyBindingB = KeyCode.None;
				}
				if (callBackOnErase != null)
				{
					callBackOnErase(keyBindingDef);
				}
			}
		}

		// Token: 0x06005F4A RID: 24394 RVA: 0x00309848 File Offset: 0x00307C48
		public void CheckConflictsFor(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != KeyCode.None)
			{
				this.EraseConflictingBindingsForKeyCode(keyDef, boundKeyCode, null);
				this.SetBinding(keyDef, slot, boundKeyCode);
			}
		}

		// Token: 0x06005F4B RID: 24395 RVA: 0x0030987C File Offset: 0x00307C7C
		public KeyPrefsData Clone()
		{
			KeyPrefsData keyPrefsData = new KeyPrefsData();
			foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyValuePair in this.keyPrefs)
			{
				keyPrefsData.keyPrefs[keyValuePair.Key] = new KeyBindingData(keyValuePair.Value.keyBindingA, keyValuePair.Value.keyBindingB);
			}
			return keyPrefsData;
		}

		// Token: 0x06005F4C RID: 24396 RVA: 0x00309914 File Offset: 0x00307D14
		public void ErrorCheck()
		{
			foreach (KeyBindingDef keyDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.A);
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.B);
			}
		}

		// Token: 0x06005F4D RID: 24397 RVA: 0x0030997C File Offset: 0x00307D7C
		private void ErrorCheckOn(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != KeyCode.None)
			{
				foreach (KeyBindingDef keyBindingDef in this.ConflictingBindings(keyDef, boundKeyCode))
				{
					bool flag = boundKeyCode != keyDef.GetDefaultKeyCode(slot);
					Log.Error(string.Concat(new object[]
					{
						"Key binding conflict: ",
						keyBindingDef,
						" and ",
						keyDef,
						" are both bound to ",
						boundKeyCode,
						".",
						(!flag) ? "" : " Fixed automatically."
					}), false);
					if (flag)
					{
						if (slot == KeyPrefs.BindingSlot.A)
						{
							this.keyPrefs[keyDef].keyBindingA = keyDef.defaultKeyCodeA;
						}
						else
						{
							this.keyPrefs[keyDef].keyBindingB = keyDef.defaultKeyCodeB;
						}
						KeyPrefs.Save();
					}
				}
			}
		}
	}
}
