using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000EE9 RID: 3817
	public class DefMap<D, V> : IExposable, IEnumerable<KeyValuePair<D, V>>, IEnumerable where D : Def, new() where V : new()
	{
		// Token: 0x04003C97 RID: 15511
		private List<V> values = null;

		// Token: 0x06005A9F RID: 23199 RVA: 0x002E7748 File Offset: 0x002E5B48
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

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x002E77E4 File Offset: 0x002E5BE4
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x17000E52 RID: 3666
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

		// Token: 0x17000E53 RID: 3667
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

		// Token: 0x06005AA5 RID: 23205 RVA: 0x002E7880 File Offset: 0x002E5C80
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

		// Token: 0x06005AA6 RID: 23206 RVA: 0x002E790C File Offset: 0x002E5D0C
		public void SetAll(V val)
		{
			for (int i = 0; i < this.values.Count; i++)
			{
				this.values[i] = val;
			}
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x002E7948 File Offset: 0x002E5D48
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x002E7964 File Offset: 0x002E5D64
		public IEnumerator<KeyValuePair<D, V>> GetEnumerator()
		{
			return (from d in DefDatabase<D>.AllDefsListForReading
			select new KeyValuePair<D, V>(d, this[d])).GetEnumerator();
		}
	}
}
