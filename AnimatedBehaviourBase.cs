using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBehaviourBase : BehaviourBase, IAnimated
{
    public Animator animator { get; set; }
}
