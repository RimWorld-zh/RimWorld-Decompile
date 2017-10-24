using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class KeyPrefsData
	{
		public Dictionary<KeyBindingDef, KeyBindingData> keyPrefs = new Dictionary<KeyBindingDef, KeyBindingData>();

		public void ResetToDefaults()
		{
			this.keyPrefs.Clear();
			this.AddMissingDefaultBindings();
		}

		public void AddMissingDefaultBindings()
		{
			foreach (KeyBindingDef allDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				if (!this.keyPrefs.ContainsKey(allDef))
				{
					this.keyPrefs.Add(allDef, new KeyBindingData(allDef.defaultKeyCodeA, allDef.defaultKeyCodeB));
				}
			}
		}

		public bool SetBinding(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot, KeyCode keyCode)
		{
			KeyBindingData keyBindingData = default(KeyBindingData);
			bool result;
			if (this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				switch (slot)
				{
				case KeyPrefs.BindingSlot.A:
				{
					keyBindingData.keyBindingA = keyCode;
					goto IL_006d;
				}
				case KeyPrefs.BindingSlot.B:
				{
					keyBindingData.keyBindingB = keyCode;
					goto IL_006d;
				}
				default:
				{
					Log.Error("Tried to set a key binding for \"" + keyDef.LabelCap + "\" on a nonexistent slot: " + slot.ToString());
					result = false;
					break;
				}
				}
			}
			else
			{
				Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
				result = false;
			}
			goto IL_0096;
			IL_0096:
			return result;
			IL_006d:
			result = true;
			goto IL_0096;
		}

		public KeyCode GetBoundKeyCode(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyBindingData keyBindingData = default(KeyBindingData);
			KeyCode result;
			if (!this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
				result = KeyCode.None;
			}
			else
			{
				switch (slot)
				{
				case KeyPrefs.BindingSlot.A:
				{
					result = keyBindingData.keyBindingA;
					break;
				}
				case KeyPrefs.BindingSlot.B:
				{
					result = keyBindingData.keyBindingB;
					break;
				}
				default:
				{
					throw new InvalidOperationException();
				}
				}
			}
			return result;
		}

		private IEnumerable<KeyBindingDef> ConflictingBindings(KeyBindingDef keyDef, KeyCode code)
		{
			using (IEnumerator<KeyBindingDef> enumerator = DefDatabase<KeyBindingDef>.AllDefs.GetEnumerator())
			{
				KeyBindingDef def;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						_003CConflictingBindings_003Ec__Iterator0 _003CConflictingBindings_003Ec__Iterator = (_003CConflictingBindings_003Ec__Iterator0)/*Error near IL_005a: stateMachine*/;
						def = enumerator.Current;
						KeyBindingData prefData;
						if (def != keyDef && ((def.category == keyDef.category && def.category.selfConflicting) || keyDef.category.checkForConflicts.Contains(def.category) || (keyDef.extraConflictTags != null && def.extraConflictTags != null && keyDef.extraConflictTags.Any((Predicate<string>)((string tag) => def.extraConflictTags.Contains(tag))))) && this.keyPrefs.TryGetValue(def, out prefData))
						{
							if (prefData.keyBindingA == code)
								break;
							if (prefData.keyBindingB == code)
								break;
						}
						continue;
					}
					yield break;
				}
				yield return def;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_01f1:
			/*Error near IL_01f2: Unexpected return in MoveNext()*/;
		}

		public void EraseConflictingBindingsForKeyCode(KeyBindingDef keyDef, KeyCode keyCode, Action<KeyBindingDef> callBackOnErase = null)
		{
			foreach (KeyBindingDef item in this.ConflictingBindings(keyDef, keyCode))
			{
				KeyBindingData keyBindingData = this.keyPrefs[item];
				if (keyBindingData.keyBindingA == keyCode)
				{
					keyBindingData.keyBindingA = KeyCode.None;
				}
				if (keyBindingData.keyBindingB == keyCode)
				{
					keyBindingData.keyBindingB = KeyCode.None;
				}
				if ((object)callBackOnErase != null)
				{
					callBackOnErase(item);
				}
			}
		}

		public void CheckConflictsFor(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != 0)
			{
				this.EraseConflictingBindingsForKeyCode(keyDef, boundKeyCode, null);
				this.SetBinding(keyDef, slot, boundKeyCode);
			}
		}

		public KeyPrefsData Clone()
		{
			KeyPrefsData keyPrefsData = new KeyPrefsData();
			foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyPref in this.keyPrefs)
			{
				keyPrefsData.keyPrefs[keyPref.Key] = new KeyBindingData(keyPref.Value.keyBindingA, keyPref.Value.keyBindingB);
			}
			return keyPrefsData;
		}

		public void ErrorCheck()
		{
			foreach (KeyBindingDef allDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				this.ErrorCheckOn(allDef, KeyPrefs.BindingSlot.A);
				this.ErrorCheckOn(allDef, KeyPrefs.BindingSlot.B);
			}
		}

		private void ErrorCheckOn(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != 0)
			{
				foreach (KeyBindingDef item in this.ConflictingBindings(keyDef, boundKeyCode))
				{
					bool flag = boundKeyCode != keyDef.GetDefaultKeyCode(slot);
					Log.Error("Key binding conflict: " + item + " and " + keyDef + " are both bound to " + boundKeyCode + "." + ((!flag) ? "" : " Fixed automatically."));
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
