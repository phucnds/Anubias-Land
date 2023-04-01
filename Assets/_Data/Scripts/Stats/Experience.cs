using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Experience : MonoBehaviour
{
    [SerializeField] float experiencePoints = 0;

    public event UnityAction onExperienceGained;

    LazyValue<float> expToLvUp;

    private void Awake()
    {
        expToLvUp = new LazyValue<float>(GetExpToLvUp);
    }

    private void Start()
    {
        expToLvUp.ForceInit();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            //GainExperience(Time.deltaTime * 100);
        }
    }

    public float GetPoints()
    {
        return experiencePoints;

    }

    public void GainExperience(float experience)
    {
        experiencePoints += experience;
        onExperienceGained();
        expToLvUp.value = GetComponent<BaseStats>().GetStat(Stat.ExperienceToLevelUp);
    }

    public object CaptureState()
    {
        return experiencePoints;
    }

    public void RestoreState(object state)
    {
        experiencePoints = (float)state;
    }



    public float GetExpToLvUp()
    {
        return GetComponent<BaseStats>().GetStat(Stat.ExperienceToLevelUp);
    }



}
