using UnityEngine;
using System;

public class Tuple<T1, T2>
{
	public T1 Item1; /*{ get; private set; }*/
	public T2 Item2; /*{ get; private set; }*/
	internal Tuple(T1 item1, T2 item2)
	{
		Item1 = item1;
		Item2 = item2;
	}
}
public static class Tuple
{
	public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
	{
		var tuple = new Tuple<T1, T2>(item1, item2);
		return tuple;
	}
}

[System.Serializable]
public class TupleInt : Tuple<int, int>
{
	public static TupleInt Zero
	{
		get { return new TupleInt(0, 0); }
	}
	public TupleInt(int a, int b) : base(a, b){ }

	//Local Functions
	public void Add(TupleInt p_tuple)
	{
		Item1 += p_tuple.Item1;
		Item2 += p_tuple.Item2;
	}
	public void AddOrientation(Orientation p_ori)
	{
		Add (OrientationToTuple (p_ori));
	}
	public Vector2 ToVector2()
	{
		return new Vector2 (Item1, Item2);
	}
	public int ToIndex(TupleInt p_gridSize)
	{
		return Item1 + (Item2 * p_gridSize.Item1);
	}
	//Static Functions
	public static TupleInt AddTuples(TupleInt p_tuple1, TupleInt p_tuple2)
	{
		return new TupleInt(p_tuple1.Item1 + p_tuple2.Item1, p_tuple1.Item2 + p_tuple2.Item2);
	}
	public static TupleInt AddTuples(TupleInt p_tuple, Orientation p_ori)
	{
		return AddTuples(p_tuple, OrientationToTuple(p_ori));
	}
	public static TupleInt OrientationToTuple(Orientation p_ori)
	{
		return new TupleInt (Mathf.RoundToInt (Mathf.Cos ((int)p_ori * 90f * Mathf.Deg2Rad)),
			Mathf.RoundToInt (Mathf.Sin ((int)p_ori * -90f * Mathf.Deg2Rad)));
	}
	public static TupleInt IndexToTuple(int p_index, TupleInt p_gridSize)
	{
		return new TupleInt (p_index % p_gridSize.Item1, p_index / p_gridSize.Item1);
	}
	public static int TupleToIndex(TupleInt p_pos, TupleInt p_gridSize)
	{
		return p_pos.Item1 + (p_pos.Item2 * p_gridSize.Item1);
	}
	public static bool IsEqual (TupleInt p_tuple1, TupleInt p_tuple2)
	{
		if (p_tuple1.Item1 == p_tuple2.Item1 && 
			p_tuple1.Item2 == p_tuple2.Item2)
			return true;
		return false;
	}

	public override bool Equals(System.Object obj)
	{
		if (obj == null)
			return false;
		TupleInt __tuple = obj as TupleInt ;
		if ((System.Object)__tuple == null)
			return false;
		
		if (__tuple.Item1 == Item1 && 
			__tuple.Item2 == Item2)
			return true;
		return false;
	}
	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}
	public bool Equals(TupleInt p_tuple)
	{
		if ((object)p_tuple == null) 
			return false;
		
		if (p_tuple.Item1 == Item1 && 
			p_tuple.Item2 == Item2)
			return true;
		return false;
	}
}