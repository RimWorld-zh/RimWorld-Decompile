using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public class KeyPrefsData
	{
		public Dictionary<KeyBindingDef, KeyBindingData> keyPrefs = new Dictionary<KeyBindingDef, KeyBindingData>();

		public KeyPrefsData()
		{
		}

		public void ResetToDefaults()
		{
			this.keyPrefs.Clear();
			this.AddMissingDefaultBindings();
		}

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

		public void CheckConflictsFor(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != KeyCode.None)
			{
				this.EraseConflictingBindingsForKeyCode(keyDef, boundKeyCode, null);
				this.SetBinding(keyDef, slot, boundKeyCode);
			}
		}

		public KeyPrefsData Clone()
		{
			KeyPrefsData keyPrefsData = new KeyPrefsData();
			foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyValuePair in this.keyPrefs)
			{
				keyPrefsData.keyPrefs[keyValuePair.Key] = new KeyBindingData(keyValuePair.Value.keyBindingA, keyValuePair.Value.keyBindingB);
			}
			return keyPrefsData;
		}

		public void ErrorCheck()
		{
			foreach (KeyBindingDef keyDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.A);
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.B);
			}
		}

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

		[CompilerGenerated]
		private sealed class <ConflictingBindings>c__Iterator0 : IEnumerable, IEnumerable<KeyBindingDef>, IEnumerator, IDisposable, IEnumerator<KeyBindingDef>
		{
			internal IEnumerator<KeyBindingDef> $locvar0;

			internal KeyBindingDef keyDef;

			internal KeyBindingData <prefData>__2;

			internal KeyCode code;

			internal KeyPrefsData $this;

			internal KeyBindingDef $current;

			internal bool $disposing;

			internal int $PC;

			private KeyPrefsData.<ConflictingBindings>c__Iterator0.<ConflictingBindings>c__AnonStorey1 $locvar1;

			[DebuggerHidden]
			public <ConflictingBindings>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = DefDatabase<KeyBindingDef>.AllDefs.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_1B6:
						break;
					}
					IL_1B7:
					if (enumerator.MoveNext())
					{
						KeyBindingDef def = enumerator.Current;
						if (def == keyDef || ((def.category != keyDef.category || !def.category.selfConflicting) && !keyDef.category.checkForConflicts.Contains(def.category) && (keyDef.extraConflictTags == null || def.extraConflictTags == null || !keyDef.extraConflictTags.Any((string tag) => def.extraConflictTags.Contains(tag)))) || !this.keyPrefs.TryGetValue(def, out prefData))
						{
							goto IL_1B7;
						}
						if (prefData.keyBindingA == code || prefData.keyBindingB == code)
						{
							this.$current = def;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_1B6;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			KeyBindingDef IEnumerator<KeyBindingDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.KeyBindingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<KeyBindingDef> IEnumerable<KeyBindingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				KeyPrefsData.<ConflictingBindings>c__Iterator0 <ConflictingBindings>c__Iterator = new KeyPrefsData.<ConflictingBindings>c__Iterator0();
				<ConflictingBindings>c__Iterator.$this = this;
				<ConflictingBindings>c__Iterator.keyDef = keyDef;
				<ConflictingBindings>c__Iterator.code = code;
				return <ConflictingBindings>c__Iterator;
			}

			private sealed class <ConflictingBindings>c__AnonStorey1
			{
				internal KeyBindingDef def;

				internal KeyPrefsData.<ConflictingBindings>c__Iterator0 <>f__ref$0;

				public <ConflictingBindings>c__AnonStorey1()
				{
				}

				internal bool <>m__0(string tag)
				{
					return this.def.extraConflictTags.Contains(tag);
				}
			}
		}
	}
}
