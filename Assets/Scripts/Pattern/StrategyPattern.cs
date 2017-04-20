using UnityEngine;
using System.Collections;

/// <summary>
/// 策略者模式(调用同一个API，输入不同的参数，计算结果也不同)
/// </summary>

public  class StrategyBase
{
    public virtual void Calculate()
    {
        
    }    
}

public class PersonalStrategy:StrategyBase
{
    float myTex;
    public PersonalStrategy(float tex)
    {
        myTex=tex;
    }
    public override void Calculate()
    {
 	  myTex=myTex*0.1f;
    }  
}

public class CompanyStrategy : StrategyBase
{
     float myTex;
    public CompanyStrategy(float tex)
    {
        myTex=tex;
    }
     public override void Calculate()
    {
 	    myTex=myTex*0.2f;
    }
}

public class StrategyTest
{
   
    public void Calc(StrategyBase straetgyBase)
    {
        straetgyBase.Calculate();
    }

   
}


public class StrategyPattern : MonoBehaviour {

	
	void Start () {

        StrategyTest test = new StrategyTest();
        PersonalStrategy ps = new PersonalStrategy(20000);
        test.Calc(ps);
      
       

	
	}
	
	void Update () {
	
	}
}
