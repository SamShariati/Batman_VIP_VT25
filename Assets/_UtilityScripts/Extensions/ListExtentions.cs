using System.Collections.Generic;

public static class ListExtentions
{

    public static void Shuffle<T>(this List<T> list)
    {
        int count = list.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = UnityEngine.Random.Range(i, count);
            (list[r], list[i]) = (list[i], list[r]);
        }
    }
}

public static class ArrayUtil
{

    public static void ShuffleList<T>(IList<T> list)
    {
        int count = list.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = UnityEngine.Random.Range(i, count);
            (list[r], list[i]) = (list[i], list[r]);
        }
    }

    public static void Shuffle<T>(T[] arr)
    {
        int count = arr.Length;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = UnityEngine.Random.Range(i, count);
            (arr[r], arr[i]) = (arr[i], arr[r]);
        }
    }

}
