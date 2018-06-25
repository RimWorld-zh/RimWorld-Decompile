using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public class BodyPartRecord
	{
		public BodyDef body;

		[TranslationHandle]
		public BodyPartDef def = null;

		[MustTranslate]
		public string customLabel;

		[TranslationHandle(Priority = 100)]
		[Unsaved]
		public string untranslatedCustomLabel = null;

		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		public BodyPartHeight height = BodyPartHeight.Undefined;

		public BodyPartDepth depth = BodyPartDepth.Undefined;

		public float coverage = 1f;

		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		[Unsaved]
		public BodyPartRecord parent = null;

		[Unsaved]
		public float coverageAbsWithChildren = 0f;

		[Unsaved]
		public float coverageAbs = 0f;

		public BodyPartRecord()
		{
		}

		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		public string Label
		{
			get
			{
				return (!this.customLabel.NullOrEmpty()) ? this.customLabel : this.def.label;
			}
		}

		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"BodyPartRecord(",
				(this.def == null) ? "NULL_DEF" : this.def.defName,
				" parts.Count=",
				this.parts.Count,
				")"
			});
		}

		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		public bool IsInGroup(BodyPartGroupDef group)
		{
			for (int i = 0; i < this.groups.Count; i++)
			{
				if (this.groups[i] == group)
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<BodyPartRecord> GetChildParts(BodyPartTagDef tag)
		{
			if (this.def.tags.Contains(tag))
			{
				yield return this;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				foreach (BodyPartRecord record in this.parts[i].GetChildParts(tag))
				{
					yield return record;
				}
			}
			yield break;
		}

		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				yield return this.parts[i];
			}
			yield break;
		}

		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		public IEnumerable<BodyPartRecord> GetConnectedParts(BodyPartTagDef tag)
		{
			BodyPartRecord ancestor = this;
			while (ancestor.parent != null && ancestor.parent.def.tags.Contains(tag))
			{
				ancestor = ancestor.parent;
			}
			foreach (BodyPartRecord child in ancestor.GetChildParts(tag))
			{
				yield return child;
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GetChildParts>c__Iterator0 : IEnumerable, IEnumerable<BodyPartRecord>, IEnumerator, IDisposable, IEnumerator<BodyPartRecord>
		{
			internal BodyPartTagDef tag;

			internal int <i>__1;

			internal IEnumerator<BodyPartRecord> $locvar0;

			internal BodyPartRecord <record>__2;

			internal BodyPartRecord $this;

			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetChildParts>c__Iterator0()
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
					if (this.def.tags.Contains(tag))
					{
						this.$current = this;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					Block_4:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							record = enumerator.Current;
							this.$current = record;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
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
					i++;
					goto IL_12C;
				default:
					return false;
				}
				i = 0;
				IL_12C:
				if (i < this.parts.Count)
				{
					enumerator = this.parts[i].GetChildParts(tag).GetEnumerator();
					num = 4294967293u;
					goto Block_4;
				}
				this.$PC = -1;
				return false;
			}

			BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
				case 2u:
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
				return this.System.Collections.Generic.IEnumerable<Verse.BodyPartRecord>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				BodyPartRecord.<GetChildParts>c__Iterator0 <GetChildParts>c__Iterator = new BodyPartRecord.<GetChildParts>c__Iterator0();
				<GetChildParts>c__Iterator.$this = this;
				<GetChildParts>c__Iterator.tag = tag;
				return <GetChildParts>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetDirectChildParts>c__Iterator1 : IEnumerable, IEnumerable<BodyPartRecord>, IEnumerator, IDisposable, IEnumerator<BodyPartRecord>
		{
			internal int <i>__1;

			internal BodyPartRecord $this;

			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetDirectChildParts>c__Iterator1()
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
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < this.parts.Count)
				{
					this.$current = this.parts[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.BodyPartRecord>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				BodyPartRecord.<GetDirectChildParts>c__Iterator1 <GetDirectChildParts>c__Iterator = new BodyPartRecord.<GetDirectChildParts>c__Iterator1();
				<GetDirectChildParts>c__Iterator.$this = this;
				return <GetDirectChildParts>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetConnectedParts>c__Iterator2 : IEnumerable, IEnumerable<BodyPartRecord>, IEnumerator, IDisposable, IEnumerator<BodyPartRecord>
		{
			internal BodyPartRecord <ancestor>__0;

			internal BodyPartTagDef tag;

			internal IEnumerator<BodyPartRecord> $locvar0;

			internal BodyPartRecord <child>__1;

			internal BodyPartRecord $this;

			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetConnectedParts>c__Iterator2()
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
					ancestor = this;
					while (ancestor.parent != null && ancestor.parent.def.tags.Contains(tag))
					{
						ancestor = ancestor.parent;
					}
					enumerator = ancestor.GetChildParts(tag).GetEnumerator();
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
					}
					if (enumerator.MoveNext())
					{
						child = enumerator.Current;
						this.$current = child;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
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

			BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.BodyPartRecord>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				BodyPartRecord.<GetConnectedParts>c__Iterator2 <GetConnectedParts>c__Iterator = new BodyPartRecord.<GetConnectedParts>c__Iterator2();
				<GetConnectedParts>c__Iterator.$this = this;
				<GetConnectedParts>c__Iterator.tag = tag;
				return <GetConnectedParts>c__Iterator;
			}
		}
	}
}
