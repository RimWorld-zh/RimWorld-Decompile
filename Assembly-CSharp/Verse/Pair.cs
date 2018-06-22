using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F08 RID: 3848
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		// Token: 0x06005C7B RID: 23675 RVA: 0x002EF3BE File Offset: 0x002ED7BE
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06005C7C RID: 23676 RVA: 0x002EF3D0 File Offset: 0x002ED7D0
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06005C7D RID: 23677 RVA: 0x002EF3EC File Offset: 0x002ED7EC
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x002EF408 File Offset: 0x002ED808
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

		// Token: 0x06005C7F RID: 23679 RVA: 0x002EF470 File Offset: 0x002ED870
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<T1>(seed, this.first);
			return Gen.HashCombine<T2>(seed, this.second);
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x002EF4A4 File Offset: 0x002ED8A4
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x002EF4D8 File Offset: 0x002ED8D8
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x002EF524 File Offset: 0x002ED924
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x002EF544 File Offset: 0x002ED944
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04003CF9 RID: 15609
		private T1 first;

		// Token: 0x04003CFA RID: 15610
		private T2 second;
	}
}
