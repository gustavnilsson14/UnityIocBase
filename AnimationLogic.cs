using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimationLogic : InterfaceLogicBase
{
    public static AnimationLogic I;

    public List<IAnimated> animateds = new List<IAnimated>();

    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        Initialize<IAnimated>(InitAnimated, newBase);
    }

    private void InitAnimated(IAnimated animated)
    {
        if(debug) animateds.Add(animated);
        animated.animator = animated.GetGameObject().GetComponent<Animator>();
    }

    public void RunAnimationEvent(Animator animator, string animationKey, bool useParameterType, AnimatorControllerParameterType parameterType) {
        if (IsNull(animator)) return;
        if (!useParameterType)
        {
            animator.Play(animationKey);
            return;
        }
        switch (parameterType)
        {
            case AnimatorControllerParameterType.Bool:
                animator.SetBool(animationKey, !animator.GetBool(animationKey));
                return;
            case AnimatorControllerParameterType.Trigger:
                animator.SetTrigger(animationKey);
                return;
        }
    }
    public void RunAnimationEvent(AnimationEvent animationEvent)
    {
        bool useParameterType = animationEvent.TryGetParameterType(out AnimatorControllerParameterType parameterType);
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        RunAnimationEvent(animationEvent.animator, animationKey, useParameterType, parameterType);
    }

    internal void RunAnimationEvent<T0>(AnimationEvent<T0> animationEvent)
    {
        bool useParameterType = animationEvent.TryGetParameterType(out AnimatorControllerParameterType parameterType);
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        RunAnimationEvent(animationEvent.animator, animationKey, useParameterType, parameterType);
    }

    internal void RunAnimationEvent<T0, T1>(AnimationEvent<T0, T1> animationEvent)
    {
        bool useParameterType = animationEvent.TryGetParameterType(out AnimatorControllerParameterType parameterType);
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        RunAnimationEvent(animationEvent.animator, animationKey, useParameterType, parameterType);
    }

    internal void RunAnimationEvent<T0, T1, T2>(AnimationEvent<T0, T1, T2> animationEvent)
    {
        bool useParameterType = animationEvent.TryGetParameterType(out AnimatorControllerParameterType parameterType);
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        RunAnimationEvent(animationEvent.animator, animationKey, useParameterType, parameterType);
    }

    internal void RunAnimationEvent<T0, T1, T2, T3>(AnimationEvent<T0, T1, T2, T3> animationEvent)
    {
        bool useParameterType = animationEvent.TryGetParameterType(out AnimatorControllerParameterType parameterType);
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        RunAnimationEvent(animationEvent.animator, animationKey, useParameterType, parameterType);
    }

    internal void SetBool(Animator animator, string animationKey, bool v) => animator.SetBool(animationKey, v);

    internal void UnsetBool(Animator animator, string animationKey)
    {
        animator.SetBool(animationKey, false);
    }
    internal void UnsetBool(AnimationEvent animationEvent)
    {
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        UnsetBool(animationEvent.animator, animationKey);
    }
    internal void UnsetBool<T0>(AnimationEvent<T0> animationEvent)
    {
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        UnsetBool(animationEvent.animator, animationKey);
    }
    internal void UnsetBool<T0, T1>(AnimationEvent<T0, T1> animationEvent)
    {
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        UnsetBool(animationEvent.animator, animationKey);
    }
    internal void UnsetBool<T0, T1, T2>(AnimationEvent<T0, T1, T2> animationEvent)
    {
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        UnsetBool(animationEvent.animator, animationKey);
    }
    internal void UnsetBool<T0, T1, T2, T3>(AnimationEvent<T0, T1, T2, T3> animationEvent)
    {
        if (!animationEvent.TryGetAnimationKey(out string animationKey)) return;
        UnsetBool(animationEvent.animator, animationKey);
    }

    public void SetActiveLayers(IAnimated animated, List<string> layers, bool invert = false)
    {
        for (int i = 0; i < animated.animator.layerCount; i++)
        {
            string name = animated.animator.GetLayerName(i);
            if (IsNull(layers) || !layers.Any() || layers.Contains(name))
            {
                animated.animator.SetLayerWeight(i, !invert ? 1 : 0);
                continue;
            }
            animated.animator.SetLayerWeight(i, !invert ? 0 : 1);
        }
    }
}
public interface IAnimated : IBase { 
    Animator animator { get; set; }
}

