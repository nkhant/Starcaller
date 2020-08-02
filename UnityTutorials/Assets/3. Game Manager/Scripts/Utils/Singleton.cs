using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SINGLETON DEFINITION - is when you want only one instance of something
//public class, accessible, type class, name is singleton and is generic(will tell what singleton type) and extends monobehaviour, and requires that the
//objects are passed in that is ment to extend the object of that same type
//singleton gameobject fails cause doesnt extend singleton

//virutal meams can be over written
//protected means methods can be called by classes that extend

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //generic so we u=can use to make an audio manager, must be pop dynamicly, need to make temp of class
    private static T instance;
    //public property needs cap at beginning
    public static T Instance 
    {
        get { return instance; }
        // dont want anything extra to class so we remove setters
        //set { }
    }

    //lets check if exists
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("[Singleton] Trying to instantiate a single instance of a singleton class.");
        }
        else
        {
            instance = (T) this;
        }
    }

    protected virtual void OnDestroy()
    { //lets us create another in future
        if(instance == this)
        {
            instance = null;
        }
    }
}
