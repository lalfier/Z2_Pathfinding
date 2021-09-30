
public class HeapMin<T> where T : class, IHeapObject<T>
{
    private T[] objects;   // Array of all objects
    private int objectCount;  // Number of open objects inside array

    public int Count { get { return objectCount; } }  // Get number of open objects inside array

    /// <summary>
    /// Constructor for heap min object.
    /// </summary>
    /// <param name="maxSize">Number of all objects.</param>
    public HeapMin(int maxSize)
    {
        objects = new T[maxSize];
    }

    /// <summary>
    /// Adds open object to the array and sort it up.
    /// </summary>
    /// <param name="hObject">Object to add.</param>
    public void Add(T hObject)
    {
        // Add new object as last and sort it up
        hObject.HeapIndex = objectCount;
        objects[objectCount] = hObject;
        SortUp(hObject);
        objectCount++;
    }

    /// <summary>
    /// Removes first object from the array and sort it down.
    /// </summary>
    /// <returns>Returns removed object.</returns>
    public T RemoveFirst()
    {
        // Get first object
        T firstObject = objects[0];
        objectCount--;
        // Set new first object as last and sort it down
        objects[0] = objects[objectCount];
        objects[0].HeapIndex = 0;
        SortDown(objects[0]);
        objects[objectCount] = null;
        return firstObject;
    }

    /// <summary>
    /// Check if array contains object.
    /// </summary>
    /// <param name="hObject">Object to check.</param>
    /// <returns>Returns true if array contains object.</returns>
    public bool Contains(T hObject)
    {
        return Equals(objects[hObject.HeapIndex], hObject);
    }

    /// <summary>
    /// Update object priority position.
    /// </summary>
    /// <param name="hObject">Object to update.</param>
    public void UpdateObject(T hObject)
    {
        SortUp(hObject);
    }

    /// <summary>
    /// Clear array of all open objects.
    /// </summary>
    public void Clear()
    {
        if(objectCount > 0)
        {
            for (int i = 0; i < objectCount; i++)
            {
                objects[i] = null;
            }
            objectCount = 0;
        }
    }

    private void SortUp(T hObject)
    {
        // Get parent object index
        int parentIndex = (hObject.HeapIndex - 1) / 2;
        while (true)
        {
            // Check if object score is lower and swap it (up) with parent object
            T parentObject = objects[parentIndex];
            if(hObject.CompareTo(parentObject) > 0)
            {
                Swap(hObject, parentObject);
            }
            else
            {
                break;
            }

            // Get new parent object index
            parentIndex = (hObject.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T hObject)
    {
        while (true)
        {
            // Get object child index on left and right
            int childIndexLeft = hObject.HeapIndex * 2 + 1;
            int childIndexRight = hObject.HeapIndex * 2 + 2;
            int swapIndex;

            // Check if object has children
            if(childIndexLeft < objectCount)
            {
                swapIndex = childIndexLeft;

                if(childIndexRight < objectCount)
                {
                    if(objects[childIndexLeft].CompareTo(objects[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // Check if object score is bigger and swap it (down) with child object
                if (hObject.CompareTo(objects[swapIndex]) < 0)
                {
                    Swap(hObject, objects[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    private void Swap(T childObject, T parentObject)
    {
        // Swap objects in array
        objects[childObject.HeapIndex] = parentObject;
        objects[parentObject.HeapIndex] = childObject;
        // Swap heap index of objects
        int tempIndex = childObject.HeapIndex;
        childObject.HeapIndex = parentObject.HeapIndex;
        parentObject.HeapIndex = tempIndex;
    }
}
