using System.Collections.Generic;

public static class ListExtension
{
    public static T Next<T>(this List<T> list, ref int currentIndex)
    {
        currentIndex++;
        if (currentIndex >= list.Count)
            currentIndex = 0;
        return list[currentIndex];
    }

    public static T Previous<T>(this List<T> list, ref int currentIndex)
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = 0;
        return list[currentIndex];
    }

    public static T Next<T>(this T[] array, ref int currentIndex)
    {
        currentIndex++;
        if (currentIndex >= array.Length)
            currentIndex = 0;
        return array[currentIndex];
    }

    public static T Previous<T>(this T[] array, ref int currentIndex)
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = 0;
        return array[currentIndex];
    }
}
