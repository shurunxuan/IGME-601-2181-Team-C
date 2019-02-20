using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SimpleNotify();

public interface Objective
{
    bool State { get; }
}
