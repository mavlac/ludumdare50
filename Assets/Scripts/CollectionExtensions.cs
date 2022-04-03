using System.Collections.Generic;

public static class CollectionExtensions
{
	public static bool IsNullOrEmpty<T>(this T[] array) => (array == null || array.Length == 0);
	public static bool IsNullOrEmpty<T>(this List<T> list) => (list == null || list.Count == 0);
	
	
	
	public static T Random<T>(this T[] array)
	{
		if (array.IsNullOrEmpty()) return default; // default(T) - null, 0..
		
		return array[UnityEngine.Random.Range(0, array.Length)];
	}
	public static T Random<T>(this List<T> list)
	{
		if (list.IsNullOrEmpty()) return default;
		
		return list[UnityEngine.Random.Range(0, list.Count)];
	}
	
	
	
	/// <summary>
	/// Shuffle the List using the Fisher-Yates method
	/// </summary>
	public static void Shuffle<T>(this List<T> list)
	{
		System.Random rng = new System.Random();
		
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
	
	
	
	/// <summary>
	/// Pick random item and remove it from the List
	/// </summary>
	/// <returns>Removed item</returns>
	public static T RandomRemove<T>(this List<T> list)
	{
		if (list.IsNullOrEmpty()) return default;
		
		var item = list.Random();
		list.Remove(item);
		
		return item;
	}
}