using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003E4 RID: 996
	public class SymbolStack
	{
		// Token: 0x04000A58 RID: 2648
		private Stack<Pair<string, ResolveParams>> stack = new Stack<Pair<string, ResolveParams>>();

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x00091BA8 File Offset: 0x0008FFA8
		public bool Empty
		{
			get
			{
				return this.stack.Count == 0;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x00091BCC File Offset: 0x0008FFCC
		public int Count
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00091BEC File Offset: 0x0008FFEC
		public void Push(string symbol, ResolveParams resolveParams)
		{
			this.stack.Push(new Pair<string, ResolveParams>(symbol, resolveParams));
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00091C04 File Offset: 0x00090004
		public void Push(string symbol, CellRect rect)
		{
			this.Push(symbol, new ResolveParams
			{
				rect = rect
			});
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00091C2C File Offset: 0x0009002C
		public void PushMany(ResolveParams resolveParams, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], resolveParams);
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00091C5C File Offset: 0x0009005C
		public void PushMany(CellRect rect, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], rect);
			}
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00091C8C File Offset: 0x0009008C
		public Pair<string, ResolveParams> Pop()
		{
			return this.stack.Pop();
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00091CAC File Offset: 0x000900AC
		public void Clear()
		{
			this.stack.Clear();
		}
	}
}
