using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    //how long it stays on screen
    public float duration = 1.0f;
    public float scrollSpeed = 1.0f;


    private TextMeshPro textMesh;
    private float startTime;

    //animation for crits
    public bool isCritical = false;
    public float standardSize = 22.0f; 

    //if this is start, will constantly try to set text resulting setting to null (error)
    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        startTime = Time.time;
        textMesh.ForceMeshUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime < duration)
        {
            //scroll up
            //transform.LookAt(Camera.main.transform);
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }

        if (isCritical)
        {
            if(textMesh.fontSize > standardSize)
            {
                //deb
                textMesh.fontSize -= 5.0f;
            }
        }
    }


    //set set value
    public void SetDamageNumber(string text)
    {
        textMesh.text = text;
    }

    //can add here for crits
    public void SetColor(Color32 color)
    {
        //textMesh.color = color;
        textMesh.faceColor = color;
    }

    public void SetGradient(VertexGradient color)
    {
        textMesh.colorGradient = color;
        //textMesh.colorGradient.
    }

    public void SetFontSize(float size)
    {
        textMesh.fontSize = size;
    }

    public void IsCritical(bool critical)
    {
        isCritical = critical;
    }
}
