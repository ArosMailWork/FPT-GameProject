// Remember that Awake and Start will only ever be called on the first instantiation
// and that member variables won't be reset automatically.  You should reset your
// object yourself after calling Spawn().  (i.e. You'll have to do things like set
// the object's HPs to max, reset animation states, etc...)s

using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    const int DEFAULT_POOL_SIZE = 3;
    //Dictionary contain all pool using GameObject as key
    private static Dictionary<GameObject, Pool> poolDict;

    //Initiate a Pool and put it inside poolDict
    private static void InitPool(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
    {
        //Check if there no pools dictionary in PoolManager then create one 
        if (poolDict == null)
        {
            poolDict = new Dictionary<GameObject, Pool>();
        }
        //Create a new pool and put inside poolDict if there is no pool for the GameObject
        if (prefab != null && poolDict.ContainsKey(prefab) == false)
        {
            poolDict[prefab] = new Pool(prefab, qty);
        }
    }

    //Get pool count by game object
    public static int GetPoolCount(GameObject prefab)
    {
        if (poolDict[prefab] != null)
        {
            return poolDict[prefab].Count();
        }
        return 0;
    }
    //Spawn GameObject with prefab and position
    public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        //Create a new pool if no pool available with InitPool()
        InitPool(prefab);
        //return the object from the pool Spawn() method
        return poolDict[prefab].Spawn(pos, rot);
    }

    //Take from an object from pool but not place in the game world yet
    public static GameObject TakeFromPool(GameObject prefab)
    {
        InitPool(prefab);
        return poolDict[prefab].TakeFromPool();
    }


    public static void Despawn(GameObject obj)
    {
        //Take the GameObject's GameUnit component if null then the object not on the pool
        //if not null then call the pool Despawn() method
        GameUnit gameUnit = obj.GetComponent<GameUnit>();
        if (gameUnit == null)
        {
            Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
        }
        else
        {
            gameUnit.pool.Despawn(obj);
        }
    }


    public static void Preload(GameObject prefab, int qty = 3)
    {
        //Create a new pool if no pool available with InitPool()
        InitPool(prefab, qty);

        //Make an array to grab the objects we about to pre-spawn
        GameObject[] objs = new GameObject[qty];
        for (int i = 0; i < qty; i++)
        {
            objs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
        }
        //Despawn and put all of them in pool with pool Despawn() method
        for (int i = 0; i < qty; i++)
        {
            Despawn(objs[i]);
        }
    }
}
