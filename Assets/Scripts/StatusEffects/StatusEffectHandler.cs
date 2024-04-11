using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatusEffectHandler : MonoBehaviour
{
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    [SerializeField] private GameObject uiPrefab; 
    [SerializeField] private RectTransform uiParent; 

    // private List<Sprite> SpriteList = new List<Sprite>(); 

    private AHealth health;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<AHealth>();
    }

    private void Update()
    {
        foreach (StatusEffect e in statusEffects)
        {
            e.UpdateStatusEffect(this);
        }
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        statusEffects.Add(statusEffect);
        statusEffect.ApplyStatusEffect(this);
        if (statusEffect.GetDuration() >= 0) StartCoroutine("DelayedRemoveStatusEffect", statusEffect);
        statusEffect.SetUIGameObject(Instantiate(uiPrefab, uiParent.transform.position, uiParent.transform.rotation, uiParent));
    }

    private IEnumerator DelayedRemoveStatusEffect(StatusEffect statusEffect)
    {
        yield return new WaitForSeconds(statusEffect.GetDuration());
        RemoveStatusEffect(statusEffect);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        Destroy(statusEffect.GetUIGameObject());
        statusEffects.Remove(statusEffect);
        statusEffect.RemoveStatusEffect(this);
    }

    public AHealth GetHealth() { return health; }
    public NavMeshAgent GetNavMeshAgent() { return agent; }

}

/*public enum StatusEffectHandlerType
{
    enemy = 0, 
    player = 1
}*/