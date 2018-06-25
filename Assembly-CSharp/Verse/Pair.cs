using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F0D RID: 3853
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		// Token: 0x04003D04 RID: 15620
		private T1 first;

		// Token: 0x04003D05 RID: 15621
		private T2 second;

		// Token: 0x06005C85 RID: 23685 RVA: 0x002EFC5E File Offset: 0x002EE05E
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06005C86 RID: 23686 RVA: 0x002EFC70 File Offset: 0x002EE070
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06005C87 RID: 23687 RVA: 0x002EFC8C File Offset: 0x002EE08C
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		// Token: 0x06005C88 RID: 23688 RVA: 0x002EFCA8 File Offset: 0x002EE0A8
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			T1 t = this.First;
			array[num] = t.ToString();
			array[2] = ", ";
			int num2 = 3;
			T2 t2 = this.Second;
			array[num2] = t2.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x06005C89 RID: 23689 RVA: 0x002EFD10 File Offset: 0x002EE110
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<T1>(seed, this.first);
			return Gen.HashCombine<T2>(seed, this.second);
		}

		// Token: 0x06005C8A RID: 23690 RVA: 0x002EFD44 File Offset: 0x002EE144
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		// Token: 0x06005C8B RID: 23691 RVA: 0x002EFD78 File Offset: 0x002EE178
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x002EFDC4 File Offset: 0x002EE1C4
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x002EFDE4 File Offset: 0x002EE1E4
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}
	}
}
