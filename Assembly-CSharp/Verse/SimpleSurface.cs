using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public class SimpleSurface : IEnumerable<SurfaceColumn>, IEnumerable
	{
		private List<SurfaceColumn> columns = new List<SurfaceColumn>();

		public SimpleSurface()
		{
		}

		public float Evaluate(float x, float y)
		{
			float result;
			if (this.columns.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve2D with no columns.", false);
				result = 0f;
			}
			else if (x <= this.columns[0].x)
			{
				result = this.columns[0].y.Evaluate(y);
			}
			else if (x >= this.columns[this.columns.Count - 1].x)
			{
				result = this.columns[this.columns.Count - 1].y.Evaluate(y);
			}
			else
			{
				SurfaceColumn surfaceColumn = this.columns[0];
				SurfaceColumn surfaceColumn2 = this.columns[this.columns.Count - 1];
				for (int i = 0; i < this.columns.Count; i++)
				{
					if (x <= this.columns[i].x)
					{
						surfaceColumn2 = this.columns[i];
						if (i > 0)
						{
							surfaceColumn = this.columns[i - 1];
						}
						break;
					}
				}
				float t = (x - surfaceColumn.x) / (surfaceColumn2.x - surfaceColumn.x);
				result = Mathf.Lerp(surfaceColumn.y.Evaluate(y), surfaceColumn2.y.Evaluate(y), t);
			}
			return result;
		}

		public void Add(SurfaceColumn newColumn)
		{
			this.columns.Add(newColumn);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IEnumerator<SurfaceColumn> GetEnumerator()
		{
			foreach (SurfaceColumn column in this.columns)
			{
				yield return column;
			}
			yield break;
		}

		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.columns.Count - 1; i++)
			{
				if (this.columns[i + 1].x < this.columns[i].x)
				{
					yield return prefix + ": columns are out of order";
					break;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GetEnumerator>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<SurfaceColumn>
		{
			internal List<SurfaceColumn>.Enumerator $locvar0;

			internal SurfaceColumn <column>__1;

			internal SimpleSurface $this;

			internal SurfaceColumn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetEnumerator>c__Iterator0()
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
					enumerator = this.columns.GetEnumerator();
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
						column = enumerator.Current;
						this.$current = column;
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
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			SurfaceColumn IEnumerator<SurfaceColumn>.Current
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
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator1 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal int <i>__1;

			internal string prefix;

			internal SimpleSurface $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					for (i = 0; i < this.columns.Count - 1; i++)
					{
						if (this.columns[i + 1].x < this.columns[i].x)
						{
							this.$current = prefix + ": columns are out of order";
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				SimpleSurface.<ConfigErrors>c__Iterator1 <ConfigErrors>c__Iterator = new SimpleSurface.<ConfigErrors>c__Iterator1();
				<ConfigErrors>c__Iterator.$this = this;
				<ConfigErrors>c__Iterator.prefix = prefix;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
