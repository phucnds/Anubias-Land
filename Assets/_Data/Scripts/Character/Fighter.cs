using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] Transform leftHandShieldTransform = null;
    [SerializeField] WeaponConfig defaultWeapon = null;

    Destructible target;
    float timeSinceLastAttack = Mathf.Infinity;

    WeaponConfig currentWeaponConfig;
    LazyValue<Weapon> currentWeapon;


    private void Awake()
    {
        currentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
    }

    private Weapon SetupDefaultWeapon()
    {
        return AttachWeapon(defaultWeapon);
    }

    private void Start()
    {
        currentWeapon.ForceInit();

    }




    private Weapon AttachWeapon(WeaponConfig weapon)
    {
        Animator animator = GetComponent<Animator>();
        return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }
}
