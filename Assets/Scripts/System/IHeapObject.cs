
public interface IHeapObject<T>
{
    public int HeapIndex { get; set; }  // Get and set the heap data index (HeapMin)

    /// <summary>
    /// Compare two heap objects to see which one has lower score.
    /// </summary>
    /// <param name="objectToCompare">Object to compare with.</param>
    /// <returns>Returns 1 if smaller score, or -1 if bigger score.</returns>
    public int CompareTo(T objectToCompare);
}
