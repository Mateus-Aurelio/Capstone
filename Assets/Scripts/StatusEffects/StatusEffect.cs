using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "newStatusEffect", menuName = "StatusEffect", order = 0)]
public abstract class StatusEffect : ScriptableObject
{
    [SerializeField] private Sprite image;
    [SerializeField] protected float duration = 1;
    private GameObject uiGameObject;

    public abstract void ApplyStatusEffect(StatusEffectHandler statusEffectHandler);

    public abstract void UpdateStatusEffect(StatusEffectHandler statusEffectHandler);

    public abstract void RemoveStatusEffect(StatusEffectHandler statusEffectHandler);

    public float GetDuration()
    {
        return duration;
    }

    public GameObject GetUIGameObject()
    {
        return uiGameObject;
    }

    public void SetUIGameObject(GameObject given)
    {
        uiGameObject = given;
    }
}
