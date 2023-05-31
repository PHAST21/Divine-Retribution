using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    private Slider Slider;
    public GameObject boss;
    private FlyingBoss Fboss;
    public Gradient gradient;
    public Image fill;
    private UIManager UI;


    void Start()
    {
        Fboss = boss.GetComponent<FlyingBoss>();
        UI = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        Slider = GetComponent<Slider>();
        Initialize(Fboss.life, Fboss.MaxHP);
    }

    void Update()
    {
        SetValue(Fboss.life);
        if (Fboss.life <= 0)
        {
            UI.FadeIn(1.5f);
        }
    }
    void SetValue(float fuel)
    {
        Slider.value = fuel;
        fill.color = gradient.Evaluate(Slider.normalizedValue);
    }
    void Initialize(float maxfuel, float fuel)
    {
        Slider.maxValue = maxfuel;
        Slider.value = fuel;
        Setmaxvalue(maxfuel);
        fill.color = gradient.Evaluate(1f);
    }
    void Setmaxvalue(float maxfuel)
    {
        Slider.maxValue = maxfuel;
    }

}