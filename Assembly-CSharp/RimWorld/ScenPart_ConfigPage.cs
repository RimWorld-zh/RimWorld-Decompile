using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class ScenPart_ConfigPage : ScenPart
	{
		public ScenPart_ConfigPage()
		{
		}

		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}

		[CompilerGenerated]
		private sealed class <GetConfigPages>c__Iterator0 : IEnumerable, IEnumerable<Page>, IEnumerator, IDisposable, IEnumerator<Page>
		{
			internal ScenPart_ConfigPage $this;

			internal Page $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetConfigPages>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = (Page)Activator.CreateInstance(this.def.pageClass);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Page IEnumerator<Page>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Page>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Page> IEnumerable<Page>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ScenPart_ConfigPage.<GetConfigPages>c__Iterator0 <GetConfigPages>c__Iterator = new ScenPart_ConfigPage.<GetConfigPages>c__Iterator0();
				<GetConfigPages>c__Iterator.$this = this;
				return <GetConfigPages>c__Iterator;
			}
		}
	}
}
