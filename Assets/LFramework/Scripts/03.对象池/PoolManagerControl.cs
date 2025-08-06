using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

/// <summary>
/// 例子：  
/// Transform transform =PoolManagerControl.instant.GetRefab("ww");
///  Transform transform =PoolManagerControl.instant.GetRefab("ww"，tra);
///transform.gameObject.SetActive(true);
///
/// 
/// 
/// 
/// 
/// </summary>

/*例子2
    public Transform parentTransform;
    public List<Transform> tranfaList= new List<Transform>();

    public void Getfab(){

        tranfaList.Add(PoolManagerControl.instant.GetRefab("ww", parentTransform)) ;
    }

    public void Deletefab() {
        if (tranfaList.Count>0)
        {
            PoolManagerControl.instant.Despawnfab(tranfaList[0]);
            tranfaList.RemoveAt(0);
        }
   
    }
*/




public class PoolManagerControl : MonoBehaviour
{

	public static PoolManagerControl Instance;
	SpawnPool spawnPool;
   // PrefabPool refabPool;
	//public GameObject ces;
	public List<GameObject> fabs = new List<GameObject>();

    private void Awake()
    {
		Instance = this;
	}
    void Start()
    {
		spawnPool = PoolManager.Pools[this.gameObject.name];
        // refabPool = new PrefabPool(Resources.Load<Transform>("ww"));
        if (fabs.Count>0)
        {
			InstantPool(fabs);
		}
     
	    
	    
	}


	/// <summary>
	/// 初始化内存池
	/// </summary>
	public void InstantPool(List<GameObject> refabs, int _preloadAmount = 5)
	{


        for (int i = 0; i < refabs.Count; i++)
        {

			PrefabPool refabPool = new PrefabPool(refabs[i].transform);
			if (!spawnPool._perPrefabPoolOptions.Contains(refabPool))
			{
				//refabPool = new PrefabPool(Resources.Load<Transform>("momo"));

				//默认初始化两个Prefab
				refabPool.preloadAmount = _preloadAmount;
				////开启限制
				//refabPool.limitInstances = true;
				////关闭无限取Prefab
				//refabPool.limitFIFO = false;
				////限制池子里最大的Prefab数量
				//refabPool.limitAmount = 5;
				////开启自动清理池子
				//refabPool.cullDespawned = true;
				////最终保留
				//refabPool.cullAbove = 10;
				////多久清理一次
				//refabPool.cullDelay = 5;
				////每次清理几个
				//refabPool.cullMaxPerPass = 5;
				//初始化内存池
				spawnPool._perPrefabPoolOptions.Add(refabPool);
				spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[i]);
			}

		}
	

	}





	/// <summary>
	/// 初始化内存池
	/// </summary>
	public void InstantPool(GameObject refab, int _preloadAmount=5) {
		PrefabPool refabPool = new PrefabPool(refab.transform);

		if (!spawnPool._perPrefabPoolOptions.Contains(refabPool))
        {
			//refabPool = new PrefabPool(Resources.Load<Transform>("momo"));
		
			//默认初始化两个Prefab
			refabPool.preloadAmount = _preloadAmount;
			////开启限制
			//refabPool.limitInstances = true;
			////关闭无限取Prefab
			//refabPool.limitFIFO = false;
			////限制池子里最大的Prefab数量
			//refabPool.limitAmount = 5;
			////开启自动清理池子
			//refabPool.cullDespawned = true;
			////最终保留
			//refabPool.cullAbove = 10;
			////多久清理一次
			//refabPool.cullDelay = 5;
			////每次清理几个
			//refabPool.cullMaxPerPass = 5;
			//初始化内存池
			spawnPool._perPrefabPoolOptions.Add(refabPool);
			spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool.Count]);
		}
		//if (spawnPool == null)
		//{
		//	spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool.Count]);
		//}
	}

	/// <summary>
	/// 初始化内存池从Resources取对象
	/// </summary>
	public void InstantPool(string refabName, int _preloadAmount = 5)
	{
		PrefabPool refabPool = new PrefabPool(Resources.Load<Transform>(refabName));

		if (!spawnPool._perPrefabPoolOptions.Contains(refabPool))
			{
				refabPool = new PrefabPool(Resources.Load<Transform>("ww"));

				//默认初始化两个Prefab
				refabPool.preloadAmount = _preloadAmount;
            ////开启限制
            //refabPool.limitInstances = true;
            ////关闭无限取Prefab
            //refabPool.limitFIFO = false;
            ////限制池子里最大的Prefab数量
            //refabPool.limitAmount = 5;
            ////开启自动清理池子
            //refabPool.cullDespawned = true;
            ////最终保留
            //refabPool.cullAbove = 10;
            ////多久清理一次
            //refabPool.cullDelay = 5;
            ////每次清理几个
            //refabPool.cullMaxPerPass = 5;
           // 初始化内存池

                spawnPool._perPrefabPoolOptions.Add(refabPool);
				spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool.Count]);
			}
			//if (spawnPool == null)
			//{
			//	spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool.Count]);
			//}
		}



	public Transform GetRefab(string name) {

	
		return spawnPool.Spawn(name);
	}

	public Transform GetRefab(string name,Transform parentTransform)
	{

		return spawnPool.Spawn(name, parentTransform);
	}



	/// <summary>
	/// 回收
	/// </summary>
	/// <param name="tran"></param>
	public void Despawnfab(Transform tran)
	{
		//spawnPool.Remove(tran);
		spawnPool.Despawn(tran, spawnPool.transform);
	}
	


/// <summary>
/// 
/// </summary>
/// <param name="tran"></param>
/// <param name="T">延时</param>
	public void Despawnfab(Transform tran,int T)
	{
		//spawnPool.Remove(tran);
		spawnPool.Despawn(tran,T, spawnPool.transform);
	}






	public void CleanPool()
	{
		//清空池子
		spawnPool.DespawnAll();

	}


	public Transform GetRandom()
	{
		return spawnPool.Spawn(fabs[Random.Range(0, fabs.Count)].name);
	}
}
