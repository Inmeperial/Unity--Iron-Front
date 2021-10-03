using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptEnrique : MonoBehaviour
{
    public GameObject objForScale = default;
    private bool _isNoCover = false;
    private bool _isHalfCover = false;
    private bool _isFullCover = false;
    private Material _matCover1 = default;
    private Material _matCover2 = default;

    private void Start()
    {
        _matCover1 = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        _matCover2 = this.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material;
        objForScale.transform.position = this.transform.position;
        SetNoCover();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("true");
            SetCover(true);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("false");
            SetCover(false); 
        }
    }

    public void SetCover(bool isActivated)
    {
        //cuando termina moverlo afuera del mapa.
        if (isActivated)
        {
            if (_isNoCover)
            {
                SetNoCover();
            }
            if (_isHalfCover)
            {
                SetHalfCover();
            }
            if (_isFullCover)
            {
                SetFullCover();
            }
        }
        else
        {
            SetNoCover();
        }
    }
    private void SetNoCover()
    {
        //_isNoCover = true;
        _matCover1.SetInt("_halfShield", 0);
        _matCover1.SetInt("_fullShield", 0);
        _matCover2.SetInt("_halfShield", 0);
        _matCover2.SetInt("_fullShield", 0);
    }
    private void SetHalfCover()
    {
        //_isHalfCover = false;
        _matCover1.SetInt("_halfShield", 1);
        _matCover1.SetInt("_fullShield", 0);
        _matCover2.SetInt("_halfShield", 1);
        _matCover2.SetInt("_fullShield", 0);
    }
    private void SetFullCover()
    {
        //_isFullCover = false;
        _matCover1.SetInt("_halfShield", 0);
        _matCover1.SetInt("_fullShield", 1);
        _matCover2.SetInt("_halfShield", 0);
        _matCover2.SetInt("_fullShield", 1);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "FeetsScale")
        {
            _isHalfCover = true;
            SetHalfCover();
            objForScale.transform.position = new Vector3(300, 300, 300);
        }
        if (collision.transform.name == "BodyScale")
        {
            _isFullCover = true;
            SetFullCover();
            objForScale.transform.position = new Vector3(300, 300, 300);
        }
    }
}

#region Old Code

//public GameObject mecha;
//private GameObject _mechaBody;
//private GameObject _mechaLegs;
//private GameObject _objShield;
//private float _colliderLimitUp;

//// Start is called before the first frame update
//void Start()
//{
//    _objShield = this.transform.GetChild(0).gameObject;
//    _colliderLimitUp = GetLimitCollider(this.gameObject, true) + this.transform.position.y;
//    SetPartsOfMecha(mecha);

//}
//private void Update()
//{
//    SetCoverShader();
//}

//private void SetPartsOfMecha(GameObject objMecha)
//{
//    _mechaBody = objMecha.transform.Find("Body GO").gameObject;
//    _mechaLegs = objMecha.transform.Find("Legs GO").gameObject;
//}

//public void SetCoverShader()
//{
//    //"Este obj es = " + this.transform.name +
//    //Debug.Log("asd = " + GetLimitCollider(objLegs, true));
//    if (_mechaLegs != null && _mechaBody != null)
//    {
//        float mechaPos = mecha.transform.position.y;
//        float mechaBodyPos = _mechaBody.transform.position.y;
//        float mechaLegsPos = _mechaLegs.transform.position.y;

//        float mechaBodyLimit = GetLimitCollider(_mechaLegs, true) + mechaPos + mechaBodyPos;
//        float mechaLegsLimit = GetLimitCollider(_mechaBody, true) + mechaPos + mechaLegsPos;


//        if (_colliderLimitUp <= mechaLegsLimit) // DeActivate shader Shield
//        {
//            for (int i = 0; i < _objShield.transform.childCount; i++)
//            {
//                if (_objShield.transform.GetChild(i).gameObject.activeSelf)
//                {
//                    _objShield.transform.GetChild(i).gameObject.SetActive(false);
//                }
//            }
//        }

//        if (_colliderLimitUp > mechaLegsLimit && _colliderLimitUp < mechaBodyLimit) // Half Shield
//        {
//            for (int i = 0; i < _objShield.transform.childCount; i++)
//            {
//                if (!_objShield.transform.GetChild(i).gameObject.activeSelf)
//                {
//                    _objShield.transform.GetChild(i).gameObject.SetActive(true);
//                }
//                Material material = _objShield.transform.GetChild(i).GetComponent<Renderer>().material;
//                material.SetInt("_halfShield", 1);
//                material.SetInt("_fullShield", 0);
//            }
//        }

//        if (_colliderLimitUp > mechaBodyLimit) // Full Shield
//        {
//            for (int i = 0; i < _objShield.transform.childCount; i++)
//            {
//                if (!_objShield.transform.GetChild(i).gameObject.activeSelf)
//                {
//                    _objShield.transform.GetChild(i).gameObject.SetActive(true);
//                }
//                Material material = _objShield.transform.GetChild(i).GetComponent<Renderer>().material;
//                material.SetInt("_halfShield", 0);
//                material.SetInt("_fullShield", 1);
//            }
//        }
//        Debug.Log("mechaBodyLimit = " + mechaBodyLimit + " y mechaLegsLimit = " + mechaLegsLimit);
//    }
//    else
//    {
//        Debug.Log("Need to set mecha Legs and body first");
//    }
//}

//private float GetLimitCollider(GameObject obj, bool isTop)
//{
//    Vector3 size = obj.GetComponent<BoxCollider>().size;
//    Vector3 center = obj.GetComponent<BoxCollider>().center;
//    //float scale = obj.transform.localScale.y;
//    float total;

//    if (isTop)
//    {
//        total = ((size.y / 2) + center.y);
//    }
//    else
//    {
//        total = (((size.y / 2) * -1) + center.y);
//    }

//    return total;
//}

#endregion