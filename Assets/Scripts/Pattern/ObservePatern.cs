using UnityEngine;
using System.Collections;


/// <summary>
/// 观察者模式(不断地去询问)
/// 缺点：没一帧都去调用，效率比较低
/// </summary>
/// 

public class ObservePatern : MonoBehaviour {

    Animation anim;
    private float timeCount;


	void Start () {
	
	}	
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAnim();
        }
        if (isPlaying())
        {
            timeCount += Time.deltaTime;
            if (timeCount>1)
            {
                print("show effect!");
            }           
        }	
	}
    public void PlayAnim()
    {
        timeCount = 0;
        anim.Play();
    }
    private bool isPlaying()
    {
        return anim.isPlaying;
    }
}
