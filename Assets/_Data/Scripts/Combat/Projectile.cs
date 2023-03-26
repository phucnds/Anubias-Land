using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] bool isHoming = true;
    [SerializeField] float speed = 1;
    [SerializeField] float acceleration = 2f;
    [SerializeField] float maxLifeTime = 10f;

    [SerializeField] GameObject impactParticle; // Effect spawned when projectile hits a collider
    [SerializeField] GameObject projectileParticle; // Effect attached to the gameobject as child
    [SerializeField] GameObject muzzleParticle; // Effect instantly spawned when gameobject is spawned
    [Header("Adjust if not using Sphere Collider")]
    [SerializeField] float colliderRadius = 1f;
    [Range(0f, 1f)] // This is an offset that moves the impact effect slightly away from the point of impact to reduce clipping of the impact effect
    [SerializeField] float collideOffset = 0.15f;


    Vector3 targetPoint;

    Destructible target = null;
    float damage = 0;
    Character instigator = null;

    private void Start()
    {
        transform.LookAt(GetAimLocation());

        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null && isHoming && !target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }

        speed += acceleration;
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);


        if (GetComponent<Rigidbody>().velocity.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity); // Sets rotation to look at direction of movement
        }

        RaycastHit hit;

        float radius; // Sets the radius of the collision detection
        if (transform.GetComponent<SphereCollider>())
            radius = transform.GetComponent<SphereCollider>().radius;
        else
            radius = colliderRadius;

        Vector3 direction = transform.GetComponent<Rigidbody>().velocity; // Gets the direction of the projectile, used for collision detection
        if (transform.GetComponent<Rigidbody>().useGravity)
            direction += Physics.gravity * Time.deltaTime; // Accounts for gravity if enabled
        direction = direction.normalized;

        float detectionDistance = transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime; // Distance of collision detection for this frame

        if (Physics.SphereCast(transform.position, radius, direction, out hit, detectionDistance)) // Checks if collision will happen
        {
            transform.position = hit.point + (hit.normal * collideOffset); // Move projectile to point of collision

            GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; // Spawns impact effect

            ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>(); // Gets a list of particle systems, as we need to detach the trails
                                                                                 //Component at [0] is that of the parent i.e. this object (if there is any)

            Destructible des = hit.collider.GetComponent<Destructible>();
            if (target != null && des != target) return;
            if (des == null || des.IsDead()) return;
            if (hit.collider.gameObject == instigator) return;
            des.TakeDamage(instigator, damage);
            speed = 0;

            for (int i = 1; i < trails.Length; i++) // Loop to cycle through found particle systems
            {
                ParticleSystem trail = trails[i];

                if (trail.gameObject.name.Contains("Trail"))
                {
                    trail.transform.SetParent(null); // Detaches the trail from the projectile
                    Destroy(trail.gameObject, 2f); // Removes the trail after seconds
                }
            }

            Destroy(projectileParticle, 3f); // Removes particle effect after delay
            Destroy(impactP, 3.5f); // Removes impact effect after delay
            Destroy(gameObject); // Removes the projectile
        }
    }

    public void SetTarget(Destructible target, Character instigator, float damage)
    {
        SetTarget(instigator, damage, target);
    }

    public void SetTarget(Vector3 targetPoint, Character instigator, float damage)
    {
        SetTarget(instigator, damage, null, targetPoint);
    }



    public void SetTarget(Character instigator, float damage, Destructible target = null, Vector3 targetPoint = default)
    {
        this.target = target;
        this.damage = damage;
        this.targetPoint = targetPoint;
        this.instigator = instigator;
        Destroy(gameObject, maxLifeTime);
    }

    private Vector3 GetAimLocation()
    {

        if (target == null)
        {
            return targetPoint;
        }

        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

        if (targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

}
