using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    public int GetTransformHashCode();
    public void Init();
    public void Disable();
    public void Enable();
    public void Reset();
}
