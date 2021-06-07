using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float _textDuration;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private Color _normalHitColor;
    [SerializeField] private float _normalHitFontSize;
    [SerializeField] private Color _criticalHitColor;
    [SerializeField] private float _criticalHitFontSize;
    [SerializeField] private Color _missColor;
    [SerializeField] private float _missFontSize;
    private TextMeshPro _text;
    private Color _color;

    private Camera _cam;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MoveAndFade();
    }

    //Setea el texto que se va a mostrar, el tipo de texto y el orden de la layer (que tan arriba se muestra)
    public void SetText(string text, int type, int order)
    {
        _text = gameObject.GetComponent<TextMeshPro>();
        
        _text.text = text;
        _text.sortingOrder = order;
        switch (type)
        {
            case 0:
                _text.fontSize = _missFontSize;
                _text.color = _missColor;
                break;
            
            case 1:
                _text.fontSize = _normalHitFontSize;
                _text.color  = _normalHitColor;
                break;
                
            case 2:
                _text.fontSize = _criticalHitFontSize;
                _text.color  = _criticalHitColor;
                break;
        }
        _color = _text.color;
    }

    //Mueve el texto hacia arriba y lo va haciendo transparente
    void MoveAndFade()
    {
        transform.rotation = _cam.transform.rotation;
        transform.position += transform.up * (_moveSpeed * Time.deltaTime);
        _color.a -= _fadeSpeed * Time.deltaTime;
        _text.color = _color;
    }
    
    public float GetDuration()
    {
        return _textDuration;
    }
}
