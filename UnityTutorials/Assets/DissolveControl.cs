using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Material dissolveMaterial;
    public float dissolveSpeed = 0.0f;
    float dissolveStart = 0.0f;
    void Start()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
        //pSystem = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(dissolveStart);
        //Shader.SetGlobalRange("_AlphaClipThreshold", dissolveStart += dissolveSpeed);
        //Shader.Set
        if (dissolveStart >= 1.0f)
            dissolveStart = 0.0f;
        dissolveMaterial.SetFloat("_AlphaClipThreshold", dissolveStart+=dissolveSpeed);

        //if(dissolveSpeed)
        //diss
    }
}
