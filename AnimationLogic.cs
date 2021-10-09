using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimationLogic : InterfaceLogicBase
{
    public static AnimationLogic I;
    /*
    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
        AnimatedInternalListeners(newBase as IAnimated);
    }
    private void AnimatedInternalListeners(IAnimated animated)
    {
        if (animated == null)
            return;
        if (!GetAnimator(animated, out Animator animator))
        {
            Debug.LogError($"{animated} has no component of type Animator");
            Destroy(animated.GetGameObject(),0.01f);
            return;
        }
        if (animated is IDamageable)
            RegisterDamageableAnimations(animated as IDamageable);
        if (animated is IShooter)
            RegisterShooterAnimations(animated as IShooter);
        if (animated is IUsableItem)
            RegisterUsableItemAnimations(animated as IUsableItem);
        if (animated is IMeleeWeapon)
            RegisterMeleeWeaponsAnimations(animated as IMeleeWeapon);
        if (animated is IComboItem)
            RegisterComboItemAnimations(animated as IComboItem);
        if (animated is IItemUser)
            RegisterItemUserAnimations(animated as IItemUser);
        animated.animator = animator;
    }

    private void RegisterInterfaceEvent(IAnimated animated, UnityEvent e)
    {
        e.AddListener(PlayAnimation);
    }

    private void PlayAnimation()
    {
        Debug.Log("fis");
    }

    private void RegisterItemUserAnimations(IItemUser itemUser)
    {
        itemUser.onItemUse.AddListener(OnItemUserUse);
    }

    private void RegisterUsableItemAnimations(IUsableItem usableItem)
    {
        usableItem.onItemUse.AddListener(OnItemUse);
    }

    private void RegisterMeleeWeaponsAnimations(IMeleeWeapon meleeWeapon)
    {
        meleeWeapon.onMeleeWeaponDealDamage.AddListener(OnMeleeWeaponDealDamage);
    }

    private void RegisterComboItemAnimations(IComboItem comboItem)
    {
        comboItem.onComboItemUse.AddListener(OnComboItemUse);
    }

    private void RegisterShooterAnimations(IShooter shooter)
    {
        shooter.onSpawn.AddListener(OnShoot);
    }

    private void RegisterDamageableAnimations(IDamageable damageable)
    {
        damageable.onDeath.AddListener(OnDeath);
        damageable.onHit.AddListener(OnHit);
    }

    private void OnHit(IDamageable animated, IDamageSource damageSource)
    {
        PlayAnimation(animated as IAnimated, "Hit");
    }

    private void OnDeath(IDamageable animated, IDamageSource damageSource)
    {
        PlayAnimation(animated as IAnimated, "Death");
    }

    private void OnShoot(ISpawner animated, GameObject arg1)
    {
        PlayAnimation(animated as IAnimated, "Shoot");
    }

    private void OnItemUse(IUsableItem animated)
    {
        PlayAnimation(animated as IAnimated, "Use");
    }
    private void OnItemUserUse(IItemUser animated)
    {
        if (!EquippedItemLogic.I.TryGetItemType(out Type t, animated))
            return;
        string animation = $"{t}";
        if (animated.currentEquippedItem is IComboItem)
            animation = $"Use{(animated.currentEquippedItem as IComboItem).currentComboIndex}";
        PlayAnimation(animated as IAnimated, animation);
    }


    private void OnMeleeWeaponDealDamage(IMeleeWeapon animated)
    {
        PlayAnimation(animated, "DealDamage");
    }

    private void OnComboItemUse(IComboItem animated)
    {
        PlayAnimation(animated as IAnimated, $"Use{animated.currentComboIndex}");
    }


    private bool GetAnimator(IAnimated animated, out Animator animator)
    {
        if (animated.GetGameObject().TryGetComponent(out animator))
            return true;
        animator = animated.GetGameObject().GetComponentInChildren<Animator>();
        return animator != null;
    }

    public void PlayAnimation(IAnimated animated, string animationName) {
        animated.animator.Play(animationName);
    }
    public void SetTrigger(IAnimated animated, string triggerName)
    {
        animated.animator.SetTrigger(triggerName);
    }
    */
    public void RunAnimationEvent(Animator animator, string animationKey, bool useParameterType, AnimatorControllerParameterType parameterType) {
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
        RunAnimationEvent(animationEvent.animator, $"{animationEvent.GetType().ToString()}_{animationEvent.GetName()}", useParameterType, parameterType);
    }

    internal void RunAnimationEvent<T0>(AnimationEvent<T0> animationEvent)
    {
        bool useParameterType = animationEvent.TryGetParameterType(out AnimatorControllerParameterType parameterType);
        Debug.Log($"{animationEvent.GetType().ToString()}_{animationEvent.GetName()}");
        RunAnimationEvent(animationEvent.animator, $"{animationEvent.GetType().ToString()}_{animationEvent.GetName()}", useParameterType, parameterType);
    }

    internal void RunAnimationEvent<T0, T1>(AnimationEvent<T0, T1> animationEvent)
    {
        bool useParameterType = animationEvent.TryGetParameterType(out AnimatorControllerParameterType parameterType);
        RunAnimationEvent(animationEvent.animator, $"{animationEvent.GetType().ToString()}_{animationEvent.GetName()}", useParameterType, parameterType);
    }

    internal void RunAnimationEvent<T0, T1, T2>(AnimationEvent<T0, T1, T2> animationEvent)
    {
        bool useParameterType = animationEvent.TryGetParameterType(out AnimatorControllerParameterType parameterType);
        RunAnimationEvent(animationEvent.animator, $"{animationEvent.GetType().ToString()}_{animationEvent.GetName()}", useParameterType, parameterType);
    }

    internal void RunAnimationEvent<T0, T1, T2, T3>(AnimationEvent<T0, T1, T2, T3> animationEvent)
    {
        bool useParameterType = animationEvent.TryGetParameterType(out AnimatorControllerParameterType parameterType);
        RunAnimationEvent(animationEvent.animator, $"{animationEvent.GetType().ToString()}_{animationEvent.GetName()}", useParameterType, parameterType);
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
}