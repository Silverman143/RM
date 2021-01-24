using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform bar;
    private Color normalColor;
    
    void Start()
    {
        bar = GameObject.Find("Bar").transform;
        normalColor = new Color(78f/255f, 152f/255f, 36f/255f, 255f/255f);
        
    }

    
    public void SetSize(float _sizeNormalized)
    {
        if (_sizeNormalized > 0)
        {
            bar.localScale = new Vector3(_sizeNormalized, 1f);
        }
        if (_sizeNormalized < 0)
        {
            bar.localScale = new Vector3(0f, 1f);
        }
        
    }

    public void LowHealth()
    {
        if (bar.Find("BarSprite").GetComponent<SpriteRenderer>().color == Color.white)
        {
            bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = Color.red;
        }else
        {
            bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void Normal_color()
    {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = normalColor;
        
    }

    
}
