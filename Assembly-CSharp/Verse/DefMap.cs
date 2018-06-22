using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000EE7 RID: 3815
	public class DefMap<D, V> : IExposable, IEnumerable<KeyValuePair<D, V>>, IEnumerable where D : Def, new() where V : new()
	{
		// Token: 0x06005A9C RID: 23196 RVA: 0x002E7628 File Offset: 0x002E5A28
		public DefMap()
		{
			int defCount = DefDatabase<D>.DefCount;
			if (defCount == 0)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Constructed DefMap<",
					typeof(D),
					", ",
					typeof(V),
					"> without defs being initialized. Try constructing it in ResolveReferences instead of the constructor."
				}));
			}
			this.values = new List<V>(defCount);
			for (int i = 0; i < defCount; i++)
			{
				this.values.Add(Activator.CreateInstance<V>());
			}
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06005A9D RID: 23197 RVA: 0x002E76C4 File Offset: 0x002E5AC4
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x17000E53 RID: 3667
		public V this[D def]
		{
			get
			{
				return this.values[(int)def.index];
			}
			set
			{
				this.values[(int)def.index] = value;
			}
		}

		// Token: 0x17000E54 RID: 3668
		public V this[int index]
		{
			get
			{
				return this.values[index];
			}
			set
			{
				this.values[index] = value;
			}
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x002E7760 File Offset: 0x002E5B60
		public void ExposeData()
		{
			Scribe_Collections.Look<V>(ref this.values, "vals", LookMode.Undefined, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int defCount = DefDatabase<D>.DefCount;
				for (int i = this.values.Count; i < defCount; i++)
				{
					this.values.Add(Activator.CreateInstance<V>());
				}
				while (this.values.Count > defCount)
				{
					this.values.RemoveLast<V>();
				}
			}
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x002E77EC File Offset: 0x002E5BEC
		public void SetAll(V val)
		{
			for (int i = 0; i < this.values.Count; i++)
			{
				this.values[i] = val;
			}
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x002E7828 File Offset: 0x002E5C28
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x002E7844 File Offset: 0x002E5C44
		public IEnumerator<KeyValuePair<D, V>> GetEnumerator()
		{
			return (from d in DefDatabase<D>.AllDefsListForReading
			select new KeyValuePair<D, V>(d, this[d])).GetEnumerator();
		}

		// Token: 0x04003C97 RID: 15511
		private List<V> values = null;
	}
}
