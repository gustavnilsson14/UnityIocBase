using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SequenceType
{
    DEFAULT, RANDOMIZE_DECK, RANDOMIZE, PICK_ONE
}

public class RandomUtil
{
    public static System.Random random = new System.Random();

    public static List<T> Shuffle<T>(List<T> list)
    {
        return list.OrderBy(x => random.Next()).ToList();
    }
    /// <summary>
    /// Method quantitatively tested and working as intended
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<IRarityObject> ShuffleRarityList(List<IRarityObject> list)
    {
        return list.OrderBy(x => x.GetRarity() * random.Next()).ToList();
    }

    public static int RandomInt(int min, int max)
    {
        return random.Next(min, max);
    }

    public static float RandomFloat(float min, float max)
    {
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }
    public static bool RandomBool()
    {
        return random.NextDouble() > 0.5f;
    }
    public static T RandomIndex<T>(List<T> list)
    {
        return list[RandomInt(0, list.Count)];
    }
}
public interface IRarityObject : IBase
{
    float GetRarity();
}

[System.Serializable]
public class SequenceList<T> : List<T>
{
    public SequenceType sequenceType;
    private int sequenceIndex = 0;
    private bool looping = false;

    public SequenceList(SequenceType sequenceType, IEnumerable<T> content, bool looping = false)
    {
        this.sequenceType = sequenceType;
        this.looping = looping;
        AddRange(content);
        InitialSort();
    }
    public void InitialSort() {
        switch (sequenceType)
        {
            case SequenceType.RANDOMIZE_DECK:
                List<T> temp = RandomUtil.Shuffle(this);
                Clear();
                AddRange(temp);
                break;
            case SequenceType.PICK_ONE:
                T theOne = RandomUtil.Shuffle(this)[0];
                int amount = this.Count;
                Clear();
                new int[amount].ToList().ForEach(x => Add(theOne));
                break;
            default:
                break;
        }
    }
    public bool Next(out T item)
    {
        item = default(T);
        if (!Loop())
            return false;
        //if (HandleRandomize(out item))
        //    return true;
        item = this[sequenceIndex++];
        return true;
    }

    private bool Loop()
    {
        if (sequenceIndex < this.Count)
            return true;
        sequenceIndex = 0;
        return looping;
    }
    /*
    private bool HandleRandomize(out T item)
    {
        item = default(T);
        if (sequenceType != SequenceType.RANDOMIZE)
            return false;
        sequenceIndex++;
        item = RandomUtil.Shuffle(this)[0];
        return true;
    }
    */
}
