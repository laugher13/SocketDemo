using UnityEngine;
using System.Collections;

/// <summary>
/// 代理者模式
/// </summary>

public delegate void DelegatePlayFinish();

public class DelegateAnim 
{
    public Animation anim;

    private DelegatePlayFinish playDelegate;

    private float timeCount;

    public DelegateAnim(DelegatePlayFinish playDelegate)
    {
        this.playDelegate = playDelegate;
    }

    public void PlayAnim()
    {
        anim.Play();
        
    }

    private bool isPlaying()
    {
        return anim.isPlaying;
    }

    public void Update()
    {
        Debug.Log("show effect!");
        if (isPlaying())
        {
            timeCount += Time.deltaTime;
            if (timeCount>1)
            {
                playDelegate();
                Debug.Log("show effect!");
            }
        }
    }
}

public class AgencyPattern : MonoBehaviour {


    DelegateAnim playDelegate;

    void PlayFinish()
    {
        print("agency success");
    }

	void Start () {
        playDelegate = new DelegateAnim(PlayFinish);
        playDelegate.PlayAnim();
	}
	
	
	void Update () {
	
	}
}
