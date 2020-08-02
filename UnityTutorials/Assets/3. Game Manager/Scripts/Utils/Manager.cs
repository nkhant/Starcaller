using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    //acccesor
    public static T Instance
    {
        get { return instance; }
        set
        {
            //if not null, set instance of the passed value
            if(null == instance)
            {
                instance = value;
                DontDestroyOnLoad(instance.gameObject);
            }
            else if (instance != value)  // this lets us check if there is another instance and destroys if so
            {
                Destroy(value.gameObject);
            }
        }
    }

    //will have to override if want something else;
    //public override void Awake() and make sure to call base.awake();
    public virtual void Awake()
    {
        Instance = this as T;
    }
}
