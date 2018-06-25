using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class NameBank
	{
		public PawnNameCategory nameType;

		private List<string>[,] names;

		private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

		private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;

		[CompilerGenerated]
		private static Func<string, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<IGrouping<string, string>, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<IGrouping<string, string>, string> <>f__am$cache2;

		public NameBank(PawnNameCategory ID)
		{
			this.nameType = ID;
			this.names = new List<string>[NameBank.numGenders, NameBank.numSlots];
			for (int i = 0; i < NameBank.numGenders; i++)
			{
				for (int j = 0; j < NameBank.numSlots; j++)
				{
					this.names[i, j] = new List<string>();
				}
			}
		}

		private IEnumerable<List<string>> AllNameLists
		{
			get
			{
				for (int i = 0; i < NameBank.numGenders; i++)
				{
					for (int j = 0; j < NameBank.numSlots; j++)
					{
						yield return this.names[i, j];
					}
				}
				yield break;
			}
		}

		public void ErrorCheck()
		{
			foreach (List<string> list in this.AllNameLists)
			{
				List<string> list2 = (from x in list
				group x by x into g
				where g.Count<string>() > 1
				select g.Key).ToList<string>();
				foreach (string str in list2)
				{
					Log.Error("Duplicated name: " + str, false);
				}
				foreach (string text in list)
				{
					if (text.Trim() != text)
					{
						Log.Error("Trimmable whitespace on name: [" + text + "]", false);
					}
				}
			}
		}

		private List<string> NamesFor(PawnNameSlot slot, Gender gender)
		{
			return this.names[(int)gender, (int)slot];
		}

		public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
		{
			foreach (string item in namesToAdd)
			{
				this.NamesFor(slot, gender).Add(item);
			}
		}

		public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
		{
			this.AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
		}

		public string GetName(PawnNameSlot slot, Gender gender = Gender.None, bool checkIfAlreadyUsed = true)
		{
			List<string> list = this.NamesFor(slot, gender);
			int num = 0;
			string result;
			if (list.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Name list for gender=",
					gender,
					" slot=",
					slot,
					" is empty."
				}), false);
				result = "Errorname";
			}
			else
			{
				string text;
				for (;;)
				{
					text = list.RandomElement<string>();
					if (checkIfAlreadyUsed && !NameUseChecker.NameWordIsUsed(text))
					{
						break;
					}
					num++;
					if (num > 50)
					{
						goto Block_4;
					}
				}
				return text;
				Block_4:
				result = text;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static NameBank()
		{
		}

		[CompilerGenerated]
		private static string <ErrorCheck>m__0(string x)
		{
			return x;
		}

		[CompilerGenerated]
		private static bool <ErrorCheck>m__1(IGrouping<string, string> g)
		{
			return g.Count<string>() > 1;
		}

		[CompilerGenerated]
		private static string <ErrorCheck>m__2(IGrouping<string, string> g)
		{
			return g.Key;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<List<string>>, IEnumerator, IDisposable, IEnumerator<List<string>>
		{
			internal int <i>__1;

			internal int <j>__2;

			internal NameBank $this;

			internal List<string> $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					goto IL_A0;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_81:
				if (j < NameBank.numSlots)
				{
					this.$current = this.names[i, j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_A0:
				if (i < NameBank.numGenders)
				{
					j = 0;
					goto IL_81;
				}
				this.$PC = -1;
				return false;
			}

			List<string> IEnumerator<List<string>>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.List<string>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<List<string>> IEnumerable<List<string>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				NameBank.<>c__Iterator0 <>c__Iterator = new NameBank.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
