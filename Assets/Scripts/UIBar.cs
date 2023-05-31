using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    private Slider Slider;
    public CharacterController2D pc;
    private GameManager GM;
    public Gradient gradient;
    public Image fill;


    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        Slider = GetComponent<Slider>();
        if (GM.HealthbarScale != 0)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GM.HealthbarScale,100);
        }
        Initialize(pc.life,pc.maxHp);
    }

    void Update()
    {
        SetValue(pc.life);
        Setmaxvalue(pc.maxHp);
        GM.HealthbarScale = gameObject.GetComponent<RectTransform>().rect.width;
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
        fill.color = gradient.Evaluate(1f);
    }
    void Setmaxvalue(float maxfuel)
    {
        Slider.maxValue = maxfuel;
    }

}