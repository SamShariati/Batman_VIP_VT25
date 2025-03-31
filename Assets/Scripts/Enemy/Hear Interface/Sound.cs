using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    public enum SoundType { Default = -1, Interesting, Danger }

    public Sound(Vector3 _pos, float _range)
    {
        pos = _pos;

        range = _range;
    }

    public SoundType soundType;

    public readonly Vector3 pos;
    public readonly float range;

}