public class AnimationEvent : UnityEvent
{
    public IBase b;
    public string name;
    public Animator animator;
    public AnimationEvent(IBase b = null, string name = "default")
    {
        this.b = b;
        this.name = name;
        if (b == null)
            return;
        this.animator = b.GetGameObject().GetComponent<Animator>();
    }
    public new void Invoke()
    {
        base.Invoke();
        if (animator == null)
            return;
        AnimationLogic.I.RunAnimationEvent(this);
    }
    public virtual bool TryGetParameterType(out AnimatorControllerParameterType parameterType) {
        parameterType = AnimatorControllerParameterType.Bool;
        return false;
    }
    public virtual string GetName() => name;
    public virtual bool TryGetAnimationKey(out string animationKey)
    {
        animationKey = $"{GetType()}_{GetName()}";
        return true;
    }
}
public class AnimationEvent<T0> : UnityEvent<T0>
{
    public IBase b;
    public string name;
    public Animator animator;
    public AnimationEvent(IBase b = null, string name = "default")
    {
        this.b = b;
        this.name = name;
        if (b == null)
            return;
        this.animator = b.GetGameObject().GetComponent<Animator>();
    }
    public new void Invoke(T0 t0)
    {
        base.Invoke(t0);
        if (animator == null)
            return;
        AnimationLogic.I.RunAnimationEvent(this);
    }
    public virtual bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Bool;
        return false;
    }
    public virtual string GetName() => name;
    public virtual bool TryGetAnimationKey(out string animationKey)
    {
        animationKey = $"{GetType()}_{GetName()}";
        return true;
    }
}
public class AnimationEvent<T0, T1> : UnityEvent<T0, T1>
{
    public IBase b;
    public string name;
    public Animator animator;
    public AnimationEvent(IBase b = null, string name = "default")
    {
        this.b = b;
        this.name = name;
        if (b == null)
            return;
        this.animator = b.GetGameObject().GetComponent<Animator>();
    }
    public new void Invoke(T0 t0, T1 t1)
    {
        base.Invoke(t0, t1);
        if (animator == null)
            return;
        AnimationLogic.I.RunAnimationEvent(this);
    }
    public virtual bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Bool;
        return false;
    }
    public virtual string GetName() => name;
    public virtual bool TryGetAnimationKey(out string animationKey)
    {
        animationKey = $"{GetType()}_{GetName()}";
        return true;
    }
}
public class AnimationEvent<T0, T1, T2> : UnityEvent<T0, T1, T2>
{
    public IBase b;
    public string name;
    public Animator animator;
    public AnimationEvent(IBase b = null, string name = "default")
    {
        this.b = b;
        this.name = name;
        if (b == null)
            return;
        this.animator = b.GetGameObject().GetComponent<Animator>();
    }
    public new void Invoke(T0 t0, T1 t1, T2 t2)
    {
        base.Invoke(t0, t1, t2);
        if (animator == null)
            return;
        AnimationLogic.I.RunAnimationEvent(this);
    }
    public virtual bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Bool;
        return false;
    }
    public virtual string GetName() => name;
    public virtual bool TryGetAnimationKey(out string animationKey)
    {
        animationKey = $"{GetType()}_{GetName()}";
        return true;
    }

}
public class AnimationEvent<T0, T1, T2, T3> : UnityEvent<T0, T1, T2, T3>
{
    public IBase b;
    public string name;
    public Animator animator;
    public AnimationEvent(IBase b = null, string name = "default")
    {
        this.b = b;
        this.name = name;
        if (b == null)
            return;
        this.animator = b.GetGameObject().GetComponent<Animator>();
    }
    public new void Invoke(T0 t0, T1 t1, T2 t2, T3 t3)
    {
        base.Invoke(t0, t1, t2, t3);
        if (animator == null)
            return;
        AnimationLogic.I.RunAnimationEvent(this);
    }
    public virtual bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Bool;
        return false;
    }
    public virtual string GetName() => name;
    public virtual bool TryGetAnimationKey(out string animationKey) { 
        animationKey = $"{GetType()}_{GetName()}";
        return true;
    } 
}