using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Anubias-Land/Inventory/Weapon", order = 10)]
public class WeaponConfig : ScriptableObject
{

    [SerializeField] Weapon weaponPrefab = null;
    [SerializeField] AnimatorOverrideController animatorOverride = null;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] bool isRightHanded = true;
    [SerializeField] Projectile projectile = null;
    [SerializeField] float weaponDamage = 5f;
    [SerializeField] float percentageBonus = 0;

    const string weaponName = "Weapon";

    public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
    {
        if (rightHand == null || leftHand == null) return null;

        DestroyOldWeapon(rightHand, leftHand);

        Weapon weapon = null;
        if (weaponPrefab != null)
        {
            Transform handTransform = isRightHanded ? rightHand : leftHand;
            weapon = Instantiate(weaponPrefab, handTransform);
            weapon.gameObject.name = weaponName;
            //Debug.Log("equip weapon");
        }

        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (animatorOverride != null)
        {
            animator.runtimeAnimatorController = animatorOverride;
        }
        else if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }

        return weapon;

    }

    private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
        Transform oldWeapon = rightHand.Find(weaponName);
        if (oldWeapon == null)
            oldWeapon = leftHand.Find(weaponName);

        if (oldWeapon == null) return;

        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject);
    }

    public bool HasProjectile()
    {
        return projectile != null;
    }

    public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Destructible target, GameObject gameObject, int v)
    {
        
    }
   
}
