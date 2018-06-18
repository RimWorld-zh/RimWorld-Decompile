using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F08 RID: 3848
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		// Token: 0x06005C53 RID: 23635 RVA: 0x002ED38A File Offset: 0x002EB78A
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06005C54 RID: 23636 RVA: 0x002ED39C File Offset: 0x002EB79C
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06005C55 RID: 23637 RVA: 0x002ED3B8 File Offset: 0x002EB7B8
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		// Token: 0x06005C56 RID: 23638 RVA: 0x002ED3D4 File Offset: 0x002EB7D4
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

		// Token: 0x06005C57 RID: 23639 RVA: 0x002ED43C File Offset: 0x002EB83C
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<T1>(seed, this.first);
			return Gen.HashCombine<T2>(seed, this.second);
		}

		// Token: 0x06005C58 RID: 23640 RVA: 0x002ED470 File Offset: 0x002EB870
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		// Token: 0x06005C59 RID: 23641 RVA: 0x002ED4A4 File Offset: 0x002EB8A4
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		// Token: 0x06005C5A RID: 23642 RVA: 0x002ED4F0 File Offset: 0x002EB8F0
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005C5B RID: 23643 RVA: 0x002ED510 File Offset: 0x002EB910
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04003CE6 RID: 15590
		private T1 first;

		// Token: 0x04003CE7 RID: 15591
		private T2 second;
	}
}
